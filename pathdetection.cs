using System.IO;

namespace GTTerminal;

public static class PathDetection
{
    public static string AutoDetectGamePath()
    {
        string[] paths =
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag",
            @"D:\SteamLibrary\steamapps\common\Gorilla Tag",
            @"C:\Program Files\Meta Horizon\Software\Software\another-axiom-gorilla-tag",
            @"D:\Steam\steamapps\common\Gorilla Tag"
        };

        foreach (var p in paths)
        {
            if (Directory.Exists(p))
                return p;
        }

        return "";
    }
}
