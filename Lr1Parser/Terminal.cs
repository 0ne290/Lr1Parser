namespace Lr1Parser;

public class Terminal : IGrammarToken
{
    private Terminal(string value) => Value = value;

    public static void AddTerminalByValue(string value)
    {
        if (!Terminals.Exists(t => t.Value == value))
            Terminals.Add(new Terminal(value));
    }

    public static Terminal? GetTerminalByValue(string value) =>
        Terminals.Find(t => t.Value == value);
    
    public string Value { get; set; }

    public static Terminal Default { get; } = new(string.Empty);

    private static readonly List<Terminal> Terminals = new();
}