using Lr1Parser.Parsers;

// ReSharper disable PossibleMultipleEnumeration

namespace Lr1Parser;

internal static class Program
{
    private static int Main()
    {
        try
        {
            var grammarParser = new GrammarParser(File.ReadAllText("../../../Input/Grammar.txt"));
            var grammar = grammarParser.Parse();
        
            grammar.Log();

            var tokenParser = new TokenParser(File.ReadAllText("../../../Input/Sequence.txt"), File.ReadAllText("../../../Input/SpecialCharacters.txt"), grammar);
        
            var tokens = tokenParser.StringToTokens();

            var tokenFile = new StreamWriter("../../../Logging/Tokens.txt", false);
        
            foreach (var token in tokens)
                tokenFile.WriteLine($"{token.Value.Value} {token.IndexInSource}");
        
            tokenFile.Dispose();
            
            var lr1Parser = new Lr1Parser.Parsers.Lr1Parser(grammar, tokens);
            
            lr1Parser.Log();

            Console.Write("Нажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.Write($"{Environment.NewLine}Нажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 1;
        }
    }
}