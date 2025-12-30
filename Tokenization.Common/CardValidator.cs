using System.Linq;

namespace Tokenization.Common
{
    public static class CardValidator
    {
        public static bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                return false;

            // must start with 3,4,5 or 6
            char first = cardNumber[0];
            if (first != '3' && first != '4' && first != '5' && first != '6')
                return false;

            return PassesLuhn(cardNumber);
        }

        private static bool PassesLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = number[i] - '0';

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }
    }
}
