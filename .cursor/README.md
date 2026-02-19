# Cursor hooks — MicroslopPeon

## Kurulum

1. Bu klasördeki `hooks.json.example` dosyasını `hooks.json` olarak kopyalayın.
2. `hooks.json` içinde `C:\path\to\MicroslopPeon.exe` kısmını kendi exe yolunuzla değiştirin.

Örnek (tek tırnak Windows CMD için gerekebilir):

```json
"command": "C:\\Users\\Adiniz\\source\\repos\\microslopPeon\\MicroslopPeon\\bin\\Release\\net8.0-windows\\win-x64\\publish\\MicroslopPeon.exe"
```

## Örnekteki hook'lar

- **sessionStart** — Agent oturumu başladığında ses.
- **stop** — Görev bittiğinde ses + “Task completed” toast.
- **afterFileEdit** — Dosya düzenlendikten sonra ses.

İstediğiniz hook'u ekleyip aynı `command` ile tetikleyebilirsiniz. Uygulama `config.json` içindeki `hookToCategory` ile hangi hook’un hangi sese/toast’a gideceğini belirler.

## Tüm hook isimleri (referans)

| Hook | Açıklama |
|------|----------|
| sessionStart / sessionEnd | Oturum başlangıç/bitiş |
| stop | Agent tamamlandı |
| beforeSubmitPrompt | Prompt gönderilmeden önce |
| beforeReadFile / afterFileEdit | Dosya okuma / düzenleme sonrası |
| beforeShellExecution / afterShellExecution | Shell öncesi/sonrası |
| beforeMCPExecution / afterMCPExecution | MCP araç öncesi/sonrası |
| preToolUse / postToolUse / postToolUseFailure | Araç kullanımı |
| subagentStart / subagentStop | Alt agent başlangıç/bitiş |
| preCompact | Bağlam sıkıştırma |
| afterAgentResponse / afterAgentThought | Agent yanıtı/düşüncesi sonrası |
| beforeTabFileRead / afterTabFileEdit | Tab tamamlama (dosya okuma/düzenleme) |

Yeni hook’lar için `config.json` → `hookToCategory` içine örn. `"afterShellExecution": "task.acknowledge"` eklemeniz yeterli.
