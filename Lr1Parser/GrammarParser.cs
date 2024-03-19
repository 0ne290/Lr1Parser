namespace Lr1Parser;

public class GrammarParser
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GrammarParser(string source) => Source = source;
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public Lr1Grammar Parse()
    {
        _grammar = new Lr1Grammar();

        var rules = PrepareLines();
        
        ParseNonterminals(rules);
        ParseTerminals(rules);
        ParseRules(rules);

        return _grammar;
    }

    private string[,] PrepareLines()
    {
        var lines = Source.Split(Environment.NewLine).ToList();
        lines.RemoveAll(string.IsNullOrWhiteSpace);
        
        if (lines.Count == 0)
            throw new Exception("Грамматика пустая.");

        var rules = new string[lines.Count, 2];

        for (var i = 0; i < lines.Count; i++)
        {
            var sides = lines[i].Split('=');

            if (sides.Length != 2)
                throw new Exception($"Правило под номером {i + 1} задано некорректно.");

            sides[0] = sides[0].Trim();
            sides[1] = sides[1].Trim();
            sides[1] = RegularExpressions.WhitespaceSequence().Replace(sides[1], " ");
            
            if (RegularExpressions.Whitespace().IsMatch(sides[0]))
                throw new Exception($"Разделительные символы не могут быть частью языка. Нетерминал в правиле под номером {i + 1} противоречит этому условию.");
            
            rules[i, 0] = sides[0].Trim();
            rules[i, 1] = sides[1].Trim();
        }

        return rules;
    }

    private void ParseRules(string[,] rules)
    {
        for (var i = 0; i < rules.GetLength(0); i++)
        {
            var subrulesRightSides = rules[i, 1].Split(" | ");

            foreach (var serializedSubruleRightSide in subrulesRightSides)
            {
                var rightSideTokens = serializedSubruleRightSide.Split(' ');

                var deserializedSubruleRightSide = new List<IGrammarToken>();

                foreach (var rightSideToken in rightSideTokens)
                {
                    IGrammarToken grammarToken = _grammar.GetTerminalByValue(rightSideToken);
                    
                    if (!grammarToken.IsEmpty())
                    {
                        deserializedSubruleRightSide.Add(grammarToken);
                        continue;
                    }

                    grammarToken = _grammar.GetNonterminalByValue(rightSideToken);
                    if (!grammarToken.IsEmpty())
                    {
                        deserializedSubruleRightSide.Add(grammarToken);
                        continue;
                    }

                    throw new Exception("Во время анализа правил встретился неизвестный токен. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");
                }

                var deserializedSubruleLeftSide = _grammar.GetNonterminalByValue(rules[i, 0]);
                
                if (deserializedSubruleLeftSide.IsEmpty())
                    throw new Exception("Во время анализа правил встретился неизвестный токен. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                _grammar.AddRule(new Rule(deserializedSubruleLeftSide, deserializedSubruleRightSide));
            }
        }
    }
    
    private void ParseTerminals(string[,] rules)
    {
        for (var i = 0; i < rules.GetLength(0); i++)
        {
            var rightSideTokens = rules[i, 1].Split(' ');

            foreach (var token in rightSideTokens)
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

    private void ParseNonterminals(string[,] rules)
    {
        for (var i = 0; i < rules.GetLength(0); i++)
            _grammar.AddNonterminal(new Nonterminal(rules[i, 0]));
    }
    
    public string Source { get; set; }

    private Lr1Grammar _grammar;
}