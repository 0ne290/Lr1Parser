namespace Lr1Parser;

public class StringToken
{
    public Terminal Value { get; set; } = Terminal.Empty;
    
    public int IndexInSource { get; set; }
}