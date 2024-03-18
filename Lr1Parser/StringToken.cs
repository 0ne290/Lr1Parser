namespace Lr1Parser;

public class StringToken
{
    public Terminal Value { get; set; } = Terminal.Default;
    
    public int IndexInSource { get; set; }
}