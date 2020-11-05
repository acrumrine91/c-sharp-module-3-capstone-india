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

<<<<<<< HEAD
        //put url in account controller
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Account> GetBalance (int id)
=======
        public string userName => User.Identity.Name;



        [HttpGet("balance")]
        public ActionResult<Account> GetBalance()
>>>>>>> 4e26e611b43ad31dd5391d339e42f3fe1a9458d5
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

    }
}




