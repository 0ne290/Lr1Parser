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
        
        //Console.WriteLine("Терминалы:");
        //foreach (var rule in grammar._terminals)
        //{
        //    Console.WriteLine($"==ZXC=={rule.Value}==ZXC==");
        //}
        //
        //Console.WriteLine("Нетерминалы:");
        //foreach (var rule in grammar._nonterminals)
        //{
        //    Console.WriteLine($"==ZXC=={rule.Value}==ZXC==");
        //}

        foreach (var rule in grammar._rules)
        {
            Console.Write($"{rule.LeftSide.Value} = ");
            
            foreach (var t in rule.RightSide)
            {
                Console.Write($"{t.Value} ");
            }
            
            Console.WriteLine();
        }

        var tokenParser = new TokenParser(File.ReadAllText("../../../Input.txt"), File.ReadAllText("../../../SpecialCharacters.txt"), grammar);

        var tokens = tokenParser.StringToTokens();

        //foreach (var token in tokens)
        //{
        //    Console.WriteLine($"{token.Value.Value} {token.IndexInSource}");
        //}

        return 0;
    }
}