using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Tokenization.Server
{
    public static class TokenDataStore
    {
        private const string FileName = "tokens.xml";

        public static Dictionary<string, string> LoadTokens()
        {
            if (!File.Exists(FileName))
                return new Dictionary<string, string>();

            FileStream fs = new FileStream(FileName, FileMode.Open);
            var list = (List<TokenPair>)new XmlSerializer(
                typeof(List<TokenPair>)).Deserialize(fs);

            return list.ToDictionary(p => p.Token, p => p.CardNumber);
        }

        public static void SaveTokens(Dictionary<string, string> data)
        {
            var list = data.Select(p => new TokenPair
            {
                Token = p.Key,
                CardNumber = p.Value
            }).ToList();

            FileStream fs = new FileStream(FileName, FileMode.Create);
            new XmlSerializer(typeof(List<TokenPair>))
                .Serialize(fs, list);
        }
    }
}
