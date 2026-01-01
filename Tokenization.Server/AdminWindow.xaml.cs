using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Tokenization.Common;

namespace Tokenization.Server
{
    public partial class AdminWindow : Window
    {
        private readonly List<TokenPair> tokens;

        public AdminWindow()
        {
            InitializeComponent();

            tokens = XmlDataStore.LoadTokens();
            dgTokens.ItemsSource = TokenStore.Tokens;

            foreach (var record in XmlDataStore.LoadTokens())
            {
                TokenStore.Tokens.Add(record);
            }
        }

        private void BtnExportByCard_Click(object sender, RoutedEventArgs e)
        {
            var sorted = tokens.OrderBy(t => t.CardNumber).ToList();
            ExportToFile(sorted, "Report_By_CardNumber.txt");
        }

        private void BtnExportByToken_Click(object sender, RoutedEventArgs e)
        {
            var sorted = tokens.OrderBy(t => t.Token).ToList();
            ExportToFile(sorted, "Report_By_Token.txt");
        }

        private void ExportToFile(List<TokenPair> data, string fileName)
        {
            // Define a folder relative to your project
            string repoRoot = AppDomain.CurrentDomain.BaseDirectory; // base folder of running app
            string reportsFolder = Path.Combine(repoRoot, "Reports");

            // Ensure the folder exists
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            string fullPath = Path.Combine(reportsFolder, fileName);

            // Use 'using' to ensure the file is properly written and closed
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var r in data)
                    writer.WriteLine($"{r.CardNumber} -> {r.Token}");
            }

            MessageBox.Show($"Report saved: {fullPath}");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // persist tokens on server shutdown
            //XmlDataStore.SaveTokens(TokenStore.Tokens.ToList());
        }

    }
}
