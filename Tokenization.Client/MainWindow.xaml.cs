using System.Linq;
using System.Windows;
using Tokenization.Common;
using Tokenization.Contracts;

namespace Tokenization.Client
{
    public partial class MainWindow : Window
    {
        private ITokenizationService client;
        private bool loggedIn = false;
        private UserRole? currentRole = null;

        public MainWindow()
        {
            InitializeComponent();
            client = ServiceClient.Create();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            currentRole = client.Login(txtUsername.Text, txtPassword.Password);

            if (currentRole == null)
            {
                txtResult.Text = "Login failed";
                return;
            }

            txtResult.Text = $"Logged in as {currentRole}";

            btnRegister.IsEnabled = currentRole == UserRole.RegisterToken;
            btnResolve.IsEnabled = currentRole == UserRole.ResolveToken;
        }



        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (currentRole != UserRole.RegisterToken)
            {
                txtResult.Text = "Access denied";
                return;
            }

            string cardNumber = txtCardNumber.Text.Trim();

            var result = client.RegisterToken(txtCardNumber.Text);

            if (!result.Success)
            {
                txtResult.Text = result.Message;
                return;
            }

            txtResult.Text = $"Token:\n{result.Token}";
        }



        private void BtnResolve_Click(object sender, RoutedEventArgs e)
        {
            if (currentRole != UserRole.ResolveToken)
            {
                txtResult.Text = "Access denied";
                return;
            }

            string token = txtToken.Text.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                txtResult.Text = "Please enter a token.";
                return;
            }

            // Normalize token before sending to server
            string normalizedToken = NormalizeToken(token);

            string card = client.ResolveToken(normalizedToken);

            if (card == null)
                txtResult.Text = "Token not found.";
            else
                txtResult.Text = $"Card number:\n{card}";
        }

        // Helper method to remove spaces from token
        private string NormalizeToken(string token)
        {
            return token.Replace(" ", "");
        }



        private bool IsValidCardNumber(string card)
        {
            return card.Length == 16 && card.All(char.IsDigit);
        }


    }
}
