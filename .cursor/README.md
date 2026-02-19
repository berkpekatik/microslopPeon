# Cursor hooks — MicroslopPeon

## Setup

1. Copy `hooks.json.example` in this folder to `hooks.json`.
2. In `hooks.json`, replace `C:\path\to\MicroslopPeon.exe` with your actual exe path.

Example:

```json
"command": "C:\\Users\\YourName\\source\\repos\\microslopPeon\\MicroslopPeon\\bin\\Release\\net8.0-windows\\win-x64\\publish\\MicroslopPeon.exe"
```

## Hooks in the example

- **sessionStart** — Sound when the agent session starts.
- **stop** — Sound + “Task completed” toast when the task finishes.
- **afterFileEdit** — Sound after a file is edited.

You can add more hooks with the same `command`. The app uses `hookToCategory` in `config.json` to decide which hook maps to which sound or toast.

## All hook names (reference)

| Hook | Description |
|------|-------------|
| sessionStart / sessionEnd | Session start/end |
| stop | Agent completed |
| beforeSubmitPrompt | Before prompt is sent |
| beforeReadFile / afterFileEdit | File read / after edit |
| beforeShellExecution / afterShellExecution | Before/after shell |
| beforeMCPExecution / afterMCPExecution | Before/after MCP tool |
| preToolUse / postToolUse / postToolUseFailure | Tool use |
| subagentStart / subagentStop | Subagent start/stop |
| preCompact | Context compaction |
| afterAgentResponse / afterAgentThought | After agent response/thought |
| beforeTabFileRead / afterTabFileEdit | Tab completion (file read/edit) |

To add new hooks, add entries in `config.json` → `hookToCategory`, e.g. `"afterShellExecution": "task.acknowledge"`.
