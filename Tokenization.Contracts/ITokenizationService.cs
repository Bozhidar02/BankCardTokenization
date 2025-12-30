using System.ServiceModel;

namespace Tokenization.Contracts
{
    [ServiceContract]
    public interface ITokenizationService
    {
        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        string RegisterToken(string cardNumber);

        [OperationContract]
        string ResolveToken(string token);
    }
}
