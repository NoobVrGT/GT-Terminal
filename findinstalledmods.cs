using Spectre.Console;
using System.IO;

namespace GTTerminal.Features;

public static class FindInstalledMods
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");

        if (!Directory.Exists(pluginsDir))
        {
            AnsiConsole.MarkupLine("[red]Plugins folder not found.[/]");
            Utils.Pause();
            return;
        }

        var files = Directory.GetFiles(pluginsDir, "*.dll");

        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No mods installed.[/]");
        }
        else
        {
            var table = new Table();
            table.AddColumn("Installed Mods");
            table.Border = TableBorder.Rounded;

            foreach (var file in files)
                table.AddRow(Path.GetFileName(file));

            AnsiConsole.Write(table);
        }

        Utils.Pause();
    }
}
