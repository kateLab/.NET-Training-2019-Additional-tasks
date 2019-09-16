using System;
using System.Collections.Generic;

namespace FileCabinet.Storages
{
    public interface IUserStorage
    {
        /// <summary>
        /// Loads user from storage
        /// </summary>
        /// <returns>uploaded users list</returns>
        List<User> Load();

        /// <summary>
        /// Saves users to storage
        /// </summary>
        /// <param name="users">users for save</param>
        void Save(List<User> users);
    }
}
