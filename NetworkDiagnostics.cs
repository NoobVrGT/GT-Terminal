using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GTTerminal.Features;

public static class NetworkDiagnostics
{
    // ============================
    // B: PORT SCAN (LOCAL)
    // ============================
    public static void RunPortScan()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Local Port Scan[/]\n");

        int start = 1;
        int end = 1024;

        var table = new Table();
        table.Title("[bold]Open Ports (1–1024)[/]");
        table.AddColumn("Port");
        table.AddColumn("Protocol");
        table.AddColumn("Status");

        foreach (var port in GetOpenTcpPorts(start, end))
            table.AddRow(port.ToString(), "TCP", "[green]OPEN[/]");

        foreach (var port in GetOpenUdpPorts(start, end))
            table.AddRow(port.ToString(), "UDP", "[green]OPEN[/]");

        if (table.Rows.Count == 0)
            AnsiConsole.MarkupLine("[grey]No open ports detected in range.[/]");
        else
            AnsiConsole.Write(table);

        Utils.Pause();
    }

    private static IEnumerable<int> GetOpenTcpPorts(int start, int end)
    {
        var list = new List<int>();
        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnections = ipProps.GetActiveTcpConnections();
        IPEndPoint[] tcpListeners = ipProps.GetActiveTcpListeners();

        foreach (var ep in tcpListeners)
        {
            if (ep.Port >= start && ep.Port <= end)
                list.Add(ep.Port);
        }

        foreach (var conn in tcpConnections)
        {
            int port = conn.LocalEndPoint.Port;
            if (port >= start && port <= end && !list.Contains(port))
                list.Add(port);
        }

        list.Sort();
        return list;
    }

    private static IEnumerable<int> GetOpenUdpPorts(int start, int end)
    {
        var list = new List<int>();
        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] udpListeners = ipProps.GetActiveUdpListeners();

        foreach (var ep in udpListeners)
        {
            if (ep.Port >= start && ep.Port <= end)
                list.Add(ep.Port);
        }

        list.Sort();
        return list;
    }

    // ============================
    // C: NETWORK TRACE
    // ============================
    public static void RunNetworkTrace()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Network Trace[/]\n");

        var table = new Table();
        table.Title("[bold]Active TCP Connections[/]");
        table.AddColumn("Local");
        table.AddColumn("Remote");
        table.AddColumn("State");

        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
        foreach (var conn in ipProps.GetActiveTcpConnections())
        {
            table.AddRow(
                conn.LocalEndPoint.ToString(),
                conn.RemoteEndPoint.ToString(),
                conn.State.ToString());
        }

        if (table.Rows.Count == 0)
            AnsiConsole.MarkupLine("[grey]No active TCP connections.[/]");
        else
            AnsiConsole.Write(table);

        Utils.Pause();
    }

    // ============================
    // D: MOD NETWORK ACTIVITY (HEURISTIC)
    // ============================
    public static void RunModNetworkActivity()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Mod Network Activity (Heuristic)[/]\n");

        var table = new Table();
        table.Title("[bold]Suspicious / Interesting Connections[/]");
        table.AddColumn("Local");
        table.AddColumn("Remote");
        table.AddColumn("State");
        table.AddColumn("Tag");

        IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
        foreach (var conn in ipProps.GetActiveTcpConnections())
        {
            string tag = ClassifyConnection(conn);
            if (tag != null)
            {
                table.AddRow(
                    conn.LocalEndPoint.ToString(),
                    conn.RemoteEndPoint.ToString(),
                    conn.State.ToString(),
                    tag);
            }
        }

        if (table.Rows.Count == 0)
            AnsiConsole.MarkupLine("[grey]No mod‑like network activity detected.[/]");
        else
            AnsiConsole.Write(table);

        Utils.Pause();
    }

    private static string? ClassifyConnection(TcpConnectionInformation conn)
    {
        string remote = conn.RemoteEndPoint.ToString().ToLowerInvariant();

        if (remote.Contains("7777") || remote.Contains("9700"))
            return "[yellow]Possible game/mod server[/]";

        if (remote.Contains("127.0.0.1"))
            return "[yellow]Local loopback (possible local mod server)[/]";

        if (remote.Contains("27015") || remote.Contains("27016"))
            return "[yellow]Steam‑related port[/]";

        return null;
    }

    // ============================
    // F: GORILLA TAG SERVER STATUS
    // ============================
    public static void RunGorillaTagStatus()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Gorilla Tag Server Status[/]\n");

        var table = new Table();
        table.Title("[bold]Server Reachability[/]");
        table.AddColumn("Target");
        table.AddColumn("Host");
        table.AddColumn("Status");
        table.AddColumn("Latency (ms)");

        AddPingRow(table, "GorillaTag", "gorillatag.net");
        AddPingRow(table, "Steam", "store.steampowered.com");
        AddPingRow(table, "Oculus", "oculus.com");

        AnsiConsole.Write(table);
        Utils.Pause();
    }

    private static void AddPingRow(Table table, string name, string host)
    {
        try
        {
            using var ping = new Ping();
            var reply = ping.Send(host, 2000);

            if (reply.Status == IPStatus.Success)
            {
                table.AddRow(
                    name,
                    host,
                    "[green]Online[/]",
                    reply.RoundtripTime.ToString());
            }
            else
            {
                table.AddRow(
                    name,
                    host,
                    "[red]Unreachable[/]",
                    "-");
            }
        }
        catch
        {
            table.AddRow(
                name,
                host,
                "[red]Error[/]",
                "-");
        }
    }

    // ============================
    // PING WRAPPER (for menu items)
    // ============================
    public static void RunPing(string host, string label)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold yellow]Ping {label} ({host})[/]\n");

        try
        {
            using var ping = new Ping();
            var reply = ping.Send(host, 2000);

            if (reply.Status == IPStatus.Success)
            {
                AnsiConsole.MarkupLine($"[green]Ping successful.[/]");
                AnsiConsole.MarkupLine($"Roundtrip: [bold]{reply.RoundtripTime} ms[/]");
                AnsiConsole.MarkupLine($"Address: [bold]{reply.Address}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Ping failed: {reply.Status}[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Ping error:[/] {ex.Message}");
        }

        Utils.Pause();
    }

    internal static void Run(Settings settings)
    {
        throw new NotImplementedException();
    }
}


