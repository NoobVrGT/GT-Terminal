using System;
using System.IO;
using System.Text.Json;

namespace GTTerminal;

public class Settings
{
    private const string SettingsFile = "settings.json";

    public string GamePath { get; set; } = "";
    public string ColorTheme { get; set; } = "cyan";

    public bool LegacyMode { get; set; } = false;




    public static Settings Load()
    {
        if (!File.Exists(SettingsFile))
            return new Settings();

        try
        {
            var json = File.ReadAllText(SettingsFile);
            var settings = JsonSerializer.Deserialize<Settings>(json);
            return settings ?? new Settings();
        }
        catch
        {
            return new Settings();
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(SettingsFile, json);
    }
}
