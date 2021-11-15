using System;

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
        
        public static bool CheckKey(string key, int messageLength)
        {
            var result = false;
            
            if (int.TryParse(key, out var value))
            {
                result = value > 1 && value < messageLength;
            }

            return result;
        }

        public string EncryptedMessage
        {
            get
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

                var matrix = new char[_key, columnsAmount];
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
                int columnsAmount;
                
                if (_message.Length % _key == 0)
                {
                    columnsAmount = _message.Length / _key;
                }
                else
                {
                    columnsAmount = (_message.Length + _key - _message.Length % _key) / _key;
                }

                var matrix = new char[_key, columnsAmount];
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