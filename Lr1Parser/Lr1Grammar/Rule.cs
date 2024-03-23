namespace Lr1Parser.Lr1Grammar;

public class Rule
{
    public Rule(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide.ToArray();
    }
    
    public Nonterminal LeftSide { get; }
    
    public IReadOnlyList<IGrammarToken> RightSide { get; }
}
