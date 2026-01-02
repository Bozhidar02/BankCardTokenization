using System.ServiceModel;
using Tokenization.Contracts;

namespace Tokenization.Client
{
    public static class ServiceClient
    {
        private static ChannelFactory<ITokenizationService> factory;

        public static ITokenizationService Create()
        {
            if (factory == null)
            {
                factory = new ChannelFactory<ITokenizationService>(
                    new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:8080/TokenizationService"));
            }

            return factory.CreateChannel();
        }
    }
}
