using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinet.Storages;

namespace FileCabinet
{
    /// <summary>
    /// Сlass to perform basic user operation
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

            List<User> users = new List<User>(storage.Load());

            foreach (var u in users)
            {
                if (u.Equals(user))
                {
                    throw new ArgumentException("User was created early");
                }
            }

            user.ID = CountUsers() + 1;
            users.Add(user);

            storage.Save(users);
        }
    }
}
