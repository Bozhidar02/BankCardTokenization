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
            StreamWriter writer = new StreamWriter(fileName);

            foreach (var r in data)
                writer.WriteLine($"{r.CardNumber} -> {r.Token}");

            MessageBox.Show($"Report saved: {fileName}");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // persist tokens on server shutdown
            XmlDataStore.SaveTokens(TokenStore.Tokens.ToList());
        }

    }
}
