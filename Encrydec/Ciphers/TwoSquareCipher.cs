using System;

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

        private static (char[,], char[,]) ConvertKeyToMatrices(string key)
        {
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var firstMatrix = new char[rows.Length / 2, rows[0].Length];
            var secondMatrix = new char[rows.Length / 2, rows[0].Length];
            
            for (var i = 0; i < rows.Length / 2; i++)
            {
                for (var j = 0; j < rows[0].Length; j++)
                {
                    firstMatrix[i, j] = rows[i][j];
                    secondMatrix[i, j] = rows[rows.Length / 2 + i][j];
                }
            }

            return (firstMatrix, secondMatrix);
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

        private static char[,] ConvertMessageToBigrams(string message)
        {
            var augmentedMessage = message + (message.Length % 2 != 0 ? " " : "");
            var result = new char[augmentedMessage.Length / 2, 2];
            
            for (var i = 0; i < augmentedMessage.Length; i++)
            {
                result[i / 2, i % 2] = augmentedMessage[i];
            }

            return result;
        }
        
        private static string ConvertBigramsToMessage(char[,] bigrams)
        {
            var result = "";
            
            for (var i = 0; i < bigrams.GetLength(0); i++)
            {
                result += bigrams[i, 0];
                result += bigrams[i, 1];
            }

            return result;
        }

        public string EncryptedMessage
        {
            get
            {
                var (firstMatrix, secondMatrix) = ConvertKeyToMatrices(_key);
                var bigrams = ConvertMessageToBigrams(_message);
                var result = new char[bigrams.GetLength(0), bigrams.GetLength(1)];

                for (var i = 0; i < bigrams.GetLength(0); i++)
                {
                    var (firstLetterI, firstLetterJ) = FindLetterIndexes(firstMatrix, bigrams[i, 0]);
                    var (secondLetterI, secondLetterJ) = FindLetterIndexes(secondMatrix, bigrams[i, 1]);

                    if (firstLetterI == secondLetterI)
                    {
                        result[i, 0] = secondMatrix[firstLetterI, firstLetterJ];
                        result[i, 1] = firstMatrix[secondLetterI, secondLetterJ];
                    }
                    else
                    {
                        result[i, 0] = secondMatrix[firstLetterI, secondLetterJ];
                        result[i, 1] = firstMatrix[secondLetterI, firstLetterJ];
                    }
                }

                return ConvertBigramsToMessage(result);
            }
        }
        
        public string DecryptedMessage
        {
            get
            {
                var (firstMatrix, secondMatrix) = ConvertKeyToMatrices(_key);
                var bigrams = ConvertMessageToBigrams(_message);
                var result = new char[bigrams.GetLength(0), bigrams.GetLength(1)];

                for (var i = 0; i < bigrams.GetLength(0); i++)
                {
                    var (firstLetterI, firstLetterJ) = FindLetterIndexes(secondMatrix, bigrams[i, 0]);
                    var (secondLetterI, secondLetterJ) = FindLetterIndexes(firstMatrix, bigrams[i, 1]);

                    if (firstLetterI == secondLetterI)
                    {
                        result[i, 0] = firstMatrix[firstLetterI, firstLetterJ];
                        result[i, 1] = secondMatrix[secondLetterI, secondLetterJ];
                    }
                    else
                    {
                        result[i, 0] = firstMatrix[firstLetterI, secondLetterJ];
                        result[i, 1] = secondMatrix[secondLetterI, firstLetterJ];
                    }
                }

                return ConvertBigramsToMessage(result);
            }
        }
    }
}