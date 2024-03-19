namespace Lr1Parser;

public class GrammarParser
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GrammarParser(string source) => Source = source;
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public Lr1Grammar Parse()
    {
        _grammar = new Lr1Grammar();

        var lines = PrepareLines();
        
        ParseNonterminals(lines);
        ParseTerminals(lines);
        ParseRules(lines);

        return _grammar;
    }

    private string[,] PrepareLines()
    {
        var lines = Source.Split(Environment.NewLine).ToList();
        lines.RemoveAll(string.IsNullOrWhiteSpace);
        
        if (lines.Count == 0)
            throw new Exception("Грамматика пустая.");

        var preparedLines = new string[lines.Count, 2];

        for (var i = 0; i < lines.Count; i++)
        {
            var lineSides = lines[i].Split('=');

            if (lineSides.Length != 2)
                throw new Exception($"Правило под номером {i + 1} задано некорректно.");

            lineSides[0] = lineSides[0].Trim();
            lineSides[1] = lineSides[1].Trim();
            lineSides[1] = RegularExpressions.WhitespaceSequence().Replace(lineSides[1], " ");
            
            if (RegularExpressions.Whitespace().IsMatch(lineSides[0]))
                throw new Exception($"Разделительные символы не могут быть частью языка. Нетерминал в правиле под номером {i + 1} противоречит этому условию.");
            
            preparedLines[i, 0] = lineSides[0].Trim();
            preparedLines[i, 1] = lineSides[1].Trim();
        }

        return preparedLines;
    }

    private void ParseRules(string[,] lines)
    {
        var sides = rule.Split('=');

        if (sides.Length != 2)
            throw new Exception("Правило задано некорректно.");


    }
    
    private void ParseTerminals(string[,] lines)
    {
        for (var i = 0; i < lines.GetLength(0); i++)
        {
            var rightSide = lines[i, 1].Split(' ');

            foreach (var token in rightSide)
            {
                if (!_grammar.GetNonterminalByValue(token).IsEmpty())
                    continue;
                
                if (token == "|")
                    continue;

                if (RegularExpressions.CharRange().IsMatch(token))
                {
                    ParseRangeOfTerminals(token[0], token[2]);
                    continue;
                }
                
                if (RegularExpressions.EscapedCharRange().IsMatch(token))
                {
                    _grammar.AddTerminal(new Terminal(token.Replace(@"\-", "-")));
                    continue;
                }
                
                if (RegularExpressions.EscapedDisjunction().IsMatch(token))
                {
                    _grammar.AddTerminal(new Terminal(token.Replace(@"\|", "|")));
                    continue;
                }
                
                _grammar.AddTerminal(new Terminal(token));
            }
        }
    }

    private void ParseRangeOfTerminals(char a, char z)
    {
        if (a < z)
            for (int i = a; i <= z; i++)
                _grammar.AddTerminal(new Terminal(((char)i).ToString()));
        else
            for (int i = z; i >= a; i--)
                _grammar.AddTerminal(new Terminal(((char)i).ToString()));
    }

    private void ParseNonterminals(string[,] lines)
    {
        for (var i = 0; i < lines.GetLength(0); i++)
            _grammar.AddNonterminal(new Nonterminal(lines[i, 0]));
    }
    
    public string Source { get; set; }

    private Lr1Grammar _grammar;
}