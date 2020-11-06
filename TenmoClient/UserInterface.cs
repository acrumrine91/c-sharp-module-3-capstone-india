﻿using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountService accountService = new AccountService();

        private bool shouldExit = false;

        public void Start()
        {
            while (!shouldExit)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1:
                            Console.Clear();
                            DisplayBalance();
                            break;
                        case 2:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 3:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 4:
                            Console.Clear();
                            DisplayAllUsers();
                            CreateNewTransfer();
                            break;
                        case 5:
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 6:
                            Console.WriteLine();
                            UserService.SetLogin(new API_User()); //wipe out previous login info
                            HandleUserLogin();
                            break;
                        default:
                            Console.WriteLine("Goodbye!");
                            shouldExit = true;
                            return;
                    }
                }
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }
        private void DisplayBalance()
        {

            API_Account account = accountService.GetBalance();
            if (account != null)
            {
                Console.WriteLine("Your account balance is " + account.Balance.ToString("C"));
            }
            else
            {
                Console.WriteLine("There was no balance available to display.");
            }
        }


        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                    this.accountService.UpdateToken(user.Token);
                    //will put the method to update token into the service class we create
                }
            }
        }
        private Dictionary<int,API_User> DisplayAllUsers()
        {

            Console.Clear();
            List<API_User> allUsers = this.accountService.GetAllUserAccounts();
            Dictionary<int, API_User> usersForDisplay = new Dictionary<int, API_User>();
            int count = 1;

            foreach (API_User user in allUsers)
            {
                Console.WriteLine(count + "   " + user.Username);
                usersForDisplay.Add(count, user);
                count++;
            }
            return usersForDisplay;

        }

        private void CreateNewTransfer()
        {
            Dictionary<int, API_User> usersForTransfer = DisplayAllUsers();
            int userSendToId = consoleService.PromptForUserIDToTransferTo();
            if (usersForTransfer.ContainsKey(userSendToId))
            {
                decimal amount = consoleService.AmountForTransfer();
                //int userTransferFrom = UserService.UserId;
                //API_Transfer transfer = new API_Transfer(userSendToId/*, userTransferFrom*/, amount);
                //transfer = accountService.TransferTEBucks(transfer);
                bool transferResult = accountService.TransferTEBucks(userSendToId, amount);
                if (transferResult)
                {
                    Console.WriteLine("Transfer Successful");
                }
                else
                {
                    Console.WriteLine("Transfer Failed");
                }
            }
        }


    }
}
