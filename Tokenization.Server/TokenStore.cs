using System.Collections.ObjectModel;

namespace Tokenization.Server
{
    public static class TokenStore
    {
        public static ObservableCollection<TokenPair> Tokens { get; }
            = new ObservableCollection<TokenPair>();

        public static void Add(string token, string card)
        {
            Tokens.Add(new TokenPair
            {
                Token = Format(token),
                CardNumber = Format(card)
            });
        }

        private static string Format(string value)
        {
            return string.Join(" ",
                new[]
                {
                    value.Substring(0,4),
                    value.Substring(4,4),
                    value.Substring(8,4),
                    value.Substring(12,4)
                });
        }
    }
}
