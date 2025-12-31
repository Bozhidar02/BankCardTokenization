using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Tokenization.Common;
using Tokenization.Contracts;

namespace Tokenization.Server
{
    public class TokenizationService : ITokenizationService
    {
        // persistent storage
        private static Dictionary<string, string> tokenMap;
        private static List<User> users;

        static TokenizationService()
        {
            tokenMap = TokenDataStore.LoadTokens();
        }

        public TokenizationService()
        {
            if (users == null)
            {
                users = XmlDataStore.LoadUsers();

                if (users.Count == 0)
                {
                    users.Add(new User
                    {
                        Username = "admin",
                        Password = "1234",
                        Role = UserRole.RegisterToken
                    });

                    users.Add(new User
                    {
                        Username = "reader",
                        Password = "1234",
                        Role = UserRole.ResolveToken
                    });

                    XmlDataStore.SaveUsers(users);
                }
            }
        }

        public UserRole? Login(string username, string password)
        {
            var user = users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);

            return user?.Role;
        }

        public string RegisterToken(string cardNumber)
        {
            string normalizedCard = Normalize(cardNumber);

            if (!IsValid(normalizedCard))
                throw new FaultException("Invalid card number");

            string token = GenerateNumericToken();

            tokenMap[token] = normalizedCard;
            TokenDataStore.SaveTokens(tokenMap);

            TokenStore.Add(token, normalizedCard);

            return Format(token);
        }

        public string ResolveToken(string token)
        {
            string normalizedToken = Normalize(token);

            if (!tokenMap.ContainsKey(normalizedToken))
                return null;

            return Format(tokenMap[normalizedToken]);
        }

        // ===== helper methods (EXPLICIT) =====

        private static bool IsValid(string value)
        {
            return value.Length == 16 && value.All(char.IsDigit);
        }

        private static string Normalize(string value)
        {
            return value.Replace(" ", "");
        }

        private static string Format(string value)
        {
            return string.Join(" ",
                Enumerable.Range(0, 4)
                          .Select(i => value.Substring(i * 4, 4)));
        }

        private static string GenerateNumericToken()
        {
            Random rnd = new Random();
            return string.Concat(Enumerable.Range(0, 16)
                .Select(_ => rnd.Next(0, 10)));
        }
    }
}
