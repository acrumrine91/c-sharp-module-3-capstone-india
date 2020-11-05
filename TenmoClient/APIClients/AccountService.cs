using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
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

        public void UpdateToken (string token)
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

        public List<API_Account> GetAllAccounts()
        {
            RestRequest request = new RestRequest(BASE_URL);

            IRestResponse<List<API_Account>> response = client.Get<List<API_Account>>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching questions");

                return new List<API_Account>();
            }
        }

        public API_Account GetBalance()
        {
            RestRequest request = new RestRequest(BASE_URL + "/1");

            var response = client.Get<API_Account>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occured fetching balance");

                return null;
            }
        } 
    }
}
