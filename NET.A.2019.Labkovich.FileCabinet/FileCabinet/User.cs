using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinet
{
    /// <summary>
    /// User class
    /// </summary>
    public class User : IEquatable<User>
    {
        private string firstName;

        private string secondName;

        private string dateOfBirth;

        /// <summary>
        /// Constructor dor user
        /// </summary>
        /// <param name="firstName">first name</param>
        /// <param name="secondName">second name</param>
        /// <param name="dateOfBirth">date of birth in format mm/dd/yyyy</param>
        public User(string firstName, string secondName, string dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            FirstName = firstName;
            SecondName = secondName;
        }

        #region Properties
        public int ID { get; set; }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                ValidateString(value);
                this.firstName = value;
            }
        }

        public string SecondName
        {
            get
            {
                return this.secondName;
            }
            set
            {
                ValidateString(value);
                this.secondName = value;
            }
        }

        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                ValidateString(value);
                ValidateData(value);
                this.dateOfBirth = value;
            }
        }
        #endregion

        #region Overrides methods
        /// <summary>
        /// Compares two users
        /// </summary>
        /// <param name="otherUser">user for comparison</param>
        /// <returns>true, if books are equal</returns>
        public bool Equals(User otherUser)
        {
            if (otherUser == null)
            {
                return false;
            }
            if (ReferenceEquals(this, otherUser))
            {
                return true;
            }
            if (GetHashCode() != otherUser.GetHashCode())
            {
                return false;
            }
            return (FirstName == otherUser.FirstName && SecondName == otherUser.SecondName && DateOfBirth == otherUser.DateOfBirth);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the user
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>true, if object is equal to user</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return this.Equals(obj as User);
        }

        /// <summary>
        /// Get hash code of the instance user
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return this.FirstName.GetHashCode() + this.SecondName.GetHashCode() + this.ID.GetHashCode();
        }

        /// <summary>
        /// Represents a book as a string
        /// </summary>
        /// <returns>user string representation</returns>
        public override string ToString()
        {
            return $"#{ID}, {FirstName}, {SecondName}, {DateOfBirth}";
        }
        #endregion

        #region Validate methods
        private void ValidateString(string stringForCheck)
        {
            if (stringForCheck == null)
            {
                throw new ArgumentNullException("String must be not null");
            }
            if (stringForCheck.Length < 3)
            {
                throw new ArgumentException("String must contain first name, or second name, or date");
            }
        }

        private void ValidateData(string dataForCheck)
        {
            bool isValid = DateTime.TryParseExact(
                dataForCheck,
                "mm/dd/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dataTime);

            if (!isValid)
            {
                throw new ArgumentException("Date of birth must be in format mm/dd/yyyy");
            }
        }
        #endregion
    }
}
