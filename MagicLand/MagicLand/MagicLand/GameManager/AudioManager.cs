using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MagicLand
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AudioManager : GameComponent
    {
        static AudioManager audioManager = null;
        public static AudioManager Instance
        {
            get
            {
                return audioManager;
            }
        }
        static readonly string soundAssetLocation = "Sound\\"; //"Sounds\\";

        // Audio Data        
        Dictionary<string, SoundEffectInstance> soundBank;
        Dictionary<string, Song> musicBank;

        static string currentSong;
        static bool isSongLoop;
        static bool isMusicStop;
        static bool isSoundStop;

        private AudioManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Alustetaan AudioManager
        /// </summary>
        public static void Initialize(Game game)
        {
            audioManager = new AudioManager(game);
            audioManager.soundBank = new Dictionary<string, SoundEffectInstance>();
            audioManager.musicBank = new Dictionary<string, Song>();

            game.Components.Add(audioManager);
        }

        /// <summary>
        /// Ladataan yksitt‰inen ‰‰nitiedosto audioManager:n, 
        /// jolle parametrin‰ paikka (kansio) miss‰
        /// ladattava audio on ja nimi (sound).
        /// </summary>
        /// <param name="contentName"></param>
        /// <param name="sound"></param>
        public static void LoadSound(string fileName, string sound)
        {
            SoundEffect soundEffect = audioManager.Game.Content.Load<SoundEffect>(soundAssetLocation + fileName);
            SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

            if (!audioManager.soundBank.ContainsKey(sound))
            {
                audioManager.soundBank.Add(sound, soundEffectInstance);
            }
        }

        /// <summary>
        /// Ladataan yksitt‰inen musiikkikappale audioManager:n
        /// parametrein‰ paikka ja nimi.
        /// </summary>
        /// <param name="contentName"></param>
        /// <param name="sound"></param>
        public static void LoadSong(string fileName, string sound)
        {
            Song song = audioManager.Game.Content.Load<Song>(soundAssetLocation + fileName);

            if (!audioManager.musicBank.ContainsKey(sound))
            {
                audioManager.musicBank.Add(sound, song);
            }
        }

        public static void LoadSounds()
        {
            //LoadSound("Kitchen", "Kitchen");
            LoadSound("bubbling", "bubbling");
            LoadSound("item", "item");
            LoadSound("splats", "splats");
        }

        public static void LoadMusic()
        {
            //LoadSong("Night Cave", "Night Cave");
            //LoadSong("Bite", "Bite");
            //LoadSong("Muumi", "Muumi");
        }

        /// <summary>
        /// Indexer. Palauttaa sound instanssin nimen.
        /// </summary>
        /// <param name="soundName"></param>
        /// <returns></returns>
        public SoundEffectInstance this[string soundName]
        {
            get
            {
                if (audioManager.soundBank.ContainsKey(soundName))
                {
                    return audioManager.soundBank[soundName];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Soittaa ‰‰nitiedoston.
        /// </summary>
        /// <param name="soundName"></param>
        public static void PlaySound(string soundName)
        {
            isSoundStop = false;

            if (audioManager.soundBank.ContainsKey(soundName))
            {
                audioManager.soundBank[soundName].Play();
            }
        }

        /// <summary>
        /// Soittaa ‰‰nitiedoston luupissa.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="isLooped"></param>
        public static void PlaySound(string soundName, bool isLooped)
        {
            isSoundStop = false;

            if (audioManager.soundBank.ContainsKey(soundName))
            {
                if (audioManager.soundBank[soundName].IsLooped != isLooped)
                {
                    audioManager.soundBank[soundName].IsLooped = isLooped;
                }

                audioManager.soundBank[soundName].Play();
            }
        }

        /// <summary>
        /// Soittaa ‰‰nitiedoston ja t‰ss‰ myˆs ‰‰nen kovuuden s‰‰tˆ.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="isLooped"></param>
        /// <param name="volume"></param>
        public static void PlaySound(string soundName, bool isLooped, float volume)
        {
            isSoundStop = false;
            if (audioManager.soundBank.ContainsKey(soundName))
            {
                if (audioManager.soundBank[soundName].IsLooped != isLooped)
                {
                    audioManager.soundBank[soundName].IsLooped = isLooped;
                }

                audioManager.soundBank[soundName].Volume = volume;
                audioManager.soundBank[soundName].Play();
            }
        }

        /// <summary>
        /// Pys‰ytet‰‰n ‰‰nitiedoston. Jos ‰‰nitiedosto ei ole soimassa,
        /// t‰m‰ ei tee mit‰‰n.
        /// </summary>
        /// <param name="soundName"></param>
        public static void StopSound(string soundName)
        {
            isSoundStop = true;

            if (audioManager.soundBank.ContainsKey(soundName))
            {
                audioManager.soundBank[soundName].Stop();
            }
        }

        /// <summary>
        /// Pys‰ytet‰‰n kaikki ‰‰nitiedostot.
        /// </summary>
        public static void StopSounds()
        {
            isSoundStop = true;

            foreach (SoundEffectInstance sound in audioManager.soundBank.Values)
            {
                if (sound.State != SoundState.Stopped)
                {
                    sound.Stop();
                }
            }
        }

        /// <summary>
        /// Kaikki ‰‰nitiedostot pauselle.
        /// </summary>
        public static void PauseResumeSounds(bool resumeSounds)
        {
            SoundState state = resumeSounds ? SoundState.Paused : SoundState.Playing;

            foreach (SoundEffectInstance sound in audioManager.soundBank.Values)
            {
                if (sound.State == state)
                {
                    if (resumeSounds)
                    {
                        sound.Resume();
                    }
                    else
                    {
                        sound.Pause();
                    }
                }
            }
        }

        public static Boolean GetIsSoundStopped()
        {
            return isSoundStop;
        }

        /// <summary>
        /// Soittaa musiikin nimen mukaan. T‰m‰ pys‰ytt‰‰ jo soivan musiikin
        /// ensin. Musiikkia soitetaan luupissa kunnes musiikki pys‰ytt‰‰n.
        /// </summary>
        /// <param name="musicSoundName"></param>
        public static void PlayMusic(string musicSoundName, bool isLooped)
        {
            currentSong = musicSoundName;
            isSongLoop = isLooped;
            isMusicStop = false;

            if (audioManager.musicBank.ContainsKey(musicSoundName))
            {
                // Pys‰ytet‰‰n entinen musiikki.
                if (MediaPlayer.State != MediaState.Stopped)
                {
                    MediaPlayer.Stop();
                }

                try
                {
                    MediaPlayer.IsRepeating = true;
                }
                catch (UnauthorizedAccessException)
                {
                    // Simply do nothing. This will happen if the Zune application is launched.
                }

                try
                {
                    if (isLooped == true)
                    {
                        MediaPlayer.Play(audioManager.musicBank[musicSoundName]);
                        MediaPlayer.IsRepeating = true;
                    }
                    else
                        MediaPlayer.Play(audioManager.musicBank[musicSoundName]);
                }
                catch (InvalidOperationException)
                {
                    // Simply do nothing. This will happen if the Zune application is launched.
                }
            }
        }

        // Pausettaa soitettavan musiikin
        public static void PauseMusic(bool pause)
        {
            if (pause == true)
                MediaPlayer.Pause();
            else
            {
                MediaPlayer.Play(audioManager.musicBank[currentSong]);

                if (isSongLoop == true)
                    MediaPlayer.IsRepeating = true;
            }
        }

        /// <summary>
        /// Pys‰ytt‰‰ soitettavan musiikin.
        /// </summary>
        public static void StopMusic()
        {
            isMusicStop = true;

            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
            }
        }

        // Palauttaa tiedon onko musiikki viel‰ k‰ytˆss‰
        public static Boolean GetIsMusicStopped()
        {
            return isMusicStop;
        }
        

        /// <summary>
        /// Tyhjent‰‰ soundBankin, kun lopetetaan.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    foreach (var item in soundBank)
                    {
                        item.Value.Dispose();
                    }
                    soundBank.Clear();
                    soundBank = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
