namespace ConsoleApp1;

public class TreeNode
{
    public string Value { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }

    public int? Level { get; set; }

    public bool IsVariable
    {
        get { return !IsConstant && !IsOperator; }
    }

    public bool IsConstant
    {
        get { return double.TryParse(Value, out _); }
    }

    public bool IsOperator => Value == "+" || Value == "*" || Value == "=";

    public TreeNode(string value, int? level = null)
    {
        Value = value;
        Left = null;
        Right = null;
        Level = level;
    }

    public TreeNode(double value,int? level = null)
    {
        Value = value.ToString(Thread.CurrentThread.CurrentCulture);
        Level = level;
    }
        
    public TreeNode(string value, TreeNode right, TreeNode left, int? level = null)
    {
        Value = value;
        Right = right;
        Left = left;
        Level = level;
    }
}


public static class TreeExtensions
{
    public static List<TreeNode> Flatten(this TreeNode root)
    {
        var list = new List<TreeNode>();
        Flatten(root, list);
        return list;
    }

    private static void Flatten(TreeNode? node, List<TreeNode> list)
    {
        if (node == null)
        {
            return;
        }
        if (node.Right != null)
        {
            Flatten(node.Right, list);
        }
        
        if (node.Left != null)
        {
            Flatten(node.Left, list);
        }
        
        list.Add(node);
        
    }
}