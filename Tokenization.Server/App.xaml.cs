using System.ServiceModel;
using System.Windows;

namespace Tokenization.Server
{
    public partial class App : Application
    {
        private ServiceHost host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            host = new ServiceHost(typeof(TokenizationService));
            host.Open();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            host.Close();
            base.OnExit(e);
        }
    }
}
