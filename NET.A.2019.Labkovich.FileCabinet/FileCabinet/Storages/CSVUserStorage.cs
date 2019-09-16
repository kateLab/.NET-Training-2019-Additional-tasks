using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace FileCabinet.Storages
{
    public class CSVUserStorage : IUserStorage
    {
        private readonly string filePath;

        public CSVUserStorage(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Loads user from storage
        /// </summary>
        /// <returns>uploaded users list</returns>
        public List<User> Load()
        {
            List<User> users = new List<User>();

            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    using (CsvReader csvReader = new CsvReader(streamReader, new CsvHelper.Configuration.Configuration { HasHeaderRecord = false }, false))
                    {
                        csvReader.Configuration.HasHeaderRecord = false;
                        csvReader.Configuration.MissingFieldFound = null;
                        csvReader.Configuration.Delimiter = ";";
                        while (csvReader.Read())
                        {
                            int id = int.Parse(csvReader.GetField(0));
                            string firstName = csvReader.GetField(1);
                            string secondName = csvReader.GetField(2);
                            string dateOfBirth = csvReader.GetField(3);
                            dateOfBirth = dateOfBirth.Replace('.', '/');
                            User user = new User(firstName, secondName, dateOfBirth)
                            {
                                ID = id
                            };
                            if (!users.Contains(user))
                            {
                                users.Add(user);
                            }
                        }

                        return users;
                    }
                }
            }
        }

        /// <summary>
        /// Saves users to storage
        /// </summary>
        /// <param name="users">users for save</param>
        public void Save(List<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException("Users must be not null");
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, new CsvHelper.Configuration.Configuration { HasHeaderRecord = false }, false))
                    {
                        csvWriter.Configuration.Delimiter = ";";
                        csvWriter.WriteRecords(users);
                        csvWriter.Dispose();
                    }

                    streamWriter.Close();
                }
            }
        }
    }
}
