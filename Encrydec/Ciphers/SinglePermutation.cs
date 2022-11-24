namespace Encrydec.Ciphers;

using System;
using System.Text;

public class SinglePermutation
{
    private readonly string _message;
    private readonly string _key;
        
    public SinglePermutation(string message, string key)
    {
        _message = message;
        _key = key;
    }

    private int GetRowsAmount()
    {
        int rowsAmount;
                
        if (_message.Length % _key.Length == 0)
        {
            rowsAmount = _message.Length / _key.Length;
        }
        else
        {
            rowsAmount = (_message.Length + _key.Length - _message.Length % _key.Length) / _key.Length;
        }

        return rowsAmount;
    }

    private char[,] PutMessageInMatrix(bool isEncryptionModeActive)
    {
        var matrix = new char[GetRowsAmount(), _key.Length];
        var k = 0;

        for (var i = 0; i < matrix.GetLength(isEncryptionModeActive ? 1 : 0); i++)
        {
            for (var j = 0; j < matrix.GetLength(isEncryptionModeActive ? 0 : 1); j++)
            {
                if (k < _message.Length)
                {
                    matrix[isEncryptionModeActive ? j : i, isEncryptionModeActive ? i : j] = _message[k++];
                }
                else
                {
                    matrix[isEncryptionModeActive ? j : i, isEncryptionModeActive ? i : j] = ' ';    // Дозаполняем пустые ячейки пробелами.
                }
            }
        }

        return matrix;
    }
    
    private static string ConvertMatrixToString(char[,] matrix, bool isEncryptionModeActive)
    {
        var result = new StringBuilder();
        
        for (var i = 0; i < matrix.GetLength(isEncryptionModeActive ? 0 : 1); i++)
        {
            for (var j = 0; j < matrix.GetLength(isEncryptionModeActive ? 1 : 0); j++)
            {
                result.Append(matrix[isEncryptionModeActive ? i : j, isEncryptionModeActive ? j : i]);
            }
        }

        return result.ToString();
    }

    public string EncryptedMessage
    {
        get
        {
            var matrix = PutMessageInMatrix(isEncryptionModeActive: true);
            var resultMatrix = new char[GetRowsAmount(), _key.Length];
            var permutation = new int[_key.Length];

            for (var i = 0; i < permutation.Length; i++)
            {
                permutation[i] = i;
            }
            
            Array.Sort(_key.ToCharArray(), permutation);
            
            for (var i = 0; i < permutation.Length; i++)
            {
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    resultMatrix[j, permutation[i]] = matrix[j, i];
                }
            }

            return ConvertMatrixToString(resultMatrix, isEncryptionModeActive: true);
        }
    }
        
    public string DecryptedMessage
    {
        get
        {
            var matrix = PutMessageInMatrix(isEncryptionModeActive: false);
            var resultMatrix = new char[GetRowsAmount(), _key.Length];
            var permutation = new int[_key.Length];

            for (var i = 0; i < permutation.Length; i++)
            {
                permutation[i] = i;
            }
            
            Array.Sort(_key.ToCharArray(), permutation);
            
            for (var i = 0; i < permutation.Length; i++)
            {
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    resultMatrix[j, i] = matrix[j, permutation[i]];
                }
            }

            return ConvertMatrixToString(resultMatrix, isEncryptionModeActive: false);
        }
    }
}