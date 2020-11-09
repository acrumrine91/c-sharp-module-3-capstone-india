using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class TransferService
    {
        private readonly string BASE_URL;
        private readonly RestClient client;
        

        public TransferService()
        {
            this.BASE_URL = AuthService.API_BASE_URL + "transfer";

            this.client = new RestClient();

            
        }
        public bool TransferTEBucks(int userID, decimal amount)

        {
            API_Transfer transfer = new API_Transfer();
            transfer.AccountTo = userID;
            transfer.Amount = amount;
            RestRequest request = new RestRequest(BASE_URL);
            request.AddJsonBody(transfer);

            IRestResponse<bool> response = client.Post<bool>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("An error response was received from the server. The status code is " + (int)response.StatusCode);

            }
            else
            {
                return response.Data;
            }
        }
    }    
}
