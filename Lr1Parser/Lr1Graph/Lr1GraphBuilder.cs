using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Lr1Graph;

public class Lr1GraphBuilder
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Lr1GraphBuilder(Lr1Grammar.Lr1Grammar grammar) => Grammar = grammar;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    public IEnumerable<State> Build()
    {
        _states = new List<State>();
        _transitionStates = new Queue<State>();
        
        var initialState = new State();
        initialState.AddItem(new StateItem(Grammar.InitialRule, 0, Grammar.FinalTerminal));
        CloseState(initialState);
        
        _states.Add(initialState);
        _transitionStates.Enqueue(initialState);

        while (_transitionStates.TryDequeue(out var transitionState))
        {
            foreach (var grammarToken in Grammar.Tokens)
            {
                var destinationState = Transition(transitionState, grammarToken);

                if (!destinationState.IsEmpty())
                    transitionState.TryAddTransition(grammarToken, destinationState);
            }
        }

        return _states;
    }

    private State Transition(State sourceState, IGrammarToken token)
    {
        var destinationState = new State();
        
        foreach (var item in sourceState.GetItemsByFirstUnrecognizedToken(token))
            destinationState.AddItem(new StateItem(item));
        
        CloseState(destinationState);

        if (destinationState.IsEmpty())
            return destinationState;

        var res = _states.Find(s => s.Equals(destinationState));

        if (res != null)
            return res;
        
        _states.Add(destinationState);
        _transitionStates.Enqueue(destinationState);
        
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
                foreach (var initialTerminal in Grammar.GetInitialTerminalsByToken(item.GetSecondUnrecognizedTokenOrReductionTerminal()))
                {
                    foreach (var rule in Grammar.GetRulesByLeftSide((Nonterminal)nonterminal))
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
    
    public Lr1Grammar.Lr1Grammar Grammar { get; set; }

    private List<State> _states;

    private Queue<State> _transitionStates;
}