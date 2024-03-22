using Lr1Parser.Lr1Grammar;

namespace Lr1Parser.Parsers.Lr1Parser;

public class Lr1Item
{
    public Lr1Item(Rule rule, int recognizedPartIndex, Terminal reductionTerminal)
    {
        Rule = rule;
        RecognizedPartIndex = recognizedPartIndex;
        ReductionTerminal = reductionTerminal;

        RecognizedPart = Rule.RightSide.Take(RecognizedPartIndex).ToArray();
        UnrecognizedPart = Rule.RightSide.Skip(RecognizedPartIndex).ToArray();
    }

    public bool Equals(Lr1Item item) => item.Rule == Rule && item.RecognizedPartIndex == RecognizedPartIndex &&
                                        item.ReductionTerminal == ReductionTerminal;

    public bool FirstUnrecognizedTokenIsNonterminal() => UnrecognizedPart[0] is Nonterminal;

    public IGrammarToken GetSecondUnrecognizedTokenOrReductionTerminal() =>
        UnrecognizedPart.Count > 0 ? UnrecognizedPart[0] : ReductionTerminal;
    
    public Rule Rule { get; }
    
    public int RecognizedPartIndex { get; }// Индекс токена правой части, ДО (т. е. НЕ включая) которого правая часть распознана. Пример: индекс равен 0 --> правило распознано до первого токена, первым следующим распознанным токеном будет первый токен прввила
    
    public IReadOnlyList<IGrammarToken> RecognizedPart { get; }
    
    public IReadOnlyList<IGrammarToken> UnrecognizedPart { get; }
    
    public Terminal ReductionTerminal { get; }
}