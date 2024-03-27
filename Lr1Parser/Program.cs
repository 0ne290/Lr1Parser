using Lr1Parser.Parsers;

// ReSharper disable PossibleMultipleEnumeration

namespace Lr1Parser;

internal static class Program
{
    private static int Main()
    {
        try
        {
            Console.WriteLine("ВНИМАНИЕ! Образец C#-кода, который описывает грамматика по умолчанию, содержится в файле \"Example.txt\". Проверка имён жёстко зашита в программу и ориентирована на входную последовательность, представляющую C#-код из образца. Если содержащаяся в файле \"Grammar.txt\" грамматика не описывает C#-код из образца, то НИ В КОЕМ СЛУЧАЕ НЕ ВЫПОЛНЯЙТЕ ПРОВЕРКУ ИМЕН (звучит грозно, но на самом деле Вы просто увидите ошибку с вероятностью 99.999%).");
            Console.Write($"{Environment.NewLine}Выполнять ли проверку на конфликты имен? Введите любую непустую последовательность, если да: ");
            var valideNames = !string.IsNullOrWhiteSpace(Console.ReadLine());
            
            var grammarParser = new GrammarParser(File.ReadAllText("../../../Input/Grammar.txt"));
            var grammar = grammarParser.Parse();
        
            grammar.Log();

            var lr1Parser = new Parsers.Lr1Parser.Lr1Parser(File.ReadAllText("../../../Input/Sequence.txt"), File.ReadAllText("../../../Input/SpecialCharacters.txt"), grammar);
            
            lr1Parser.Parse(valideNames);

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