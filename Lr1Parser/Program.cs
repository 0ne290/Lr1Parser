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

            var lr1Parser = new Parsers.Lr1Parser.Lr1Parser(File.ReadAllText("../../../Input/Sequence.txt"), File.ReadAllText("../../../Input/SpecialCharacters.txt"), grammar);
            
            lr1Parser.Parse();

            Console.Write("Входная последовательность соответствует грамматике. Нажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 0;
        }
        catch (Exception e)
        {
            Console.Write($"{e.Message} Нажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 1;
        }
    }
}