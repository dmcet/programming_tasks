using FibonacciCalculator;

if (args.Length != 1)
{
    Console.WriteLine("Usage: FibonacciCalculator.ConsoleApp.exe '<Path_To_File_With_Numbers>'");
    return;
}

if (!File.Exists(args[0]))
{
    Console.WriteLine($"ERROR: File {args[0]} does not exist.");
    return;
}

var provider = new StringNumberProvider(await File.ReadAllTextAsync(args[0]));

var calculator = new MemoizingFibonacciCalculator();

var numbers = await provider.ParseNumbers().ConfigureAwait(false);

foreach (var number in numbers)
{
    var result = calculator.CalculateFibonacciNumber(number);
    Console.WriteLine($"Die Fibonacci Zahl für {number} ist: {result}");
}