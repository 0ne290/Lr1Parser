namespace Lr1Parser.Logger;

public class Log
{
    public Log(StreamWriter file) => File = file;
    
    public StreamWriter File { get; set; }
    
    public void Register(object logged)
    {
        lock (_locker)
        {
            File.WriteLine(logged + Environment.NewLine);
        }
    }

    private static object _locker = new();
}