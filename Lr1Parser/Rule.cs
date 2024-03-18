namespace Lr1Parser;

public class Rule
{
    private Rule(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide;
    }

    public bool AddRuleBySides(Nonterminal leftSide, IEnumerable<IGrammarToken> rightSide)
    {
        if (!Rules.Exists(r => r.LeftSide == leftSide && r.RightSide.SequanceEqual(rightSide, new GrammarTokenComparer())))
        {
            Rules.Add(new Rule(leftSide, rightSide));
            return true;
        }

        return false;
    }

    public IEnumerable<Rule> GetRulesByLeftSides(Nonterminal leftSide) => Rules.Where(r => r.LeftSide == leftSide);
    
    public readonly Nonterminal LeftSide;
    
    public readonly IEnumerable<IGrammarToken> RightSide;

    private static readonly List<Rule> Rules = new();
}
