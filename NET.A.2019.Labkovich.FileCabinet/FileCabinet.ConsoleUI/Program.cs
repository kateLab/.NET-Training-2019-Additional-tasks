using System;
using System.Collections.Generic;
using FileCabinet.Exceptions;
using FileCabinet.Storages;

namespace FileCabinet.ConsoleUI
{
    class Program
    {
        private static List<string> command = new List<string>() { "info", "create", "list", "list", "dateBirth", "stat", "exit", "find", "edit", "import", "export", "remove", "purge" };
        private static FileCabinet fileCabinet = new FileCabinet(new FakeUserStorage());
        private static CSVUserStorage CSVstorage = new CSVUserStorage("file.csv");
        private static XMLUserStorage XMLstorage = new XMLUserStorage("fileXML.xml");

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the command. Enter \"info\" to see a list of possible commands:");
            bool check = true;
            while (check)
            {
                char[] serparator = new char[] { ' ' };
                string[] enterCommands = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    ChooseAction(enterCommands);
                }
                catch (UserExistsException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (UserDoesNotExist exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (ArgumentNullException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                Console.WriteLine();
            }
        }

        private static void ChooseAction(string[] enterCommands)
        {
            if (enterCommands.Length == 0)
            {
                throw new ArgumentException("Enter the command.");
            }
            string enterCommand = enterCommands[0];
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
                    ShowUsers(enterCommands);
                    break;
                case "stat":
                    ShowCountNumbers();
                    break;
                case "find":
                    Find(enterCommands);
                    break;
                case "edit":
                    EditUser(enterCommands);
                    break;
                case "export":
                    Export(enterCommands);
                    break;
                case "import":
                    Import(enterCommands);
                    break;
                case "remove":
                    RemoveUser(enterCommands);
                    break;
                case "purge":
                    RemoveAll();
                    break;
                default:
                    Console.WriteLine("This command is not supported.");
                    break;
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

        private static void ShowUsers(string[] showOptions)
        {
            List<User> users = fileCabinet.GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No items in storage.");
            }
            if (showOptions.Length == 1)
            {
                foreach (var user in users)
                {
                    Console.WriteLine(user);
                }
            }
            else
            {
                string userToString = "";
                foreach (var user in users)
                {
                    for (int i = 1; i < showOptions.Length; i++)
                    {
                        switch (showOptions[i].TrimEnd(new char[1] { ',' }))
                        {
                            case "id":
                                userToString += $"#{user.ID}, ";
                                break;
                            case "firstname":
                                userToString += user.FirstName + ", ";
                                break;
                            case "secondname":
                                userToString += user.SecondName + ", ";
                                break;
                            case "dateBirth":
                                userToString += user.DateOfBirth + ", ";
                                break;
                            default:
                                Console.WriteLine("This command is not supported.");
                                break;
                        }
                    }
                    userToString = userToString.Remove(userToString.Length - 2);
                    Console.WriteLine(userToString);
                    userToString = "";
                }
            }
        }

        private static void Find(string[] searchOptions)
        {
            if (searchOptions.Length == 3)
            {
                FindUserByName(searchOptions[2]);
            }
            else
            {
                if (searchOptions.Length == 5)
                {
                    FindUserByFirstAndSecond(searchOptions[2], searchOptions[4]);
                }
                else
                {
                    Console.WriteLine("Enter search parameters.");
                }
            }
        }

        #region Export and Import methods
        private static void Export(string[] searchOptions)
        {
            if (searchOptions.Length < 2)
            {
                throw new ArgumentException("Enter the type file: csv or xml.");
            }
            if (searchOptions[1] == "csv")
            {
                ExportCSV();
            }
            else
            {
                if (searchOptions[1] == "xml")
                {
                    ExportXML();
                }
                else
                {
                    Console.WriteLine("This format is not supported.");
                }
            }
        }

        private static void Import(string[] searchOptions)
        {
            if (searchOptions.Length < 2)
            {
                throw new ArgumentException("Enter the type file: csv or xml.");
            }
            if (searchOptions[1] == "csv")
            {
                ImportCsv();
            }
            else
            {
                if (searchOptions[1] == "xml")
                {
                    ImportXML();
                }
                else
                {
                    Console.WriteLine("This format is not supported.");
                }
            }
        }

        private static void ImportCsv()
        {
            List<User> users = CSVstorage.Load();
            foreach (var user in users)
            {
                fileCabinet.CrateUser(user);
            }
            Console.WriteLine("Data loaded from file \"file.csv\"");
        }

        private static void ImportXML()
        {
            List<User> users = XMLstorage.Load();
            foreach (var user in users)
            {
                fileCabinet.CrateUser(user);
            }
            Console.WriteLine("Data loaded from file \"fileXML.xml\"");
        }

        private static void ExportCSV()
        {
            List<User> users = fileCabinet.GetAllUsers();
            CSVstorage.Save(users);
            Console.WriteLine("Data exported to file \"file.csv\"");
        }

        private static void ExportXML()
        {
            List<User> users = fileCabinet.GetAllUsers();
            XMLstorage.Save(users);
            Console.WriteLine("Data exported to file \"fileXML.xml\"");
        }
        #endregion

        private static void FindUserByName(string userName)
        {
            userName = userName.Trim(new char[] { '"'});
            List<User> users = fileCabinet.FindUser(userName);
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }

        private static void FindUserByFirstAndSecond(string userName, string secondName)
        {
            userName = userName.Trim(new char[] { '"', ',' });
            secondName = secondName.Trim(new char[] { '"' });
            List<User> users = fileCabinet.FindUser(userName, secondName);
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }

        private static void EditUser(string[] userIdString)
        {
            if (userIdString.Length < 2)
            {
                throw new ArgumentException("Enter ID user");
            }
            int userID = 0;
            bool check = int.TryParse(userIdString[1].TrimStart(new char[] { '#' }), out userID);
            if (!check)
            {
                throw new ArgumentException("Wrong ID");
            }
            List<User> users = fileCabinet.GetAllUsers();
            if (users.Count < userID)
            {
                throw new UserDoesNotExist("User with this ID does nor exist in storage.");
            }
            Console.Write("First name: ");
            string userName = Console.ReadLine();
            Console.Write("Last name: ");
            string userLastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            string userBirth = Console.ReadLine();
            User user = new User(userName, userLastName, userBirth);
            fileCabinet.EditUser(userID, user);
            Console.WriteLine($"Record #{user.ID} is changed.");
        }

        private static void RemoveUser(string[] userIdString)
        {
            if (userIdString.Length == 1)
            {
                throw new ArgumentException("Enter the ID");
            }
            int userID = 0;
            bool check = int.TryParse(userIdString[1].TrimStart(new char[] { '#' }), out userID);
            if (!check)
            {
                throw new ArgumentException("Wrong ID");
            }
            int userId = int.Parse(userIdString[1].TrimStart(new char[] { '#' }));
            fileCabinet.RemoveUser(userId);
            Console.WriteLine($"Record #{userID} is removed.");
        }

        private static void RemoveAll()
        {
            fileCabinet.RemoveAll();
            Console.WriteLine("All users are clean");
        }
    }
}
