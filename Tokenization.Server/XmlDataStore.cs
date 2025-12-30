using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Tokenization.Common;

namespace Tokenization.Server
{
    public static class XmlDataStore
    {
        private const string UsersFile = "users.xml";
        private const string TokensFile = "tokens.xml";

        public static List<User> LoadUsers()
        {
            if (!File.Exists(UsersFile))
                return new List<User>();

            XmlSerializer ser = new XmlSerializer(typeof(List<User>));
            FileStream fs = new FileStream(UsersFile, FileMode.Open);
            return (List<User>)ser.Deserialize(fs);
        }

        public static void SaveUsers(List<User> users)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<User>));
            FileStream fs = new FileStream(UsersFile, FileMode.Create);
            ser.Serialize(fs, users);
        }

        public static List<TokenRecord> LoadTokens()
        {
            if (!File.Exists(TokensFile))
                return new List<TokenRecord>();

            XmlSerializer ser = new XmlSerializer(typeof(List<TokenRecord>));
            FileStream fs = new FileStream(TokensFile, FileMode.Open);
            return (List<TokenRecord>)ser.Deserialize(fs);
        }

        public static void SaveTokens(List<TokenRecord> tokens)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<TokenRecord>));
            FileStream fs = new FileStream(TokensFile, FileMode.Create);
            ser.Serialize(fs, tokens);
        }
    }
}
