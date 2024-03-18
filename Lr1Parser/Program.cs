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

        var tokenParser = new TokenParser();

        var tokens = tokenParser.StringToTokens("public struct StructName\n{\n\tstruct StructName1\n\t{\n\t\tinternal bool Zf;\n\t}\n\t\n\tprivate int[,]? Name;\n}");

        foreach (var token in tokens)
        {
            Console.WriteLine($"{token.Value} {token.IndexInSource}");
        }

        return 0;
    }
}