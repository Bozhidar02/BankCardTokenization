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
        string RegisterToken(string cardNumber);

        [OperationContract]
        string ResolveToken(string token);
    }
}
