namespace Lr1Parser.Lr1Grammar;

public class Lr1GrammarBuilder// В идеале надо было сделать так: добавить класс "Построитель грамматики" - в нем бы содержалась вся эта конченная логика с методами Get и Add. Класс парсера грамматики использовал бы этот построитель вместо самой грамматики и в конце своей логики он бы просто вызвал некоторый метод у построителя, который бы вызвал конструктор у класса "Грамматика", передав в качестве аргумента своё состояние. Сам класс "Грамматика" при этом был бы неизменяем (то, ради чего и стоит затевать всю суету) и содержал бы только ту логику, которая необходима клиентам для работы с грамматикой
{
    public bool AddRule(Rule rule)
    {
        if (_rules.Contains(rule))
            return false;
        if (_rules.Exists(r => r.LeftSide == rule.LeftSide && r.RightSide.SequenceEqual(rule.RightSide)))
            return false;
        
        _rules.Add(rule);
        
        return true;
    }

    public IEnumerable<Rule> GetRulesByLeftSide(Nonterminal leftSide) => _rules.Where(r => r.LeftSide == leftSide);
    
    public bool AddTerminal(Terminal terminal)
    {
        if (_terminals.Contains(terminal))
            return false;
        if (_terminals.Exists(t => t.Value == terminal.Value))
            return false;
        
        _terminals.Add(terminal);
        _tokens.Add(terminal);
        
        return true;
    }

    public Terminal GetTerminalByValue(string value) =>
        _terminals.Find(t => t.Value == value) ?? Terminal.Empty;

    public IEnumerable<Terminal> GetKeywords() => _terminals.FindAll(t => t.Value.Length > 1);
    
    public bool AddNonterminal(Nonterminal nonterminal)
    {
        if (_nonterminals.Contains(nonterminal))
            return false;
        if (_nonterminals.Exists(n => n.Value == nonterminal.Value))
            return false;
        
        _nonterminals.Add(nonterminal);
        _tokens.Add(nonterminal);
        
        return true;
    }

    public Nonterminal GetNonterminalByValue(string value) =>
        _nonterminals.Find(n => n.Value == value) ?? Nonterminal.Empty;

    private readonly List<Rule> _rules = new();

    private readonly List<IGrammarToken> _tokens = new();

    private readonly List<Terminal> _terminals = new();

    private readonly List<Nonterminal> _nonterminals = new();
}