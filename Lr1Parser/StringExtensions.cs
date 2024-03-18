namespace Lr1Parser;

public static class StringExtensions
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
}