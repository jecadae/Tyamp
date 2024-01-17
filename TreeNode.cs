namespace ConsoleApp1;

public class TreeNode
{
    public string Value { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }

    public bool IsVariable
    {
        get { return !IsConstant && !IsOperator; }
    }

    public bool IsConstant
    {
        get { return double.TryParse(Value, out _); }
    }

    public bool IsOperator
    {
        get { return Value == "+" || Value == "*" || Value == "="; }
    }

    public TreeNode(string value)
    {
        Value = value;
        Left = null;
        Right = null;
    }

    public TreeNode(double value)
    {
        Value = value.ToString(Thread.CurrentThread.CurrentCulture);
    }
        
    public TreeNode(string value, TreeNode right, TreeNode left)
    {
        Value = value;
        Right = right;
        Left = left;
    }
}