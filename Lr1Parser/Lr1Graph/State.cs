using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Lr1Graph;

public class State
{
    public bool AddItem(StateItem item)
    {
        if (Items.Exists(i => i.Equals(item)))
            return false;
        
        Items.Add(item);

        return true;
    }

    public IEnumerable<StateItem> GetItemsWhereFirstUnrecognizedTokenIsNonterminal() =>
        Items.Where(i => i.FirstUnrecognizedTokenIsNonterminal());
    
    public IEnumerable<StateItem> GetItemsByFirstUnrecognizedToken(IGrammarToken token) =>
        Items.Where(i => i.FirstUnrecognizedTokenEquals(token));

    public List<StateItem> Items { get; } = new();

    public Dictionary<Terminal, State> Transitions { get; } = new();
}