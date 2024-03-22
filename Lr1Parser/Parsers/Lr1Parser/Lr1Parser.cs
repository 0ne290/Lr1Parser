using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser;

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
            _initialTerminalsByTokens.Add(terminal, new[] { terminal });
        
        foreach (var nonterminal in _grammar.Nonterminals)
            if (!_initialTerminalsByTokens.ContainsKey(nonterminal))
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
            
            if (_initialTerminalsByTokens.TryGetValue(rule.RightSide[0], out var initialTerminals))
                initialTerminalsForNonterminal.AddRange(initialTerminals);
            else
            {
                CalculateInitialTerminalsForNonterminal((Nonterminal)rule.RightSide[0]);

                if (_initialTerminalsByTokens.TryGetValue(rule.RightSide[0], out initialTerminals))
                    initialTerminalsForNonterminal.AddRange(initialTerminals);
                else
                    throw new Exception(
                        "Во время построения множеств символов-предшественников произошла ошибка. Обратитесь к разработчику.");
            }
        }
        
        _initialTerminalsByTokens.Add(nonterminal, initialTerminalsForNonterminal.Distinct());
    }

    private void BuildStateGraph()
    {
        var initialState = new Lr1ParserState();
        initialState.AddItem(new Lr1Item(_grammar.InitialRule, 0, Terminal.End));
        
        _states.Add(initialState);
    }

    private void CloseState(Lr1ParserState state)
    {
        while (true)
        {
            var numberAddedItems = 0;
            
            foreach (var item in state.GetItemsWhereFirstUnrecognizedTokenIsNonterminal())
            {
                var nonterminal = item.UnrecognizedPart[0];
                foreach (var initialTerminal in _initialTerminalsByTokens[item.GetSecondUnrecognizedTokenOrReductionTerminal()])
                {
                    foreach (var rule in _grammar.GetRulesByLeftSide((Nonterminal)nonterminal))
                    {
                        if (state.AddItem(new Lr1Item(rule, 0, initialTerminal)))
                            numberAddedItems++;
                    }
                }
            }
            
            if (numberAddedItems < 1)
                break;
        }
        
    }

    public void Log()
    {
        LogInitialTerminals();
    }

    private void LogInitialTerminals()
    {
        var initialTerminalsFile = new StreamWriter("../../../Logging/InitialTerminals.txt", false);
        
        foreach (var initialTerminalsPair in _initialTerminalsByTokens)
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

    private readonly Dictionary<IGrammarToken, IEnumerable<Terminal>> _initialTerminalsByTokens = new();

    private readonly List<Lr1ParserState> _states = new();
}