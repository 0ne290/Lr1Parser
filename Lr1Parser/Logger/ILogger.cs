namespace Lr1Parser.Logger;

public interface ILogger
{
    void Register(object logged, string logName);
}