using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        //public bool TransferFunds(int userTo, int userFrom, decimal amount)
        //{
        //    int accountTo = GetAccountId(userTo);
        //    int accountFrom = GetAccountId(userFrom);

        //    bool successful = false;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            SqlCommand command = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@type, @status, @from, @to, @amount)", conn);
        //            command.Parameters.AddWithValue("@type", 1001);
        //            command.Parameters.AddWithValue("@status", 2001);
        //            command.Parameters.AddWithValue("@from", accountFrom);
        //            command.Parameters.AddWithValue("@to", accountTo);
        //            command.Parameters.AddWithValue("@amount", amount);
        //            command.ExecuteNonQuery();

        //            //command = new SqlCommand("SELECT @@IDENTITY", conn);
        //            successful = true;
        //            return successful;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        return successful;
        //    }

        //}
        public int GetAccountId(int user_id)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id FROM accounts WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", user_id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["account_id"]);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public Transfer TransferFunds(Transfer transfer)
        {
            int accountTo = GetAccountId(transfer.AccountTo);
            int accountFrom = GetAccountId(transfer.AccountFrom);


            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO transfers VALUES (@type, @status, @from, @to, @amount); SELECT SCOPE_IDENTITY();", conn);
                    command.Parameters.AddWithValue("@type", 1001);
                    command.Parameters.AddWithValue("@status", 2001);
                    command.Parameters.AddWithValue("@from", accountFrom);
                    command.Parameters.AddWithValue("@to", accountTo);
                    command.Parameters.AddWithValue("@amount", transfer.Amount);

                    int id = Convert.ToInt32(command.ExecuteScalar());
                    return new Transfer
                    {
                        TransferID = id,
                        TransferType = 1001,
                        TransferStatus = 2001,
                        Amount = transfer.Amount,
                        AccountFrom = accountFrom,
                        AccountTo = accountTo
                    };
                }


            }


        }


    }
}


