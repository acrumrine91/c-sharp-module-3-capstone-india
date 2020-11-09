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
        //public string userName => User.Identity.Name;

        public TransferController(IAccountDAO accountDAO, ITransferDAO transferDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.transferDAO = transferDAO;
            this.userDAO = userDAO;
        }


        [HttpPost]
        public ActionResult<Transfer> TransferMoneyToUser(Transfer transfer)
        {


            //decimal userFromBalance = accountDAO.GetBalance(transfer.AccountFrom);
            //if (userFromBalance >= transfer.Amount && transfer.AccountFrom != transfer.AccountTo)
            //{

                accountDAO.TransferFundsSendersBalance(transfer);
                accountDAO.TransferFundsReceiversBalance(transfer);
                transferDAO.TransferFunds(transfer);
            //}
            

            return Created($"/transfer/{transfer.TransferID}", transfer);


        }
    }
}
