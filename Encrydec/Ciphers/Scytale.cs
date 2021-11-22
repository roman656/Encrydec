namespace Encrydec.Ciphers
{
    public class Scytale
    {
        private readonly string _message;
        private readonly int _key;
        
        public Scytale(string message, int key)
        {
            _message = message;
            _key = key;
        }

        private int GetColumnsAmount()
        {
            int columnsAmount;
                
            if (_message.Length % _key == 0)
            {
                columnsAmount = _message.Length / _key;
            }
            else
            {
                columnsAmount = (_message.Length + _key - _message.Length % _key) / _key;
            }

            return columnsAmount;
        }

        public string EncryptedMessage
        {
            get
            {
                var matrix = new char[_key, GetColumnsAmount()];
                var result = "";
                var k = 0;

                for (var i = 0; i < matrix.GetLength(0); i++)
                {
                    for (var j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (k < _message.Length)
                        {
                            matrix[i, j] = _message[k];
                            k++;
                        }
                        else
                        {
                            matrix[i, j] = ' ';
                        }
                    }
                }
                
                for (var i = 0; i < matrix.GetLength(1); i++)
                {
                    for (var j = 0; j < matrix.GetLength(0); j++)
                    {
                        result += matrix[j, i];
                    }
                }

                return result;
            }
        }
        
        public string DecryptedMessage
        {
            get
            {
                var matrix = new char[_key, GetColumnsAmount()];
                var result = "";
                var k = 0;

                for (var i = 0; i < matrix.GetLength(1); i++)
                {
                    for (var j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (k < _message.Length)
                        {
                            matrix[j, i] = _message[k];
                            k++;
                        }
                        else
                        {
                            matrix[j, i] = ' ';
                        }
                    }
                }
                
                for (var i = 0; i < matrix.GetLength(0); i++)
                {
                    for (var j = 0; j < matrix.GetLength(1); j++)
                    {
                        result += matrix[i, j];
                    }
                }

                return result;
            }
        }
    }
}