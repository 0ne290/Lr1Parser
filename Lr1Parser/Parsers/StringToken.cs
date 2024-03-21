using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers;

public class StringToken
{
    public Terminal Value { get; set; } = Terminal.Empty;
    
    public int IndexInSource { get; set; }
}