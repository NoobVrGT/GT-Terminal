using Spectre.Console;
using GTTerminal;
using System.IO;

namespace GTTerminal.Features;

public static class OpenModsFolder
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");

        if (!Directory.Exists(pluginsDir))
        {
            Directory.CreateDirectory(pluginsDir);
        }

        Utils.OpenFolder(pluginsDir);
        Utils.Pause();
    }
}
