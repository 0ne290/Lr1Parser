namespace Lr1Parser;

public class Lr1Grammar
{
    public bool AddRule(Rule rule)
    {
        if (_rules.Contains(rule))
            return false;
        if (_rules.Exists(r => r.LeftSide == rule.LeftSide && r.RightSide.SequenceEqual(rule.RightSide, new GrammarTokenComparer())))
            return false;
        
        _rules.Add(rule);
        
        return true;
    }

    public IEnumerable<Rule> GetRulesByLeftSides(Nonterminal leftSide) => _rules.Where(r => r.LeftSide == leftSide);
    
    public bool AddTerminal(Terminal terminal)
    {
        if (_terminals.Contains(terminal))
            return false;
        if (_terminals.Exists(t => t.Value == terminal.Value))
            return false;
        
        _terminals.Add(terminal);
        
        return true;
    }

    public Terminal GetTerminalByValue(string value) =>
        _terminals.Find(t => t.Value == value) ?? Terminal.Empty;
    
    public bool AddNonterminal(Nonterminal nonterminal)
    {
        if (_nonterminals.Contains(nonterminal))
            return false;
        if (_nonterminals.Exists(n => n.Value == nonterminal.Value))
            return false;
        
        _nonterminals.Add(nonterminal);
        
        return true;
    }

    public Nonterminal GetNonterminalByValue(string value) =>
        _nonterminals.Find(n => n.Value == value) ?? Nonterminal.Empty;

    private readonly List<Rule> _rules = new();

    private readonly List<Terminal> _terminals = new();

    private readonly List<Nonterminal> _nonterminals = new();
}