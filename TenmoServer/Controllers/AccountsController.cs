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


        public AccountsController(IAccountDAO accountDAO, ITransferDAO transferDAO)
        {
            this.accountDAO = accountDAO;
            this.transferDAO = transferDAO;

        }

        public string userName => User.Identity.Name;



        [HttpGet("balance")]
        public ActionResult<Account> GetBalance()
        {

            Account account = this.accountDAO.GetBalance(userName);

            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpGet("transfer")]
        public ActionResult<List<ReturnUser>> GetAllUsers()
        {
            return Ok(this.transferDAO.GetUsersList());
        }

        [HttpPost("transfer")]
        public ActionResult<Transfer> TransferMoneyToUser(Transfer transfer)
        {
            Account account = this.accountDAO.GetBalance(userName);
            Transfer transferAttempt;

            if(userName == transfer.AccountTo.Username)
            {
                return Forbid();
            }    

            if (account.Balance >= transfer.Amount)
            {
                transferAttempt = transferDAO.ExecuteTransfer(transfer);
                transferDAO.BeginTransfer(transferAttempt);

                return Ok(transferAttempt);
            }

            return BadRequest();
        }
    }
}




