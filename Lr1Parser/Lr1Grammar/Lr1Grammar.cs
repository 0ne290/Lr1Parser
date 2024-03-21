namespace Lr1Parser.Lr1Grammar;

public class Lr1Grammar
{
    public void Log()
    {
        LogNonterminals();
        LogTerminals();
        LogRules();
    }

    private void LogRules()
    {
        var rulesFile = new StreamWriter("../../../Logging/Rules.txt", false);

        foreach (var rule in _rules)
        {
            rulesFile.Write($"{rule.LeftSide.Value} = ");

            foreach (var t in rule.RightSide)
            {
                rulesFile.Write($"{t.Value} ");
            }

            rulesFile.WriteLine();
        }

        rulesFile.Dispose();
    }

    private void LogTerminals()
    {
        var terminalsFile = new StreamWriter("../../../Logging/Terminals.txt", false);
        
        foreach (var terminal in _terminals)
            terminalsFile.WriteLine(terminal.Value);
        
        terminalsFile.Dispose();
    }
    
    private void LogNonterminals()
    {
        var nonterminalsFile = new StreamWriter("../../../Logging/Nonterminals.txt", false);
        
        foreach (var nonterminal in _nonterminals)
            nonterminalsFile.WriteLine(nonterminal.Value);
        
        nonterminalsFile.Dispose();
    }
    
    public bool AddRule(Rule rule)
    {
        if (_rules.Contains(rule))
            return false;
        if (_rules.Exists(r => r.LeftSide == rule.LeftSide && r.RightSide.SequenceEqual(rule.RightSide)))
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

    public IEnumerable<Terminal> GetKeywords() => _terminals.FindAll(t => t.Value.Length > 1);
    
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