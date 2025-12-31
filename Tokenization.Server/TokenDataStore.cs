using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Tokenization.Server
{
    public static class TokenDataStore
    {
        private static readonly object _fileLock = new object();
        private const string FileName = "tokens.xml";

        public static Dictionary<string, string> LoadTokens()
        {
            lock (_fileLock)
            {
                if (!File.Exists(FileName))
                    return new Dictionary<string, string>();

                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var list = (List<TokenPair>)new XmlSerializer(typeof(List<TokenPair>)).Deserialize(fs);
                    return list.ToDictionary(p => p.Token, p => p.CardNumber);
                }
            }
        }

        public static void SaveTokens(Dictionary<string, string> data)
        {
            lock (_fileLock)
            {
                var list = data.Select(p => new TokenPair
                {
                    Token = p.Key,
                    CardNumber = p.Value
                }).ToList();

                using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    new XmlSerializer(typeof(List<TokenPair>)).Serialize(fs, list);
                }
            }
        }
    }
}
