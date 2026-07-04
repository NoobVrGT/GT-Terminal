using Spectre.Console;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class ModDetection
{
    // Patterns from your batch script (ported to C#)
    private static readonly string[] SuspiciousPatterns =
    {
        "seralyth", "atlas", "menu", "bepinex", "plugin", "mod",
        "monke", "utilla", "grip", "broken", "space", "pull",
        "bark", "fly", "hack", "ban"
    };

    public static void Run(string gamePath)
    {
        AnsiConsole.Clear();

        if (string.IsNullOrWhiteSpace(gamePath) || !Directory.Exists(gamePath))
        {
            AnsiConsole.MarkupLine("[red]Game path not found.[/]");
            Console.ReadKey(true);
            return;
        }

        var root = Path.GetFullPath(gamePath);

        // Protected folders (same logic as your batch script)
        var protectedFolders = new[]
        {
            Path.Combine(root, "Gorilla Tag_Data"),
            Path.Combine(root, "reshade-shaders")
        };

        var suspiciousFiles = Directory
            .EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
            .Where(file =>
            {
                string full = Path.GetFullPath(file);

                // Skip protected folders
                if (protectedFolders.Any(p => full.StartsWith(p)))
                    return false;

                // Check patterns
                return SuspiciousPatterns.Any(pattern =>
                    Regex.IsMatch(Path.GetFileName(full), pattern, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(full, pattern, RegexOptions.IgnoreCase));
            })
            .ToList();

        // UI Output
        var panel = new Panel("[bold cyan]Mod Detection Results[/]");
        panel.Border = BoxBorder.Rounded;
        panel.BorderStyle = new Style(Color.Cyan);
        AnsiConsole.Write(panel);

        if (suspiciousFiles.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]No suspicious mod traces found.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]Suspicious mod traces detected:[/]\n");

            foreach (var file in suspiciousFiles)
            {
                AnsiConsole.MarkupLine($"[red]- {file}[/]");
            }
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey(true);
    }
}
