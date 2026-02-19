using System.Diagnostics;
using System.Text.Json;
using System.Windows.Forms;

namespace MicroslopPeon;

static class Program
{
    private const string ConfigFileName = "config.json";
    private const string TaskCompleteCategory = "task.complete";
    private const string ToastSwitch = "--toast";

    [STAThread]
    static int Main(string[] args)
    {
        if (args is [ToastSwitch, {} avatarPath, {} title, {} durationStr, {} packFolder] && int.TryParse(durationStr, out var duration))
        {
            RunToastOnly(packFolder, avatarPath, title, duration);
            return 0;
        }

        if (!Console.IsInputRedirected)
            return 0;

        string? stdinLine = null;
        try
        {
            using var reader = new StreamReader(Console.OpenStandardInput());
            stdinLine = reader.ReadLine();
        }
        catch { /* ignore */ }

        if (string.IsNullOrWhiteSpace(stdinLine))
            return 0;

        HookPayload? payload = null;
        try
        {
            payload = JsonSerializer.Deserialize<HookPayload>(stdinLine);
        }
        catch { /* ignore */ }

        var hookEventName = payload?.HookEventName;
        if (string.IsNullOrWhiteSpace(hookEventName))
            return 0;

        var config = LoadConfig();
        var category = config.GetCategoryForHook(hookEventName);
        if (string.IsNullOrWhiteSpace(category))
            return 0;

        var resolvedPack = AppConfig.ResolvePackFolder(config.PackFolder, config.PeonFolder);
        var openPeon = OpenPeonLoader.Load(resolvedPack);
        var soundPath = OpenPeonLoader.PickRandomSoundPath(openPeon, resolvedPack, category);

        var isTaskComplete = category.Equals(TaskCompleteCategory, StringComparison.OrdinalIgnoreCase);
        if (!string.IsNullOrEmpty(soundPath) && !isTaskComplete)
            SoundHelper.Play(soundPath, config.Volume, sync: true);

        if (isTaskComplete)
        {
            var toastAvatarPath = Path.Combine(resolvedPack, "avatar.png");
            var toastTitle = config.ToastTitle ?? "Task completed";
            var toastDuration = config.ToastDurationSeconds;

            var exePath = Environment.ProcessPath ?? Application.ExecutablePath;
            var toastSpawned = false;
            if (!string.IsNullOrEmpty(exePath))
            {
                try
                {
                    using var p = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = exePath,
                            UseShellExecute = true,
                            ArgumentList = { ToastSwitch, toastAvatarPath, toastTitle, toastDuration.ToString(), resolvedPack }
                        }
                    };
                    p.Start();
                    toastSpawned = true;
                }
                catch { /* fallback to in-process */ }
            }
            if (!toastSpawned)
                RunToastInProcess(resolvedPack, toastAvatarPath, toastTitle, toastDuration);
        }

        return 0;
    }

    static void RunToastOnly(string packFolder, string avatarPath, string title, int durationSeconds)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var config = LoadConfig();
        var openPeon = OpenPeonLoader.Load(packFolder);
        var soundPath = OpenPeonLoader.PickRandomSoundPath(openPeon, packFolder, TaskCompleteCategory);
        if (!string.IsNullOrEmpty(soundPath))
            SoundHelper.Play(soundPath, config.Volume, sync: false);

        var form = new ToastForm(avatarPath, title, durationSeconds);
        var area = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 320, 120);
        form.Location = new Point(area.Left + (area.Width - form.Width) / 2, area.Top + 16);
        form.StartCloseTimer();
        Application.Run(form);
    }

    static void RunToastInProcess(string packFolder, string avatarPath, string title, int duration)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var config = LoadConfig();
        var openPeon = OpenPeonLoader.Load(packFolder);
        var soundPath = OpenPeonLoader.PickRandomSoundPath(openPeon, packFolder, TaskCompleteCategory);
        if (!string.IsNullOrEmpty(soundPath))
            SoundHelper.Play(soundPath, config.Volume, sync: false);

        var form = new ToastForm(avatarPath, title, duration);
        var area = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 320, 120);
        form.Location = new Point(area.Left + (area.Width - form.Width) / 2, area.Top + 16);
        form.StartCloseTimer();
        Application.Run(form);
    }

    static AppConfig LoadConfig()
    {
        var exeDir = AppContext.BaseDirectory;
        var configPath = Path.Combine(exeDir, ConfigFileName);
        if (!File.Exists(configPath))
            return new AppConfig();

        try
        {
            var json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        catch
        {
            return new AppConfig();
        }
    }
}
