using System.Text.Json.Serialization;

namespace MicroslopPeon;

/// <summary>
/// App config: pack folder (openpeon.json + avatar.png + sounds/), volume, hook map, toast.
/// </summary>
public sealed class AppConfig
{
    /// <summary>Folder containing openpeon.json, avatar.png, sounds/ — any name (e.g. peon, myorc).</summary>
    [JsonPropertyName("packFolder")]
    public string? PackFolder { get; set; }

    [JsonPropertyName("peonFolder")]
    public string? PeonFolder { get; set; }

    [JsonPropertyName("hookToCategory")]
    public Dictionary<string, string>? HookToCategory { get; set; }

    /// <summary>Playback volume 0.0–1.0 (default 1.0).</summary>
    [JsonPropertyName("volume")]
    public double Volume { get; set; } = 1.0;

    [JsonPropertyName("toastDurationSeconds")]
    public int ToastDurationSeconds { get; set; } = 4;

    [JsonPropertyName("toastTitle")]
    public string? ToastTitle { get; set; }

    /// <summary>
    /// Resolve pack folder: config packFolder or peonFolder, then exe-relative "peon", then parent "peon".
    /// </summary>
    public static string ResolvePackFolder(string? configPackFolder, string? configPeonFolder)
    {
        var fromPack = ResolveOne(configPackFolder);
        if (!string.IsNullOrEmpty(fromPack)) return fromPack;
        var fromPeon = ResolveOne(configPeonFolder);
        if (!string.IsNullOrEmpty(fromPeon)) return fromPeon;

        var exeDir = AppContext.BaseDirectory;
        var next = Path.Combine(exeDir, "peon");
        if (Directory.Exists(next)) return Path.GetFullPath(next);
        var parent = Path.Combine(exeDir, "..", "peon");
        if (Directory.Exists(parent)) return Path.GetFullPath(parent);
        return Path.GetFullPath(next);
    }

    static string? ResolveOne(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        var expanded = Environment.ExpandEnvironmentVariables(path);
        return Directory.Exists(expanded) ? Path.GetFullPath(expanded) : null;
    }

    /// <summary>
    /// Default hook event name → openpeon category.
    /// </summary>
    public static IReadOnlyDictionary<string, string> DefaultHookToCategory => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["stop"] = "task.complete",
        ["afterFileEdit"] = "task.acknowledge",
        ["sessionStart"] = "session.start",
        ["beforeSubmitPrompt"] = "task.acknowledge",
        ["task.error"] = "task.error",
        ["input.required"] = "input.required",
        ["resource.limit"] = "resource.limit",
        ["user.spam"] = "user.spam",
    };

    /// <summary>
    /// Get category for hook; config overrides, then defaults.
    /// </summary>
    public string? GetCategoryForHook(string? hookEventName)
    {
        if (string.IsNullOrWhiteSpace(hookEventName))
            return null;

        if (HookToCategory != null && HookToCategory.TryGetValue(hookEventName, out var fromConfig) && !string.IsNullOrWhiteSpace(fromConfig))
            return fromConfig;

        return DefaultHookToCategory.TryGetValue(hookEventName, out var fromDefault) ? fromDefault : null;
    }
}
