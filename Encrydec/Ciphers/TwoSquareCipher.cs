namespace Encrydec.Ciphers
{
    public class TwoSquareCipher
    {
        private readonly string _message;
        private readonly string _key;
        
        public TwoSquareCipher(string message, string key)
        {
            _message = message;
            _key = key;
        }
        
        public static bool CheckKey(string key, int messageLength)
        {
            var result = false;

            return result;
        }

        public string EncryptedMessage
        {
            get
            {
                return "";
            }
        }
        
        public string DecryptedMessage
        {
            get
            {
                return "";
            }
        }
    }
}