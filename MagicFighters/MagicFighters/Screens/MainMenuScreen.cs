// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MainMenuScreen.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 09/23/12    By: William McHugh
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Screens;
using System.Windows.Forms;
#endregion

namespace MagicFighters
{
    class MainMenuScreen : MenuScreen
    {
        #region Fields
#if DEBUG
        static bool OnetimeShowTODO = true;
#endif
        #endregion
        #region Initialization
        public MainMenuScreen()
            : base(String.Empty)
        {
            IsPopup = true;

            MenuEntry startGameMenuEntry = new MenuEntry("Play");
            MenuEntry InstructionsSelect = new MenuEntry("Instructions");
            MenuEntry OptionsSelect = new MenuEntry("Options");
            MenuEntry CreditsSelect = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");


            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            InstructionsSelect.Selected += InstructionsSelectEntrySelected;
            OptionsSelect.Selected += OptionsSelectEntrySelected;
            CreditsSelect.Selected += CreditsSelectEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(InstructionsSelect);
            MenuEntries.Add(OptionsSelect);
            MenuEntries.Add(CreditsSelect);
            MenuEntries.Add(exitMenuEntry);

           
        }
        #endregion

        #region Overrides

        #region Loading
        public override void LoadContent()
        {
            ((MagicFightersGame)ScreenManager.Game).SetResolution(MagicFightersGame.DEFAULT_SCREEN_WIDTH, MagicFightersGame.DEFAULT_SCREEN_HEIGHT);

            base.LoadContent();
           
        }
        #endregion
      
        protected override void UpdateMenuEntryLocations()
        {
#if DEBUG
            if (OnetimeShowTODO)
            {
                OnetimeShowTODO = false;
                MF_TodoList.Load();
                MF_TodoList.Show();
            }
#endif
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                var position = entry.Position;

                position.Y += 60;

                entry.Position = position;
            }
        }
        #endregion

       
        #region Event Handlers for Menu Items
        /// <summary>
        /// Handles "Play" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            
            try
            {
                //This is throwing an exception!
                if (AudioManager.isPlaying("mainmenu"))
                {
                    AudioManager.StopSounds();
                    AudioManager.PlayMusic("level1BG");
                }
                else
                    AudioManager.StopSounds();

                ScreenManager.AddScreen(new LevelOneScreen(), null);
                //ScreenManager.AddScreen(new KenEndingScreen(), null);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
                
            }
             
        }

        /// <summary>
        /// Handles "Instructions" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InstructionsSelectEntrySelected(object sender, EventArgs e)
        {
            if (AudioManager.isPlaying("mainmenu"))
            {
                AudioManager.StopSounds();
                AudioManager.PlayMusic("instructions");
                var backgroundScreen = new BackgroundScreen();
                ScreenManager.AddScreen(backgroundScreen, null);
                ScreenManager.AddScreen(new InstructionsScreen(backgroundScreen), null);
            }
            else
            {
                AudioManager.StopSounds();
                var backgroundScreen = new BackgroundScreen();
                ScreenManager.AddScreen(backgroundScreen, null);
                ScreenManager.AddScreen(new InstructionsScreen(backgroundScreen), null);
            }
        }

        /// <summary>
        /// Handles "Credits" menu item selection
        /// </summary>
        void CreditsSelectEntrySelected(object sender, EventArgs e)
        {
            if (AudioManager.isPlaying("mainmenu"))
            {
                AudioManager.StopSounds();
                AudioManager.PlayMusic("credits");
                var backgroundScreen = new BackgroundScreen();
                ScreenManager.AddScreen(backgroundScreen, null);
                ScreenManager.AddScreen(new CreditsScreen(backgroundScreen), null);
            }
            else
            {
                AudioManager.StopSounds();
                var backgroundScreen = new BackgroundScreen();
                ScreenManager.AddScreen(backgroundScreen, null);
                ScreenManager.AddScreen(new CreditsScreen(backgroundScreen), null);
            }
        }

        ///<summary>
        ///Handles "options" menu item selection
        ///</summary>
        void OptionsSelectEntrySelected(object sender, EventArgs e)
        {
            var backgroundScreen = new BackgroundScreen();
            ScreenManager.AddScreen(backgroundScreen, null);
            ScreenManager.AddScreen(new OptionScreen(backgroundScreen), null);
        }

        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        /// 
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            AudioManager.StopSounds();

            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
            ScreenManager.Game.Exit();
        }
        #endregion
    }
}
