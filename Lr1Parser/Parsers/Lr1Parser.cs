using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers;

public class Lr1Parser
{
    public Lr1Parser(Lr1Grammar.Lr1Grammar grammar, IEnumerable<StringToken> tokens)
    {
        _grammar = grammar;
        _tokens = tokens;
        
        CalculateInitialTerminals();
    }

    private void CalculateInitialTerminals()
    {
        foreach (var terminal in _grammar.Terminals)
            _initialTerminals.Add(terminal, new[] { terminal });
        
        foreach (var nonterminal in _grammar.Nonterminals)
            if (!_initialTerminals.ContainsKey(nonterminal))
                CalculateInitialTerminalsForNonterminal(nonterminal);
    }

    private void CalculateInitialTerminalsForNonterminal(Nonterminal nonterminal)
    {
        var rules = _grammar.GetRulesByLeftSide(nonterminal);

        var initialTerminalsForNonterminal = new List<Terminal>();
        foreach (var rule in rules)
        {
            if (rule.RightSide[0] == nonterminal)
                continue;
            
            if (_initialTerminals.TryGetValue(rule.RightSide[0], out var initialTerminals))
                initialTerminalsForNonterminal.AddRange(initialTerminals);
            else
            {
                CalculateInitialTerminalsForNonterminal((Nonterminal)rule.RightSide[0]);

                if (_initialTerminals.TryGetValue(rule.RightSide[0], out initialTerminals))
                    initialTerminalsForNonterminal.AddRange(initialTerminals);
                else
                    throw new Exception(
                        "Во время построения множеств символов-предшественников произошла ошибка. Обратитесь к разработчику.");
            }
        }
        
        _initialTerminals.Add(nonterminal, initialTerminalsForNonterminal.Distinct());
    }

    public void Log()
    {
        LogInitialTerminals();
    }

    private void LogInitialTerminals()
    {
        var initialTerminalsFile = new StreamWriter("../../../Logging/InitialTerminals.txt", false);
        
        foreach (var initialTerminalsPair in _initialTerminals)
        {
            initialTerminalsFile.Write($"{initialTerminalsPair.Key.Value} = {{ ");
            
            foreach (var initialTerminal in initialTerminalsPair.Value)
                initialTerminalsFile.Write($"{initialTerminal.Value} ");
            
            initialTerminalsFile.WriteLine("}");
        }
        
        initialTerminalsFile.Dispose();
    }

    private readonly Lr1Grammar.Lr1Grammar _grammar;

    private readonly IEnumerable<StringToken> _tokens;

    private readonly Dictionary<IGrammarToken, IEnumerable<Terminal>> _initialTerminals = new();
}