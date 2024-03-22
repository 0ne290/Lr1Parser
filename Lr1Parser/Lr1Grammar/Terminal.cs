namespace Lr1Parser.Lr1Grammar;

public class Terminal : IGrammarToken
{
    public Terminal(string value) => Value = value;

    public bool IsEmpty() => this == Empty;
    
    public string Value { get; set; }

    public static Terminal Empty { get; } = new(string.Empty);

    public static Terminal End { get; } = new("\0");
}
