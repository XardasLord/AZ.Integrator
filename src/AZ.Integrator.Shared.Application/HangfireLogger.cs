using Hangfire.Console;
using Hangfire.Server;

namespace AZ.Integrator.Shared.Application;

public static class HangfireLogger
{
    public static void Info(this PerformContext context, string message)
    {
        context.SetTextColor(ConsoleTextColor.Gray);
        context.WriteLine($"[INFO] {message}");
    }

    public static void Success(this PerformContext context, string message)
    {
        context.SetTextColor(ConsoleTextColor.DarkGreen);
        context.WriteLine($"[SUCCESS] {message}");
        context.ResetTextColor();
    }

    public static void Warning(this PerformContext context, string message)
    {
        context.SetTextColor(ConsoleTextColor.Yellow);
        context.WriteLine($"[WARN] {message}");
        context.ResetTextColor();
    }

    public static void Error(this PerformContext context, string message)
    {
        context.SetTextColor(ConsoleTextColor.Red);
        context.WriteLine($"[ERROR] {message}");
        context.ResetTextColor();
    }

    public static void Step(this PerformContext context, string message)
    {
        context.SetTextColor(ConsoleTextColor.Cyan);
        context.WriteLine($"→ {message}");
        context.ResetTextColor();
    }
}