using System.Text.RegularExpressions;
using ConsoleApp1;

//Console.WriteLine("Введите выражение:");
var expression = "COST = (PRICE+TAX) * 98 + 12";
var expressionHandler = new ExpressionHandler(expression);
// Построение бинарного дерева
try
{
    var tree = expressionHandler.BuildExpressionTree();
    
    // Генерация таблицы имен
    var variableTable = expressionHandler.GenerateVariableTable(tree);

    // Вывод результатов
    Console.WriteLine("Таблица имен:");
    foreach (var variable in variableTable)
    {
        Console.WriteLine(
            $"Номер: {variable.Value.Number}, Идентификатор: {variable.Key}, Тип данных: {variable.Value.DataType}");
    }
    var b = CodeGenerator1.NextGenCodeGenerator(tree);
    Console.WriteLine(CodeGenerator1.OptimizeCode(b));
    // var str = a.GenerateCode(tree);
    // Console.WriteLine("Оптимизированный код:");
    // Console.WriteLine(str);


    // Console.WriteLine(unoptimizedCode);
    // Console.WriteLine("Оптимизированный код:");
    // Console.WriteLine(optimizedCode);

}
catch (Exception e) 
{
    Console.WriteLine(e);
    Console.WriteLine("Неправильно ввдено выражение.");
}

Console.ReadKey();
