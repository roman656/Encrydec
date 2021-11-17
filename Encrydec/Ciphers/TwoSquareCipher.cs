using System;

namespace Encrydec.Ciphers
{
    public class TwoSquareCipher
    {
        private readonly string _message;
        private readonly string _key;
        
        public TwoSquareCipher(string message, string key)
        {
            _message = message.Replace("\n", "");
            _key = key;
        }
        
        public static bool CheckKey(string key, string message)
        {
            var hasError = false;
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var columnsAmount = rows[0].Length;
            var uniqueElements1 = "";
            var uniqueElements2 = "";

            if (rows.Length % 2 == 0 && rows.Length + columnsAmount > 2)
            {
                for (var i = 0; i < rows.Length / 2; i++)
                {
                    if (rows[i].Length != columnsAmount || rows[i + rows.Length / 2].Length != columnsAmount)
                    {
                        hasError = true;
                        break;
                    }

                    foreach (var element in rows[i])
                    {
                        if (uniqueElements1.Contains(element))
                        {
                            hasError = true;
                            break;
                        }

                        uniqueElements1 += element;
                    }
                    
                    foreach (var element in rows[i + rows.Length / 2])
                    {
                        if (uniqueElements2.Contains(element))
                        {
                            hasError = true;
                            break;
                        }

                        uniqueElements2 += element;
                    }

                    if (hasError)
                    {
                        break;
                    }
                }

                if (!hasError)
                {
                    if (uniqueElements1 == uniqueElements2)
                    {
                        hasError = true;
                    }
                }

                if (!hasError)
                {
                    foreach (var letter in uniqueElements1)
                    {
                        if (uniqueElements2.IndexOf(letter) == -1)
                        {
                            hasError = true;
                            break;
                        }
                    }
                }

                if (!hasError)
                {
                    foreach (var letter in message)
                    {
                        if (!uniqueElements1.Contains(letter) && letter != '\n')
                        {
                            hasError = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                hasError = true;
            }

            return !hasError;
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