﻿using TenmoClient.Data;

namespace TenmoClient
{
    public static class UserService
    {
        private static API_User user = new API_User();

        public static void SetLogin(API_User u)
        {
            user = u;
        }

        public static int UserId()
        
            
            {
                return user.UserId;
            }
        

        public static bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(user.Token);
            }
        }

        public static string GetToken()
        {
            if (user == null || user.Token == null)
            {
                return string.Empty;
            }

            return user.Token;
        }
    }
}
