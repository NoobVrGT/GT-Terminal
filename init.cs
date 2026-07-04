using Spectre.Console;

namespace GTTerminal;

public static class Init
{
    public static Settings Initialize()
    {
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("cyan"))
            .Start("Initializing GT Terminal...", ctx =>
            {
                System.Threading.Thread.Sleep(600);
            });

        var settings = Settings.Load();

        if (string.IsNullOrWhiteSpace(settings.GamePath))
        {
            var auto = PathDetection.AutoDetectGamePath();
            if (!string.IsNullOrWhiteSpace(auto))
            {
                settings.GamePath = auto;
                settings.Save();
            }
        }

        return settings;
    }
}
