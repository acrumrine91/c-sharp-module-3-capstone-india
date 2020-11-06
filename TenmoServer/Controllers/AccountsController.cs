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


        public AccountsController(IAccountDAO accountDAO, ITransferDAO transferDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.transferDAO = transferDAO;
            this.userDAO = userDAO;

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
        public ActionResult<List<User>> GetAllUsers()
        {

            return Ok(this.userDAO.GetUsers());
        }

        [HttpPost("transfer")]
        public ActionResult<Transfer> TransferMoneyToUser(Transfer transfer)
        {
            Transfer newTransfer = this.transferDAO.AddTransfer(transfer);
            return Created($"/transfer/{newTransfer.Amount}", newTransfer);

        }


    }
}




