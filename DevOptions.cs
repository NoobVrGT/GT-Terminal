using Spectre.Console;

namespace GTTerminal.Features;

public static class DevOptions
{
    public static void Run(Settings settings)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Developer Options[/]")
                .AddChoices("Check mods.json", "Secret Access", "Back"));

        switch (choice)
        {
            case "Check mods.json":
                CheckModsJson(settings);
                break;

            case "Secret Access":
                SecretMenu.Run(settings);
                break;

            case "Back":
                return;
        }
    }

    private static void CheckModsJson(Settings settings)
    {
        AnsiConsole.Clear();

        string jsonPath = Path.Combine(settings.GamePath, "mods.json");

        if (!File.Exists(jsonPath))
        {
            AnsiConsole.MarkupLine("[red]mods.json not found.[/]");
            Utils.Pause();
            return;
        }

        string json = File.ReadAllText(jsonPath);

        AnsiConsole.MarkupLine("[green]mods.json contents:[/]");
        AnsiConsole.Write(new Markup($"[grey]{json}[/]"));

        Utils.Pause();
    }
}
