using System;
using System.Collections.Generic;

namespace FileCabinet.Storages
{
    /// <summary>
    /// Implenting working with memory 
    /// </summary>
    public class FakeUserStorage : IUserStorage
    {
        private static List<User> users = new List<User>();

        /// <summary>
        /// Saves users to storage
        /// </summary>
        /// <param name="users">users for save</param>
        public void Save(List<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException("List must be not null");
            }

            List<User> tempList = new List<User>();

            foreach (var user in users)
            {
                if (user == null)
                {
                    throw new ArgumentException("Elements of list must be not null");
                }

                tempList.Add(user);
            }

            FakeUserStorage.users = tempList;
        }

        /// <summary>
        /// Loads user from storage
        /// </summary>
        /// <returns>uploaded users list</returns>
        public List<User> Load()
        {
            return users;
        }
    }
}
