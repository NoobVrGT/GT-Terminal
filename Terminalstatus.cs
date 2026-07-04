using Spectre.Console;
using System;
using System.IO;

namespace GTTerminal.Features;

public static class TerminalStatus
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumn("Status Item");
        table.AddColumn("Value");

        // Game Path
        table.AddRow("Game Path", 
            string.IsNullOrWhiteSpace(settings.GamePath)
                ? "[red]Not Set[/]"
                : $"[green]{settings.GamePath}[/]");

        // BepInEx Installed?
        bool bepInstalled = Directory.Exists(Path.Combine(settings.GamePath, "BepInEx"));
        table.AddRow("BepInEx Installed", bepInstalled ? "[green]Yes[/]" : "[red]No[/]");

        // Plugins Count
        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");
        int modCount = Directory.Exists(pluginsDir)
            ? Directory.GetFiles(pluginsDir, "*.dll").Length
            : 0;

        table.AddRow("Installed Mods", modCount > 0 ? $"[green]{modCount}[/]" : "[yellow]0[/]");

        // Timestamp
        table.AddRow("Last Checked", $"[grey]{DateTime.Now}[/]");

        AnsiConsole.Write(table);

        Utils.Pause();
    }
}
