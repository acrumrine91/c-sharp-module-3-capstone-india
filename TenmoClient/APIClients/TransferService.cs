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
        public API_Transfer TransferTEBucks(API_Transfer transfer)

        {           
          
            RestRequest request = new RestRequest(BASE_URL);
            request.AddJsonBody(transfer);

            IRestResponse<API_Transfer> response = client.Post<API_Transfer>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred attempting to transfer funds");

                return null;
            }
        }
    }    
}
