﻿using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.APIClients;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountService accountService = new AccountService();
        //private readonly TransferService transferService = new TransferService();

        private bool shouldExit = false;

        public void Start()
        {
            while (!shouldExit)
            {
                while (!UserService.IsLoggedIn)
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
            Console.WriteLine();
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine();
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
                Console.WriteLine();
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests - (Coming Soon!)");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks - (Coming Soon!)");
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
                            Console.Clear();
                            DisplayAllTransferForUser();
                            DisplayTransferDetails();
                            Console.WriteLine();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Look for this feature in a future update!");
                            Console.WriteLine();
                            break;
                        case 4:
                            Console.Clear();
                            DisplayAllUsers();
                            PromptForTransfer();
                            Console.WriteLine();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Look for this feature in a future update!");
                            Console.WriteLine();
                            break;
                        case 6:
                            Console.WriteLine();
                            UserService.SetLogin(new API_User()); //wipe out previous login info
                            this.accountService.UpdateToken(null);
                            return;
                        case 0:
                            Console.WriteLine("Goodbye!");
                            shouldExit = true;
                            return;
                        default:
                            Console.WriteLine("That's not a valid choice. Try again.");
                            break;
                    }
                }
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please register your account:");
                Console.WriteLine();
                LoginUser registerUser = consoleService.PromptForLogin();

                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
            Console.WriteLine();
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please login:");
                Console.WriteLine();

                LoginUser loginUser = consoleService.PromptForLogin();

                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                    this.accountService.UpdateToken(user.Token);
                    Console.Clear();

                    //will put the method to update token into the service class we create
                }
            }
        }

        private void DisplayBalance()
        {
            decimal balance = accountService.GetBalance();

            //if (balance != null)
            //{
            Console.WriteLine();
            Console.WriteLine("Your account balance is: " + balance.ToString("C"));
            Console.WriteLine();
            //}
            //else
            //{
            //    Console.WriteLine("There was no balance available to display.");
            //}
        }

        private void DisplayAllUsers()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID\t\tName");
            Console.WriteLine("-------------------------------------------");

            List<API_User> allUsers = accountService.GetAllUserAccounts();

            foreach (API_User user in allUsers)
            {
                if (user.UserId != UserService.UserId())
                {
                    Console.WriteLine(user.UserId + "\t\t" + user.Username);
                }
            }
        }


        public void PromptForTransfer()
        {
            API_Transfer transfer = new API_Transfer();
            transfer.AccountFrom = UserService.UserId();
            List<API_User> allUsers = accountService.GetAllUserAccounts();
            List<int> userIDs = new List<int>();
            foreach (API_User user in allUsers)
            {
                userIDs.Add(user.UserId);
            }

            Console.WriteLine("---------");
            Console.WriteLine();
            Console.Write("Enter ID of user you are sending to (0 to cancel): ");
            int selection = -1;
            if (!int.TryParse(Console.ReadLine(), out selection))
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please enter only a number.");
                return;
            }
            if (selection == 0)
            {
                return;
            }
            transfer.AccountTo = selection;
            if (!userIDs.Contains(transfer.AccountTo))
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please enter a valid user ID to transfer to!");
                return;

            }
            else if (transfer.AccountFrom == transfer.AccountTo)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Cannot transfer funds to yourself!");
                return;

            }

            Console.Write("Enter amount: ");
            decimal transferAmount = -1;
            if (!decimal.TryParse(Console.ReadLine(), out transferAmount))
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please enter only valid dollar amount.");
                return;

            }
            transfer.Amount = transferAmount;

            decimal balance = accountService.GetBalance();

            if (this.accountService.TransferTEBucks(transfer) != null)
            {
                if (balance < transfer.Amount)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Insufficient funds for transfer");
                }

                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Transfer Complete!");
                    Console.WriteLine("You have sent " + transfer.Amount.ToString("C") + " to User " + transfer.AccountTo);
                }
            }
        }

        private void DisplayAllTransferForUser()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("ID\t\tFrom/To\t\t\tAmount");
            Console.WriteLine("------------------------------------------------");
            List<string> allTransfers = accountService.GetAllTransfersForUser();
            foreach (string transferListing in allTransfers)
            {
                Console.WriteLine(transferListing);
            }
        }

        public void DisplayTransferDetails()
        {
            Console.WriteLine();
            Console.Write($"Please enter transfer ID to see more details (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine(), out int transferID))
            {
               
                Console.WriteLine();
                //Console.WriteLine("Invalid input. Only input a number.");

            }
            else if (transferID == 0)
            {
                Console.Clear();
                return;
            }

            Dictionary<string, string> transferDetail = accountService.GetTransferByID(transferID);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Transfer Details *No details with invalid entry!");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine();
            foreach (KeyValuePair<string, string> kvp in transferDetail)
            {
                if (transferDetail.ContainsValue(transferID.ToString()))
                {
                    Console.Write(kvp.Key);
                    Console.WriteLine(kvp.Value);
                }

            }

        }
    }
}


