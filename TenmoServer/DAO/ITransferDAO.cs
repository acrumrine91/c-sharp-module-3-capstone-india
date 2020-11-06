using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<ReturnUser> GetUsersList(string userName);

        void BeginTransfer(Transfer transfer);

        Transfer ExecuteTransfer(Transfer newTransfer);


    }
}
