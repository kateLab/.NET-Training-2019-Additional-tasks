using System;

namespace FileCabinet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when this user does not exist in storage
    /// </summary>
    public class UserDoesNotExist : ArgumentException
    {
        public UserDoesNotExist(string message) : base(message)
        {
        }
    }
}
