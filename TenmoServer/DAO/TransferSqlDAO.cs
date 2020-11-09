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

        public bool TransferFunds(int accountTo, int accountFrom, decimal amount)
        {
            bool successful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@type, @status, @from, @to, @amount)", conn);
                    command.Parameters.AddWithValue("@type", 2);
                    command.Parameters.AddWithValue("@status", 2);
                    command.Parameters.AddWithValue("@from", accountFrom);
                    command.Parameters.AddWithValue("@to", accountTo);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.ExecuteNonQuery();

                    command = new SqlCommand("SELECT @@IDENTITY", conn);
                    successful = true;
                    return successful;
                }
            }
            catch (SqlException ex)
            {
                return successful;
            }

        }

    }


}


