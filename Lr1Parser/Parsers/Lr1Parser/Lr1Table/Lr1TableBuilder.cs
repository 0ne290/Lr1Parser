using Lr1Parser.Parsers.Lr1Parser.Lr1Graph;

namespace Lr1Parser.Parsers.Lr1Parser.Lr1Table;

public class Lr1TableBuilder
{
    public Lr1Table Build()
    {
        var table = new Lr1Table();

        foreach (var state in Graph)
        {
            foreach (var item in state.GetItemsWhereUnrecognizedPartIsNotEmpty())
            {
                if (state.TryPerformTransition(item.GetFirstUnrecognizedToken(), out var destinationState))
                {
                    table[state, item.GetFirstUnrecognizedToken()] = new Shift { State = destinationState! };
                }
            }

            foreach (var item in state.GetItemsWhereUnrecognizedPartIsEmpty())
            {
                if (item.Rule == Grammar.InitialRule)
                {
                    if (item.ReductionTerminal != Grammar.FinalTerminal)
                        throw new Exception("Этой ошибки быть не должно.");

                    table[state, item.ReductionTerminal] = new Halt();
                }
                else
                {
                    table[state, item.ReductionTerminal] = new Reduction
                        { Nonterminal = item.Rule.LeftSide, NumberOfReducedTokens = item.Rule.RightSide.Count };
                }
            }
        }

        return table;
    }

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Lr1Grammar.Lr1Grammar Grammar { get; set; }

    
    public IEnumerable<State> Graph { get; set; }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}