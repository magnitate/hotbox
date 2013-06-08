using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace Hotbox
{
    //class for background music and sound effects
    //using singleton pattern
    public class AudioManager : GameComponent
    {
        #region Singleton

        private static AudioManager audioManager = null;

        #endregion

        #region Audio Data

        private AudioEngine audioEngine;    //The audio engine used to play all cues
        private SoundBank soundBank;        //The soundbank that contains all cues

        private WaveBank sfxWaveBank;       //sound effect (sfx) bank
        private WaveBank bgmWaveBank;       //background music (bgm) bank

        private AudioCategory bgmCategory;  //bgm category
        private AudioCategory sfxCategory;  //sfx category

        private float bgmVolume = 0.0f;
        /// <summary>
        /// Sets the volume between 0 and 100.
        /// </summary>
        /// <param name="volume"></param>
        public static void SetMusicVolume(float volume)
        {
            if (volume < 0)
                volume = 0;
            if (volume > 100)
                volume = 100;

            audioManager.SetBgmVolume((volume / 100));
        }

        private float sfxVolume = 0.35f;
        /// <summary>
        /// Sets the volume between 0 and 100.
        /// </summary>
        /// <param name="volume"></param>
        public static void SetSFXVolume(int volume)
        {
            if (volume < 0)
                volume = 0;
            if (volume > 100)
                volume = 100;
            
            audioManager.SetSfxVolume((float)(volume / 100));
        }


        private Cue sfxMusicCue;            //current cue for sfx
        private Cue bgmMusicCue;            //current cue for bgm

        #endregion

        #region Initialization

        private AudioManager(Game game,
            string settingsFile,
            string seWaveBankFile,
            string bgmWaveBankFile,
            string soundBankFile)
            : base(game)
        {
            try
            {
                audioEngine = new AudioEngine(settingsFile);
                sfxWaveBank = new WaveBank(audioEngine, seWaveBankFile);
                bgmWaveBank = new WaveBank(audioEngine, bgmWaveBankFile);
                soundBank = new SoundBank(audioEngine, soundBankFile);

                /*// Ensure the wave bank is prepared 
                while (!bgmWaveBank.IsPrepared)
                {
                    audioEngine.Update();
                }

                while (!sfxWaveBank.IsPrepared)
                {
                    audioEngine.Update();
                }*/
            }
            catch (NoAudioHardwareException)
            {
                // silently fall back to silence
                audioEngine = null;
                sfxWaveBank = null;
                bgmWaveBank = null;
                soundBank = null;
            }
        }

        public static void Initialize(Game game)
        {
            string settingsFile = @"Content\Audio\HotboxTest.xgs";
            string seWaveBankFile = @"Content\Audio\SeWaveBank.xwb";
            string bgmWaveFile = @"Content\Audio\BgmWaveBank.xwb";
            string soundBankFile = @"Content\Audio\Sound Bank.xsb";

            audioManager = new AudioManager(game,
                settingsFile,
                seWaveBankFile,
                bgmWaveFile,
                soundBankFile);

            if (game != null)
                game.Components.Add(audioManager);

            audioManager.sfxCategory = audioManager.audioEngine.GetCategory("Default");
            audioManager.bgmCategory = audioManager.audioEngine.GetCategory("Music");

            audioManager.sfxCategory.SetVolume(audioManager.sfxVolume);
            audioManager.bgmCategory.SetVolume(audioManager.bgmVolume);

        }

        #endregion

        #region Cue Methods

        public static Cue GetSfxCue(string cueName)
        {
            if ((audioManager == null) || (audioManager.audioEngine == null) ||
                (audioManager.soundBank == null) || (audioManager.sfxWaveBank == null))
                return null;

            return audioManager.soundBank.GetCue(cueName);
        }

        public static void PlaySfxCue(string cueName)
        {
            if ((audioManager != null) && (audioManager.audioEngine != null) &&
                (audioManager.soundBank != null) && (audioManager.sfxWaveBank != null))
                audioManager.soundBank.PlayCue(cueName);

        }

        public static void PlaySfxMusic(string musicCueName)
        {
            if ((audioManager == null) || (audioManager.audioEngine == null) ||
                (audioManager.soundBank == null) || (audioManager.sfxWaveBank == null))
                return;

            if (audioManager.sfxMusicCue != null)
                audioManager.sfxMusicCue.Stop(AudioStopOptions.AsAuthored);

            audioManager.sfxMusicCue = GetSfxCue(musicCueName);

            if (audioManager.sfxMusicCue != null)
                audioManager.sfxMusicCue.Play();

        }

        public static Cue GetBgmCue(string cueName)
        {
            if ((audioManager == null) || (audioManager.audioEngine == null) ||
                (audioManager.soundBank == null) || (audioManager.bgmWaveBank == null))
                return null;

            return audioManager.soundBank.GetCue(cueName);
        }

        public static void PlayBgmCue(string cueName)
        {
            if ((audioManager != null) && (audioManager.audioEngine != null) &&
                (audioManager.soundBank != null) && (audioManager.bgmWaveBank != null))
                audioManager.soundBank.PlayCue(cueName);
        }

        public static void PlayBgmMusic(string musicCueName)
        {
            if ((audioManager == null) || (audioManager.audioEngine == null) ||
                (audioManager.soundBank == null) || (audioManager.bgmWaveBank == null))
                return;

            if (audioManager.bgmMusicCue != null)
                audioManager.bgmMusicCue.Stop(AudioStopOptions.AsAuthored);

            audioManager.bgmMusicCue = GetBgmCue(musicCueName);

            if (audioManager.bgmMusicCue != null)
                audioManager.bgmMusicCue.Play();

        }

        #endregion

        #region Updating Methods

        public override void Update(GameTime gameTime)
        {
            // update the audio engine
            if (audioEngine != null)
                audioEngine.Update();

            base.Update(gameTime);
        }

        public void SetBgmVolume(float value)
        {
            audioManager.bgmVolume = value;

            if (value < 0.1f)
                audioManager.bgmVolume = 0.1f;
            else if (value > 10.0f)
                audioManager.bgmVolume = 10.0f;
        }

        public void SetSfxVolume(float value)
        {
            audioManager.sfxVolume = value;

            if (value < 0.1f)
                audioManager.sfxVolume = 0.1f;
            else if (value > 10.0f)
                audioManager.sfxVolume = 10.0f;
        }

        #endregion

        #region Instance Disposal Methods

        /// <summary>
        /// Clean up the component when it is disposing
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (soundBank != null)
                    {
                        soundBank.Dispose();
                        soundBank = null;
                    }

                    if (sfxWaveBank != null)
                    {
                        sfxWaveBank.Dispose();
                        sfxWaveBank = null;
                    }

                    if (bgmWaveBank != null)
                    {
                        bgmWaveBank.Dispose();
                        bgmWaveBank = null;
                    }

                    if (audioEngine != null)
                    {
                        audioEngine.Dispose();
                        audioEngine = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}