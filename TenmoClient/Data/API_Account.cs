﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Account
    {
        public int UserID { get; set; }
        public int AccountID { get; set; }
        public decimal Balance { get; set; }

        public API_Account()
        {

        }
    }
}