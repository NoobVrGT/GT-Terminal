using Spectre.Console;
using System;
using System.IO;
using GTTerminal.Features;

namespace GTTerminal.Features;

public static class SecretMenu
{
    private const string Password = "Z3R0";

    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();
        string entered = AnsiConsole.Ask<string>("Enter secret password:");

        if (entered != Password)
        {
            AnsiConsole.MarkupLine("[red]Incorrect password.[/]");
            Utils.Pause();
            return;
        }

        Menu(settings);
    }

    private static void Menu(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Secret Menu[/]")
                    .AddChoices(
                        "Tracing (Advanced)",
                        "Network Tools",
                        "Delete GTAG (deep check)",
                        "Delete all mod traces",
                        "Back"
                    ));

            switch (choice)
            {
                case "Tracing (Advanced)":
                    RunAdvancedTracing(settings);
                    break;

                case "Network Tools":
                    NetworkMenu(settings);
                    break;

                case "Delete GTAG (deep check)":
                    DeleteGTAG(settings);
                    break;

                case "Delete all mod traces":
                    DeleteAllModTraces(settings);
                    break;

                case "Back":
                    return;
            }
        }
    }

    private static void NetworkMenu(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Network Tools[/]")
                    .AddChoices(
                        "Port Scan (Local)",
                        "Network Trace",
                        "Mod Network Activity",
                        "Gorilla Tag Server Status",
                        "Ping GorillaTag",
                        "Ping Steam",
                        "Ping Oculus",
                        "Back"
                    ));

            switch (choice)
            {
                case "Port Scan (Local)":
                    NetworkDiagnostics.RunPortScan();
                    break;

                case "Network Trace":
                    NetworkDiagnostics.RunNetworkTrace();
                    break;

                case "Mod Network Activity":
                    NetworkDiagnostics.RunModNetworkActivity();
                    break;

                case "Gorilla Tag Server Status":
                    NetworkDiagnostics.RunGorillaTagStatus();
                    break;

                case "Ping GorillaTag":
                    NetworkDiagnostics.RunPing("gorillatag.net", "GorillaTag");
                    break;

                case "Ping Steam":
                    NetworkDiagnostics.RunPing("store.steampowered.com", "Steam");
                    break;

                case "Ping Oculus":
                    NetworkDiagnostics.RunPing("oculus.com", "Oculus");
                    break;

                case "Back":
                    return;
            }
        }
    }

    // ===== existing advanced tracing, export, delete methods stay the same =====

    private static void RunAdvancedTracing(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Initializing Advanced Trace Scanner...[/]\n");

        string[] fakePaths =
        {
            "Scanning: BepInEx/plugins/AtlasRemade.dll",
            "Scanning: SeralythMenu/Sounds/montagem-rabeta.mp3",
            "Scanning: UserData/modsettings.json",
            "Scanning: AppData/Another Axiom/Gorilla Tag/PlayerData.dat",
            "Scanning: BepInEx/config/SeralythMenu.cfg",
            "Scanning: SeralythMenu/icon.png",
            "Scanning: BepInEx/patchers/PlatformDetector.dll",
            "Scanning: Gorilla Tag_Data/resources.assets"
        };

        var pb = AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn()
            });

        pb.Start(ctx =>
        {
            var task = ctx.AddTask("[green]Performing deep scan...[/]", maxValue: 100);
            Random rnd = new();

            for (int i = 0; i < 100; i++)
            {
                task.Increment(1);

                if (i % 10 == 0)
                {
                    string path = fakePaths[rnd.Next(fakePaths.Length)];
                    AnsiConsole.MarkupLine($"[grey]{path}[/]");
                }

                System.Threading.Thread.Sleep(50);
            }
        });

        AnsiConsole.MarkupLine("\n[bold green]Scan complete. Processing results...[/]\n");

        var results = ModTraceScanner.RunAndReturn(settings);

        AnsiConsole.MarkupLine("[bold yellow]Displaying scan results...[/]\n");
        ModTraceScanner.Run(settings);

        var exportChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Export results to GTTerminal_ModTraceReport.txt?[/]")
                .AddChoices("Yes", "No"));

        if (exportChoice == "Yes")
        {
            ExportResults(results);
            AnsiConsole.MarkupLine("[green]Results exported successfully.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[grey]Export skipped.[/]");
        }

        Utils.Pause();
    }

    private static void ExportResults(ModTraceResults results)
    {
        string file = "GTTerminal_ModTraceReport.txt";

        using var sw = new StreamWriter(file);

        sw.WriteLine("GT Terminal v4 - Advanced Mod Trace Report");
        sw.WriteLine($"Generated: {DateTime.Now}");
        sw.WriteLine($"Game Path: {results.GamePath}");
        sw.WriteLine(new string('-', 60));

        void WriteSection(string title, string[] items)
        {
            sw.WriteLine($"\n=== {title} ===");
            if (items.Length == 0)
                sw.WriteLine("None");
            else
                foreach (var i in items)
                    sw.WriteLine(i);
        }

        WriteSection("Suspicious Files", results.SuspiciousFiles);
        WriteSection("Plugins", results.Plugins);
        WriteSection("Patchers", results.Patchers);
        WriteSection("Config Files", results.ConfigFiles);
        WriteSection("Core Assemblies", results.CoreAssemblies);
        WriteSection("SeralythMenu", results.SeralythMenu);
        WriteSection("Seralyth Sounds", results.SeralythSounds);
        WriteSection("UserData", results.UserData);
        WriteSection("AppData", results.AppData);
        WriteSection("Leftover Assets", results.LeftoverAssets);
        WriteSection("Unknown Files", results.UnknownFiles);

        sw.WriteLine($"\nRisk Score: {results.RiskScore}/100");
    }

    private static void DeleteGTAG(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]Deleting Gorilla Tag folder...[/]");

        try
        {
            Directory.Delete(settings.GamePath, true);
            AnsiConsole.MarkupLine("[green]GTAG deleted successfully.[/]");
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Failed to delete GTAG.[/]");
        }

        Utils.Pause();
    }

    private static void DeleteAllModTraces(Settings settings)
    {
        AnsiConsole.Clear();

        string pluginsDir = Path.Combine(settings.GamePath, "BepInEx", "plugins");

        if (!Directory.Exists(pluginsDir))
        {
            AnsiConsole.MarkupLine("[yellow]No mod traces found.[/]");
            Utils.Pause();
            return;
        }

        try
        {
            Directory.Delete(pluginsDir, true);
            Directory.CreateDirectory(pluginsDir);

            AnsiConsole.MarkupLine("[green]All mod traces deleted.[/]");
        }
        catch
        {
            AnsiConsole.MarkupLine("[red]Failed to delete mod traces.[/]");
        }

        Utils.Pause();
    }
}
