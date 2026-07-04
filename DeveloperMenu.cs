using Spectre.Console;

namespace GTTerminal.Features;

public static class DeveloperMenu
{
    public static void Run(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Developer Menu[/]")
                    .AddChoices(
                        "Developer Options",
                        "Enable Legacy Mode (Unsafe)",
                        "Disable Legacy Mode",
                        "Back"
                    ));

            switch (choice)
            {
                case "Developer Options":
                    DevOptions.Run(settings);
                    break;

                case "Enable Legacy Mode (Unsafe)":
                    EnableLegacyMode(settings);
                    break;

                case "Disable Legacy Mode":
                    settings.LegacyMode = false;
                    settings.Save();
                    AnsiConsole.MarkupLine("[green]Legacy Mode disabled.[/]");
                    Utils.Pause();
                    break;

                case "Back":
                    return;
            }
        }
    }

    private static void EnableLegacyMode(Settings settings)
    {
        AnsiConsole.MarkupLine("[red]WARNING: Legacy Mode is unsafe and unorganized.[/]");
        AnsiConsole.MarkupLine("[red]It may break your install or behave unpredictably.[/]");
        AnsiConsole.MarkupLine("[yellow]Are you sure you want to continue?[/]");

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Yes", "No"));

        if (confirm == "No")
            return;

        // Second safeguard
        AnsiConsole.Markup("[yellow]Type YES to confirm: [/]");
        string input = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (input != "YES")
        {
            AnsiConsole.MarkupLine("[red]Cancelled.[/]");
            Utils.Pause();
            return;
        }

        settings.LegacyMode = true;
        settings.Save();

        AnsiConsole.MarkupLine("[green]Legacy Mode enabled![/]");
        Utils.Pause();
    }
}
