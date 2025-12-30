using System.Windows;
using Tokenization.Contracts;

namespace Tokenization.Client
{
    public partial class MainWindow : Window
    {
        private ITokenizationService client;
        private bool loggedIn = false;

        public MainWindow()
        {
            InitializeComponent();
            client = new ServiceClient.Create();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            loggedIn = client.Login(txtUsername.Text, txtPassword.Password);

            txtResult.Text = loggedIn
                ? "Login successful"
                : "Login failed";
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                txtResult.Text = "Please login first";
                return;
            }

            string result = client.RegisterToken(txtInput.Text);
            txtResult.Text = result;
        }

        private void BtnResolve_Click(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                txtResult.Text = "Please login first";
                return;
            }

            string result = client.ResolveToken(txtInput.Text);
            txtResult.Text = result;
        }
    }
}
