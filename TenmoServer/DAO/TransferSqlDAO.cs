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

       

        public void BeginTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmdSend = new SqlCommand("UPDATE accounts SET balance = balance - @amountSent WHERE user_id = @Sender;", conn);
                cmdSend.Parameters.AddWithValue("@amountSent", transfer.Amount);
                cmdSend.Parameters.AddWithValue("@Sender", transfer.AccountFrom.UserId);
                cmdSend.ExecuteNonQuery();

                SqlCommand cmdReceive = new SqlCommand("UPDATE accounts SET balance = balance + @amountReceived WHERE user_id = @Receipient;", conn);
                cmdReceive.Parameters.AddWithValue("@amountReceived", transfer.Amount);
                cmdReceive.Parameters.AddWithValue("@Receipient", transfer.AccountTo.UserId);
                cmdReceive.ExecuteNonQuery();

            }

        }
        public Transfer ExecuteTransfer(Transfer newTransfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers VALUES (@transferTypeID, @transferStatusID, @accountFromUser, @accountToUser, @amount); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@transferTypeID", newTransfer.TransferType);
                cmd.Parameters.AddWithValue("@transferStatusID", newTransfer.TransferStatus);
                cmd.Parameters.AddWithValue("@accountFromUser", newTransfer.AccountFrom.UserId);
                cmd.Parameters.AddWithValue("@accountToUser", newTransfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", newTransfer.Amount);

                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return new Transfer
                {
                    TransferID = id,
                    TransferType = newTransfer.TransferType,
                    TransferStatus = newTransfer.TransferStatus,
                    AccountFrom = newTransfer.AccountFrom,
                    AccountTo = newTransfer.AccountTo,
                    Amount = newTransfer.Amount,
                };

            }
        }


      

    }
}
