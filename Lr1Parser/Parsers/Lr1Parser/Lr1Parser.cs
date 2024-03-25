using Lr1Parser.Parsers.Lr1Parser.Lr1Graph;
using Lr1Parser.Parsers.Lr1Parser.Lr1Table;

namespace Lr1Parser.Parsers.Lr1Parser;

public class Lr1Parser
{
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Lr1Parser(string source, string specialCharacters, Lr1Grammar.Lr1Grammar grammar)
    {
        _source = source.Replace("\0", string.Empty) + '\0';
        _specialCharacters = specialCharacters;
        _grammar = grammar;
        
        _tokenParser = new TokenParser(_source, _specialCharacters, _grammar);
        _graphBuilder = new Lr1GraphBuilder(_grammar);
        _tableBuilder = new Lr1TableBuilder { Grammar = _grammar };
    }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public void Parse()
    {
        var tokens = _tokenParser.Parse();
        var graph = _graphBuilder.Build();

        var i = 0;
        foreach (var state in graph)
        {
            Console.Write($"State {i}; ");
            i++;
            state.Log();
        }
        
        var tokenFile = new StreamWriter("../../../Logging/Tokens.txt", false);
        
        foreach (var token in tokens)
            tokenFile.WriteLine($"{token.Value.Value} {token.IndexInSource}");
        
        tokenFile.Dispose();

        _tableBuilder.Graph = graph;

        var table = _tableBuilder.Build();
    }
    
    public string Source
    {
        get => _source;
        set
        {
            _source = value.Replace("\0", string.Empty) + '\0';
            _tokenParser.Source = _source;
        }
    }

    public string SpecialCharacters
    {
        get => _specialCharacters;
        set
        {
            _specialCharacters = value;
            _tokenParser.SpecialCharacters = _specialCharacters;
        }
    }
    
    public Lr1Grammar.Lr1Grammar Grammar
    {
        get => _grammar;
        set
        {
            _grammar = value;
            _tokenParser.Grammar = _grammar;
            _graphBuilder.Grammar = _grammar;
            _tableBuilder.Grammar = _grammar;
        }
    }

    private string _source;

    private string _specialCharacters;

    private Lr1Grammar.Lr1Grammar _grammar;
    
    private readonly TokenParser _tokenParser;

    private readonly Lr1GraphBuilder _graphBuilder;

    private readonly Lr1TableBuilder _tableBuilder;
}