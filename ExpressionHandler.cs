using System.Text.RegularExpressions;

namespace ConsoleApp1;

public class ExpressionHandler
{
    private readonly string _expression;

    private bool _correctForSigns;

    private bool _correctForParenthesis;

    public ExpressionHandler(string expression)
    {
        _expression = expression;
    }

    /// <summary>
    ///     Проверка на подряд идущие знаки, или открытые знаки,
    /// </summary>
    /// <returns> False - если выражение некорректно, true если корректно</returns>
    public bool CheckForSigns()
    {
        var pattern = RegexpLibrary.CheckForSigns;

        var isMatch = !Regex.IsMatch(_expression, pattern);

        _correctForSigns = isMatch;

        return isMatch;
    }

    /// <summary>
    ///     Проверить есть ли в выражении, незакртые скобки.
    /// </summary>
    /// <returns>False - есть,True - нет </returns>
    public bool CheckForParenthesis()
    {
        var pattern = RegexpLibrary.CheckForParenthesis;

        var isMatch = Regex.IsMatch(_expression, pattern);

        _correctForParenthesis = isMatch;

        return isMatch;
    }

    /// <summary>
    ///     Выполнить все проверки.
    /// </summary>
    /// <returns>True - если проверки пройдены, в противном случае false</returns>
    private bool _complexCheck()
    {
        var a = false;
        var b = false;
        if (!_correctForSigns) a = CheckForSigns();

        if (!_correctForParenthesis) b = CheckForParenthesis();

        return a && b;
    }

    /// <summary>
    ///     Составить дерево выражений.
    /// </summary>
    /// <returns>Деревео выражений, или исключение если оно неверное.</returns>
    /// <exception cref="ArgumentException"></exception>
    public TreeNode BuildExpressionTree()
    {
        if (!_complexCheck()) throw new ArgumentException();

        var pattern = RegexpLibrary.CollectVariable;
        var regex = new Regex(pattern);

        var stack = new Stack<TreeNode>();

        MatchEvaluator matchEvaluator = match =>
        {
            stack.Push(double.TryParse(match.Value, out var constant)
                ? new TreeNode(constant)
                : new TreeNode(match.Value));

            return "";
        };

        var expression = regex.Replace(_expression, matchEvaluator);

        // Регулярное выражение для поиска операторов и скобок
        pattern = RegexpLibrary.CollectSigns;
        regex = new Regex(pattern);
        var matches = regex.Matches(expression);
        
        var operations = ExpandExpression(expression);
        var tree = BuildExpressionTree(operations, stack, matches);
        return tree;
    }

    private TreeNode BuildExpressionTree(string operations, Stack<TreeNode> arguments, MatchCollection matchCollection)
    {
        if(operations.Length + 1 < arguments.Count)
        {
            throw new ArgumentException();
        }

        var last = arguments.Last();
        arguments = GetFirst(arguments);
        operations = operations.Substring(1);
        var root = new TreeNode("=", GetTree(operations, arguments), last);
        return root;
    }

    private TreeNode? GetTree(string operations, Stack<TreeNode> stack)
    {
        if (operations.Length > 0)
        {
            var operation = operations.Last().ToString();
            operations = operations.Remove(operations.Length - 1);
            var left = stack.Pop();
            if (left.Value == "q") Console.WriteLine("aaa");

            var node = new TreeNode(operation)
            {
                Left = left,
                Right = GetTree(operations, stack)
            };
            return node;
        }

        if (stack.Count == 1)
            return stack.Pop();
        return null;
    }

    private string ExpandExpression(string expression)
    {
        // Удаление пробелов из выражения
        expression = expression.Replace(" ", "");

        // Создание словаря приоритетов операций
        var precedence = new Dictionary<char, int>
        {
            ['+'] = 1,
            ['*'] = 2
        };

        var output = "";
        var operators = new Stack<char>();

        foreach (var c in expression)
            switch (c)
            {
                case '(':
                    operators.Push(c);
                    break;
                case ')':
                {
                    while (operators.Count > 0 && operators.Peek() != '(') output += operators.Pop();

                    operators.Pop();
                    break;
                }
                default:
                {
                    if (IsOperator(c))
                    {
                        while (operators.Count > 0 && operators.Peek() != '(' && precedence[c] <= precedence[operators.Peek()])
                            output += operators.Pop();

                        operators.Push(c);
                    }
                    else
                    {
                        output += c;
                    }

                    break;
                }
            }

        while (operators.Count > 0) output += operators.Pop();

        return output;
    }

    private Stack<TreeNode> GetFirst(Stack<TreeNode> stack)
    {
        var tempStack = new Stack<TreeNode>();
        var count = stack.Count;
        for (var i = 1; i < count; i++) tempStack.Push(stack.Pop());
        stack.Pop();
        while (tempStack.Count > 0) stack.Push(tempStack.Pop());
        return stack;
    }

    private bool IsOperator(char c)
    {
        return c is '+' or '*';
    }


    public Dictionary<string, VariableInfo> GenerateVariableTable(TreeNode tree)
    {
        var variableTable = new Dictionary<string, VariableInfo>();
        var variableNumber = 1;

        CodeGenerator.TraverseTree(tree, node =>
        {
            if (node.IsConstant)
            {
                variableTable.Add(node.Value,
                    int.TryParse(node.Value, out var canCovert)
                        ? new VariableInfo(variableNumber++, "Целочисленное значение")
                        : new VariableInfo(variableNumber++, "Константа с плавающей точкой"));
            }

            if (node.IsVariable) // Проверка на то, что значение не является оператором
            {
                var variableName = node.Value;

                if (!variableTable.ContainsKey(variableName))
                    variableTable.Add(variableName,
                        new VariableInfo(variableNumber++, "Переменная с плавающей точкой"));
            }
        });

        return variableTable;
    }
}