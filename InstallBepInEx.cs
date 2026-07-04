using Spectre.Console;
using GTTerminal;
using System.IO;

namespace GTTerminal.Features;

public static class InstallBepInEx
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string zipPath = Path.Combine(Environment.CurrentDirectory, "BepInEx.zip");

        AnsiConsole.MarkupLine("[yellow]Downloading BepInEx...[/]");

        bool ok = Downloader.DownloadFile(
            "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_win_x64_5.4.23.4.zip",
            zipPath
        );

        if (!ok)
        {
            Utils.Pause();
            return;
        }

        AnsiConsole.MarkupLine("[yellow]Extracting...[/]");

        if (!ZipExtractor.ExtractZip(zipPath, settings.GamePath))
        {
            Utils.Pause();
            return;
        }

        File.Delete(zipPath);

        AnsiConsole.MarkupLine("[green]BepInEx installed successfully![/]");
        Utils.Pause();
    }
}
