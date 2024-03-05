using System.Text.RegularExpressions;
using ConsoleApp1;

//Console.WriteLine("Введите выражение:");
var expression = "COST = (PRICE+TAX)* 98";
var expressionHandler = new ExpressionHandler(expression);
// Построение бинарного дерева
try
{
    var tree = expressionHandler.BuildExpressionTree();
    
    // Генерация таблицы имен
    var variableTable = expressionHandler.GenerateVariableTable(tree);

    // Генерация неоптимизированного кода
    var unoptimizedCode = CodeGenerator.GenerateUnoptimizedCode(tree.Right!);
    var unopmiz = NewCodeGenerator.GenerateUnOptimaizeCode(tree);
    var optimizedCode = CodeGenerator.GenerateOptimizedCode(tree);

    // Вывод результатов
    Console.WriteLine("Таблица имен:");
    foreach (var variable in variableTable)
    {
        Console.WriteLine(
            $"Номер: {variable.Value.Number}, Идентификатор: {variable.Key}, Тип данных: {variable.Value.DataType}");
    }

    Console.WriteLine("Неоптимизированный код:");
    CodeGenerator1 a = new CodeGenerator1();
    Console.WriteLine(a.GenerateCode(tree));
    // Console.WriteLine(unoptimizedCode);
    // Console.WriteLine("Оптимизированный код:");
    // Console.WriteLine(optimizedCode);

}
catch (Exception e) 
{
    Console.WriteLine("Неправильно ввдено выражение.");
}

Console.ReadKey();
