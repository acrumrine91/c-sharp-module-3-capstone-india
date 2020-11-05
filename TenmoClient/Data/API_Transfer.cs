using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public int TransferID { get; set; }

        public int TransferType { get; set; }

        public int TransferStatus { get; set; }

        public decimal Amount { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }


    }
}
