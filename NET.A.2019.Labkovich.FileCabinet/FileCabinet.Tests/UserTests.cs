using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileCabinet.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void UserTests_CreateNewUser_ToString()
        {
            User user = new User("Ivan", "Ivanov", "05/05/1999");
            Assert.AreEqual("#0, Ivan, Ivanov, 05/05/1999", user.ToString());
        }

        [Test]
        public void UserTests_CreateUser_EqualsSameUser()
        {
            User user = new User("Ivan", "Ivanov", "05/05/1999");
            User equalIser = new User("Ivan", "Ivanov", "05/05/1999");
            Assert.IsTrue(user.Equals(equalIser));
        }

        [Test]
        public void UserTests_CreateUser_EqualsUser()
        {
            User user = new User("Ivan", "Ivanov", "05/05/1999");
            User equalIser = new User("Ivan", "Petrov", "05/05/1999");
            Assert.IsFalse(user.Equals(equalIser));
        }

        [Test]
        public void UserTests_GetHashCode_WithEqualUser()
        {
            User user = new User("Ivan", "Ivanov", "05/05/1999");
            User equalIser = new User("Ivan", "Ivanov", "05/05/1999");
            Assert.AreEqual(user.GetHashCode(), equalIser.GetHashCode());
        }

        [Test]
        public void UserTests_GetHashCode_WithNotEqualUser()
        {
            User user = new User("Ivan", "Ivanov", "05/05/1999");
            User equalIser = new User("Ivan", "Petrov", "05/05/1999");
            Assert.AreNotEqual(user.GetHashCode(), equalIser.GetHashCode());
        }

        [Test]
        public void UserTests_CreateUserWithNull_ThrowArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new User(null, "Abcd", "11/05/1999"));

        [Test]
        public void UserTests_CreateUserWithUncorrectName_ThrowArgumentException()
            => Assert.Throws<ArgumentException>(() => new User("Ab", "Abcd", "11/05/1999"));

        [Test]
        public void UserTests_CreateUserWithUncorrectData_ThrowArgumentException()
            => Assert.Throws<ArgumentException>(() => new User("Abcd", "Abcd", "21.21.1999"));
    }
}
