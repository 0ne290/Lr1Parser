namespace Lr1Parser;

public class Nonterminal : IGrammarToken
{
    private Nonterminal(string value) => Value = value;

    public static bool AddNonterminalByValue(string value)
    {
        if (!Nonterminals.Exists(n => n.Value == value))
        {
            Nonterminals.Add(new Nonterminal(value));
            return true;
        }

        return false;
    }

    public static Nonterminal GetNonterminalByValue(string value) =>
        Nonterminals.Find(n => n.Value == value) ?? Empty;

    public bool IsEmpty() => this == Empty;
    
    public string Value { get; set; }

    public static Nonterminal Empty { get; } = new(string.Empty);

    private static readonly List<Nonterminal> Nonterminals = new();
}
