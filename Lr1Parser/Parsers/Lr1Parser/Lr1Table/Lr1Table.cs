using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;
using Lr1Parser.Parsers.Lr1Parser.Lr1Graph;

namespace Lr1Parser.Parsers.Lr1Parser.Lr1Table;

public class Lr1Table
{
    public ITableAction this[State state, IGrammarToken token]
    {
        get => _table[Hash(state, token)];
        set
        {
            if (_table.TryGetValue(Hash(state, token), out var action))
            {
                switch (action)
                {
                    case Reduction when value is Shift:
                        _table[Hash(state, token)] = value;
                        break;
                    case Shift when value is Reduction:
                        break;
                    case Shift oldShift when value is Shift newShift:
                        if (oldShift.State != newShift.State)
                            throw new Exception("Lr1-грамматика составлена неверно.");
                        break;
                    case Reduction oldReduction when value is Reduction newReduction:
                        if (oldReduction.Nonterminal != newReduction.Nonterminal)
                            throw new Exception("Lr1-грамматика составлена неверно.");
                        break;
                }
            }
            else
                AddAction(state, token, value);
        }
    }
    
    private void AddAction(State state, IGrammarToken token, ITableAction action) =>
            _table.Add(Hash(state, token), action);

    private static int Hash(State state, IGrammarToken token) => HashCode.Combine(state, token);
    
    private readonly Dictionary<int, ITableAction> _table = new();
}