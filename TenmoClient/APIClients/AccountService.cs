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
        private readonly string token;
        

        public AccountService()
        {
            this.BASE_URL = AuthService.API_BASE_URL + "accounts";

            this.client = new RestClient();

            //token = UserService.GetToken();

            
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
            RestRequest request = new RestRequest(BASE_URL + "/transfer");

            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occured getting all users.");

                return new List<API_User>();
            }
        }


        public API_Account GetBalance()
        {

            RestRequest request = new RestRequest(BASE_URL + "/balance");

            IRestResponse<API_Account> response = client.Get<API_Account>(request);

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

        public API_Transfer TransferTEBucks(API_Transfer transfer)
        {
            RestRequest request = new RestRequest(BASE_URL + "/transfer");

            request.AddJsonBody(transfer);

            IRestResponse<API_Transfer> response = client.Post<API_Transfer>(request);

            return response.Data;
        }

     
    }
}
