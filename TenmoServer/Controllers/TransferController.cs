using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("transfer")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;

        public TransferController(ITransferDAO transferDAO, IAccountDAO accountDAO, IUserDAO userDAO)
        {
            this.transferDAO = transferDAO;
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
        }

        [HttpPost("transferfunds")]
        public ActionResult<bool> SendTransfer(TransferController transfer)
        {
            bool successful = false;

            int transferFrom = -1;

            foreach (var claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    transferFrom = int.Parse(claim.Value);
                    break;
                }
            }

            decimal sendingAccount = accountDAO.GetBalance(transferFrom);
            if (sendingAccount >= transfer.Amount)
            {
                try
                {
                    accountDAO.MoveFundsFrom(transferFrom, transfer.Amount);
                    accountDAO.MoveFundsTo(transfer.TransferTo, transfer.Amount);
                    transferDAO.AddSendTransfer(transfer.TransferTo, transferFrom, transfer.Amount);
                    successful = true;
                }
                catch (Exception ex)
                {
                    return successful;
                }
            }
            return successful;
        }
    }
}
