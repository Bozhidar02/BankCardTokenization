using System.ServiceModel;
using Tokenization.Common;

namespace Tokenization.Contracts
{
    [ServiceContract]
    public interface ITokenizationService
    {
        [OperationContract]
        UserRole? Login(string username, string password);


        [OperationContract]
        TokenResponse RegisterToken(string cardNumber);

        [OperationContract]
        string ResolveToken(string token);
    }
}
