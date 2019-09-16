using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinet.Storages
{
    public class XMLUserStorage : IUserStorage
    {
        private readonly string filePath;

        public XMLUserStorage(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Loads user from storage
        /// </summary>
        /// <returns>uploaded users list</returns>
        public List<User> Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement root = doc.DocumentElement;
            List<User> users = new List<User>();

            foreach (XmlNode node in root)
            {
                int id = 0;
                string firstName = string.Empty;
                string secondName = string.Empty;
                string dateOfBirth = string.Empty;
                if (root.Attributes.Count > 0)
                {
                    XmlNode attr = node.Attributes.GetNamedItem("ID");
                    if (attr != null)
                    {
                        id = int.Parse(attr.Value);
                    }
                }

                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "FirstName")
                    {
                        firstName = child.InnerText;
                    }

                    if (child.Name == "SecondName")
                    {
                        secondName = child.InnerText;
                    }

                    if (child.Name == "DateOfBirth")
                    {
                        dateOfBirth = child.InnerText;
                    }
                }

                User user = new User(firstName, secondName, dateOfBirth)
                {
                    ID = id
                };
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Saves users to storage
        /// </summary>
        /// <param name="users">users for save</param>
        public void Save(List<User> users)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);
            XmlElement root = doc.CreateElement("FileCabinet");

            foreach (var user in users)
            {
                var userNode = doc.CreateElement("user");
                var idAtr = doc.CreateAttribute("ID");
                idAtr.InnerText = user.ID.ToString();
                userNode.Attributes.Append(idAtr);

                AddChildNode("FirstName", user.FirstName, userNode, doc);
                AddChildNode("SecondName", user.SecondName, userNode, doc);
                AddChildNode("DateOfBirth", user.DateOfBirth, userNode, doc);

                root.AppendChild(userNode);
            }

            doc.AppendChild(root);
            doc.Save(filePath);
        }

        private static void AddChildNode(string childName, string childText, XmlElement parent, XmlDocument doc)
        {
            XmlElement child = doc.CreateElement(childName);
            child.InnerText = childText;
            parent.AppendChild(child);
        }
    }
}
