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

    public override string ToString()
    {
        var res = new StringBuilder(512);
        
        res.Append($"Терминалы:{Environment.NewLine + Environment.NewLine}");
        foreach (var terminal in Terminals)
            res.Append(terminal.Value + Environment.NewLine);
        
        res.Append($"{Environment.NewLine}Нетерминалы:{Environment.NewLine + Environment.NewLine}");
        foreach (var nonterminal in Nonterminals)
            res.Append(nonterminal.Value + Environment.NewLine);
        
        res.Append($"{Environment.NewLine}Правила:{Environment.NewLine}");
        foreach (var rule in Rules)
        {
            res.Append($"{Environment.NewLine + rule.LeftSide.Value} =");

            foreach (var t in rule.RightSide)
                res.Append($" {t.Value}");
        }
        
        return res.ToString();
    }

    public IEnumerable<Rule> Rules { get; }

    public IEnumerable<IGrammarToken> Tokens { get; }

    public IEnumerable<Terminal> Terminals { get; }

    public IEnumerable<Nonterminal> Nonterminals { get; }

    public Rule InitialRule { get; }

    public Nonterminal InitialNonterminal { get; }

    public static string LogPath { get; set; } = "../../../Logging/Grammar.txt";
}