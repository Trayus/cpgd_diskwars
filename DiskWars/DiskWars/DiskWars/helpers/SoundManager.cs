using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Helpers
{
    /// <summary>
    /// Manages sound effects and songs
    /// </summary>
    class SoundManager
    {
        /// <summary>
        /// Stores all loaded sound effects
        /// </summary>
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        /// <summary>
        /// stores all loaded songs
        /// </summary>
        Dictionary<string, Song> music = new Dictionary<string, Song>();
        /// <summary>
        /// A game from which Content can be references, thereby allowing this class to load resources
        /// </summary>
        Game game;
        /// <summary>
        /// A static reference to a single instance of this class
        /// </summary>
        private static SoundManager instance;
        /// <summary>
        /// The volume for music and sound respectively
        /// </summary>
        public float volumeM = 1f, volumeS = 1f;
        /// <summary>
        /// A reference to the name of the currently playing song
        /// </summary>
        private string currentlyplaying = null;

        /// <summary>
        /// Initializes the soundmanager
        /// </summary>
        /// <param name="game">The game for which this sound manager will serve</param>
        public SoundManager(Game game)
        {
            this.game = game;
            SoundManager.instance = this;
        }

        /// <summary>
        /// Plays a SoundEffect with the given file name
        /// </summary>
        /// <param name="filename">The name of the sound effect file to play</param>
        public static void PlaySound(string filename)
        {
            instance.playsound(filename);
        }
        /// <summary>
        /// Plays the song, given by its file name, without repeating
        /// </summary>
        /// <param name="filename">The song to play</param>
        public static void PlayMusicOnce(string filename)
        {
            instance.playmusic(filename, false);
        }
        /// <summary>
        /// Plays the song, given by its file name, infinitely (until stopped)
        /// </summary>
        /// <param name="filename">The song to play</param>
        public static void PlayMusicLooped(string filename)
        {
            instance.playmusic(filename, true);
        }
        /// <summary>
        /// Pauses the currently playing song
        /// </summary>
        public static void PauseMusic()
        {
            MediaPlayer.Pause();
        }
        /// <summary>
        /// Resumes the currently playing song
        /// </summary>
        public static void ResumeMusic()
        {
            MediaPlayer.Resume();
        }
        /// <summary>
        /// Stops the currently playing song (it cannot be resumed (use pause instead))
        /// </summary>
        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
        /// <summary>
        /// Sets the volume of the Song
        /// </summary>
        /// <param name="volume">the volume to set music to (0 = nothing, 1 = max)</param>
        public static void setMusicVolume(float volume)
        {
            instance.volumeM = volume;
            MediaPlayer.Volume = volume;
        }
        /// <summary>
        /// returns the current volume of the song 
        /// </summary>
        /// <returns>the current music volume</returns>
        public static float getMusicVolume()
        {
            return instance.volumeM;
        }
        /// <summary>
        /// Sets the volume of all sound effects
        /// </summary>
        /// <param name="volume">the volume to set sound to (0 = nothing, 1 = max)</param>
        public static void setSoundVolume(float volume)
        {
            instance.volumeS = volume;
            SoundEffect.MasterVolume = volume;
        }
        /// <summary>
        /// Gets the current volume of the sound effects
        /// </summary>
        /// <returns>the volume of sound effects</returns>
        public static float getSoundVolume()
        {
            return instance.volumeS;
        }
        /// <summary>
        /// Returns the name of the currently playing song
        /// </summary>
        /// <returns>the name of the song playing</returns>
        public static string getCurrentlyPlaying()
        {
            return instance.currentlyplaying;
        }

        /// <summary>
        /// Plays the song referenced by the file name. The song is loaded if it has never been loaded so far
        /// </summary>
        /// <param name="file">the filename of the song to play</param>
        /// <param name="repeat">whether or not to play the song infinitely</param>
        private void playmusic(string file, bool repeat)
        {
            if (!music.ContainsKey(file))
            {
                music[file] = game.Content.Load<Song>(file);
            }
            MediaPlayer.Play(music[file]);
            MediaPlayer.IsRepeating = repeat;
            currentlyplaying = file;
        }

        /// <summary>
        /// Plays the sound effect referenced by the file name. The sound is loaded if it has never been loaded so far
        /// </summary>
        /// <param name="file">the filename of the sound to play</param>
        private void playsound(string file)
        {
            if (!sounds.ContainsKey(file))
            {
                sounds[file] = game.Content.Load<SoundEffect>(file);
            }
            sounds[file].Play();
        }

    }
}
