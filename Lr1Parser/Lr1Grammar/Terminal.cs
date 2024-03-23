namespace Lr1Parser.Lr1Grammar;

public class Terminal : IGrammarToken
{
    public Terminal(string value) => Value = value;

    public bool IsEmpty() => this == Empty;
    
    public string Value { get; }

    public static Terminal Empty { get; } = new(string.Empty);

    public static Terminal Final { get; } = new("\0");
}
