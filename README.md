# MicroslopPeon

A small Windows app that plays sounds and shows a "Task completed" popup when Cursor IDE runs its hooks (e.g. when the agent finishes a task).

- One exe, no extra UI except the popup and a config file.
- You choose a **pack folder** (e.g. `peon` or any name) with sounds and an avatar. You can set **volume** and which hook does what in **config.json**.

This project is inspired by **openPeon** and **peonPing**; it is not a commercial product. Sound files are **peonPing-compatible** — you can use packs from the peonPing ecosystem (e.g. openpeon-style packs with `openpeon.json`, `sounds/`, `avatar.png`).

---

## Repository / Git

The **peon/** and **bat/** folders are in `.gitignore` and are not committed. Add your own pack (e.g. copy a peonPing-compatible pack into `peon/`) and the `bat/` test scripts locally if you need them; see **bat/README.md** for the script list and how to restore or create the batch files.

---

## How it works

1. **Cursor** runs your command (MicroslopPeon.exe) and sends one line of JSON to it (stdin). The JSON says which hook fired (e.g. `stop`, `sessionStart`).

2. **MicroslopPeon** reads that line and looks up the hook in config. Each hook is mapped to a **category** (e.g. `stop` → `task.complete`). Categories come from your pack’s JSON file (e.g. `openpeon.json`).

3. For that category it picks a **random sound** from the pack and plays it (using the **volume** from config).

4. If the category is **task.complete**, it also starts a **second process** (no console window). That process plays the sound again and shows a **blue popup** at the top of the screen with the pack’s avatar and "Task completed". So you see the popup and hear the sound together.

5. If you run the exe without stdin (e.g. double‑click), it does nothing and exits.

---

## What you need

- Windows 10 or 11
- .NET 8
- Cursor IDE (to use hooks)

---

## Build

```bash
dotnet build MicroslopPeon.sln -c Release
```

Single exe for sharing:

```bash
dotnet publish MicroslopPeon/MicroslopPeon.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Exe path: `MicroslopPeon/bin/Release/net8.0-windows/win-x64/publish/MicroslopPeon.exe` (or without `win-x64/publish` if you only run `dotnet build`).

---

## Setup

**1. Pack folder**

Put a folder next to the exe (or point to it in config). The folder can have any name (e.g. `peon`, `myorc`). Inside it you must have:

- `openpeon.json` – list of categories and sound files
- `sounds/*.wav` – the WAV files
- `avatar.png` – image used in the "Task completed" popup

**2. config.json** (optional, next to the exe)

```json
{
  "packFolder": "",
  "peonFolder": "",
  "volume": 1.0,
  "hookToCategory": {
    "stop": "task.complete",
    "afterFileEdit": "task.acknowledge",
    "sessionStart": "session.start"
  },
  "toastDurationSeconds": 4,
  "toastTitle": "Task completed"
}
```

- **packFolder** / **peonFolder**: Path to the pack folder. Leave empty to use a folder named `peon` next to the exe (or one level up).
- **volume**: Sound volume from 0.0 to 1.0 (default 1.0).
- **hookToCategory**: Which Cursor hook (e.g. `stop`) maps to which category (e.g. `task.complete`). Others use built‑in defaults if you don’t set them.
- **toastDurationSeconds**: How long the popup stays (only for task.complete).
- **toastTitle**: Text on the popup.

---

## Cursor hooks

Create a `hooks.json` file so Cursor runs MicroslopPeon on the events you want.

- In the project: `.cursor/hooks.json`
- For your user: `%USERPROFILE%\.cursor\hooks.json`

Example (use your real exe path):

```json
{
  "version": 1,
  "hooks": {
    "stop": [
      { "command": "C:\\path\\to\\MicroslopPeon.exe" }
    ]
  }
}
```

Cursor sends JSON to the exe’s stdin. The app reads it, plays the right sound, and for `task.complete` shows the popup (in a separate process so the window and sound work correctly).

---

## Testing from the command line

See **bat/** for batch files that send test JSON to the exe (e.g. `test-task-complete.bat`). Run them from CMD. The exe must find its config and pack folder (next to the exe or set in config). See **bat/README.md** for the list of test scripts.

---

## License

See the repo. This app is not for commercial use. Pack sounds and assets follow their own terms (e.g. openpeon pack author / CC-BY-NC-4.0).
