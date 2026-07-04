using Spectre.Console;
using System.Diagnostics;
using System.IO;

namespace GTTerminal.Features;

public static class OpenGTAG
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        AnsiConsole.MarkupLine("[yellow]Launch Gorilla Tag[/]\n");
        AnsiConsole.MarkupLine("1. Launch Gorilla Tag (Direct EXE)");
        AnsiConsole.MarkupLine("2. Launch Gorilla Tag (Steam)");
        AnsiConsole.MarkupLine("0. Back\n");

        int choice = AnsiConsole.Ask<int>("Enter number:");

        switch (choice)
        {
            case 1:
                LaunchDirect(settings);
                break;

            case 2:
                LaunchSteam();
                break;

            case 0:
                return;

            default:
                AnsiConsole.MarkupLine("[red]Invalid option.[/]");
                Utils.Pause();
                break;
        }
    }

    private static void LaunchDirect(Settings settings)
    {
        string exePath = Path.Combine(settings.GamePath, "Gorilla Tag.exe");

        if (!File.Exists(exePath))
        {
            AnsiConsole.MarkupLine("[red]Gorilla Tag.exe not found in the game folder.[/]");
            Utils.Pause();
            return;
        }

        AnsiConsole.MarkupLine("[green]Launching Gorilla Tag (Direct)...[/]");

        try
        {
            Process.Start(exePath);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to launch Gorilla Tag.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
        }

        Utils.Pause();
    }

    private static void LaunchSteam()
    {
        AnsiConsole.MarkupLine("[green]Launching Gorilla Tag via Steam...[/]");

        try
        {
            Process.Start("steam://rungameid/1533390");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to launch via Steam.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
        }

        Utils.Pause();
    }
}
