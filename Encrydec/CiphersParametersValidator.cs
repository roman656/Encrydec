namespace Encrydec;

using System;
using System.Linq;
using Ciphers;

public static class CiphersParametersValidator
{
    /*
     * Если в исходном тексте встретится \n его надо будет зашифровать. Для этого он должен быть
     * в таблице (ключ), как элемент. В текущем формате записи входных таблиц это невозможно.
     */
    private static bool CheckMessage(string message, CipherType cipherType)
    {
        return cipherType switch
        {
            CipherType.Scytale => message.Length > 0,
            CipherType.PolybiusSquare => message.Length > 0 && !message.Contains('\n'),
            CipherType.Gronsfeld => message.Length > 0,
            CipherType.SinglePermutation => message.Length > 0 && !message.Contains('\n'),
            _ => message.Length > 0 && !message.Contains('\n') && message.Length % 2 == 0
        };
    }
        
    private static bool CheckKey(string key, string message, CipherType cipherType)
    {
        return cipherType switch
        {
            CipherType.Scytale => CheckScytaleKey(key, message),
            CipherType.PolybiusSquare => CheckPolybiusSquareKey(key, message),
            CipherType.Gronsfeld => CheckGronsfeldKey(key, message),
            CipherType.SinglePermutation => CheckSinglePermutationKey(key),
            _ => CheckTwoSquareCipherKey(key, message)
        };
    }
        
    public static bool CheckMessageAndKey(string key, string message, CipherType cipherType)
    {
        return CheckMessage(message, cipherType) && CheckKey(key, message, cipherType);
    }

    private static bool CheckScytaleKey(string key, string message)
    {
        var isValid = false;
            
        if (int.TryParse(key, out var value))
        {
            isValid = value > 1 && value < message.Length;
        }

        return isValid;
    }
        
    private static bool CheckPolybiusSquareKey(string key, string message)
    {
        var isValid = false;

        if (key.Length > 0)
        {
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length > 1)
            {
                isValid = CheckColumnsAmount(rows) && CheckTableElementsUniqueness(rows)
                                                   && CheckIfKeyContainsAllMessageLetters(key, message);
            }
        }

        return isValid;
    }
    
    private static bool CheckSinglePermutationKey(string key) => key.Length > 0 && key.All(char.IsLetterOrDigit);

    private static bool CheckGronsfeldKey(string key, string message)
    {
        if (key.Length > message.Length || key.Length <= 0)    // Если длина ключа некорректна.
        {
            return false;
        }

        /* Все символы ключа должны быть числами. */
        return key.All(symbol => int.TryParse(symbol.ToString(), out _));
    }
        
    private static bool CheckTwoSquareCipherKey(string key, string message)
    {
        var isValid = false;

        if (key.Length > 0)
        {
            var rows = key.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var columnsAmount = rows[0].Length;

            if (rows.Length % 2 == 0 && rows.Length / 2 + columnsAmount > 2)
            {
                var firstTableRows = new string[rows.Length / 2];
                var secondTableRows = new string[rows.Length / 2];
                    
                for (var i = 0; i < rows.Length / 2; i++)
                {
                    firstTableRows[i] = rows[i];
                    secondTableRows[i] = rows[i + rows.Length / 2];
                }
                    
                isValid = CheckColumnsAmount(rows) && CheckTableElementsUniqueness(firstTableRows) 
                                                   && CheckTableElementsUniqueness(secondTableRows) 
                                                   && CheckIfTablesContainSameElements(firstTableRows, secondTableRows)
                                                   && CheckIfTablesAreNotEqual(firstTableRows, secondTableRows)
                                                   && CheckIfKeyContainsAllMessageLetters(key, message);
            }
        }

        return isValid;
    }

    private static bool CheckColumnsAmount(string[] tableRows)
    {
        var isValid = true;
        var columnsAmount = tableRows[0].Length;
            
        foreach (var row in tableRows)
        {
            if (row.Length != columnsAmount)
            {
                isValid = false;
                break;
            }
        }
            
        return isValid;
    }
        
    private static bool CheckTableElementsUniqueness(string[] tableRows)
    {
        var result = true;
        var uniqueElements = string.Empty;

        foreach (var row in tableRows)
        {
            foreach (var element in row)
            {
                if (uniqueElements.Contains(element))
                {
                    result = false;
                    break;
                }

                uniqueElements += element;
            }

            if (!result)
            {
                break;
            }
        }
            
        return result;
    }

    private static bool CheckIfKeyContainsAllMessageLetters(string key, string message)
    {
        var result = true;
            
        foreach (var letter in message)
        {
            if (!key.Contains(letter))
            {
                result = false;
                break;
            }
        }

        return result;
    }
        
    private static bool CheckIfTablesContainSameElements(string[] firstTableRows, string[] secondTableRows)
    {
        var result = true;
        var secondTableElements = secondTableRows.Aggregate("", (current, row) => current + row);

        foreach (var row in firstTableRows)
        {
            foreach (var element in row)
            {
                if (!secondTableElements.Contains(element))
                {
                    result = false;
                    break;
                }
            }

            if (!result)
            {
                break;
            }
        }

        return result;
    }
        
    private static bool CheckIfTablesAreNotEqual(string[] firstTableRows, string[] secondTableRows)
    {
        var result = false;

        for (var i = 0; i < firstTableRows.Length; i++)
        {
            for (var j = 0; j < firstTableRows[i].Length; j++)
            {
                if (firstTableRows[i][j] != secondTableRows[i][j])
                {
                    result = true;
                    break;
                }
            }

            if (result)
            {
                break;
            }
        }

        return result;
    }
}