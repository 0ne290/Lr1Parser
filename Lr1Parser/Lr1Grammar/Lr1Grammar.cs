namespace Lr1Parser.Lr1Grammar;

public class Lr1Grammar
{
    public Lr1Grammar(IEnumerable<Rule> rules, IEnumerable<IGrammarToken> tokens, IEnumerable<Terminal> terminals,
        IEnumerable<Nonterminal> nonterminals, Rule initialRule, Nonterminal initialNonterminal, Terminal finalTerminal)
    {
        Rules = rules;
        Tokens = tokens;
        Terminals = terminals;
        Nonterminals = nonterminals;
        InitialRule = initialRule;
        InitialNonterminal = initialNonterminal;
        FinalTerminal = finalTerminal;
        
        CalculateInitialTerminals();
    }
    
    private void CalculateInitialTerminals()
    {
        foreach (var terminal in Terminals)
            _initialTerminalsByTokens.Add(terminal, new[] { terminal });
        
        foreach (var nonterminal in Nonterminals)
            if (!_initialTerminalsByTokens.ContainsKey(nonterminal))
                CalculateInitialTerminalsForNonterminal(nonterminal);
    }

    private void CalculateInitialTerminalsForNonterminal(Nonterminal nonterminal)
    {
        var rules = GetRulesByLeftSide(nonterminal);

        var initialTerminalsForNonterminal = new List<Terminal>();
        foreach (var rule in rules)
        {
            if (rule.RightSide[0] == nonterminal)
                continue;
            
            if (_initialTerminalsByTokens.TryGetValue(rule.RightSide[0], out var initialTerminals))
                initialTerminalsForNonterminal.AddRange(initialTerminals);
            else
            {
                CalculateInitialTerminalsForNonterminal((Nonterminal)rule.RightSide[0]);

                if (_initialTerminalsByTokens.TryGetValue(rule.RightSide[0], out initialTerminals))
                    initialTerminalsForNonterminal.AddRange(initialTerminals);
                else
                    throw new Exception(
                        "Во время построения множеств символов-предшественников произошла ошибка. Обратитесь к разработчику.");
            }
        }
        
        _initialTerminalsByTokens.Add(nonterminal, initialTerminalsForNonterminal.Distinct());
    }

    public void Log()
    {
        LogNonterminals();
        LogTerminals();
        LogInitialTerminals();
        LogRules();
    }

    private void LogRules()
    {
        var rulesFile = new StreamWriter("../../../Logging/Rules.txt", false);

        foreach (var rule in Rules)
        {
            rulesFile.Write($"{rule.LeftSide.Value} =");

            foreach (var t in rule.RightSide)
            {
                rulesFile.Write($" {t.Value}");
            }

            rulesFile.WriteLine();
        }

        rulesFile.Dispose();
    }
    
    private void LogInitialTerminals()
    {
        var initialTerminalsFile = new StreamWriter("../../../Logging/InitialTerminals.txt", false);
        
        foreach (var initialTerminalsPair in _initialTerminalsByTokens)
        {
            initialTerminalsFile.Write($"{initialTerminalsPair.Key.Value} = {{ ");
            
            foreach (var initialTerminal in initialTerminalsPair.Value)
                initialTerminalsFile.Write($"{initialTerminal.Value} ");
            
            initialTerminalsFile.WriteLine("}");
        }
        
        initialTerminalsFile.Dispose();
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
    
    public IEnumerable<Rule> GetRulesByLeftSide(Nonterminal leftSide) => Rules.Where(r => r.LeftSide == leftSide);

    public IEnumerable<Terminal> GetInitialTerminalsByToken(IGrammarToken token) =>
        _initialTerminalsByTokens.TryGetValue(token, out var initialTerminals)
            ? initialTerminals
            : Enumerable.Empty<Terminal>();

    public Terminal GetTerminalByValue(string value) =>
        Terminals.FirstOrDefault(t => t.Value == value, Terminal.Empty);

    public IEnumerable<Terminal> GetKeywords() => Terminals.Where(t => t.Value.Length > 1);

    public IEnumerable<Rule> Rules { get; }

    public IEnumerable<IGrammarToken> Tokens { get; }
    
    public IEnumerable<Terminal> Terminals { get; }

    public IEnumerable<Nonterminal> Nonterminals { get; }

    public Rule InitialRule { get; }

    public Nonterminal InitialNonterminal { get; }
    
    public Terminal FinalTerminal { get; }
    
    private readonly Dictionary<IGrammarToken, IEnumerable<Terminal>> _initialTerminalsByTokens = new();
}