using System.Runtime.Serialization;


namespace Tokenization.Contracts
{
    [DataContract]
    public class TokenResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

}
