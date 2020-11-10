using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;


namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;
        private readonly IAccountDAO accountDAO;
        private readonly IUserDAO userDAO;


        public AccountsController(IAccountDAO accountDAO, IUserDAO userDAO, ITransferDAO transferDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
            this.transferDAO = transferDAO;

        }




        [Authorize]
        [HttpGet("balance")]
        public decimal GetBalance()
        {
            int userId = -1;

            foreach (Claim claim in User.Claims)
            {
                if (claim.Type == "sub")
                {
                    userId = int.Parse(claim.Value);
                    break;
                }
            }
            decimal accountBalance = accountDAO.GetBalance(userId);
            return accountBalance;

        }
        [Authorize]
        [HttpGet("allusers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(this.userDAO.GetUsers());
        }

        [Authorize]
        [HttpPost("transfer")]
        public ActionResult<Transfer> TransferMoneyToUser(Transfer transfer)
        {


            decimal userFromBalance = accountDAO.GetBalance(transfer.AccountFrom);
            if (userFromBalance >= transfer.Amount && transfer.AccountFrom != transfer.AccountTo)
            {

                accountDAO.TransferFundsSendersBalance(transfer);
                accountDAO.TransferFundsReceiversBalance(transfer);
                transferDAO.TransferFunds(transfer);
            }


            return Created($"/transfer/{transfer.TransferID}", transfer);


        }






    }
}




