namespace Lr1Parser;

public class Rule
{
    public Rule(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide;
    }
    
    public readonly Nonterminal LeftSide;
    
    public readonly IEnumerable<IGrammarToken> RightSide;
}
