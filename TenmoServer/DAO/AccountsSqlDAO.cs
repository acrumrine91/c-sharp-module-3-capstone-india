using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountsSqlDAO : IAccountsDAO
    {
        private readonly string connectionString;

        public AccountsSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetBalance()
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts", conn);



                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read(); // Move to first row

                    return GetBalanceFromReader(reader);
                }

            }
            return null;
        }
        private Account GetBalanceFromReader(SqlDataReader reader)
        {
            return new Account()
            {
                AccountID = Convert.ToInt32(reader["account_id"]),
                UserID = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
        }
    }
}

