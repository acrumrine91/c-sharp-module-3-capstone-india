using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountsController : ControllerBase
    {

        private readonly IAccountsDAO accountsDAO;

        public AccountsController(IAccountsDAO accountsDAO)
        {
            this.accountsDAO = accountsDAO;
        }

        [HttpGet]
        public ActionResult<Account> GetBalance()
        {
            return Ok(this.accountsDAO.GetBalance());
        }
    }
}
