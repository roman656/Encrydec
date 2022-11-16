namespace Encrydec.Ciphers;

using System.Text;

public class Gronsfeld
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюяABCDEFGHIJKLMNOPQRSTUVWXYZ_АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ 1234567890:;?!,.-\n";
    private readonly string _message;
    private readonly string _key;
        
    public Gronsfeld(string message, string key)
    {
        _message = message;
        _key = key;
    }

    public string EncryptedMessage
    {
        get
        {
            var result = new StringBuilder();

            for (var i = 0; i < _message.Length; i++)
            {
                var indexOfSymbol = Alphabet.IndexOf(_message[i]);
                var keyOffset = int.Parse(_key[i % _key.Length].ToString());

                result.Append(indexOfSymbol == -1
                        ? _message[i]    // Если в алфавите нет такого символа - оставляем как есть.
                        : Alphabet[(indexOfSymbol + keyOffset) % Alphabet.Length]);
            }

            return result.ToString();
        }
    }
        
    public string DecryptedMessage
    {
        get
        {
            var result = new StringBuilder();

            for (var i = 0; i < _message.Length; i++)
            {
                var indexOfSymbol = Alphabet.IndexOf(_message[i]);
                var keyOffset = int.Parse(_key[i % _key.Length].ToString());

                if (indexOfSymbol == -1)    // Если в алфавите нет такого символа - оставляем как есть.
                {
                    result.Append(_message[i]);
                }
                else
                {
                    var offset = indexOfSymbol - keyOffset;
                    
                    result.Append(Alphabet[offset < 0 ? offset + Alphabet.Length : offset]);
                }
            }

            return result.ToString();
        }
    }
}