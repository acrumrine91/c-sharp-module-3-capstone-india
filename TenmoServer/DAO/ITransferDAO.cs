using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer TransferFunds(Transfer transfer);

        int GetUserId(int accountTo);

        int GetAccountId(int user_id);

        List<Transfer> FullListofUserTransfers(int userID);








    }
}
