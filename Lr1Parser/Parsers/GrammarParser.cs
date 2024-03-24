using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers;

public class GrammarParser
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GrammarParser(string source) => Source = source;
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public Lr1Grammar.Lr1Grammar Parse()
    {
        var rules = PrepareLines();
        
        _grammarBuilder = new Lr1GrammarBuilder(new Nonterminal(rules[0, 0]));
        
        ParseNonterminals(rules);
        ParseTerminals(rules);
        ParseRules(rules);

        return _grammarBuilder.Build();
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

            foreach (var subruleRightSide in subrulesRightSides)
            {
                var subruleRightSideTokenOptions = new List<IEnumerable<IGrammarToken>>();

                var subruleRightSideTokens = subruleRightSide.Split(' ');

                foreach (var subruleRightSideToken in subruleRightSideTokens)
                {
                    if (subruleRightSideToken == "|")
                        continue;

                    IGrammarToken grammarToken = _grammarBuilder.GetTerminalByValue(subruleRightSideToken);
                    if (!grammarToken.IsEmpty())
                    {
                        subruleRightSideTokenOptions.Add(new[] { grammarToken });
                        continue;
                    }

                    grammarToken = _grammarBuilder.GetNonterminalByValue(subruleRightSideToken);
                    if (!grammarToken.IsEmpty())
                    {
                        subruleRightSideTokenOptions.Add(new[] { grammarToken });
                        continue;
                    }

                    if (RegularExpressions.CharRange().IsMatch(subruleRightSideToken))
                    {
                        var characters = subruleRightSideToken[0].RangeTo(subruleRightSideToken[2]);
                        
                        var charactersTerminals = new List<IGrammarToken>();

                        foreach (var character in characters)
                        {
                            grammarToken = _grammarBuilder.GetTerminalByValue(character.ToString());

                            if (grammarToken.IsEmpty())
                                throw new Exception(
                                    $"Во время анализа правил встретился неизвестный терминал '{character}' при попытке распарсить диапазон символов на терминалы. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                            charactersTerminals.Add(grammarToken);
                        }
                        
                        subruleRightSideTokenOptions.Add(charactersTerminals);

                        continue;
                    }

                    if (RegularExpressions.EscapedCharRange().IsMatch(subruleRightSideToken))
                    {
                        grammarToken = _grammarBuilder.GetTerminalByValue(subruleRightSideToken.Replace(@"\-", "-"));

                        if (grammarToken.IsEmpty())
                            throw new Exception(
                                $"Во время анализа правил встретился неизвестный терминал \"{subruleRightSideToken}\" при попытке распарсить токен с экранированным диапазоном символов. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                        subruleRightSideTokenOptions.Add(new[] { grammarToken });

                        continue;
                    }

                    if (RegularExpressions.EscapedDisjunction().IsMatch(subruleRightSideToken))
                    {
                        grammarToken = _grammarBuilder.GetTerminalByValue(subruleRightSideToken.Replace(@"\|", "|"));

                        if (grammarToken.IsEmpty())
                            throw new Exception(
                                $"Во время анализа правил встретился неизвестный терминал \"{subruleRightSideToken}\" при попытке распарсить токен с экранированной дизъюнкцией правил. Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                        subruleRightSideTokenOptions.Add(new[] { grammarToken });

                        continue;
                    }

                    throw new Exception(
                        $"Во время анализа правил встретился неизвестный токен \"{subruleRightSideToken}\". Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");
                }
                
                var subruleLeftSide = _grammarBuilder.GetNonterminalByValue(rules[i, 0]);

                if (subruleLeftSide.IsEmpty())
                    throw new Exception($"Во время анализа правил встретился неизвестный нетерминал \"{rules[i, 0]}\". Вообще-то этой ошибки не должно быть - обратитесь к разработчику.");

                var products = subruleRightSideTokenOptions.CartesianProduct();

                foreach (var product in products)
                    _grammarBuilder.AddRule(new Rule(subruleLeftSide, product));
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
                if (!_grammarBuilder.GetNonterminalByValue(token).IsEmpty())
                    continue;
                
                if (token == "|")
                    continue;

                if (RegularExpressions.CharRange().IsMatch(token))
                {
                    var characters = token[0].RangeTo(token[2]);

                    foreach (var character in characters)
                        _grammarBuilder.AddTerminal(new Terminal(character.ToString()));
                    
                    continue;
                }
                
                if (RegularExpressions.EscapedCharRange().IsMatch(token))
                {
                    _grammarBuilder.AddTerminal(new Terminal(token.Replace(@"\-", "-")));
                    continue;
                }
                
                if (RegularExpressions.EscapedDisjunction().IsMatch(token))
                {
                    _grammarBuilder.AddTerminal(new Terminal(token.Replace(@"\|", "|")));
                    continue;
                }
                
                _grammarBuilder.AddTerminal(new Terminal(token));
            }
        }
    }
    
    private void ParseNonterminals(string[,] rules)
    {
        for (var i = 0; i < rules.GetLength(0); i++)
            _grammarBuilder.AddNonterminal(new Nonterminal(rules[i, 0]));
    }
    
    public string Source { get; set; }

    private Lr1GrammarBuilder _grammarBuilder;
}