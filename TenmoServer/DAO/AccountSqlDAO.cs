using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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



        public decimal GetBalance(int userId)
        {
            decimal balance = 0.00M;
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @UserID", conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);


                    SqlDataReader reader = cmd.ExecuteReader(); //scaler?

                    while (reader.Read())
                    {
                        balance = Convert.ToDecimal(reader["balance"]);
                    }
                    return balance;
                }
            }
            catch (SqlException ex)
            {
                return balance;
            }
        }


        public bool TransferFundsReceiversBalance(Transfer transfer)
        {

            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                {
                    conn.Open();

                    decimal newBalance = GetBalance(transfer.AccountTo);
                    newBalance += transfer.Amount;


                    SqlCommand command = new SqlCommand("UPDATE accounts SET balance = @newBalance WHERE user_id = @UserID", conn);

                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@UserID", transfer.AccountTo);

                    command.ExecuteNonQuery();
                    successful = true;
                    return successful;
                }
            }
            catch (Exception ex)
            {
                return successful;
            }
        }

        public bool TransferFundsSendersBalance(Transfer transfer)
        {
            
            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                {
                    conn.Open();

                    decimal newBalance = GetBalance(transfer.AccountFrom);
                    newBalance -= transfer.Amount;

                    SqlCommand command = new SqlCommand("UPDATE accounts SET balance = @newBalance WHERE user_id = @UserID", conn);
                   
                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@UserID", transfer.AccountFrom);

                    command.ExecuteNonQuery();
                    successful = true;
                    return successful;
                }
            }
            catch (Exception ex)
            {
                return successful;
            }
        }








    }
}

