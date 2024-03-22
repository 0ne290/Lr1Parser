namespace Lr1Parser.Lr1Grammar;

public class Nonterminal : IGrammarToken
{
    public Nonterminal(string value) => Value = value;
    
    public bool IsEmpty() => this == Empty;
    
    public string Value { get; set; }

    public static Nonterminal Empty { get; } = new(string.Empty);
}