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

        public Transfer AddTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@type, @status, @from, @to, @amount)", conn);
                command.Parameters.AddWithValue("@type", transfer.TransferType);
                command.Parameters.AddWithValue("@status", transfer.TransferStatus);
                command.Parameters.AddWithValue("@from", transfer.AccountFrom);
                command.Parameters.AddWithValue("@to", transfer.AccountTo);
                command.Parameters.AddWithValue("@amount", transfer.Amount);
                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT @@IDENTITY", conn);
                transfer.TransferID = Convert.ToInt32(command.ExecuteScalar());

            }
            return transfer;

            //public void Transfer(Transfer transfer)
            //{
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        conn.Open();

            //        SqlCommand cmdSend = new SqlCommand("UPDATE accounts SET balance = balance - @amountSent WHERE user_id = @Sender;", conn);
            //        cmdSend.Parameters.AddWithValue("@amountSent", transfer.Amount);
            //        cmdSend.Parameters.AddWithValue("@Sender", transfer.AccountFrom.UserId);
            //        cmdSend.ExecuteNonQuery();

            //        SqlCommand cmdReceive = new SqlCommand("UPDATE accounts SET balance = balance + @amountReceived WHERE user_id = @Receipient;", conn);
            //        cmdReceive.Parameters.AddWithValue("@amountReceived", transfer.Amount);
            //        cmdReceive.Parameters.AddWithValue("@Receipient", transfer.AccountTo.UserId);
            //        cmdReceive.ExecuteNonQuery();

            //        SqlCommand cmd = new SqlCommand("INSERT INTO transfers VALUES (@transferTypeID, @transferStatusID, @accountFromUser, @accountToUser, @amount); SELECT SCOPE_IDENTITY();", conn);
            //        cmd.Parameters.AddWithValue("@transferTypeID", transfer.TransferType);
            //        cmd.Parameters.AddWithValue("@transferStatusID", transfer.TransferStatus);
            //        cmd.Parameters.AddWithValue("@accountFromUser", transfer.AccountFrom.UserId);
            //        cmd.Parameters.AddWithValue("@accountToUser", transfer.AccountTo);
            //        cmd.Parameters.AddWithValue("@amount", transfer.Amount);

            //        int id = Convert.ToInt32(cmd.ExecuteScalar());
            //        return new Transfer
            //        {
            //            TransferID = id,
            //            TransferType = transfer.TransferType,
            //            TransferStatus = transfer.TransferStatus,
            //            AccountFrom = transfer.AccountFrom,
            //            AccountTo = transfer.AccountTo,
            //            Amount = transfer.Amount,
            //        };

            //    }

            //}
            //public Transfer ExecuteTransfer(Transfer newTransfer)
            //{
            //    using (SqlConnection conn = new SqlConnection(connectionString))
            //    {
            //        conn.Open();



            //    }
            //}




        }
    }
}
