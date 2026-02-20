namespace MicroslopPeon;

/// <summary>
/// Top-most smooth, horizontal dark toast with peon avatar and colored title; closes after N seconds.
/// No title bar, no close/move — borderless. Hidden from taskbar and Alt+Tab.
/// </summary>
public sealed class ToastForm : Form
{
    private readonly System.Windows.Forms.Timer _closeTimer;

    private const int WS_EX_TOOLWINDOW = 0x00000080;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_TOOLWINDOW;
            return cp;
        }
    }

    public ToastForm(string avatarPath, string title, int durationSeconds)
    {
        FormBorderStyle = FormBorderStyle.None;
        ControlBox = false;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(400, 80); // Daha yatay ve ince boyut
        BackColor = Color.FromArgb(26, 26, 26); // Koyu siyah arka plan (resimdeki siyahla uyumlu)
        DoubleBuffered = true;
        Text = string.Empty;

        var pictureBox = new PictureBox
        {
            SizeMode = PictureBoxSizeMode.Zoom,
            Size = new Size(60, 60), // Daha küçük avatar
            Location = new Point(10, 10), // Kenarlara daha yakın
            BackColor = Color.Transparent
        };
        if (File.Exists(avatarPath))
        {
            try
            {
                pictureBox.Image = Image.FromFile(avatarPath);
            }
            catch { /* ignore */ }
        }
        Controls.Add(pictureBox);

        var label = new Label
        {
            Text = title,
            AutoSize = false,
            Bounds = new Rectangle(80, 0, 310, 80), // Daha geniş ve yatay metin alanı
            ForeColor = Color.FromArgb(92, 219, 92), // Parlak yeşil yazı (orkun ten rengiyle uyumlu)
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            BackColor = BackColor,
            TextAlign = ContentAlignment.MiddleLeft // Metni sola hizala
        };
        Controls.Add(label);

        _closeTimer = new System.Windows.Forms.Timer();
        _closeTimer.Interval = Math.Max(1000, durationSeconds * 1000);
        _closeTimer.Tick += (_, _) =>
        {
            _closeTimer.Stop();
            Close();
        };
    }

    public void StartCloseTimer()
    {
        _closeTimer.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _closeTimer?.Stop();
        _closeTimer?.Dispose();
        base.OnFormClosing(e);
    }
}