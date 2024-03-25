using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers;

public class StringToken
{
    public IGrammarToken Value { get; set; } = Terminal.Empty;
    
    public int IndexInSource { get; set; }
}