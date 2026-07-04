using Spectre.Console;

namespace GTTerminal.Features;

public static class SystemMenu
{
    public static void Run(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]System Menu[/]")
                    .AddChoices(
                        "Terminal Status",
                        "Check for Updates",
                        "Settings",
                        "Back"
                    ));

            switch (choice)
            {
                case "Terminal Status":
                    TerminalStatus.Run(settings);
                    break;

                case "Check for Updates":
                    CheckUpdates.Run(settings);
                    break;

                case "Settings":
                    SettingsMenu.Run(settings);
                    break;

                case "Back":
                    return;
            }
        }
    }
}
