using Spectre.Console;
using System;
using System.Net;

namespace GTTerminal;

public static class Downloader
{
    public static bool DownloadFile(string url, string outputPath)
    {
        try
        {
            using var client = new WebClient();
            client.DownloadFile(url, outputPath);
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Download failed.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
            return false;
        }
    }
}
