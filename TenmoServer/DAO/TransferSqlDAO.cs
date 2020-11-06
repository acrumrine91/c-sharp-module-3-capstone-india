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

        public List<ReturnUser> GetUsersList(string userName)
        {
            List<ReturnUser> users = new List<ReturnUser>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * From users WHERE users.username != @UserName", conn);
                cmd.Parameters.AddWithValue("@UserName", userName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReturnUser user = GetReturnUserFromReader(reader);
                    users.Add(user);
                }
            }
            return users;
        }

        public void BeginTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmdSend = new SqlCommand("UPDATE accounts SET balance = balance - @amountSent WHERE user_id = @Sender;", conn);
                cmdSend.Parameters.AddWithValue("@amountSent", transfer.Amount);
                cmdSend.Parameters.AddWithValue("@Sender", transfer.AccountFrom);
                cmdSend.ExecuteNonQuery();

                SqlCommand cmdReceive = new SqlCommand("UPDATE accounts SET balance = balance + @amountReceived WHERE user_id = @Receipient;", conn);
                cmdReceive.Parameters.AddWithValue("@amountReceived", transfer.Amount);
                cmdReceive.Parameters.AddWithValue("@Receipient", transfer.AccountTo);
                cmdReceive.ExecuteNonQuery();

            }

        }
        public Transfer ExecuteTransfer(Transfer newTransfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@transferTypeID, @transferStatusID, @accountFromUser, @accountToUser, @amount); SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@transferTypeID", newTransfer.TransferType);
                cmd.Parameters.AddWithValue("@transferStatusID", newTransfer.TransferStatus);
                cmd.Parameters.AddWithValue("@accountFromUser", newTransfer.AccountFrom.UserId);
                cmd.Parameters.AddWithValue("@accountToUser", newTransfer.AccountTo.UserId);
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


                private ReturnUser GetReturnUserFromReader(SqlDataReader reader)
                {
                    ReturnUser user = new ReturnUser();

                    user.UserId = Convert.ToInt32(reader["user_id"]);
                    user.Username = Convert.ToString(reader["username"]);

                    return user;
                }

            }
        }
