using Spectre.Console;

Console.Clear();
var menu = new Rule("File Sorter");

AnsiConsole.Write(menu);

var choice = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select an option:")
        .AddChoices("Sort files", "Exit")
);

FileSort.Run();

AnsiConsole.MarkupLine("\nPress [yellow]Enter[/] to return to menu...");
Console.ReadLine();
Console.Clear();

