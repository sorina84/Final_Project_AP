using System;
using System.IO;
using System.Media;

namespace GameEntity
{
    public static class AudioManager
    {
        public static bool MusicEnabled { get; set; } = true;
        public static bool SoundEnabled { get; set; } = true;

        private static SoundPlayer _musicPlayer;
        private static SoundPlayer _shootPlayer;
        private static SoundPlayer _coinPlayer;
        private static SoundPlayer _gameOverPlayer;

        private static string FindSoundPath(string fileName)
        {
            return AssetLoader.FindAssetPath(fileName);
        }

        public static void PlayMenuMusic()
        {
            PlayLoopingMusic("menu_music.wav");
        }

        public static void PlayGameMusic()
        {
            PlayLoopingMusic("game_music.wav");
        }

        public static void PlayGameOverMusic()
        {
            if (!SoundEnabled)
                return;

            PlaySoundEffect(ref _gameOverPlayer, "game_over.wav");
        }

        public static void StopMusic()
        {
            try
            {
                if (_musicPlayer != null)
                {
                    _musicPlayer.Stop();
                    _musicPlayer.Dispose();
                    _musicPlayer = null;
                }
            }
            catch
            {
                _musicPlayer = null;
            }
        }

        public static void PlayShoot()
        {
            if (!SoundEnabled)
                return;

            PlaySoundEffect(ref _shootPlayer, "shoot.wav");
        }

        public static void PlayCoin()
        {
            if (!SoundEnabled)
                return;

            PlaySoundEffect(ref _coinPlayer, "coin.wav");
        }

        private static void PlayLoopingMusic(string fileName)
        {
            if (!MusicEnabled)
                return;

            try
            {
                StopMusic();

                string path = FindSoundPath(fileName);

                if (path == null)
                    return;

                _musicPlayer = new SoundPlayer(path);
                _musicPlayer.Load();
                _musicPlayer.PlayLooping();
            }
            catch
            {
                // اگر فایل صدا خراب باشد یا wav واقعی نباشد، بازی کرش نکند.
            }
        }

        private static void PlaySoundEffect(ref SoundPlayer player, string fileName)
        {
            if (!SoundEnabled)
                return;

            try
            {
                string path = FindSoundPath(fileName);

                if (path == null)
                    return;

                if (player != null)
                {
                    player.Stop();
                    player.Dispose();
                    player = null;
                }

                player = new SoundPlayer(path);
                player.Load();
                player.Play();
            }
            catch
            {
                // اگر افکت صدا اجرا نشد، بازی متوقف نشود.
            }
        }
    }
}