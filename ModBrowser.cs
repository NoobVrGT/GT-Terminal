using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace GTTerminal.Features;

public static class ModBrowser
{
    private static readonly string LocalModsJson =
    Path.Combine(AppContext.BaseDirectory, "mods.json");

    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Mod Browser[/]\n");

        if (!Utils.EnsureGamePath(settings.GamePath))
            return;

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");
        Directory.CreateDirectory(pluginsDir);

        // Load mods.json locally
        List<ModInfo> mods;
        try
        {
            mods = LoadLocalModsJson();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to read mods.json:[/] {ex.Message}");
            Utils.Pause();
            return;
        }

        if (mods.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No mods found in mods.json.[/]");
            Utils.Pause();
            return;
        }

        // Display mods in a table
        var table = new Table();
        table.Title("[bold]Available Mods[/]");
        table.AddColumn("Name");
        table.AddColumn("Author");
        table.AddColumn("Version");
        table.AddColumn("Description");

        foreach (var m in mods)
        {
            table.AddRow(
                m.Name,
                m.Author,
                m.Version,
                m.Description);
        }

        AnsiConsole.Write(table);

        // Let user pick a mod
        var modNames = mods.ConvertAll(m => m.Name);
        modNames.Add("Back");

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a mod to install[/]")
                .AddChoices(modNames));

        if (choice == "Back")
            return;

        var selected = mods.Find(m => m.Name == choice);
        InstallMod(selected, pluginsDir);
    }

    private static List<ModInfo> LoadLocalModsJson()
    {
        if (!File.Exists(LocalModsJson))
            throw new FileNotFoundException("mods.json not found in GTTerminal folder.");

        string json = File.ReadAllText(LocalModsJson);

        var mods = JsonSerializer.Deserialize<List<ModInfo>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return mods ?? new List<ModInfo>();
    }

    

    private class ModInfo
    {
        public string Name { get; set; } = "";
        public string Author { get; set; } = "";
        public string Version { get; set; } = "";
        public string Description { get; set; } = "";
        public string DownloadUrl { get; set; } = "";
    }





private static void InstallMod(ModInfo mod, string pluginsDir)
{
    AnsiConsole.MarkupLine($"[yellow]Downloading {mod.Name}...[/]");

    string output = Path.Combine(pluginsDir, $"{mod.Name}.dll");

    UiEffects.ProgressDownload($"Downloading {mod.Name}", progress =>
    {
        try
        {
            using var client = new HttpClient();

            // Download bytes
            var bytes = client.GetByteArrayAsync(mod.DownloadUrl).GetAwaiter().GetResult();

            // Report 100%
            progress.Report(1.0);

            // Save file
            File.WriteAllBytes(output, bytes);

            return true;
        }
        catch
        {
            return false;
        }
    });

    Utils.Pause();
}

}