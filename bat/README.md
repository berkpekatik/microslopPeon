# Test batch files (categories)

Each batch file sends one JSON line to MicroslopPeon.exe (stdin) to simulate a Cursor hook. **Run them from CMD** (not PowerShell).

## How to use

1. Build or publish the app (see main README).
2. Make sure the exe can find its **pack folder**: put a pack (e.g. `peon` with `openpeon.json`, `sounds/`, `avatar.png`) next to the exe, or set **packFolder** (or **peonFolder**) and **volume** in `config.json` next to the exe.
3. From the repo root or from `bat`:
   ```cmd
   cd bat
   test-task-complete.bat
   ```

## Exe path

Each script looks for the exe in this order:

1. `..\MicroslopPeon\bin\Release\net8.0-windows\win-x64\publish\MicroslopPeon.exe` (after `dotnet publish`)
2. `..\MicroslopPeon\bin\Release\net8.0-windows\MicroslopPeon.exe` (after `dotnet build`)

If your exe is somewhere else, edit the `set "EXE=..."` line in the batch file.

## Scripts

| File | Hook / category | What happens |
|------|------------------|--------------|
| test-session-start.bat | sessionStart → session.start | Sound only |
| test-task-acknowledge.bat | afterFileEdit → task.acknowledge | Sound only |
| test-task-complete.bat | stop → task.complete | Sound + blue "Task completed" popup (avatar) |
| test-task-error.bat | task.error → task.error | Sound only |
| test-input-required.bat | input.required → input.required | Sound only |
| test-resource-limit.bat | resource.limit → resource.limit | Sound only |
| test-user-spam.bat | user.spam → user.spam | Sound only |

Sound volume and pack folder come from **config.json** next to the exe (see main README).
