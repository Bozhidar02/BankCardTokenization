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

        public TokenResponse RegisterToken(string cardNumber)
        {
            string normalized = Normalize(cardNumber);

            if (!IsValid(normalized))
            {
                return new TokenResponse
                {
                    Success = false,
                    Message = "Invalid card number."
                };
            }

            string token = GenerateNumericToken(normalized);
            tokenMap[token] = normalized;
            TokenDataStore.SaveTokens(tokenMap);

            return new TokenResponse
            {
                Success = true,
                Token = token,
                Message = "Token generated successfully."
            };
        }


        public string ResolveToken(string token)
        {
            // Normalize input token
            
            string normalizedToken = Normalize(token);
            string cardNumber;
            // Check if the normalized token exists in the map
            if (!tokenMap.TryGetValue(normalizedToken, out cardNumber) && !tokenMap.TryGetValue(token, out cardNumber))
                return null;

            // Format the card number for display only
            return Format(cardNumber);
        }


        // ===== helper methods (EXPLICIT) =====

        private static bool IsValid(string value)
        {
            value = Normalize(value);

            if (value.Length != 16) { 
                return false;
            }

            // Must start with 3, 4, 5, or 6
            if (!"3456".Contains(value[0])) { 
                return false;
            }

            return PassesLuhn(value);
        }


        private static bool PassesLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = number[i] - '0';

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }



        private static string Normalize(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }


        private static string Format(string value)
        {
            return string.Join(" ",
                Enumerable.Range(0, 4)
                          .Select(i => value.Substring(i * 4, 4)));
        }

        private static string GenerateNumericToken(string cardNumber)
        {
            int[] token = new int[16];
            int sum;

            do
            {
                sum = 0;

                // ---- FIRST DIGIT (must NOT be 3,4,5,6) ----
                int[] allowedFirstDigits = { 0, 1, 2, 7, 8, 9 };
                token[0] = allowedFirstDigits[random.Next(allowedFirstDigits.Length)];
                sum += token[0];

                // ---- NEXT 11 DIGITS (random, must differ from card) ----
                for (int i = 1; i < 12; i++)
                {
                    int digit;
                    do
                    {
                        digit = random.Next(0, 10);
                    }
                    while (digit == (cardNumber[i] - '0'));

                    token[i] = digit;
                    sum += digit;
                }

                // ---- LAST 4 DIGITS (must match card) ----
                for (int i = 12; i < 16; i++)
                {
                    token[i] = cardNumber[i] - '0';
                    sum += token[i];
                }

            } while (sum % 10 == 0); // must NOT be divisible by 10

            return string.Concat(token);
        }

    }
}
