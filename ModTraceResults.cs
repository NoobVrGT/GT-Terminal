using System;
using System.Collections.Generic;

namespace GTTerminal.Features;

public class ModTraceResults
{
    public string GamePath { get; set; } = "";

    public string[] SuspiciousFiles { get; set; } = Array.Empty<string>();
    public string[] Plugins { get; set; } = Array.Empty<string>();
    public string[] Patchers { get; set; } = Array.Empty<string>();
    public string[] ConfigFiles { get; set; } = Array.Empty<string>();
    public string[] CoreAssemblies { get; set; } = Array.Empty<string>();
    public string[] SeralythMenu { get; set; } = Array.Empty<string>();
    public string[] SeralythSounds { get; set; } = Array.Empty<string>();
    public string[] UserData { get; set; } = Array.Empty<string>();
    public string[] AppData { get; set; } = Array.Empty<string>();
    public string[] LeftoverAssets { get; set; } = Array.Empty<string>();
    public string[] UnknownFiles { get; set; } = Array.Empty<string>();

    public int RiskScore { get; set; }
}
