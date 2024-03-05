using System.Text;

namespace ConsoleApp1;

public class CodeGenerator1
{
    private readonly StringBuilder _builder;
    private int _variableCounter;
    private List<int> order;
    public CodeGenerator1()
    {
        _builder = new StringBuilder();
        _variableCounter = 1;
        order = new List<int>();
    }
    public void Visit(TreeNode node)
    {
        if (node.IsConstant)
        {
            GenerateConstant(node);
        }
        else if (node.IsVariable)
        {
            GenerateVariable(node);
        }
        else if (node.IsOperator)
        {
            GenerateOperator(node);
        }
    }

    private void GenerateConstant(TreeNode node)
    {
        _builder.AppendLine($"LOAD {node.Value}");
    }

    private void GenerateVariable(TreeNode node)
    {
        if (node.Left == null && node.Right == null)
        {
            order.Add(_variableCounter);
            string tempVariable = $"${_variableCounter++}";
            _builder.AppendLine($"STORE {tempVariable}");
            _builder.AppendLine($"LOAD {node.Value}");
        }
    }

    private void GenerateOperator(TreeNode node)
    {

        if (node.Value == "=")
        {
            Visit(node.Right);
            _builder.AppendLine($"STORE {node.Left.Value}");
        }
        else
        {
            Visit(node.Left);
            Visit(node.Right);
            string tempVariable = $"${_variableCounter}";
            string rightVariable = order.First().ToString();
            order.RemoveAt(0);
            switch (node.Value)
            {
                case "+":
                    _builder.AppendLine($"ADD {rightVariable}");
                    break;
                case "*":
                    _builder.AppendLine($"MPY {rightVariable}");
                    break;
            }
        }
    }

    public string GenerateCode(TreeNode root)
    {
        Visit(root);
        return _builder.ToString();
    }
}