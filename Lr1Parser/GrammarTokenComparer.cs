namespace Lr1Parser;

public class GrammarTokenComparer : IEqualityComparer<IGrammarToken>
{
    public bool Equals(IGrammarToken? x, IGrammarToken? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Value == y.Value;
    }

    public int GetHashCode(IGrammarToken grammarToken) => grammarToken.Value.GetHashCode();
}