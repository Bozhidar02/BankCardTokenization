using System;
using System.Collections.Generic;
using System.Linq;

namespace Tokenization.Common
{
    public static class TokenGenerator
    {
        private static readonly Random rnd = new Random();

        public static string GenerateToken(
            string cardNumber,
            HashSet<string> existingTokens)
        {
            if (!CardValidator.IsValidCardNumber(cardNumber))
                throw new ArgumentException("Invalid card number");

            string lastFour = cardNumber.Substring(12, 4);
            string token;

            do
            {
                token = GenerateCandidate(lastFour);
            }
            while (!IsValidToken(token, existingTokens));

            return token;
        }

        private static string GenerateCandidate(string lastFour)
        {
            char firstDigit;
            do
            {
                firstDigit = (char)('0' + rnd.Next(1, 10));
            }
            while (firstDigit == '3' || firstDigit == '4' ||
                   firstDigit == '5' || firstDigit == '6');

            char[] digits = new char[16];
            digits[0] = firstDigit;

            for (int i = 1; i < 12; i++)
                digits[i] = (char)('0' + rnd.Next(0, 10));

            for (int i = 0; i < 4; i++)
                digits[12 + i] = lastFour[i];

            return new string(digits);
        }

        private static bool IsValidToken(string token, HashSet<string> existingTokens)
        {
            if (existingTokens.Contains(token))
                return false;

            int sum = token.Sum(c => c - '0');
            if (sum % 10 == 0)
                return false;

            return true;
        }
    }
}
