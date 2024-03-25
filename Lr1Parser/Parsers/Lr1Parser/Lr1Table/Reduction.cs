using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser.Lr1Table;

public class Reduction : ITableAction
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Nonterminal Nonterminal { get; init; }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}