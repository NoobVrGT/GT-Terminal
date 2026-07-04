using Spectre.Console;
using System;
using System.Diagnostics;
using System.IO;

namespace GTTerminal;

public static class Utils
{
    public static void Pause()
    {
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    public static bool EnsureGamePath(string gamePath)
    {
        if (string.IsNullOrWhiteSpace(gamePath))
        {
            AnsiConsole.MarkupLine("[red]Game path is not set.[/]");
            Pause();
            return false;
        }

        if (!Directory.Exists(gamePath))
        {
            AnsiConsole.MarkupLine("[red]Game path does not exist.[/]");
            Pause();
            return false;
        }

        return true;
    }

    public static void OpenFolder(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Failed to open folder.[/]");
        }
    }

    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Failed to open URL.[/]");
        }
    }
}
