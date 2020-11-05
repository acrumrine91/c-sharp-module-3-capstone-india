using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        
        public Account GetBalance(string userName)
        {

            Account account = new Account();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM accounts JOIN users ON users.user_id = accounts.user_id WHERE users.username = @UserName", conn);
                cmd.Parameters.AddWithValue("@UserName", userName);


                SqlDataReader reader = cmd.ExecuteReader(); //scaler?

                while (reader.Read())
                {
                    account = GetAccountFromReader(reader);
                }    

            }
            return account;
        }
        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account();
            account.AccountID = Convert.ToInt32(reader["account_id"]);
            account.UserID = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);
            account.UserName = Convert.ToString(reader["username"]);

            return account;
        }
    }
}

