using Spectre.Console;
using System.Net;

namespace GTTerminal.Features;

public static class CheckUpdates
{
    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]Checking for GT Terminal updates...[/]");

        try
        {
            using var client = new HttpClient();

            string latest = client.GetStringAsync("https://example.com/gtterminal/latest-version.txt").Result;

            string current = "3.0.0"; // your version

            if (latest.Trim() != current.Trim())
            {
                AnsiConsole.MarkupLine($"[green]Update available![/] Latest version: {latest}");
                AnsiConsole.MarkupLine("[grey]Visit the GitHub page to download.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[green]You are up to date![/]");
            }
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Failed to check updates.[/]");
        }

        Utils.Pause();
    }
}
