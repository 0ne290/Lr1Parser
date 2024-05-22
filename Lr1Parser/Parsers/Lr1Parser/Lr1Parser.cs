using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;
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
        _nameParser = new NameParser();
    }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public void Parse(bool valideNames)
    {
        var tokens = _tokenParser.Parse().ToArray();
        var graph = _graphBuilder.Build().ToArray();

        var tokenFile = new StreamWriter("../../../Logging/Tokens.txt", false);
        
        foreach (var t in tokens)
            tokenFile.WriteLine($"{(t.Value.Value == Grammar.FinalTerminal.Value ? "\\0" : t.StringValue)} {t.IndexInSource}");
        
        tokenFile.Dispose();

        _tableBuilder.Graph = graph;

        var table = _tableBuilder.Build();

        var tokenStack = new Stack<IGrammarToken>();
        var stateStack = new Stack<State>();
        stateStack.Push(graph[0]);

        var i = 0;
        IGrammarToken token = tokens[i].Value;
        while (true)
        {
            ITableAction tableAction;
            try
            {
                tableAction = table[stateStack.Peek(), token];
            }
            catch
            {
                if (tokens[i].Value == _grammar.FinalTerminal)
                    break;
                
                var position = Source.GetPosition(tokens[i].IndexInSource);
                throw new Exception(
                    $"Неожиданный токен \"{tokens[i].StringValue}\". Номер строки: {position.LineNumber}, номер токена: {position.CharNumber - 1}. Вместо этого токена ожидался один из: {{ {GetExpectedTerminals(table, stateStack)} }}.");
            }

            if (tableAction is Shift shift)
            {
                stateStack.Push(shift.State);
                tokenStack.Push(token);
                i++;

                if (i == tokens.Length)
                    break;
                
                token = tokens[i].Value;
            }
            else if (tableAction is Reduction reduction)
            {
                for (var j = 0; j < reduction.NumberOfReducedTokens; j++)
                {
                    tokenStack.Pop();
                    stateStack.Pop();
                }

                i--;

                token = reduction.Nonterminal;
            }
            else if (tableAction is Halt)
            {
                if (valideNames)
                    _nameParser.Parse(tokens, _source);
                
                return;
            }
        }

        throw new Exception($"Следующим токеном ожидался один из: {{ {GetExpectedTerminals(table, stateStack)} }}.");
    }

    private string GetExpectedTerminals(Lr1Table.Lr1Table table, Stack<State> stateStack)
    {
        var tokensByState = table.GetTokensByState(stateStack.Peek());

        var expectedTerminals = new List<Terminal>();

        foreach (var c in tokensByState)
            expectedTerminals.AddRange(_grammar.GetInitialTerminalsByToken(c));

        var expectedTerminalsWithoutDuplicates = expectedTerminals.Distinct().Select(t => "\"" + t.Value + "\"");

        return string.Join(", ", expectedTerminalsWithoutDuplicates);
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

    private readonly NameParser _nameParser;
}