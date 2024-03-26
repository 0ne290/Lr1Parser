namespace Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

public class Lr1GrammarBuilder
{
    public Lr1GrammarBuilder(Nonterminal initialNonterminal)
    {
        _nonterminals.Add(Nonterminal.Initial);
        _terminals.Add(Terminal.Final);
        _tokens.Add(_nonterminals[0]);
        _tokens.Add(_terminals[0]);

        _nonterminals.Add(initialNonterminal);
        _tokens.Add(_nonterminals[1]);

        //_rules.Add(new Rule(_nonterminals[0], new[] { _nonterminals[1] }));

        _initialRule = new Rule(_nonterminals[0], new[] { _nonterminals[1] });
    }

    private readonly Rule _initialRule;
    
    public Lr1Grammar Build() => new(_rules, _tokens, _terminals, _nonterminals, _initialRule, _nonterminals[0], _terminals[0]);
    
    public bool AddRule(Rule rule)
    {
        if (_rules.Contains(rule))
            return false;
        if (_rules.Exists(r => r.LeftSide == rule.LeftSide && r.RightSide.SequenceEqual(rule.RightSide)))
            return false;
        
        _rules.Add(rule);
        
        return true;
    }
    
    public bool AddTerminal(Terminal terminal)
    {
        if (terminal.Value == "\0")
            throw new Exception(
                "В грамматике не должно содержаться терминала \"\\0\" - он зарезервирован системой в качестве маркера конца анализируемого текста.");
        
        if (_terminals.Contains(terminal))
            return false;
        if (_terminals.Exists(t => t.Value == terminal.Value))
            return false;
        
        _terminals.Add(terminal);
        _tokens.Add(terminal);
        
        return true;
    }

    public Terminal GetTerminalByValue(string value) =>
        _terminals.Find(t => t.Value == value) ?? Terminal.Empty;
    
    public bool AddNonterminal(Nonterminal nonterminal)
    {
        if (nonterminal.Value == "InitialNonterminal")
            throw new Exception(
                "В грамматике не должно содержаться нетерминала \"InitialNonterminal\" - он зарезервирован системой для создания искусственных начальных нетерминала и правила.");
        
        if (_nonterminals.Contains(nonterminal))
            return false;
        if (_nonterminals.Exists(n => n.Value == nonterminal.Value))
            return false;
        
        _nonterminals.Add(nonterminal);
        _tokens.Add(nonterminal);
        
        return true;
    }

    public Nonterminal GetNonterminalByValue(string value) =>
        _nonterminals.Find(n => n.Value == value) ?? Nonterminal.Empty;

    private readonly List<Rule> _rules = new();

    private readonly List<IGrammarToken> _tokens = new();

    private readonly List<Terminal> _terminals = new();

    private readonly List<Nonterminal> _nonterminals = new();
}
