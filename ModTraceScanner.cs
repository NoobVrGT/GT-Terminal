using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;

namespace GTTerminal.Features;

public static class ModTraceScanner
{
    // ============================================================
    // PUBLIC: Run() — prints tables to screen
    // ============================================================
    public static void Run(Settings settings)
    {
        var results = RunAndReturn(settings);

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]Advanced Mod-Trace Scanner[/]\n");

        // 1) Suspicious Files
        WriteTable("Suspicious Files (Keyword Match)", results.SuspiciousFiles);

        // 2) Plugins
        WriteTable("Plugins", results.Plugins);

        // 3) Patchers
        WriteTable("Patchers", results.Patchers);

        // 4) Config Files
        WriteTable("Config Files", results.ConfigFiles);

        // 5) Core Assemblies
        WriteTable("Core Assemblies", results.CoreAssemblies);

        // 6) SeralythMenu
        WriteTable("SeralythMenu", results.SeralythMenu);

        // 7) Seralyth Sounds
        WriteTable("Seralyth Sounds", results.SeralythSounds);

        // 8) UserData
        WriteTable("UserData", results.UserData);

        // 9) AppData
        WriteTable("AppData", results.AppData);

        // 10) Leftover Assets
        WriteTable("Leftover Assets", results.LeftoverAssets);

        // 11) Unknown Files
        WriteTable("Unknown Files", results.UnknownFiles);

        // 12) Summary
        WriteSummaryTable(results);

        // 13) Risk Score
        AnsiConsole.MarkupLine($"\n[bold yellow]Risk Score:[/] [bold]{results.RiskScore}[/] / 100\n");

