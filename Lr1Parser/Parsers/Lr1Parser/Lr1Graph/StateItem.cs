using Lr1Parser.Parsers.Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser.Lr1Graph;

public class StateItem : IEquatable<StateItem>
{
    private StateItem(Rule rule, int recognizedPartIndex, Terminal reductionTerminal)
    {
        Rule = rule;
        _recognizedPartIndex = recognizedPartIndex;
        ReductionTerminal = reductionTerminal;
        
        var mutableUnrecognizedPart = new IGrammarToken[Rule.RightSide.Count - _recognizedPartIndex];

        for (var i = 0; i < mutableUnrecognizedPart.Length; i++)
            mutableUnrecognizedPart[i] = Rule.RightSide[i + _recognizedPartIndex];

        UnrecognizedPart = mutableUnrecognizedPart;
    }

    // Создаем новую (либо получаем уже имеющуюся, если она была создана ранее) Lr1-ситуацию, идентичную заданной, но с увеличенным индексом распознанной части
    public static StateItem GetItem(StateItem item, int recognizedPartIndexIncrement = 1) => GetItem(item.Rule, item._recognizedPartIndex + recognizedPartIndexIncrement, item.ReductionTerminal);

    public static StateItem GetItem(Rule rule, int recognizedPartIndex, Terminal reductionTerminal)
    {
        foreach (var i in _items)
            if (i.Rule == rule && i._recognizedPartIndex == recognizedPartIndex && i.ReductionTerminal == reductionTerminal)
                return item;

        var item = new StateItem(rule, recognizedPartIndex, reductionTerminal)

        _items.Add(item);

        return item;
    }

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
    
    private IReadOnlyList<IGrammarToken> UnrecognizedPart { get; }
    
    public Terminal ReductionTerminal { get; }

    private static readonly List<StateItem> _items = new();
}
