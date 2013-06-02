#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System.Collections.Generic;
#endregion

namespace Hotbox
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry resolutionMenuEntry;
        MenuEntry toggleFullscreenMenuEntry;
        MenuEntry bgmMenuEntry;
        MenuEntry sfxMenuEntry;
        MenuEntry applyMenuEntry;

        static bool isFullscreen = false;

        struct resolutionList
        {
            public string name;
            public int width;
            public int height;
        }

        static List<resolutionList> ListOfResolution = new List<resolutionList>();
        static int currentResolution = 0;

        static int currentBGM = 100;
        static int currentSFX = 100;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            resolutionMenuEntry = new MenuEntry(string.Empty);
            toggleFullscreenMenuEntry = new MenuEntry(string.Empty);
            bgmMenuEntry = new MenuEntry(string.Empty);
            sfxMenuEntry = new MenuEntry(string.Empty);
            applyMenuEntry = new MenuEntry("Apply");

            loadResolutions();

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
            toggleFullscreenMenuEntry.Selected += ToggleFullscreenMenuEntrySelected;
            bgmMenuEntry.Selected += BGMMenuEntrySelected;
            sfxMenuEntry.Selected += SFXMenuEntrySelected;
            applyMenuEntry.Selected += ApplyMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(toggleFullscreenMenuEntry);
            MenuEntries.Add(bgmMenuEntry);
            MenuEntries.Add(sfxMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(back);
        }

        public void loadResolutions()
        {
            resolutionList thisRes = new resolutionList();

            thisRes.name = "1366x768";
            thisRes.width = 1366;
            thisRes.height = 768;

            ListOfResolution.Add(thisRes);

            //new res
            thisRes = new resolutionList();

            thisRes.name = "960x540";
            thisRes.width = 960;
            thisRes.height = 540;

            ListOfResolution.Add(thisRes);

            //new res
            thisRes = new resolutionList();
            
            thisRes.name = "1920x1080";
            thisRes.width = 1920;
            thisRes.height = 1080;

            ListOfResolution.Add(thisRes);

        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            resolutionMenuEntry.Text = "Resolution: " + ListOfResolution[currentResolution].name;
            toggleFullscreenMenuEntry.Text = "Fullscreen: " + isFullscreen;
            bgmMenuEntry.Text = "Music Volume: " + currentBGM;
            sfxMenuEntry.Text = "SFX Volume: " + currentSFX;
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the resolution menu entry is selected.
        /// </summary>
        void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentResolution++;

            if (currentResolution >= ListOfResolution.Count)
                currentResolution = 0;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the resolution menu entry is selected.
        /// </summary>
        void ToggleFullscreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            isFullscreen = !isFullscreen;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void BGMMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentBGM++;

            if (currentBGM > 100)
                currentBGM = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SFXMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentSFX++;

            if (currentSFX > 100)
                currentSFX = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void ApplyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            AudioManager.SetMusicVolume(currentBGM);
            AudioManager.SetSFXVolume(currentSFX);

            ScreenManager.GraphicsManager.PreferredBackBufferWidth = ListOfResolution[currentResolution].width;
            ScreenManager.GraphicsManager.PreferredBackBufferHeight = ListOfResolution[currentResolution].height;
            ScreenManager.GraphicsManager.IsFullScreen = isFullscreen;
            ScreenManager.GraphicsManager.ApplyChanges();

            SetMenuEntryText();
        }


        #endregion
    }
}
