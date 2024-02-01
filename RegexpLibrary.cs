namespace ConsoleApp1;

public static class RegexpLibrary
{
    
    /// <summary>
    /// Проверяет на то стоят ли несколько знаков подряд или есть открытый знак.
    /// </summary>
    public static string CheckForSigns { get; } = @"(?:\s*[+*]\s*){2,}|(?:\(\s*[+*]\s*\)){2,}|[+*]\s*$";

    /// <summary>
    /// Собрать все знаки из выражения.
    /// </summary>
    public static string CollectSigns { get; } = @"(\+|\*|\(|\))";
    
    /// <summary>
    /// Проверка есть ли в выражении непарные скобки.
    /// </summary>
    public static string CheckForParenthesis { get; } = @"^(?>[^()]+|\((?<Open>)|\)(?<-Open>))*(?(Open)(?!))$";

    /// <summary>
    /// Собрать все переменные и константы из строки.
    /// </summary>
    public static string CollectVariable { get; } = @"\b[A-Za-z_][A-Za-z_0-9]*|\b\d+(?:,\d+)?(?:[eE][+-]?\d+)?\b";
}