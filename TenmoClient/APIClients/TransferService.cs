﻿using RestSharp;
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
            this.BASE_URL = AuthService.API_BASE_URL + "transfers";

            this.client = new RestClient();
        }
        public API_Transfer TransferTEBucks(API_Transfer transfer)
        {
            RestRequest request = new RestRequest(BASE_URL);
            request.AddJsonBody(transfer);

            IRestResponse<API_Transfer> response = client.Post<API_Transfer>(request);

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