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
            catch (Exception)
            {
                return result;
            }
        }

        public int GetUserId(int accountTo)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT user_id FROM accounts WHERE account_id = @accountTo", conn);
                    cmd.Parameters.AddWithValue("@accountTo", accountTo);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader["user_id"]);
                    }
                    return result;
                }
            }
            catch (Exception)
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
                    command.Parameters.AddWithValue("@type", transfer.TransferType);
                    command.Parameters.AddWithValue("@status", transfer.TransferStatus);
                    command.Parameters.AddWithValue("@from", accountFrom);
                    command.Parameters.AddWithValue("@to", accountTo);
                    command.Parameters.AddWithValue("@amount", transfer.Amount);


                    int id = Convert.ToInt32(command.ExecuteScalar());
                    return new Transfer
                    {
                        TransferID = id,
                        TransferType = transfer.TransferType,
                        TransferStatus = transfer.TransferStatus,
                        Amount = transfer.Amount,
                        AccountFrom = accountFrom,
                        AccountTo = accountTo
                    };

                }


            }


        }
        public List<Transfer> FullListofUserTransfers(int userID)
        {
            int accountID = GetAccountId(userID);
            List<Transfer> transferList = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE account_from = @accountID OR account_to = @accountID", conn);
                    cmd.Parameters.AddWithValue("@accountID", accountID);

                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Transfer transfer = new Transfer();

                        transfer.TransferID = Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferType = (TransferType)Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferStatus = (TransferStatus)Convert.ToInt32(reader["transfer_status_id"]);
                        transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);

                        transferList.Add(transfer);

                    }
                }
                return transferList;
            }
            catch (SqlException)
            {
                throw;
            }

        }
        public Transfer GetTransferByID(int transferID, int userID)
        {
            Transfer transfer = new Transfer();
            int accountID = GetAccountId(userID);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE transfer_id = @TransferID AND (account_from = @AccountID OR account_to = @AccountID)", conn);
                    cmd.Parameters.AddWithValue("@TransferID", transferID);
                    cmd.Parameters.AddWithValue("@AccountID", accountID);

                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {

                        transfer.TransferID = Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferType = (TransferType)Convert.ToInt32(reader["transfer_id"]);
                        transfer.TransferStatus = (TransferStatus)Convert.ToInt32(reader["transfer_status_id"]);
                        transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
                        transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
                        transfer.Amount = Convert.ToDecimal(reader["amount"]);

                    }
                }
                return transfer;
            }
            catch (SqlException)
            {
                throw;
            }

        }

    }

}



