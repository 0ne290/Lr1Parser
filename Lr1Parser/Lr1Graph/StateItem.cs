using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Lr1Graph;

public class StateItem : IEquatable<StateItem>
{
    public StateItem(StateItem item, int recognizedPartIndexIncrement = 1)// Создаем новую Lr1-ситуацию, идентичную заданной, но с увеличенным индексом распознанной части
    {
        Rule = item.Rule;
        RecognizedPartIndex = item.RecognizedPartIndex + recognizedPartIndexIncrement;
        ReductionTerminal = item.ReductionTerminal;

        RecognizedPart = Rule.RightSide.Take(RecognizedPartIndex).ToArray();
        UnrecognizedPart = Rule.RightSide.Skip(RecognizedPartIndex).ToArray();
    }
    
    public StateItem(Rule rule, int recognizedPartIndex, Terminal reductionTerminal)
    {
        Rule = rule;
        RecognizedPartIndex = recognizedPartIndex;
        ReductionTerminal = reductionTerminal;

        RecognizedPart = Rule.RightSide.Take(RecognizedPartIndex).ToArray();
        UnrecognizedPart = Rule.RightSide.Skip(RecognizedPartIndex).ToArray();
    }

    public bool Equals(StateItem? item) => item != null && item.Rule == Rule &&
                                           item.RecognizedPartIndex == RecognizedPartIndex &&
                                           item.ReductionTerminal == ReductionTerminal;

    public override bool Equals(object? obj) => obj is StateItem item && Equals(item);

    public override int GetHashCode() => HashCode.Combine(Rule, RecognizedPartIndex, ReductionTerminal);

    public bool FirstUnrecognizedTokenEquals(IGrammarToken token) =>
        UnrecognizedPart.Count > 0 && UnrecognizedPart[0] == token;

    public bool FirstUnrecognizedTokenIsNonterminal() => UnrecognizedPart.Count > 0 && UnrecognizedPart[0] is Nonterminal;

    public IGrammarToken GetSecondUnrecognizedTokenOrReductionTerminal() =>
        UnrecognizedPart.Count > 0 ? UnrecognizedPart[0] : ReductionTerminal;
    
    public Rule Rule { get; }
    
    public int RecognizedPartIndex { get; }// Индекс токена правой части, ДО (т. е. НЕ включая) которого правая часть распознана. Пример: индекс равен 0 --> правило распознано до первого токена, первым следующим распознанным токеном будет первый токен прввила
    
    public IReadOnlyList<IGrammarToken> RecognizedPart { get; }
    
    public IReadOnlyList<IGrammarToken> UnrecognizedPart { get; }
    
    public Terminal ReductionTerminal { get; }
}