using System.Text;

namespace Lr1Parser.Lr1Grammar;

public class Lr1Grammar
{
    public Lr1Grammar(IEnumerable<Rule> rules, IEnumerable<IGrammarToken> tokens, IEnumerable<Terminal> terminals,
        IEnumerable<Nonterminal> nonterminals, Rule initialRule, Nonterminal initialNonterminal)
    {
        Rules = rules;
        Tokens = tokens;
        Terminals = terminals;
        Nonterminals = nonterminals;
        InitialRule = initialRule;
        InitialNonterminal = initialNonterminal;
    }

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
            rulesFile.Write($"{rule.LeftSide.Value} =");

            foreach (var t in rule.RightSide)
            {
                rulesFile.Write($" {t.Value}");
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

    public IEnumerable<Rule> Rules { get; }

    public IEnumerable<IGrammarToken> Tokens { get; }

    public IEnumerable<Terminal> Terminals { get; }

    public IEnumerable<Nonterminal> Nonterminals { get; }

    public Rule InitialRule { get; }

    public Nonterminal InitialNonterminal { get; }
}
