using Lr1Parser.Lr1Grammar;
using Lr1Parser.Lr1Graph;

namespace Lr1Parser.Parsers;

public class Lr1Parser
{
    public Lr1Parser(Lr1Grammar.Lr1GrammarBuilder grammarBuilder, IEnumerable<StringToken> tokens)
    {
        _grammarBuilder = grammarBuilder;
        _tokens = tokens;
        
        CalculateInitialTerminals();
    }

    private void CalculateInitialTerminals()
    {
        foreach (var terminal in _grammarBuilder.Terminals)
            _initialTerminalsByTokens.Add(terminal, new[] { terminal });
        
        foreach (var nonterminal in _grammarBuilder.Nonterminals)
            if (!_initialTerminalsByTokens.ContainsKey(nonterminal))
                CalculateInitialTerminalsForNonterminal(nonterminal);
    }

    private void CalculateInitialTerminalsForNonterminal(Nonterminal nonterminal)
    {
        var rules = _grammarBuilder.GetRulesByLeftSide(nonterminal);

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
        var initialState = new State();
        initialState.AddItem(new StateItem(_grammarBuilder.InitialRule, 0, Terminal.End));
        CloseState(initialState);

        var transitionStates = new Queue<State>();
        
        _states.Add(initialState);
        transitionStates.Enqueue(initialState);

        while (transitionStates.TryDequeue(out var transitionState))
        {
            foreach (var grammarToken in _grammarBuilder.Tokens)
            {
                Transition(transitionState, grammarToken);
            }
        }
    }

    private State Transition(State sourceState, IGrammarToken token)
    {
        var destinationState = new State();
        
        foreach (var item in sourceState.GetItemsByFirstUnrecognizedToken(token))
        {
            destinationState.AddItem(new StateItem(item));
        }
        
        CloseState(destinationState);

        return destinationState;
    }

    private void CloseState(State state)
    {
        while (true)
        {
            var numberAddedItems = 0;
            
            foreach (var item in state.GetItemsWhereFirstUnrecognizedTokenIsNonterminal())
            {
                var nonterminal = item.UnrecognizedPart[0];
                foreach (var initialTerminal in _initialTerminalsByTokens[item.GetSecondUnrecognizedTokenOrReductionTerminal()])
                {
                    foreach (var rule in _grammarBuilder.GetRulesByLeftSide((Nonterminal)nonterminal))
                    {
                        if (state.AddItem(new StateItem(rule, 0, initialTerminal)))
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

    private readonly Lr1Grammar.Lr1GrammarBuilder _grammarBuilder;

    private readonly IEnumerable<StringToken> _tokens;

    private readonly Dictionary<IGrammarToken, IEnumerable<Terminal>> _initialTerminalsByTokens = new();

    private readonly List<State> _states = new();
}