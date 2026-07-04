using Spectre.Console;

namespace GTTerminal.Features;

public static class StartupLogo
{
    public static void Show()
    {
        AnsiConsole.Clear();

        var logo = new FigletText("GT Terminal")
            .Centered()
            .Color(Color.Cyan);

        AnsiConsole.Write(logo);

        AnsiConsole.MarkupLine("[grey]Initializing modules...[/]");
        Thread.Sleep(900);

        AnsiConsole.MarkupLine("[grey]Loading features...[/]");
        Thread.Sleep(900);

        AnsiConsole.MarkupLine("[grey]Starting UI...[/]");
        Thread.Sleep(900);
    }
}
