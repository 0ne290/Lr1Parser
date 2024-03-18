namespace Lr1Parser;

public class StringToken
{
    public Terminal Value { get; set; } = Terminal.Default;
    
    public int PositionInSource { get; set; }
}