using System.Text.Json.Serialization;

namespace MicroslopPeon;

/// <summary>
/// Cursor hook stdin JSON payload (hook_event_name, conversation_id, workspace_roots, etc.).
/// </summary>
public sealed class HookPayload
{
    [JsonPropertyName("hook_event_name")]
    public string? HookEventName { get; set; }

    [JsonPropertyName("conversation_id")]
    public string? ConversationId { get; set; }

    [JsonPropertyName("generation_id")]
    public string? GenerationId { get; set; }

    [JsonPropertyName("workspace_roots")]
    public string[]? WorkspaceRoots { get; set; }
}
