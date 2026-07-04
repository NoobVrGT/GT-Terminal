using Spectre.Console;

namespace GTTerminal.Features;

public static class CreditsMenu
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[bold cyan]GT Terminal — Credits[/]\n");

        var panel = new Panel(@"
[green]Lead Developer:[/] NoobVRGT and Z3R0
[green]Core Features & Architecture:[/] NoobVRGT and Z3R0
[green]UI/UX Design:[/] NoobVRGT and Z3R0
[green]Modding Tools & Integration:[/] NoobVRGT and Z3R0
[green]Installer Systems:[/] NoobVRGT and Z3R0

[grey]Special Thanks:[/]
- Spotify (keeping me locked in)
- steelseries (keeping me locked in)

[green]And a special thanks to you for downloading and using GT Terminal! We hope you enjoy it![/]
")
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 1),
            Header = new PanelHeader("About GT Terminal")
        };

        AnsiConsole.Write(panel);

        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}
