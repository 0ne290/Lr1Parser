namespace Lr1Parser.Logger;

public class Catalog
{
    public Catalog(IDictionary<string, Log> logs) => Logs = logs;

    public IDictionary<string, Log> Logs { get; set; }
}