using System.Text.Json.Serialization;

namespace MicroslopPeon;

/// <summary>
/// openpeon.json model: categories with sounds[] (file, label).
/// </summary>
public sealed class OpenPeonRoot
{
    [JsonPropertyName("categories")]
    public Dictionary<string, OpenPeonCategory>? Categories { get; set; }
}

public sealed class OpenPeonCategory
{
    [JsonPropertyName("sounds")]
    public List<OpenPeonSound>? Sounds { get; set; }
}

public sealed class OpenPeonSound
{
    [JsonPropertyName("file")]
    public string? File { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

/// <summary>
/// Load openpeon.json and pick a random sound file for a category.
/// </summary>
public static class OpenPeonLoader
{
    public static OpenPeonRoot? Load(string peonFolder)
    {
        var path = Path.Combine(peonFolder, "openpeon.json");
        if (!File.Exists(path))
            return null;

        var json = File.ReadAllText(path);
        return System.Text.Json.JsonSerializer.Deserialize<OpenPeonRoot>(json);
    }

    /// <summary>
    /// Returns full path to a random sound file for the category, or null if none.
    /// </summary>
    public static string? PickRandomSoundPath(OpenPeonRoot? root, string peonFolder, string category)
    {
        if (root?.Categories == null || !root.Categories.TryGetValue(category, out var cat) || cat.Sounds == null || cat.Sounds.Count == 0)
            return null;

        var sound = cat.Sounds[Random.Shared.Next(cat.Sounds.Count)];
        var file = sound?.File;
        if (string.IsNullOrWhiteSpace(file))
            return null;

        var fullPath = Path.Combine(peonFolder, file);
        return File.Exists(fullPath) ? fullPath : null;
    }
}
