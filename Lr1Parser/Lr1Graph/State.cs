using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Lr1Graph;

public class State
{
    public bool Equals(State state) => _items.SequenceEquivalent(state._items);

    public bool IsEmpty() => _items.Count < 1;
    
    public bool AddItem(StateItem item)
    {
        if (_items.Exists(i => i.Equals(item)))
            return false;
        
        _items.Add(item);

        return true;
    }

    public IEnumerable<StateItem> GetItemsWhereFirstUnrecognizedTokenIsNonterminal() =>
        _items.FindAll(i => i.FirstUnrecognizedTokenIsNonterminal());
    
    public IEnumerable<StateItem> GetItemsByFirstUnrecognizedToken(IGrammarToken token) =>
        _items.Where(i => i.FirstUnrecognizedTokenEquals(token));

    public bool TryAddTransition(IGrammarToken token, State destinationState) => _transitions.TryAdd(token, destinationState);

    public State PerformTransition(IGrammarToken token) =>
        _transitions.TryGetValue(token, out var state) ? state : this;

    private readonly List<StateItem> _items = new();

    private readonly Dictionary<IGrammarToken, State> _transitions = new();
}