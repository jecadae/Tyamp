using ConsoleApp1;


Console.WriteLine("Введите выражение:");
string expression = Console.ReadLine()!;
var expressionHandler = new ExpressionHandler();
// Построение бинарного дерева
TreeNode tree = expressionHandler.BuildExpressionTree(expression);

// Генерация таблицы имен
Dictionary<string, VariableInfo> variableTable = expressionHandler.GenerateVariableTable(tree);

// Генерация неоптимизированного кода
string unoptimizedCode = expressionHandler.GenerateUnoptimizedCode(tree);

// Генерация оптимизированного кода
string optimizedCode = expressionHandler.GenerateOptimizedCode(tree);

// Вывод результатов
Console.WriteLine("Таблица имен:");
foreach (var variable in variableTable)
{
    Console.WriteLine(
        $"Номер: {variable.Value.Number}, Идентификатор: {variable.Key}, Тип данных: {variable.Value.DataType}");
}

Console.WriteLine("Неоптимизированный код:");
Console.WriteLine(unoptimizedCode);

Console.WriteLine("Оптимизированный код:");
Console.WriteLine(optimizedCode);
