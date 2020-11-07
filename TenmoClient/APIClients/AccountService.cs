using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using TenmoClient.Data;


namespace TenmoClient.APIClients
{
    public class AccountService
    {
        private readonly string BASE_URL;
        private readonly RestClient client;


        public AccountService()
        {
            this.BASE_URL = AuthService.API_BASE_URL + "accounts";

            this.client = new RestClient();


        }

        public void UpdateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                this.client.Authenticator = null;
            }
            else
            {
                this.client.Authenticator = new JwtAuthenticator(token);
            }
        }
        public List<API_User> GetAllUserAccounts()
        {
            RestRequest request = new RestRequest(BASE_URL + "/allusers");

            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occured getting all users.");

                //return new List<API_User>();
                return null;
            }
        }


        public decimal GetBalance()
        {

            RestRequest request = new RestRequest(BASE_URL + "/balance");

            IRestResponse<decimal> response = client.Get<decimal>(request);

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
