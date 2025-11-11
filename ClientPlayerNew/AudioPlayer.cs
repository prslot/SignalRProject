using ClientPlayerNew.Players;
using NetCoreAudio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlayerNew
{
    public class AudioPlayer : Players.IPlayer
    {

        private readonly Players.IPlayer _audioPlayer;


        public bool Playing => _audioPlayer.Playing;

        public bool Paused => _audioPlayer.Paused;

        public event EventHandler PlaybackFinished;

        public AudioPlayer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _audioPlayer = new WindowsPlayer();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _audioPlayer = new LinuxPlayer();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _audioPlayer = new MacPlayer();
            else
                throw new Exception("No implementation exist for the current OS");

            _audioPlayer.PlaybackFinished += OnPlaybackFinished;
        }

        public async Task Pause()
        {
            await _audioPlayer.Pause();
        }

        public async Task Play(string fileName)
        {
            await _audioPlayer.Play(fileName);
        }

        public async Task Resume()
        {
            await _audioPlayer.Resume();
        }

        public async Task SetVolume(byte percent)
        {
            await _audioPlayer.SetVolume(percent);
        }

        public async Task Stop()
        {
            await _audioPlayer.Stop();
        }

        private void OnPlaybackFinished(object sender, EventArgs e)
        {
            PlaybackFinished?.Invoke(this, e);
        }
    }
}
