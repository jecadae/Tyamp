namespace ConsoleApp1;

public static class NewCodeGenerator
{
    public static string GenerateUnOptimaizeCode(TreeNode treeNode)
    {
        var node  = getOperations(treeNode);
        return "";
    }

    public static TreeNode getOperations(TreeNode treeNode)
    {
        var tempNode = treeNode.Right;
        var i = 0;
        while (tempNode.Left != null)
        {
            i++;
            tempNode = tempNode.Left;
        }
        Console.WriteLine("STORE $"+i);
        Console.WriteLine("LOAD "+tempNode.Value);
        return tempNode;
    }
}