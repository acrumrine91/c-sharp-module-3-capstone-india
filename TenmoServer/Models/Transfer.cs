using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }

        public int TransferType { get; set; }

        public int TransferStatus { get; set; }

        public decimal Amount { get; set; }

        public User AccountFrom { get; set; }

        public User AccountTo { get; set; }


    }
}
