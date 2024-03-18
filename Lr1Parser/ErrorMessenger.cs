namespace Lr1Parser;

public class ErrorMessenger
{
    public ErrorMessenger(string source) => _source = source;

    public Tuple<int, int> GetErrorMessageByStringToken(int j)
    {
        var lineCounter = 1;
        var charCounter = 1;
        
        for (var i = 0; i < j; i++)
        {
            if (_source[i] == '\n')
            {
                charCounter = 1;
                lineCounter++;
            }
            else
                charCounter++;
        }

        return new Tuple<int, int>(lineCounter, charCounter);
    }

    private readonly string _source;
}