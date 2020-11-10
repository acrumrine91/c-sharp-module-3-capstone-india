using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    //could remove this class
    public class Account
    {
        public int AccountID { get; set; } 
        
        public int UserID { get; set; }

        public decimal Balance { get; set; }

        public string UserName { get; set; }
    }
}
