namespace Lr1Parser;

public interface IGrammarToken
{
    bool IsEmpty();
    
    string Value { get; set; }
}