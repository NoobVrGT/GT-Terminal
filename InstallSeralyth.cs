using Spectre.Console;
using GTTerminal;
using System.IO;

namespace GTTerminal.Features;

public static class InstallSeralyth
{
    // This is the method your menus will call
    public static void Run(Settings settings)
    {
        Menu(settings);
    }

    public static void Menu(Settings settings)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Install Seralyth[/]")
                .AddChoices("Download latest", "Open GitHub", "Back"));

        switch (choice)
        {
            case "Download latest":
                Download(settings);
                break;

            case "Open GitHub":
                Utils.OpenUrl("https://github.com/Seralyth/Seralyth-Menu");
                Utils.Pause();
                break;

            case "Back":
                return;
        }
    }

    public static void Download(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");
        Directory.CreateDirectory(pluginsDir);

        string outputFile = Path.Combine(pluginsDir, "SeralythMenu.dll");

        AnsiConsole.MarkupLine("[yellow]Downloading Seralyth Menu...[/]");

        bool ok = Downloader.DownloadFile(
            "https://github.com/Seralyth/Seralyth-Menu/releases/latest/download/Seralyth-Menu.dll",
            outputFile
        );

        if (!ok)
        {
            Utils.Pause();
            return;
        }

        AnsiConsole.MarkupLine("[green]Seralyth Menu installed successfully![/]");
        Utils.Pause();
    }
}
