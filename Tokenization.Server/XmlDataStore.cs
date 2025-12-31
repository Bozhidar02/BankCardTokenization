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

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<User>));
                FileStream fs = new FileStream(UsersFile, FileMode.Open);
                return (List<User>)ser.Deserialize(fs);
            }
            catch
            {
                // users.xml exists but is invalid or empty
                File.Delete(UsersFile);
                return new List<User>();
            }
        }

        public static void SaveUsers(List<User> users)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<User>));
            FileStream fs = new FileStream(UsersFile, FileMode.Create);
            ser.Serialize(fs, users);
        }

        public static List<TokenPair> LoadTokens()
        {
            if (!File.Exists(TokensFile))
                return new List<TokenPair>();

            XmlSerializer ser = new XmlSerializer(typeof(List<TokenPair>));

            try
            {
                using (FileStream fs = new FileStream(TokensFile, FileMode.Open, FileAccess.Read))
                {
                    return (List<TokenPair>)ser.Deserialize(fs);
                }
            }
            catch
            {
                // at this point the FileStream is CLOSED
                File.Delete(TokensFile);
                return new List<TokenPair>();
            }
        }


        public static void SaveTokens(List<TokenPair> tokens)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<TokenPair>));
            FileStream fs = new FileStream(TokensFile, FileMode.Create);
            ser.Serialize(fs, tokens);
        }
    }
}
