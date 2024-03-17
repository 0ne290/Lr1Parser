using System.Text;
using System.Text.RegularExpressions;

namespace Lr1Parser;

public partial class TokenParser
{
    public Token[] StringToTokens(string source)
    {
        var copyOfSource = new StringBuilder(Whitespaces().Replace(source, " "));
	
        var tokens = new List<Token>(copyOfSource.Length);
	
        for (var i = 0; i < tokens.Capacity; i++)
            tokens.Add(new Token());

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
            
            tokens[i].Value = copyOfSource[i].ToString();
            tokens[i].PositionInSource = i;
        }
	
        return tokens.Where(t => t.Value != string.Empty).ToArray();
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