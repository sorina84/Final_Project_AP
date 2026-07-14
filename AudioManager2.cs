using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace GameEntity
{
    public static class AudioManager
    {
        public static bool MusicEnabled { get; set; } = true;
        public static bool SoundEnabled { get; set; } = true;

        // موسیقی
        private static WindowsMediaPlayer musicPlayer = new WindowsMediaPlayer();

        // افکت ها
        private static WindowsMediaPlayer shootPlayer = new WindowsMediaPlayer();
        private static WindowsMediaPlayer coinPlayer = new WindowsMediaPlayer();
        private static WindowsMediaPlayer explosionPlayer = new WindowsMediaPlayer();
        private static WindowsMediaPlayer gameOverPlayer = new WindowsMediaPlayer();

        //------------------------------------------------------

        private static string MusicFolder =>
            Path.Combine(Application.StartupPath, "Music2");

        private static string GetPath(string fileName)
        {
            return Path.Combine(MusicFolder, fileName);
        }

        //------------------------------------------------------

        public static void PlayMenuMusic()
        {
            if (!MusicEnabled)
                return;

            musicPlayer.controls.stop();

            musicPlayer.URL = GetPath("menu_music.mp3");
            musicPlayer.settings.setMode("loop", true);
            musicPlayer.controls.play();
        }

        //------------------------------------------------------

        public static void PlayGameMusic()
        {
            if (!MusicEnabled)
                return;

            musicPlayer.controls.stop();

            musicPlayer.URL = GetPath("game_music.mp3");
            musicPlayer.settings.setMode("loop", true);
            musicPlayer.controls.play();
        }

        //------------------------------------------------------

        public static void PlayGameOverMusic()
        {
            if (!MusicEnabled)
                return;

            gameOverPlayer.controls.stop();

            gameOverPlayer.URL = GetPath("gameover.mp3");
            gameOverPlayer.settings.setMode("loop", false);
            gameOverPlayer.controls.play();
        }

        //------------------------------------------------------

        public static void StopMusic()
        {
            musicPlayer.controls.stop();
        }

        //------------------------------------------------------

        public static void PlayShoot()
        {
            if (!SoundEnabled)
                return;

            shootPlayer.controls.stop();
            shootPlayer.URL = GetPath("shoot.mp3");
            shootPlayer.controls.play();
        }

        //------------------------------------------------------

        public static void PlayCoin()
        {
            if (!SoundEnabled)
                return;

            coinPlayer.controls.stop();
            coinPlayer.URL = GetPath("coin.mp3");
            coinPlayer.controls.play();
        }

        //------------------------------------------------------

        public static void PlayExplosion()
        {
            if (!SoundEnabled)
                return;

            explosionPlayer.controls.stop();
            explosionPlayer.URL = GetPath("explosion.mp3");
            explosionPlayer.controls.play();
        }
    }
}