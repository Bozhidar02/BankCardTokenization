using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using Tokenization.Common;
using Tokenization.Contracts;

namespace Tokenization.Server
{
    public class TokenizationService : ITokenizationService
    {
        // persistent storage
        private static Dictionary<string, string> tokenMap;
        private static List<User> users;
        private static readonly Random random = new Random();

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

            return token;//Format(token);
        }

        public string ResolveToken(string token)
        {
            // Normalize input token
            
            string normalizedToken = Normalize(token);
            MessageBox.Show("Token number: " + normalizedToken);
            string cardNumber;
            // Check if the normalized token exists in the map
            if (!tokenMap.TryGetValue(normalizedToken, out cardNumber) && !tokenMap.TryGetValue(token, out cardNumber))
                return null;

            // Format the card number for display only
            MessageBox.Show("Token number: " + Format(cardNumber));
            return Format(cardNumber);
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
            var sum = 0;
            int[] allowedNums = { 1, 2, 7, 8, 9 };
            int[] generatedNumbers = new int[16];
            do
            {
                int position = random.Next(0, 5);
                generatedNumbers[0] = allowedNums[position];
                sum = allowedNums[position];
                //string result = allowedNums[position];
                for (int i = 1; i < 16; i++)
                {
                    generatedNumbers[i] = random.Next(0, 10);
                    sum += generatedNumbers[i];
                }
            } while (sum % 10 != 0);
            return string.Join("", generatedNumbers);
            //return result + string.Concat(Enumerable.Range(0, 15)
            //    .Select(_ => random.Next(0, 10)));

        }
    }
}
