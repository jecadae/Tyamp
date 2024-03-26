using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1;

public class CodeGenerator1
{
    public static string NextGenCodeGenerator(TreeNode root)
    { 
        var code = new StringBuilder();
        var orderList = root.Flatten();
        orderList.Reverse();
        for(var index=2; index < orderList.Count ; index++)
        {
            if (orderList[index].IsOperator) continue;
            if (!IsLast(index,orderList))
            {
                code.AppendLine($"LOAD {orderList[index].Value}");
                code.AppendLine($"STORE ${orderList[index].Level}");
            }
            else
            {
                code.AppendLine($"LOAD {orderList[index].Value}");
            }
        }
        
        orderList.Reverse();
        for(var ind=2; ind < orderList.Count; ind++)
        {
            if(orderList[ind].IsOperator){
                switch (orderList[ind].Value)
                {
                    case "+":
                        code.AppendLine($"ADD ${orderList[ind-1].Level}");
                        break;
                    case "*":
                        code.AppendLine($"MPY ${orderList[ind-1].Level}");
                        break;
                }
            }
        }
        
        Console.WriteLine(code.ToString());
        return code.ToString();
    }

    public static string OptimizeCode(string code)
    {
        bool shouldexit = false;
        while (!shouldexit)
        {
            shouldexit = true;
            Match a;

            // 3
            a = Regex.Match(code, @"STORE (.+)\nLOAD \1");
            if (a.Success) {
                Match n = Regex.Match(code.Substring(a.Index+a.Length), @"(.*) " + Regex.Escape(a.Groups[1].Value));
                while (a.Success && (!n.Success || n.Groups[1].Value == "STORE"))
                {
                    code = String.Concat(code.Substring(0, a.Index), code.Substring(a.Index + a.Length));
                    shouldexit = false;
                    a = Regex.Match(code, @"STORE (.+)\nLOAD \1");
                    if (a.Success)
                    {
                        n = Regex.Match(code.Substring(a.Index + a.Length), @"(.*) " + Regex.Escape(a.Groups[1].Value));
                    }
                }
            }
            // 4
                
            a = Regex.Match(code, @"LOAD (.+)\nSTORE (.+)\n(?=LOAD)((?:.|\s)*?)(?=STORE \2|$)");
            while (a.Success)
            {
                code = String.Concat(code.Substring(0, a.Index), Regex.Replace(a.Groups[3].Value, Regex.Escape(a.Groups[2].Value), a.Groups[1].Value), code.Substring(a.Index + a.Length));
                shouldexit = false;
                a = Regex.Match(code, @"LOAD (.+)\nSTORE (.+)\n(?=LOAD)((?:.|\s)*?)(?=STORE \2|$)");
            }
                
            // 1-2 - полезные только если до них идёт STORE b (?)
                
            if (Regex.Match(code, @"STORE (.*)\nLOAD (.*)\n(ADD|MPY) \1").Success)
            {
                code = Regex.Replace(code, @"STORE (.*)\nLOAD (.*)\n(ADD|MPY) \1", @"$3 $2");
                shouldexit = false;
            }
        }
        return code;
    }
    
    private static bool IsLast(int index, IReadOnlyCollection<TreeNode> list)
    {
        var sublist = list.Skip(index + 1).Take(list.Count - index).ToList();
        var nonOperatorNodesCount = sublist.Count(node => !node.IsOperator);
        return nonOperatorNodesCount == 0;
    }
}