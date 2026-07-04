using Spectre.Console;
using System;
using System.IO;
using System.IO.Compression;

namespace GTTerminal;

public static class ZipExtractor
{
    public static bool ExtractZip(string zipPath, string targetDir)
    {
        try
        {
            if (!File.Exists(zipPath))
            {
                AnsiConsole.MarkupLine("[red]ZIP file not found.[/]");
                return false;
            }

            ZipFile.ExtractToDirectory(zipPath, targetDir, true);
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to extract ZIP.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
            return false;
        }
    }

    public static void CreateZip(string sourceDir, string zipPath)
    {
        try
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException("Source directory does not exist.");
            }

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            ZipFile.CreateFromDirectory(sourceDir, zipPath);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to create ZIP.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
        }
    }
}
