using System;

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

        private static char[,] ConvertKeyToMatrix(string key)
        {
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var matrix = new char[rows.Length, rows[0].Length];
            
            for (var i = 0; i < rows.Length; i++)
            {
                for (var j = 0; j < rows[0].Length; j++)
                {
                    matrix[i, j] = rows[i][j];
                }
            }

            return matrix;
        }

        private static (int, int) FindLetterIndexes(char[,] matrix, char letter)
        {
            var rowIndex = -1;
            var columnIndex = -1;
            
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == letter)
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
                    result += matrix[i >= matrix.GetLength(0) ? 0 : i, j];
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
                    result += matrix[i < 0 ? matrix.GetLength(0) - 1 : i, j];
                }

                return result;
            }
        }
    }
}