﻿using System;
using System.Collections.Generic;
using System.Transactions;
using TenmoClient.Data;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(/*string action*/)
        {
            Console.WriteLine("");
            Console.Write($"Please enter transfer ID to action (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }

            return userId;
        }

        public API_Transfer PromptForTransfer()
        {
            Console.WriteLine("What is your question's text?");
            int userId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("What is your question's answer?");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            return new API_Transfer()
            {
                AccountTo = userId,
                Amount = amount,
            };
        }

        public int PromptForUserIDToTransferTo()
        {
            Console.WriteLine("");
            Console.Write("Please enter the user ID to transfer to (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }

            return userId;
        }



        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            return new LoginUser
            {
                Username = username,
                Password = password
            };
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");

            return pass;
        }

        public decimal AmountForTransfer()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter amount : ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }

            return amount;
        }

    }



}