using System.Text;
using System.Text.RegularExpressions;

namespace Lr1Parser;

public class TokenParser
{
    public StringToken[] StringToTokens(string source)
    {
        var copyOfSource = new StringBuilder(RegularExpressions.Whitespaces().Replace(source, " "));
	
        var tokens = new List<StringToken>(copyOfSource.Length);
	
        for (var i = 0; i < tokens.Capacity; i++)
            tokens.Add(new StringToken());

        foreach (var delimitedKeyword in DelimitedKeywords)
        {
            int occurrenceIndex;
            while ((occurrenceIndex = copyOfSource.ToString().IndexOf(delimitedKeyword, StringComparison.Ordinal)) > -1)
            {
                var keyword = delimitedKeyword.Remove(delimitedKeyword.Length - 1);
                
                var terminal = Terminal.GetTerminalByValue(keyword);
                
                if (terminal == null)
                    throw new Exception(
                        $"Ключевое слово \"{keyword}\" не является частью языка. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                tokens[occurrenceIndex].Value = terminal;
                
                tokens[occurrenceIndex].IndexInSource = occurrenceIndex;
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
                var position = source.GetPosition(i);
                throw new Exception(
                    $"Токен '{copyOfSource[i]}' не принадлежит алфавиту языка. Номер строки: {position.LineNumber}, номер токена: {position.CharNumber}.");
            }
            
            tokens[i].Value = terminal;
            tokens[i].IndexInSource = i;
        }
	
        return tokens.Where(t => !t.Value.IsEmpty()).ToArray();
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
}