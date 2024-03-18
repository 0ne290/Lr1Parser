using System.Text;
using System.Text.RegularExpressions;

namespace Lr1Parser;

public partial class TokenParser
{
    public StringToken[] StringToTokens(string source)
    {
        var copyOfSource = new StringBuilder(Whitespaces().Replace(source, " "));
	
        var tokens = new List<StringToken>(copyOfSource.Length);
	
        for (var i = 0; i < tokens.Capacity; i++)
            tokens.Add(new StringToken());

        foreach (var delimitedKeyword in DelimitedKeywords)
        {
            int occurrenceIndex;
            while ((occurrenceIndex = copyOfSource.ToString().IndexOf(delimitedKeyword, StringComparison.Ordinal)) > -1)
            {
                var keyword = delimitedKeyword.Remove(delimitedKeyword.Length - 1);
                tokens[occurrenceIndex].Value = keyword;
                tokens[occurrenceIndex].PositionInSource = occurrenceIndex;
                copyOfSource.Replace(keyword, new string(' ', keyword.Length), occurrenceIndex, keyword.Length);
            }
        }
	
        for (var i = 0; i < copyOfSource.Length; i++)
        {
            if (copyOfSource[i] == ' ')
                continue;

            var terminal = Terminal.GetTerminalByValue(copyOfSource[i].ToString());

            if (terminal == null)
            {
                var errorMessenger = new ErrorMessenger(source);
                var errorPosition = errorMessenger.GetErrorMessageByStringToken(i);
                throw new Exception(
                    $"Токен не принадлежит алфавиту языка. Строка: {errorPosition.Item1}; токен {errorPosition.Item2}");
            }
            
            tokens[i].Value = terminal;
            tokens[i].PositionInSource = i;
        }
	
        return tokens.Where(t => t.Value != Terminal.Default).ToArray();
    }

    private static readonly IEnumerable<string> DelimitedKeywords = new[]
    {
        "public ", "internal ", "private ",
        
        "struct ",
        
        "int ", "int[", "int?",
        "double ", "double[", "double?",
        "bool ", "bool[", "bool?",
        "char ", "char[", "char?",
        "string ", "string[", "string?"
    };

    [GeneratedRegex("[\\s]")]
    private static partial Regex Whitespaces();
}