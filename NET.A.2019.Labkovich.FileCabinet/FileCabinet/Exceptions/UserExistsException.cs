using System;

namespace FileCabinet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when this user is already in storage
    /// </summary>
    public class UserExistsException : ArgumentException
    {
        public UserExistsException(string message) : base(message)
        {
        }
    }
}
