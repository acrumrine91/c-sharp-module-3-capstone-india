﻿using System;
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


        public bool TransferFundsReceiversBalance(decimal amount, int userId)
        {
          
            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                {
                    conn.Open();

                    decimal newBalance = GetBalance(userId);
                    newBalance += amount;
                    // UPDATE accounts SET balance = @newBalance WHERE user_id = @UserID
                    SqlCommand command = new SqlCommand("UPDATE accounts JOIN users ON users.user_id = accounts.user_id SET balance = @newBalance WHERE users.user_id = @UserID", conn);
                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@UserID", userId);

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

        public bool TransferFundsSendersBalance(decimal amount, int userID)
        {
            
            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))

                {
                    conn.Open();

                    decimal newBalance = GetBalance(userID);
                    newBalance -= amount;

                    SqlCommand command = new SqlCommand("UPDATE accounts JOIN users ON users.user_id = accounts.user_id SET balance = @newBalance WHERE users.user_id = @UserID", conn);
                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@UserID", userID);

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

