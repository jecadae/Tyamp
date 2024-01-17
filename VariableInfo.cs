namespace ConsoleApp1;

public class VariableInfo
{
    public int Number { get; set; }
    public string DataType { get; set; }

    public VariableInfo(int number, string dataType)
    {
        Number = number;
        DataType = dataType;
    }
}