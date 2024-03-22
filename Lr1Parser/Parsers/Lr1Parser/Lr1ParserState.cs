using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser;

public class Lr1ParserState
{
    public bool AddItem(Lr1Item item)
    {
        if (Items.Exists(i => i.Equals(item)))
            return false;
        
        Items.Add(item);

        return true;
    }

    public IEnumerable<Lr1Item> GetItemsWhereFirstUnrecognizedTokenIsNonterminal() =>
        Items.Where(i => i.FirstUnrecognizedTokenIsNonterminal());

    public List<Lr1Item> Items { get; } = new();

    public Dictionary<Terminal, Lr1ParserState> Transitions { get; } = new();
}