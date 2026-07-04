using Spectre.Console;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GTTerminal.Features;

public static class StarterPackageInstaller
{
    private static readonly HttpClient http = new HttpClient();

    public static void Run(Settings settings)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[cyan]Installing Starter Package...[/]");

        string gamePath = settings.GamePath;
        string pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");

        Directory.CreateDirectory(pluginsDir);

        InstallBepInEx(gamePath).Wait();
        InstallSeralyth(pluginsDir).Wait();
        InstallUtilla(pluginsDir).Wait();

        AnsiConsole.MarkupLine("[green]Starter Package installed successfully![/]");
        AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
        Console.ReadKey();
    }

    private static async Task InstallBepInEx(string gamePath)
{
    AnsiConsole.MarkupLine("[yellow]Downloading BepInEx...[/]");

    string url = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip";
    string zipPath = Path.Combine(gamePath, "bepinex.zip");

    try
    {
        // Download with redirect support
        using HttpClient client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(url);

        // Validate ZIP (GitHub sometimes returns HTML)
        if (bytes.Length < 50000) // BepInEx is ~5–7 MB
        {
            AnsiConsole.MarkupLine("[red]Download failed: GitHub returned an invalid file.[/]");
            Utils.Pause();
            return;
        }

        File.WriteAllBytes(zipPath, bytes);

        // Ensure BepInEx folder exists BEFORE extraction
        string bepFolder = Path.Combine(gamePath, "BepInEx");
        Directory.CreateDirectory(bepFolder);

        // Extract safely
        ZipFile.ExtractToDirectory(zipPath, gamePath, true);

        File.Delete(zipPath);

        AnsiConsole.MarkupLine("[green]BepInEx installed.[/]");
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]BepInEx installation failed:[/] {ex.Message}");
        Utils.Pause();
    }
}


    private static async Task InstallSeralyth(string pluginsDir)
    {
        AnsiConsole.MarkupLine("[yellow]Downloading Seralyth...[/]");

        string url = "https://github.com/Seralyth/Seralyth-Menu/releases/latest";
        string dllPath = Path.Combine(pluginsDir, "Seralyth.dll");

        var bytes = await http.GetByteArrayAsync(url);
        File.WriteAllBytes(dllPath, bytes);

        AnsiConsole.MarkupLine("[green]Seralyth installed.[/]");
    }

   private static async Task InstallUtilla(string pluginsDir)
{
    AnsiConsole.MarkupLine("[yellow]Downloading Utilla...[/]");

    string url = "https://github.com/Seralyth/Utilla/releases/latest/download/Utilla.dll";

    // Create Utilla subfolder inside plugins
    string utillaFolder = Path.Combine(pluginsDir, "Utilla");
    Directory.CreateDirectory(utillaFolder);

    // Save Utilla.dll inside that folder
    string dllPath = Path.Combine(utillaFolder, "Utilla.dll");

    var bytes = await http.GetByteArrayAsync(url);
    File.WriteAllBytes(dllPath, bytes);

    AnsiConsole.MarkupLine("[green]Utilla installed.[/]");
}

}
