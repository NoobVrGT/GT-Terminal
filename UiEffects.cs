using Spectre.Console;
using System;
using System.Threading;

namespace GTTerminal.Features;

public static class UiEffects
{
    public static void Spinner(string message, Action action)
    {
        AnsiConsole.Status()
            .Start(message, ctx =>
            {
                action();
            });
    }

    public static void SimpleSpinner(string message, int ms = 800)
    {
        AnsiConsole.Status()
            .Start(message, ctx =>
            {
                Thread.Sleep(ms);
            });
    }

    public static void ProgressDownload(string taskName, Func<IProgress<double>, bool> work)
    {
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask(taskName, maxValue: 100);

                var progress = new Progress<double>(value =>
                {
                    task.Value = value * 100;
                });

                bool ok = work(progress);

                task.StopTask();

                if (ok)
                    AnsiConsole.MarkupLine(ThemeManager.Success("Done."));
                else
                    AnsiConsole.MarkupLine(ThemeManager.Error("Failed."));
            });
    }
}