        Utils.Pause();
    }

    // ============================================================
    // PUBLIC: RunAndReturn() — returns results for exporting
    // ============================================================
    public static ModTraceResults RunAndReturn(Settings settings)
    {
        var results = new ModTraceResults();
        results.GamePath = settings.GamePath;

        if (string.IsNullOrWhiteSpace(settings.GamePath) ||
            !Directory.Exists(settings.GamePath))
            return results;

        string gameRoot = Path.GetFullPath(settings.GamePath);

        string[] protectedPaths =
        {
            Path.Combine(gameRoot, "Gorilla Tag_Data"),
            Path.Combine(gameRoot, "reshade-shaders")
        };

        // ============================================================
        // 1. KEYWORD SCAN
        // ============================================================
        var keywordPatterns = new[]
        {
            "seralyth","atlas","menu","bepinex","plugin","mod","monke","utilla",
            "grip","broken","space","pull","bark","fly","hack","ban","trace",
            "cheat","inject","patch","loader","dll","voicemod","sound","config",
            "core","user","data"
        };

        var suspicious = new List<string>();

        try
        {
            foreach (var file in Directory.EnumerateFileSystemEntries(gameRoot, "*", SearchOption.AllDirectories))
            {
                if (IsProtected(file, protectedPaths))
                    continue;

                string name = Path.GetFileName(file).ToLowerInvariant();
                string full = file.ToLowerInvariant();

                foreach (var kw in keywordPatterns)
                {
                    if (name.Contains(kw) || full.Contains(kw))
                    {
                        suspicious.Add(file);
                        break;
                    }
                }
            }
        }
        catch { }

        results.SuspiciousFiles = suspicious.ToArray();

        // ============================================================
        // 2. FOLDER SCAN
        // ============================================================
        var categories = new Dictionary<string, string>
        {
            { "Plugins", Path.Combine(gameRoot, "BepInEx", "plugins") },
            { "Patchers", Path.Combine(gameRoot, "BepInEx", "patchers") },
            { "Config Files", Path.Combine(gameRoot, "BepInEx", "config") },
            { "Core Assemblies", Path.Combine(gameRoot, "BepInEx", "core") },
            { "SeralythMenu", Path.Combine(gameRoot, "SeralythMenu") },
            { "Seralyth Sounds", Path.Combine(gameRoot, "SeralythMenu", "Sounds") },
            { "UserData", Path.Combine(gameRoot, "UserData") },
            { "AppData", Path.Combine(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "AppData",
                    "LocalLow"
                ),
                "Another Axiom",
                "Gorilla Tag"
            ) }
        };

        var dict = new Dictionary<string, List<string>>();

        foreach (var kvp in categories)
        {
            string name = kvp.Key;
            string path = kvp.Value;

            if (!Directory.Exists(path))
                continue;

            var list = new List<string>();

            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    if (!IsProtected(file, protectedPaths))
                        list.Add(file);
                }
            }
            catch { }

            dict[name] = list;
        }

        results.Plugins = dict.GetValueOrDefault("Plugins", new()).ToArray();
        results.Patchers = dict.GetValueOrDefault("Patchers", new()).ToArray();
        results.ConfigFiles = dict.GetValueOrDefault("Config Files", new()).ToArray();
        results.CoreAssemblies = dict.GetValueOrDefault("Core Assemblies", new()).ToArray();
        results.SeralythMenu = dict.GetValueOrDefault("SeralythMenu", new()).ToArray();
        results.SeralythSounds = dict.GetValueOrDefault("Seralyth Sounds", new()).ToArray();
        results.UserData = dict.GetValueOrDefault("UserData", new()).ToArray();
        results.AppData = dict.GetValueOrDefault("AppData", new()).ToArray();

        // ============================================================
        // 3. LEFTOVER ASSETS
        // ============================================================
        var leftover = new List<string>();
        foreach (var list in dict.Values)
        {
            foreach (var file in list)
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (ext is ".png" or ".mp3" or ".json" or ".txt" or ".ini")
                    leftover.Add(file);
            }
        }
        results.LeftoverAssets = leftover.ToArray();

        // ============================================================
        // 4. UNKNOWN FILES
        // ============================================================
        var unknown = new List<string>();
        foreach (var list in dict.Values)
        {
            foreach (var file in list)
            {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (ext is not ".dll" and not ".png" and not ".mp3" and not ".json" and not ".txt" and not ".ini")
                    unknown.Add(file);
            }
        }
        results.UnknownFiles = unknown.ToArray();

        // ============================================================
        // 5. RISK SCORE
        // ============================================================
        int score = 0;
        score += Math.Min(results.SuspiciousFiles.Length * 2, 40);
        score += Math.Min(results.Plugins.Length * 2, 20);
        score += Math.Min(results.Patchers.Length * 3, 20);
        score += Math.Min(results.AppData.Length, 10);
        score += Math.Min(results.UserData.Length, 10);

        results.RiskScore = Math.Clamp(score, 0, 100);

        return results;
    }

    // ============================================================
    // HELPERS
    // ============================================================
    private static bool IsProtected(string path, string[] protectedRoots)
    {
        var full = Path.GetFullPath(path);
        foreach (var root in protectedRoots)
        {
            var pr = Path.GetFullPath(root);
            if (full.Equals(pr, StringComparison.OrdinalIgnoreCase))
                return true;
            if (full.StartsWith(pr + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static void WriteTable(string title, string[] files)
    {
        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine($"[grey]{title}: None[/]\n");
            return;
        }

        var table = new Table();
        table.Title($"[bold]{title}[/]");
        table.AddColumn("File");
        table.AddColumn("Path");

        foreach (var file in files)
            table.AddRow(Path.GetFileName(file), file);

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static void WriteSummaryTable(ModTraceResults r)
    {
        var table = new Table();
        table.Title("[bold]Summary[/]");
        table.AddColumn("Category");
        table.AddColumn("Count");

        table.AddRow("Suspicious", r.SuspiciousFiles.Length.ToString());
        table.AddRow("Plugins", r.Plugins.Length.ToString());
        table.AddRow("Patchers", r.Patchers.Length.ToString());
        table.AddRow("Config Files", r.ConfigFiles.Length.ToString());
        table.AddRow("Core Assemblies", r.CoreAssemblies.Length.ToString());
        table.AddRow("SeralythMenu", r.SeralythMenu.Length.ToString());
        table.AddRow("Seralyth Sounds", r.SeralythSounds.Length.ToString());
        table.AddRow("UserData", r.UserData.Length.ToString());
        table.AddRow("AppData", r.AppData.Length.ToString());
        table.AddRow("Leftover Assets", r.LeftoverAssets.Length.ToString());
        table.AddRow("Unknown Files", r.UnknownFiles.Length.ToString());

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    // ============================================================
    // RECORD TYPE
    // ============================================================
    private record ScanCategory(string Name, string Path);
}
