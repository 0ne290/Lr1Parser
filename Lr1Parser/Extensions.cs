namespace Lr1Parser;

public static class Extensions
{
    public static (int LineNumber, int CharNumber) GetPosition(this string text, int index)
    {
        var lineCounter = 1;
        var charCounter = 1;
        
        for (var i = 0; i < index; i++)
        {
            if (text[i] == '\n')
            {
                charCounter = 1;
                lineCounter++;
            }
            else
                charCounter++;
        }

        return (lineCounter, charCounter);
    }
    
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

        return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
    }
}