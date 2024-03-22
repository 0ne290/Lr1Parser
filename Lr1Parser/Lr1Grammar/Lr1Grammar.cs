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

        foreach (var rule in Rules)
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
        
        foreach (var terminal in Terminals)
            terminalsFile.WriteLine(terminal.Value);
        
        terminalsFile.Dispose();
    }
    
    private void LogNonterminals()
    {
        var nonterminalsFile = new StreamWriter("../../../Logging/Nonterminals.txt", false);
        
        foreach (var nonterminal in Nonterminals)
            nonterminalsFile.WriteLine(nonterminal.Value);
        
        nonterminalsFile.Dispose();
    }
    
    public bool AddRule(Rule rule)
    {
        if (Rules.Contains(rule))
            return false;
        if (Rules.Exists(r => r.LeftSide == rule.LeftSide && r.RightSide.SequenceEqual(rule.RightSide)))
            return false;
        
        Rules.Add(rule);
        
        return true;
    }

    public IEnumerable<Rule> GetRulesByLeftSide(Nonterminal leftSide) => Rules.Where(r => r.LeftSide == leftSide);
    
    public bool AddTerminal(Terminal terminal)
    {
        if (Terminals.Contains(terminal))
            return false;
        if (Terminals.Exists(t => t.Value == terminal.Value))
            return false;
        
        Terminals.Add(terminal);
        
        return true;
    }

    public Terminal GetTerminalByValue(string value) =>
        Terminals.Find(t => t.Value == value) ?? Terminal.Empty;

    public IEnumerable<Terminal> GetKeywords() => Terminals.FindAll(t => t.Value.Length > 1);
    
    public bool AddNonterminal(Nonterminal nonterminal)
    {
        if (Nonterminals.Contains(nonterminal))
            return false;
        if (Nonterminals.Exists(n => n.Value == nonterminal.Value))
            return false;
        
        Nonterminals.Add(nonterminal);
        
        return true;
    }

    public Nonterminal GetNonterminalByValue(string value) =>
        Nonterminals.Find(n => n.Value == value) ?? Nonterminal.Empty;

    public List<Rule> Rules { get; } = new();

    public List<Terminal> Terminals { get; } = new();

    public List<Nonterminal> Nonterminals { get; } = new();
    
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Rule InitialRule { get; set; }
    
    public Nonterminal InitialNonterminal { get; set; }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}