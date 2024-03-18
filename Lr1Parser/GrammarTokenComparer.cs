public class GrammarTokenComparer : IEqualityComparer<IGrammarToken>
{
    public bool Equals(IGrammarToken x, IGrammarToken y)
    {
        if (Object.ReferenceEquals(x, y))
          return true;

        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;

        return x.Value == y.Value;
    }

    public int GetHashCode(IGrammarToken grammarToken)
    {
        if (Object.ReferenceEquals(grammarToken, null))
          return 0;

        return grammarToken.Value == null ? 0 : grammarToken.Value.GetHashCode();
    }
}
