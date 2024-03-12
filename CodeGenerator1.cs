using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1;

public class CodeGenerator1
{
    private readonly StringBuilder _builder;
    private int _variableCounter;
    private List<int> _order;
    private List<string> _variable;
    public CodeGenerator1()
    {
        _builder = new StringBuilder();
        _variableCounter = 0;
        _order = new List<int>();
        _variable = new List<string>();
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
        _variable.Add(node.Value);
        _variableCounter++;
        _builder.AppendLine($"LOAD {node.Value}");
    }

    private void GenerateVariable(TreeNode node)
    {
        if (node.Left == null && node.Right == null)
        {
            _variable.Add(node.Value);
            _order.Add(_variableCounter);
            string tempVariable = $"{_variableCounter++}";
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
            string rightVariable = _order.Last().ToString();
            _order.RemoveAt(_order.Count-1);
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
        Console.WriteLine("Неоптимизированный код:");
        Console.WriteLine(_builder.ToString());
        var stroke = _builder.ToString();
        foreach (var variable in _variable)
        {
            string patt = $@"LOAD {variable}\r\nSTORE (\d+)";

            Regex regex = new Regex(patt);

            var matches = regex.Matches(stroke).ToList().SelectMany(match => match.Groups.Values).ToList();
            var index = matches.LastOrDefault()?.Value ?? _variableCounter.ToString();
            if (stroke.Split(variable).Length - 1 == 1)
            {
                string pattern = $@"LOAD {variable}\r\nSTORE (\d+)\r\n";
                stroke = Regex.Replace(stroke, pattern, "");
                stroke = Regex.Replace(stroke, $@"{index}", variable);
            }
        }
        return stroke;
    }
    
}