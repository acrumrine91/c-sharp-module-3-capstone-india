using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;
        public string userName => User.Identity.Name;

        public TransferController(IAccountDAO accountDAO, ITransferDAO transferDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.transferDAO = transferDAO;
            this.userDAO = userDAO;
        }

        [HttpPost]
        public ActionResult<bool> TransferMoneyToUser(Transfer transfer)
        {

            bool successful = false;

            int transferFromID = -1;



            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    transferFromID = int.Parse(claim.Value);
                    break;
                }
                decimal userFromBalance = accountDAO.GetBalance(transferFromID);
                if (userFromBalance > transfer.Amount)
                {
                    try
                    {
                        accountDAO.TransferFundsSendersBalance(transfer.Amount, transferFromID);
                        accountDAO.TransferFundsReceiversBalance(transfer.Amount, transfer.AccountTo);
                        transferDAO.TransferFunds(transfer.AccountTo, transferFromID, transfer.Amount);
                        successful = true;
                    }
                    catch (Exception ex)
                    {
                        return successful;
                    }
                }
                
            }
            return successful;


        }

    }
}
