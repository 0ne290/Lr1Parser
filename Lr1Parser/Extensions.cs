using System.Text.RegularExpressions;

namespace Lr1Parser;

public static class Extensions
{
    public static (int LineNumber, int CharNumber) GetPosition(this string text, int index)
    {
        var lineCounter = 1;
        var charCounter = 1;
        
        for (var i = 0; i < index; i++)
        {
            if (text[i..(i + Environment.NewLine.Length)] == Environment.NewLine)
            {
                charCounter = 1;
                i += Environment.NewLine.Length - 1;
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
    
    public static bool SequenceEqual<T>(this IReadOnlyList<T> sequence1, IReadOnlyList<T> sequence2) where T : class
    {
        if (sequence1.Count != sequence2.Count)
            return false;

        for (var i = 0; i < sequence1.Count; i++)
            if (sequence1[i] != sequence2[i])
                return false;

        return true;
    }

    public static string Escape(this string pattern, bool escapeClosingSquareBracket = false, bool escapeClosingCurlyBrace = false)
    {
        var res = Regex.Escape(pattern);

        if (escapeClosingSquareBracket)
            res = res.Replace("]", @"\]");
        
        if (escapeClosingCurlyBrace)
            res = res.Replace("}", @"\}");

        return res;
    }
}