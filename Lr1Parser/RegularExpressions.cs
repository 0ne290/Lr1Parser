using System.Text.RegularExpressions;

namespace Lr1Parser;

public static partial class RegularExpressions
{
    [GeneratedRegex("[\\s]")]
    public static partial Regex Whitespace();
    
    [GeneratedRegex("[\\s+]")]
    public static partial Regex WhitespaceSequence();
    
    [GeneratedRegex("[^.-.$]")]
    public static partial Regex CharRange();
    
    [GeneratedRegex("[^.\\+-.$]")]
    public static partial Regex EscapedCharRange();
    
    [GeneratedRegex("[^\\+|$]")]
    public static partial Regex EscapedDisjunction();
}