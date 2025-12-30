using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Tokenization.Common;
using Tokenization.Contracts;

namespace Tokenization.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TokenizationService : ITokenizationService
    {
        private readonly List<User> users;
        private readonly List<TokenRecord> tokens;

        public TokenizationService()
        {
            users = XmlDataStore.LoadUsers();
            tokens = XmlDataStore.LoadTokens();

            if (!users.Any())
                SeedUsers();
        }

        public bool Login(string username, string password)
        {
            return users.Any(u =>
                u.Username == username &&
                u.Password == password);
        }

        public string RegisterToken(string cardNumber)
        {
            if (!CardValidator.IsValidCardNumber(cardNumber))
                return "INVALID CARD";

            if (tokens.Any(t => t.CardNumber == cardNumber))
                return "CARD ALREADY TOKENIZED";

            var existingTokens = tokens
                .Select(t => t.Token)
                .ToHashSet();

            string token = TokenGenerator.GenerateToken(cardNumber, existingTokens);

            tokens.Add(new TokenRecord
            {
                CardNumber = cardNumber,
                Token = token
            });

            XmlDataStore.SaveTokens(tokens);

            return token;
        }

        public string ResolveToken(string token)
        {
            var record = tokens.FirstOrDefault(t => t.Token == token);
            return record?.CardNumber ?? "TOKEN NOT FOUND";
        }

        private void SeedUsers()
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
