using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO 
    {
<<<<<<< HEAD:TenmoServer/DAO/IAccountsDAO.cs
        Account GetBalance(int id);        
=======
        Account GetBalance(string userName);

>>>>>>> 4e26e611b43ad31dd5391d339e42f3fe1a9458d5:TenmoServer/DAO/IAccountDAO.cs
    }
}
