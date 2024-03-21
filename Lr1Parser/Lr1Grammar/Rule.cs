namespace Lr1Parser.Lr1Grammar;

public class Rule
{
    public Rule(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide.ToArray();
    }
    
    public readonly Nonterminal LeftSide;
    
    public readonly IReadOnlyList<IGrammarToken> RightSide;
}
