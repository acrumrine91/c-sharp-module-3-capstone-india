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

        //put url in account controller
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccountBalance (int id)
        {
            foreach (Account user in )
        }
    }
}
