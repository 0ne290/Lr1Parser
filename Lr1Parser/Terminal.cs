namespace Lr1Parser;

public class Terminal : IGrammarToken
{
    private Terminal(string value) => Value = value;

    public static bool AddTerminalByValue(string value)
    {
        if (!Terminals.Exists(t => t.Value == value))
        {
            Terminals.Add(new Terminal(value));
            return true;
        }

        return false;
    }

    public static Terminal GetTerminalByValue(string value) =>
        Terminals.Find(t => t.Value == value) ?? Empty;

    public bool IsEmpty() => this == Empty;
    
    public string Value { get; set; }

    public static Terminal Empty { get; } = new(string.Empty);

    private static readonly List<Terminal> Terminals = new();
}
