public class GrammarParser
{
  public void Parse(string grammar)// можно заранее разбить входную строку построчно (каждая строка - правило)
    {
        // Тут наверняка будет много всякого
        // ParseRule();
  }
  
  public IEnumerable<Rule> ParseRule(string rule)
    {
        var sides = rule.Split('=');

        if (sides.Length != 2)
            throw new Exception("Правило задано некорректно.");

        
  }
}
