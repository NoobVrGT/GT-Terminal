using Spectre.Console;
using System;
using System.IO;

namespace GTTerminal.Features;

public static class BackupRestore
{
    public static void Run(Settings settings)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Backup / Restore[/]")
                .AddChoices("Create Backup", "Restore Backup", "Back"));

        switch (choice)
        {
            case "Create Backup":
                CreateBackup(settings);
                break;

            case "Restore Backup":
                RestoreBackup(settings);
                break;

            case "Back":
                return;
        }
    }

    private static void CreateBackup(Settings settings)
    {
        AnsiConsole.Clear();

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string backupDir = Path.Combine(Environment.CurrentDirectory, "Backups");
        Directory.CreateDirectory(backupDir);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string backupPath = Path.Combine(backupDir, $"backup_{timestamp}.zip");

        try
        {
            ZipExtractor.CreateZip(settings.GamePath, backupPath);
            AnsiConsole.MarkupLine("[green]Backup created successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Backup failed.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
        }

        Utils.Pause();
    }

    private static void RestoreBackup(Settings settings)
    {
        AnsiConsole.Clear();

        string backupDir = Path.Combine(Environment.CurrentDirectory, "Backups");

        if (!Directory.Exists(backupDir))
        {
            AnsiConsole.MarkupLine("[red]No backups found.[/]");
            Utils.Pause();
            return;
        }

        var backups = Directory.GetFiles(backupDir, "*.zip");

        if (backups.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No backups available.[/]");
            Utils.Pause();
            return;
        }

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Choose a backup to restore[/]")
                .AddChoices(backups));

        AnsiConsole.MarkupLine("[yellow]Restoring backup...[/]");

        try
        {
            ZipExtractor.ExtractZip(choice, settings.GamePath);
            AnsiConsole.MarkupLine("[green]Backup restored![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Restore failed.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
        }

        Utils.Pause();
    }
}
