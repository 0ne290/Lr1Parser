namespace Lr1Parser;

public class Terminal : IGrammarToken
{
    public Terminal(string value) => Value = value;

    public bool IsEmpty() => this == Empty;
    
    public string Value { get; set; }

    public static Terminal Empty { get; } = new(string.Empty);
}
