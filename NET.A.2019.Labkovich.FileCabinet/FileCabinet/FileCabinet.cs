using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinet.Exceptions;
using FileCabinet.Storages;

namespace FileCabinet
{
    /// <summary>
    /// Represents basic user operation
    /// </summary>
    public class FileCabinet
    {
        private IUserStorage storage;

        /// <summary>
        /// Constructor for file cabinet class
        /// </summary>
        /// <param name="storage">storage for users</param>
        public FileCabinet(IUserStorage storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Gets a list with all users in storage
        /// </summary>
        /// <returns>list with all users</returns>
        public List<User> GetAllUsers()
        {
            return storage.Load();
        }

        /// <summary>
        /// Returns the number of users
        /// </summary>
        /// <returns>number of users</returns>
        public int CountUsers()
        {
            List<User> temp = GetAllUsers();
            return temp.Count;
        }

        /// <summary>
        /// Creates new storage entry
        /// </summary>
        /// <param name="user">user for add</param>
        public void CrateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User must be not null");
            }

            List<User> users = new List<User>(GetAllUsers());

            foreach (var u in users)
            {
                if (u.FirstName == user.FirstName && u.SecondName == user.SecondName && u.DateOfBirth == user.DateOfBirth)
                {
                    throw new UserExistsException("This user already created.");
                }
            }

            user.ID = CountUsers() + 1;
            users.Add(user);
            storage.Save(users);
        }

        /// <summary>
        /// Finds user by first name
        /// </summary>
        /// <param name="firstName">name to search</param>
        /// <returns>list users</returns>
        public List<User> FindUser(string firstName)
        {
            ValidateString(firstName);

            List<User> users = new List<User>(storage.Load());
            if (users.Find(us => us.FirstName == firstName) == null)
            {
                throw new UserDoesNotExist("User with this first name does not exist in storage.");
            }

            List<User> findUsers = users.Where(us => us.FirstName == firstName).ToList();
            return findUsers;       
        }

        /// <summary>
        /// Finds user by first name and second name
        /// </summary>
        /// <param name="firstName">first name to search</param>
        /// <param name="secondName">second name to search</param>
        /// <returns>list users</returns>
        public List<User> FindUser(string firstName, string secondName)
        {
            List<User> users = new List<User>(storage.Load());

            if (users.Find(us => us.FirstName == firstName && us.SecondName == secondName) == null)
            {
                throw new UserExistsException("User with this peremeters does not exist in storage.");
            }

            List<User> findUsers = users.Where(us => us.FirstName == firstName && us.SecondName == secondName).ToList();
            return findUsers;
        }

        /// <summary>
        /// Edits user with this ID
        /// </summary>
        /// <param name="userID">ID user</param>
        /// <param name="newUser">user after editing</param>
        public void EditUser(int userID, User newUser)
        {
            ValidateID(userID);

            List<User> users = new List<User>(storage.Load());

            if (userID > users.Count)
            {
                throw new UserDoesNotExist("User with this ID does nor exist in storage.");
            }

            if (users.Find(us => us.ID == userID) == null)
            {
                throw new UserDoesNotExist("User with this ID does not exist in storage.");
            }

            User userToEdit = FindUser(userID)[0];
            users.Remove(userToEdit);
            newUser.ID = userID;
            users.Add(newUser);
            users.Sort();
            storage.Save(users);
        }

        /// <summary>
        /// Finds user by ID
        /// </summary>
        /// <param name="userID">ID user</param>
        /// <returns>List users</returns>
        public List<User> FindUser(int userID)
        {
            ValidateID(userID);

            List<User> users = new List<User>(storage.Load());

            if (userID > users.Count)
            {
                throw new UserDoesNotExist("User with this ID does nor exist in storage.");
            }

            List<User> findUsers = users
                .Where(us => us.ID == userID).ToList();
            return findUsers;
        }

        /// <summary>
        /// Removes the user with the given ID from the storage
        /// </summary>
        /// <param name="userID">ID user to removing</param>
        public void RemoveUser(int userID)
        {
            ValidateID(userID);

            List<User> users = new List<User>(storage.Load());

            if (userID > users.Count)
            {
                throw new UserDoesNotExist("User with this ID does not exist in storage.");
            }

            User userToRemove = users.Find(us => us.ID == userID);
            users.Remove(userToRemove);

            foreach (var user in users)
            {
                user.ID = users.IndexOf(user) + 1;
            }
            
            storage.Save(users);
        }

        /// <summary>
        /// Cleans storage
        /// </summary>
        public void RemoveAll()
        {
            List<User> users = new List<User>(storage.Load());
            users.Clear();
            storage.Save(users);
        }

        #region Validate methods
        private static void ValidateID(int userID)
        {
            if (userID < 0)
            {
                throw new ArgumentException("ID must be more than zero.");
            }
        }

        private static void ValidateString(string sourceString)
        {
            if (sourceString == null)
            {
                throw new ArgumentNullException("String must be not null");
            }

            if (sourceString == string.Empty)
            {
                throw new ArgumentException("String must contain elements");
            }
        }
        #endregion
    }
}
