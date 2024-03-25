namespace Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

public interface IGrammarToken
{
    bool IsEmpty();
    
    string Value { get; }
}