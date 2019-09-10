using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinet.Storages;

namespace FileCabinet.ConsoleUI
{
    class Program
    {
        private static List<string> command = new List<string>() { "info", "create", "list", "stat", "exit" };
        private static FileCabinet fileCabinet = new FileCabinet(new FakeUserStorage());

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the command. Enter \"info\" to see a list of possible commands:");
            bool check = true;
            while (check)
            {
                string enterCommand = Console.ReadLine();
                if (command.Contains(enterCommand))
                {
                    switch (enterCommand)
                    {
                        case "exit":
                            Environment.Exit(0);
                            break;
                        case "info":
                            ShowCommand();
                            break;
                        case "create":
                            CreateUser();
                            break;
                        case "list":
                            ShowUsers();
                            break;
                        case "stat":
                            ShowCountNumbers();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Uncorrect command");
                }
                Console.WriteLine();
            }
        }

        private static void ShowCommand()
        {
            Console.WriteLine("Valid Commands:");
            foreach(var item in command)
            {
                Console.Write($"{item}; ");
            }
            Console.WriteLine();
        }

        private static void CreateUser()
        {
            Console.Write("First name: ");
            string userName = Console.ReadLine();
            Console.Write("Last name: ");
            string userLastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            string userBirth = Console.ReadLine();
            User user = new User(userName, userLastName, userBirth);
            fileCabinet.CrateUser(user);
            Console.WriteLine($"Record #{user.ID} is created.");
        }

        private static void ShowCountNumbers()
        {
            Console.WriteLine($"{fileCabinet.CountUsers()} records.");
        }

        private static void ShowUsers()
        {
            List<User> users = fileCabinet.GetAllUsers();
            foreach(var user in users)
            {
                Console.WriteLine(user);
            }
        }
    }
}
