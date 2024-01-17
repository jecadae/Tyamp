using ConsoleApp1;

//Console.WriteLine("Введите выражение:");
var expression = "qwe = q * (1 + fg * asd + 5)) * 23"; //Console.ReadLine()!;
var expressionHandler = new ExpressionHandler();
// Построение бинарного дерева
try
{
    var tree = expressionHandler.BuildExpressionTree(expression);
    
    // Генерация таблицы имен
    var variableTable = expressionHandler.GenerateVariableTable(tree);

    // Генерация неоптимизированного кода
    var unoptimizedCode = expressionHandler.GenerateUnoptimizedCode(tree);
    
    //var optimizedCode = expressionHandler.GenerateOptimizedCode(tree);

    // Вывод результатов
    Console.WriteLine("Таблица имен:");
    foreach (var variable in variableTable)
    {
        Console.WriteLine(
            $"Номер: {variable.Value.Number}, Идентификатор: {variable.Key}, Тип данных: {variable.Value.DataType}");
    }

    Console.WriteLine("Неоптимизированный код:");
    Console.WriteLine(unoptimizedCode);
    
}
catch (Exception e) 
{
    throw new ArgumentException("Неправильно ввдено выражение.");
}

Console.ReadKey();
