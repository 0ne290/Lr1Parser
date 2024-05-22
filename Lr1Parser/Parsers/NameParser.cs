namespace Lr1Parser.Parsers;

public class NameParser
{
    public void Parse(IEnumerable<StringToken> stringTokens, string source)
    {
        var stringTokensList = stringTokens.Where(t => t.Value.Value != "@").ToList();

        for (var j = 0; j < stringTokensList.Count;)
        {
            if (stringTokensList[j].StringValue == "," && !char.IsLetter(stringTokensList[j + 1].StringValue[0]))
                stringTokensList.RemoveAt(j);
            else
                j++;
        }
        
        stringTokens = stringTokensList.Where(t => t.StringValue.Length == 1 &&
                                                   (char.IsLetter(t.StringValue[0]) || t.StringValue[0] == '{' ||
                                                    t.StringValue[0] == '}' || t.StringValue[0] == ';' || t.StringValue[0] == ','));
        
        var nameToken = new NameToken();
        var nameTokens = new List<NameToken>();
        var i = 0;
        
        foreach (var stringToken in stringTokens)
        {
            if (char.IsLetter(stringToken.StringValue[0]))
            {
                if (i < 1)
                    nameToken.IndexInSource = stringToken.IndexInSource;
                
                nameToken.Value += stringToken.StringValue;

                i++;
            }
            else
            {
                if (i < 1)
                    nameTokens.Add(new NameToken
                        { Value = stringToken.StringValue, IndexInSource = stringToken.IndexInSource });
                else
                {
                    nameTokens.Add(nameToken);
                    nameTokens.Add(new NameToken
                        { Value = stringToken.StringValue, IndexInSource = stringToken.IndexInSource });
                    nameToken = new NameToken();
                    i = 0;
                }
            }
        }

        var root = new IdentifierTreeNode();
        var node = root;
        
        for (i = 0; i < nameTokens.Count; i++)
        {
            if (nameTokens[i].Value == "{")
            {
                node = node.GoToChild();
                continue;
            }
            if (nameTokens[i].Value == ",")
            {
                node = node.GoToParent();

                node = node.GoToChild();

                continue;
            }
            if (nameTokens[i].Value == ";" || nameTokens[i].Value == "}")
            {
                node = node.GoToParent();
				
                if (i == nameTokens.Count - 1)
                    break;
                
                if (nameTokens[i + 1].Value != "}")
                    node = node.GoToChild();
                
                continue;
            }
            
            node.Value = nameTokens[i];
        }
        
        IdentifierTreeNode.ValideTree(source, root);
    }
}