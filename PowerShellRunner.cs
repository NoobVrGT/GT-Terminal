using Spectre.Console;
using System;
using System.Diagnostics;

namespace GTTerminal;

public static class PowerShellRunner
{
    public static int RunScript(string script)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            if (proc == null)
            {
                AnsiConsole.MarkupLine("[red]Failed to start PowerShell.[/]");
                return -1;
            }

            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
                AnsiConsole.MarkupLine($"[grey]{output}[/]");

            if (!string.IsNullOrWhiteSpace(error))
                AnsiConsole.MarkupLine($"[red]{error}[/]");

            return proc.ExitCode;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]PowerShell error.[/]");
            AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
            return -1;
        }
    }
}
