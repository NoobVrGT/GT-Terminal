using Spectre.Console;
using System.IO;
using System.Linq;

namespace GTTerminal.Features;

public static class Dashboard
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine(ThemeManager.Accent("GT Terminal Dashboard"));
        AnsiConsole.MarkupLine(ThemeManager.Muted("Overview of your setup.\n"));

        string gamePath = settings.GamePath ?? "Not set";
        bool gamePathValid = Utils.EnsureGamePath(settings.GamePath);

        int installedMods = CountInstalledMods(settings);
        bool modsJsonExists = File.Exists("mods.json");

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow("Game path:", gamePathValid
            ? ThemeManager.Success(gamePath)
            : ThemeManager.Error(gamePath));

        grid.AddRow("Installed mods:", ThemeManager.Accent(installedMods.ToString()));
        grid.AddRow("mods.json:", modsJsonExists
            ? ThemeManager.Success("Found")
            : ThemeManager.Error("Missing"));

        // You can later add:
        // - Updates available
        // - Network status
        // - Recent actions

        AnsiConsole.Write(grid);

        Utils.Pause();
    }

    private static int CountInstalledMods(Settings settings)
    {
        if (!Utils.EnsureGamePath(settings.GamePath))
            return 0;

        string pluginsDir = Path.Combine(settings.GamePath!, "BepInEx", "plugins");
        if (!Directory.Exists(pluginsDir))
            return 0;

        return Directory.GetFiles(pluginsDir, "*.dll").Length;
    }
}
