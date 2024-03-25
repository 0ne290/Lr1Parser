using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser.Lr1Graph;

public class StateItem : IEquatable<StateItem>
{
    public StateItem(StateItem item, int recognizedPartIndexIncrement = 1)// Создаем новую Lr1-ситуацию, идентичную заданной, но с увеличенным индексом распознанной части
    {
        Rule = item.Rule;
        _recognizedPartIndex = item._recognizedPartIndex + recognizedPartIndexIncrement;
        ReductionTerminal = item.ReductionTerminal;

        RecognizedPart = Rule.RightSide.Take(_recognizedPartIndex).ToArray();
        UnrecognizedPart = Rule.RightSide.Skip(_recognizedPartIndex).ToArray();
    }
    
    public StateItem(Rule rule, int recognizedPartIndex, Terminal reductionTerminal)
    {
        Rule = rule;
        _recognizedPartIndex = recognizedPartIndex;
        ReductionTerminal = reductionTerminal;

        RecognizedPart = Rule.RightSide.Take(_recognizedPartIndex).ToArray();
        UnrecognizedPart = Rule.RightSide.Skip(_recognizedPartIndex).ToArray();
    }

    public void Log() => Console.Write($"Lef: {Rule.LeftSide.Value}, Red: {(ReductionTerminal.Value == "\0" ? "\\0" : ReductionTerminal.Value)}, Ind: {_recognizedPartIndex}; -=- ");

    public bool Equals(StateItem? item) => item != null && item.Rule == Rule &&
                                           item._recognizedPartIndex == _recognizedPartIndex &&
                                           item.ReductionTerminal == ReductionTerminal;

    public override bool Equals(object? obj) => obj is StateItem item && Equals(item);

    public override int GetHashCode() => HashCode.Combine(Rule, _recognizedPartIndex, ReductionTerminal);

    public bool FirstUnrecognizedTokenEquals(IGrammarToken token) =>
        UnrecognizedPart.Count > 0 && UnrecognizedPart[0] == token;

    public bool FirstUnrecognizedTokenIsNonterminal() => UnrecognizedPart.Count > 0 && UnrecognizedPart[0] is Nonterminal;

    public bool UnrecognizedPartIsEmpty() => UnrecognizedPart.Count < 1;

    public IGrammarToken GetSecondUnrecognizedTokenOrReductionTerminal() =>
        UnrecognizedPart.Count > 1 ? UnrecognizedPart[1] : ReductionTerminal;

    public IGrammarToken GetFirstUnrecognizedToken() => UnrecognizedPart[0];
    
    public Rule Rule { get; }

    private readonly int _recognizedPartIndex;// Индекс токена правой части, ДО (т. е. НЕ включая) которого правая часть распознана. Пример: индекс равен 0 --> правило распознано до первого токена, первым следующим распознанным токеном будет первый токен прввила
    
    private IReadOnlyList<IGrammarToken> RecognizedPart { get; }
    
    private IReadOnlyList<IGrammarToken> UnrecognizedPart { get; }
    
    public Terminal ReductionTerminal { get; }
}