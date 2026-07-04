using Spectre.Console;

namespace GTTerminal.Features;

public static class ToolsMenu
{
    public static void Run(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Tools Menu[/]")
                    .AddChoices(
                        "Launch Gorilla Tag",
                        "Open Mods Folder",
                        "Backup / Restore",
                        "Verify Install",
                        "Back"
                    ));

            switch (choice)
            {
                case "Launch Gorilla Tag":
                    OpenGTAG.Run(settings);
                    break;

                case "Open Mods Folder":
                    OpenModsFolder.Run(settings);
                    break;

                case "Backup / Restore":
                    BackupRestore.Run(settings);
                    break;

                case "Verify Install":
                    VerifyInstall.Run(settings);
                    break;

                case "Back":
                    return;
            }
        }
    }
}
    