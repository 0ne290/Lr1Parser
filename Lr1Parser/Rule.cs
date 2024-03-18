namespace Lr1Parser;

public class Rule
{
    public Rule(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide;
        
        Rules.Add(this);
    }

    public IEnumerable<Rule> GetRulesByLeftSides(Nonterminal leftSide) => Rules.Where(r => r.LeftSide == leftSide);
    
    public readonly Nonterminal LeftSide;
    
    public readonly IEnumerable<IGrammarToken> RightSide;

    private static readonly List<Rule> Rules = new();
}