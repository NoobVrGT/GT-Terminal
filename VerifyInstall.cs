using Spectre.Console;
using System.IO;

namespace GTTerminal.Features;

public static class VerifyInstall
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        bool hasBepInEx = Directory.Exists(Path.Combine(settings.GamePath, "BepInEx"));
        bool hasCore = Directory.Exists(Path.Combine(settings.GamePath, "BepInEx", "core"));
        bool hasPlugins = Directory.Exists(Path.Combine(settings.GamePath, "BepInEx", "plugins"));

        var table = new Table();
        table.AddColumn("Check");
        table.AddColumn("Status");
        table.Border = TableBorder.Rounded;

        table.AddRow("BepInEx folder", hasBepInEx ? "[green]OK[/]" : "[red]Missing[/]");
        table.AddRow("Core folder", hasCore ? "[green]OK[/]" : "[red]Missing[/]");
        table.AddRow("Plugins folder", hasPlugins ? "[green]OK[/]" : "[red]Missing[/]");

        AnsiConsole.Write(table);

        Utils.Pause();
    }
}
