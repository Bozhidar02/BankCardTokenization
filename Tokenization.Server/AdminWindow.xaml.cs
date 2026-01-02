using System;
using System.IO;
using System.Linq;
using System.Windows;
using Tokenization.Common;

namespace Tokenization.Server
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            dgTokens.ItemsSource = TokenStore.Tokens;
            RefreshTokens();
        }

        private void RefreshTokens()
        {
            TokenStore.Tokens.Clear();

            foreach (var record in XmlDataStore.LoadTokens())
                TokenStore.Tokens.Add(record);

            dgTokens.Items.Refresh(); //forces the UI refresh
        }


        private void BtnExportByCard_Click(object sender, RoutedEventArgs e)
        {
            var sorted = TokenStore.Tokens
                .OrderBy(t => t.CardNumber)
                .ToList();

            ExportToFile(sorted, "Report_By_CardNumber.txt");
        }

        private void BtnExportByToken_Click(object sender, RoutedEventArgs e)
        {
            var sorted = TokenStore.Tokens
                .OrderBy(t => t.Token)
                .ToList();

            ExportToFile(sorted, "Report_By_Token.txt");
        }

        private void ExportToFile(System.Collections.Generic.List<TokenPair> data, string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string reportsFolder = Path.Combine(baseDir, "Reports");

            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string fullPath = Path.Combine(reportsFolder, fileName);

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var r in data)
                    writer.WriteLine($"{r.CardNumber} -> {r.Token}");
            }

            MessageBox.Show($"Report saved: {fullPath}");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTokens();
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RefreshTokens();
        }
    }
}
