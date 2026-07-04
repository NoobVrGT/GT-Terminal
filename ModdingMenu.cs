using Spectre.Console;

namespace GTTerminal.Features;

public static class ModdingMenu
{
    public static void Run(Settings settings)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Modding Menu[/]")
                    .AddChoices(
                        "Install BepInEx",
                        "Install Seralyth",
                        "Quick Install",
                        "Find Installed Mods",
                        "Mod Browser",
                        "Back"
                    ));

            switch (choice)
            {
                case "Install BepInEx":
                    InstallBepInEx.Run(settings);
                    break;

                case "Install Seralyth":
                    InstallSeralyth.Run(settings);
                    break;

                case "Quick Install":
                    QuickInstall.Run(settings);
                    break;

                case "Find Installed Mods":
                    FindInstalledMods.Run(settings);
                    break;

                case "Mod Browser":
                    ModBrowser.Run(settings);
                    break;

                case "Back":
                    return;
            }
        }
    }
}
