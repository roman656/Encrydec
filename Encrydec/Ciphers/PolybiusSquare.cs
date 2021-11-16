using System;
using System.Linq;

namespace Encrydec.Ciphers
{
    public class PolybiusSquare
    {
        private readonly string _message;
        private readonly string _key;
        
        public PolybiusSquare(string message, string key)
        {
            _message = message;
            _key = key;
        }
        
        public static bool CheckKey(string key, string message)
        {
            var result = false;
            var hasError = false;
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length > 1)
            {
                foreach (var row in rows)
                {
                    var elements = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (elements.Length != rows.Length)
                    {
                        break;
                    }

                    foreach (var element in elements)
                    {
                        if (element.Length != 1)
                        {
                            hasError = true;
                            break;
                        }
                    }

                    if (hasError)
                    {
                        break;
                    }
                }
                /*
                var matrix = ConvertKeyToMatrix(key);
                
                if (matrix.Distinct().Count() != matrix.Length)
                {
                    hasError = true;
                }*/

                foreach (var letter in message)
                {
                    if (!key.Contains(letter))
                    {
                        hasError = true;
                        break;
                    }
                }
            }

            return result && !hasError;
        }

        private static char[][] ConvertKeyToMatrix(string key)
        {
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var matrix = new char[rows.Length][];
            
            for (var i = 0; i < rows.Length; i++)
            {
                var elements = rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                matrix[i] = new char[elements.Length];
                
                for (var j = 0; j < elements.Length; j++)
                {
                    matrix[i][j] = elements[j].Trim().ToCharArray()[0];
                }
            }

            return matrix;
        }

        private static (int, int) FindLetterIndexes(char[][] matrix, char letter)
        {
            var rowIndex = -1;
            var columnIndex = -1;
            
            for (var i = 0; i < matrix[0].Length; i++)
            {
                for (var j = 0; j < matrix[0].Length; j++)
                {
                    if (matrix[i][j] == letter)
                    {
                        rowIndex = i;
                        columnIndex = j;
                        break;
                    }
                }
            }

            return (rowIndex, columnIndex);
        }

        public string EncryptedMessage
        {
            get
            {
                var matrix = ConvertKeyToMatrix(_key);
                var result = "";

                foreach (var letter in _message)
                {
                    var (i, j) = FindLetterIndexes(matrix, letter);
                    
                    i++;
                    result += matrix[i >= matrix[0].Length ? 0 : i][j];
                }

                return result;
            }
        }
        
        public string DecryptedMessage
        {
            get
            {
                var matrix = ConvertKeyToMatrix(_key);
                var result = "";

                foreach (var letter in _message)
                {
                    var (i, j) = FindLetterIndexes(matrix, letter);
                    
                    i--;
                    result += matrix[i < 0 ? matrix[0].Length - 1 : i][j];
                }

                return result;
            }
        }
    }
}