using Spectre.Console;
using System.IO;

namespace GTTerminal.Features;

public static class QuickInstall
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");
        Directory.CreateDirectory(pluginsDir);

        AnsiConsole.MarkupLine("[yellow]Quick installing mods...[/]");

        string[] urls =
        {
            "https://example.com/AirJump.dll",
            "https://example.com/IronMonke.dll",
            "https://example.com/Platforms.dll"
        };

        foreach (var url in urls)
        {
            string fileName = Path.GetFileName(url);
            string output = Path.Combine(pluginsDir, fileName);

            Downloader.DownloadFile(url, output);
        }

        AnsiConsole.MarkupLine("[green]Quick install complete![/]");
        Utils.Pause();
    }
}
