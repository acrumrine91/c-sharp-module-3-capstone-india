using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }

        public TransferType TransferType { get; set; } = TransferType.Send;

        public TransferStatus TransferStatus { get; set; } = TransferStatus.Approved;

        public decimal Amount { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

    }
    public enum TransferType
    {
        Request = 1000,
        Send = 1001 
    }

    public enum TransferStatus
    {
        Pending = 2000,
        Approved = 2001,
        Rejected = 2002
    }
}
