using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        private int userId
        {
            get
            {
                foreach (Claim claim in User.Claims)
                {
                    if (claim.Type == "sub")
                    {
                        return Convert.ToInt32(claim.Value);
                    }
                }

                return -1;
            }
        }


        [HttpGet("balance")]
        public decimal GetBalance()
        {

            decimal accountBalance = accountDAO.GetBalance(userId);
            return accountBalance;

        }

        [HttpGet("allusers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(this.userDAO.GetUsers());
        }


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
        [HttpGet("transfer/history")]
        public ActionResult<List<string>> GetTransferHistoryForUser()
        {
            List<Transfer> transfers = transferDAO.FullListofUserTransfers(userId);
            List<string> displayStringList = new List<string>();
            int accountID = transferDAO.GetAccountId(userId);
            User user = new User();

            foreach (Transfer transfer in transfers)
            {
                if (transfer.AccountFrom == accountID)
                {                    
                    int otherPersonID = transferDAO.GetUserId(transfer.AccountTo);
                    user = userDAO.GetName(otherPersonID);
                    string userName = user.Username;
                    string display = transfer.TransferID + "\t\tTo:\t" + userName + "\t\t" + transfer.Amount.ToString("C");
                    displayStringList.Add(display);
                }
                else if (transfer.AccountTo == accountID)
                {
                    int otherPersonID = transferDAO.GetUserId(transfer.AccountFrom);
                    user = userDAO.GetName(otherPersonID);
                    string userName = user.Username;
                    string display = transfer.TransferID + "\t\tFrom:\t" + userName + "\t\t" + transfer.Amount.ToString("C");
                    displayStringList.Add(display);
                }
                else if (transfers == null)
                {

                    return NoContent();
                }

            }
            return displayStringList;










        }

    }
}




