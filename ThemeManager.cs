using Spectre.Console;

namespace GTTerminal.Features;

public enum Theme
{
    Default,
    Neon,
    Matrix,
    Vaporwave,
    GorillaTag
}

public static class ThemeManager
{
    public static Theme CurrentTheme { get; private set; } = Theme.Default;

    public static void SetTheme(Theme theme)
    {
        CurrentTheme = theme;
    }

    public static string Accent(string text)
    {
        return CurrentTheme switch
        {
            Theme.Neon      => $"[magenta]{text}[/]",
            Theme.Matrix    => $"[green]{text}[/]",
            Theme.Vaporwave => $"[pink1]{text}[/]",
            Theme.GorillaTag => $"[orange1]{text}[/]",
            _               => $"[yellow]{text}[/]"
        };
    }

    public static string Muted(string text)
    {
        return "[grey]" + text + "[/]";
    }

    public static string Error(string text)
    {
        return "[red]" + text + "[/]";
    }

    public static string Success(string text)
    {
        return "[green]" + text + "[/]";
    }
}
