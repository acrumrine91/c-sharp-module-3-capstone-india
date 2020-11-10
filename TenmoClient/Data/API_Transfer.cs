using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{   
    public class API_Transfer
    {
        public int TransferID { get; set; }

        public int AccountFrom { get; set; }

        public decimal Amount { get; set; }

        public int AccountTo { get; set; }

        public API_TransferType TransferType { get; set; } = API_TransferType.Send;

        public API_TransferStatus TransferStatus { get; set; } = API_TransferStatus.Approved;
    }
    public enum API_TransferType
    {
        Request = 1000,
        Send = 1001
    }
    public enum API_TransferStatus
    {
        Pending = 2000,
        Approved = 2001,
        Rejected = 2002
    }

}






