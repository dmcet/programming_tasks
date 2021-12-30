using LabSolver;

if (args.Length != 1)
{
    Console.WriteLine("Usage: LabSolver.ConsoleApp.exe '<Path_To_File_With_Lab_Text>'");
    return;
}

if (!File.Exists(args[0]))
{
    Console.WriteLine($"ERROR: File {args[0]} does not exist.");
    return;
}

var parser = new StringLabParser(await File.ReadAllTextAsync(args[0]));

var labs = await parser.Parse().ConfigureAwait(false);

if (!labs.Success)
{
    Console.WriteLine($"ERROR: Unable to parse labyrinths: {labs.Message}.");
    return;
}

var solver = new SmartLabSolver();

foreach (var lab in labs.Result)
{
    var solveResult = await solver.SolveLabyrinth(lab);

    if (solveResult.Success)
    {
        Console.WriteLine($"Entkommen in {solveResult.Result} Minute(n)!");
    }
    else
    {
        Console.WriteLine("Gefangen :-(");
    }
}
