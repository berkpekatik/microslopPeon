using NAudio.Wave;

namespace MicroslopPeon;

/// <summary>
/// Play WAV with volume (0.0–1.0).
/// </summary>
public static class SoundHelper
{
    public static void Play(string wavPath, double volume, bool sync)
    {
        var vol = (float)Math.Clamp(volume, 0.0, 1.0);
        if (string.IsNullOrEmpty(wavPath) || !File.Exists(wavPath))
            return;

        if (sync)
            PlaySync(wavPath, vol);
        else
            ThreadPool.QueueUserWorkItem(_ => PlaySync(wavPath, vol));
    }

    static void PlaySync(string wavPath, float vol)
    {
        try
        {
            using var reader = new AudioFileReader(wavPath);
            using var channel = new WaveChannel32(reader) { Volume = vol };
            using var waveOut = new WaveOutEvent();
            waveOut.Init(channel);
            waveOut.Play();
            while (waveOut.PlaybackState == PlaybackState.Playing)
                Thread.Sleep(50);
        }
        catch { /* ignore */ }
    }
}
