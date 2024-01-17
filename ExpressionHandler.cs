using System.Text.RegularExpressions;

namespace ConsoleApp1;

public class ExpressionHandler
{
    public TreeNode BuildExpressionTree(string expression)
    {
        // Регулярное выражение для поиска переменных и констант
        string pattern = @"[A-Za-z_][A-Za-z_0-9]*|[0-9]+(?:\.[0-9]*)?(?:[eE][+-]?[0-9]+)?";
        Regex regex = new Regex(pattern);

        Queue<TreeNode> queue = new Queue<TreeNode>();

        MatchEvaluator matchEvaluator = ((match) =>
        {
            if (double.TryParse(match.Value, out double constant))
            {
                queue.Enqueue(new TreeNode(constant));
            }
            else
            {
                queue.Enqueue(new TreeNode(match.Value));
            }

            return "";
        });

        expression = regex.Replace(expression, matchEvaluator);

        // Регулярное выражение для поиска операторов и скобок
        pattern = @"(\+|\*|\(|\))";
        regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(expression);
        var success1 = queue.TryDequeue(out var test);
        var matchesList = matches.ToList();
        if (!success1)
        {
            throw new ArgumentException("Неправильное выражение");
        }

        var tree = new TreeNode("=", GetTree(matchesList, queue), new TreeNode(test!.Value));

        return tree;
    }

    private TreeNode GetTree(List<Match> matches, Queue<TreeNode> queue)
    {
        var match = matches.First();
        if (matches.Skip(1).ToList().Count == 0)
        {
            var right = queue.Dequeue();
            var left = queue.Dequeue();
            return new TreeNode(value: match.Value, right: right, left: left);
        }
        else
        {
            var newMatches = matches.Skip(1).ToList();
            var left = queue.Dequeue();
            var sign = matches.First().Value;
            var treeNode = new TreeNode(sign, left: left, right: GetTree(newMatches, queue));
            return treeNode;
        }
    }


    public Dictionary<string, VariableInfo> GenerateVariableTable(TreeNode tree)
    {
        Dictionary<string, VariableInfo> variableTable = new Dictionary<string, VariableInfo>();
        int variableNumber = 1;

        TraverseTree(tree, (node) =>
        {
            if (node.IsVariable && !node.IsOperator) // Проверка на то, что значение не является оператором
            {
                string variableName = node.Value;

                if (!variableTable.ContainsKey(variableName))
                {
                    variableTable.Add(variableName, new VariableInfo(variableNumber++, "Переменная"));
                }
            }
        });

        return variableTable;
    }

    private void TraverseTree(TreeNode? node, Action<TreeNode> action)
    {
        if (node != null)
        {
            TraverseTree(node.Left, action);
            action(node);
            TraverseTree(node.Right, action);
        }
    }

   public string GenerateUnoptimizedCode(TreeNode tree)
    {
        string code = "";

        TraverseTree(tree, (node) =>
        {
            if (node.IsVariable)
            {
                code += $"LOAD {node.Value}" + Environment.NewLine;
            }
            else if (node.IsConstant)
            {
                code += $"PUSH {node.Value}" + Environment.NewLine;
            }
            else
            {
                code += GenerateOperatorCode(node.Value);
            }
        });

        code += "RETURN" + Environment.NewLine;

        return code;
    }

    private string GenerateOperatorCode(string symbol)
    {
        switch (symbol)
        {
            case "+":
                return "ADD" + Environment.NewLine;
            case "*":
                return "MULTIPLY" + Environment.NewLine;
            default:
                return "";
        }
    }

    public string GenerateOptimizedCode(TreeNode tree)
    {
        // Оптимизация кода здесь

        return GenerateUnoptimizedCode(tree);
    }
}


