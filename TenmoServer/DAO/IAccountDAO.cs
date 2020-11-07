using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        decimal GetBalance(int userId);

        bool TransferFundsReceiversBalance(decimal amount, int userId);

        bool TransferFundsSendersBalance(decimal amount, int userID);
    }
}
