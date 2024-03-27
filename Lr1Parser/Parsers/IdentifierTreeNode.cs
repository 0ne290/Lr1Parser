namespace Lr1Parser.Parsers;

public class IdentifierTreeNode
{
    public IdentifierTreeNode GoToChild()
    {
        var child = new IdentifierTreeNode();
        child._parent = this;
        _children.Add(child);
        return child;
    }

    public IdentifierTreeNode GoToParent() => _parent;

    public static void ValideTree(string source, IdentifierTreeNode root)
    {
        var scopeIdentifiers = new List<string> { root.Value.Value };
        foreach (var child in root._children)
        {
            if (scopeIdentifiers.Contains(child.Value.Value))
            {
                var position = source.GetPosition(child.Value.IndexInSource);
                throw new Exception(
                    $"Конфликт имен. Имя \"{child.Value.Value}\" уже используется в этой области видимости. Номер строки: {position.LineNumber}, номер токена: {position.CharNumber - 1}.");
            }
            
            scopeIdentifiers.Add(child.Value.Value);
            
            ValideTree(source, child);
        }
    }

    public NameToken Value { get; set; }

    private IdentifierTreeNode _parent;

    private readonly List<IdentifierTreeNode> _children = new();
}