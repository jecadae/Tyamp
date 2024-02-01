namespace ConsoleApp1;

public static class CodeGenerator
{
    public static string GenerateOptimizedCode(TreeNode tree)
    {
        var evaluatedConstants = new Dictionary<TreeNode, string>();

        var code = "";

        TraverseTree(tree, node =>
        {
            if (node.IsVariable)
            {
                code += $"LOAD {node.Value}" + Environment.NewLine;
            }
            else if (node.IsConstant)
            {
                if (evaluatedConstants.ContainsKey(node))
                {
                    code += $"PUSH {evaluatedConstants[node]}" + Environment.NewLine;
                }
                else
                {
                    var constantValue = EvaluateConstantSubtree(node);
                    evaluatedConstants[node] = constantValue;
                    code += $"PUSH {constantValue}" + Environment.NewLine;
                }
            }
            else
            {
                if (IsConstantSubtree(node.Left) && IsConstantSubtree(node.Right))
                {
                    var constantValue = EvaluateOperation(node);
                    evaluatedConstants[node] = constantValue;
                    code += $"PUSH {constantValue}" + Environment.NewLine;
                }
                else
                {
                    code += GenerateOperatorCode(node.Value);
                }
            }
        });

        return code;
    }

    private static string EvaluateOperation(TreeNode node)
    {
        var leftValue = node.Left!.Value;
        var rightValue = node.Right!.Value;

        switch (node.Value)
        {
            case "+":
                return $"ADD {leftValue}, {rightValue}" + Environment.NewLine;
            case "*":
                return $"MULTIPLY {leftValue}, {rightValue}" + Environment.NewLine;
            default:
                return "";
        }
    }

    private static bool IsConstantSubtree(TreeNode? node)
    {
        if (node == null) return false;

        return node.IsConstant || (IsConstantSubtree(node.Left) && IsConstantSubtree(node.Right));
    }

    private static string EvaluateConstantSubtree(TreeNode node)
    {
        if (node.Left == null && node.Right == null) return node.Value;
        
        var leftValue = EvaluateConstantSubtree(node.Left!);
        var rightValue = EvaluateConstantSubtree(node.Right!);
        
        switch (node.Value)
        {
            case "+":
                return (double.Parse(leftValue) + double.Parse(rightValue)).ToString();
            case "*":
                return (double.Parse(leftValue) * double.Parse(rightValue)).ToString();
            default:
                return "";
        }
    }

    public static void TraverseTree(TreeNode? node, Action<TreeNode> action)
    {
        if (node != null)
        {
            TraverseTree(node.Left, action);
            action(node);
            TraverseTree(node.Right, action);
        }
    }

    public static string GenerateUnoptimizedCode(TreeNode tree)
    {
        var code = "";

        TraverseTree(tree, node =>
        {
            if (node.IsVariable)
                code += $"LOAD {node.Value}" + Environment.NewLine;
            else if (node.IsConstant)
                code += $"PUSH {node.Value}" + Environment.NewLine;
            else
                code += GenerateOperatorCode(node.Value);
        });

        return code;
    }

    private static string GenerateOperatorCode(string symbol)
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
    
}