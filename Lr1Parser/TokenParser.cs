using System.Text;

namespace Lr1Parser;

public class TokenParser
{
    public TokenParser(string source, string specialCharacters, Lr1Grammar grammar)
    {
        Source = source;
        SpecialCharacters = specialCharacters;
        Grammar = grammar;
    }
    
    public IEnumerable<StringToken> StringToTokens()
    {
        var copyOfSource = new StringBuilder(RegularExpressions.Whitespace().Replace(Source, " "));

        // Для корректного распознавания ключевых слов, в начале и конце входной последовательности должен быть символ-разделитель
        copyOfSource.Insert(0, ' ');
        copyOfSource.Append(' ');
	
        var tokens = new List<StringToken>(copyOfSource.Length);
	
        for (var i = 0; i < tokens.Capacity; i++)
            tokens.Add(new StringToken());

        var keywords = Grammar.GetKeywords();

        foreach (var keyword in keywords)
        {
            var match = RegularExpressions.Keyword(keyword.Value, SpecialCharacters).Match(copyOfSource.ToString());
            
            while (match.Success)
            {
                tokens[match.Index].Value = keyword;
                
                tokens[match.Index].IndexInSource = match.Index;
                copyOfSource.Replace(keyword.Value, new string(' ', keyword.Value.Length), match.Index + 1, keyword.Value.Length);
            }
        }
        
        Console.WriteLine(copyOfSource.ToString());
	
        for (var i = 0; i < copyOfSource.Length; i++)
        {
            if (copyOfSource[i] == ' ')
                continue;

            var terminal = Grammar.GetTerminalByValue(copyOfSource[i].ToString());

            if (terminal == null)
            {
                var position = Source.GetPosition(i);
                throw new Exception(
                    $"Токен '{copyOfSource[i]}' не принадлежит алфавиту языка. Номер строки: {position.LineNumber}, номер токена: {position.CharNumber}.");
            }
            
            tokens[i].Value = terminal;
            tokens[i].IndexInSource = i;
        }
	
        return tokens.Where(t => !t.Value.IsEmpty());
    }
    
    public string Source { get; set; }
    
    public string SpecialCharacters { get; set; }
    
    public Lr1Grammar Grammar { get; set; }
}