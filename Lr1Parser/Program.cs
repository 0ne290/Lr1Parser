namespace Lr1Parser;

internal static class Program
{
    private static int Main()
    {
        /*try
        {
            // Work

            Console.Write("Нажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.Write("\nНажмите любую клавишу для завершения программы...");
            Console.ReadKey();

            return 1;
        }*/

        var grammarParser = new GrammarParser(File.ReadAllText("../../../Grammar.txt"));
        var grammar = grammarParser.Parse();

        var tokenParser = new TokenParser(File.ReadAllText("../../../Input.txt"), File.ReadAllText("../../../SpecialCharacters.txt"), grammar);

        //foreach (var t in grammar.GetKeywords())
        //{
        //    Console.WriteLine(t.Value);
        //}

        var tokens = tokenParser.StringToTokens();
        
        foreach (var token in tokens)
        {
            Console.WriteLine($"{token.Value.Value} {token.IndexInSource}");
        }

        return 0;
    }
}