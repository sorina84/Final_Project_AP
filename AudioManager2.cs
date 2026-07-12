

using WMPLib;
using NAudio.Wave;
using System.IO;
using System.Windows.Forms;


namespace GameEntity
{
    public static class AudioManager
    {
        private static WindowsMediaPlayer musicPlayer = new WindowsMediaPlayer();
        private static readonly string MusicFolder =Path.Combine(Application.StartupPath, "Music2");

        public static bool MusicEnabled { get; set; } = true;
        public static bool SoundEnabled { get; set; } = true;
     
        //---------------------------

        public static void PlayMenuMusic()
        {
            if (!MusicEnabled)
                return;

            musicPlayer.controls.stop();

            musicPlayer.URL = @"Music\menu.mp3";
            musicPlayer.settings.setMode("loop", true);
            musicPlayer.controls.play();
        }

        //---------------------------

        public static void PlayGameMusic()
        {
            if (!MusicEnabled)
                return;

            musicPlayer.controls.stop();

            musicPlayer.URL = @"Music\game.mp3";
            musicPlayer.settings.setMode("loop", true);
            musicPlayer.controls.play();
        }

        //---------------------------

        public static void PlayGameOverMusic()
        {
            if (!MusicEnabled)
                return;

            musicPlayer.controls.stop();

            musicPlayer.URL = @"Music\gameover.mp3";
            musicPlayer.settings.setMode("loop", false);
            musicPlayer.controls.play();
        }

        //---------------------------

        public static void StopMusic()
        {
            musicPlayer.controls.stop();
        }

        //---------------------------

        private static void PlaySound(string fileName)
        {
            if (!SoundEnabled)
                return;

            string path = Path.Combine(MusicFolder, fileName);

            if (!File.Exists(path))
                return;

            var reader = new AudioFileReader(path);

            var output = new WaveOutEvent();

            output.Init(reader);

            output.Play();

            output.PlaybackStopped += (s, e) =>
            {
                output.Dispose();
                reader.Dispose();
            };
        }

        public static void PlayCoin()
        {
            PlaySound("coin.mp3");
        }

        public static void PlayShoot()
        {
            PlaySound("shoot.mp3");
        }
        public static void PlayExplosion()
        {
            PlaySound("explosion.mp3");
        }
    }
}
