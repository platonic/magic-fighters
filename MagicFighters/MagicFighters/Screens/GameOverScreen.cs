// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// -

#region File Description
//-----------------------------------------------------------------------------
//PauseCreen.cs
//Author        : William McHugh
//Comments      : Optimized by William McHugh
//Date          : 9/26/2012
//Last Modified : 9/26/2012    By: William McHugh
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using MagicFighters.Model;
using Microsoft.Xna.Framework.Media;
#endregion

namespace MagicFighters
{
    class GameOverScreen : MenuScreen
    {
        #region Regions
        GameScreen backgroundScreen;
        String CurLevel; 
        #endregion

        #region Initialization
        public GameOverScreen(GameScreen backgroundScreen, List<MF_Object> players, String level)
            : base("GAME OVER")
        {
            IsPopup = true;

            this.backgroundScreen = backgroundScreen;
            CurLevel = level; 
            //create menu entried
            MenuEntry mainMenuReturn = new MenuEntry("Return to Main Menu");
            MenuEntry retry = new MenuEntry("Retry"); 

            //hook up menu event handlers
            mainMenuReturn.Selected += MainMenuReturnSelected;
            retry.Selected += RetrySelected; 

            //Add entries to menu
            MenuEntries.Add(mainMenuReturn);
            MenuEntries.Add(retry); 
        }
        #endregion

        #region Overrides
        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                Vector2 position = entry.Position;
                position.Y += 60;
                entry.Position = position; 
            }
        }
        #endregion
        
        #region Event Handlers for menu items
        
        void MainMenuReturnSelected(object sender, EventArgs e)
        {
            if (!AudioManager.isPlaying("level1bg"))
            {
                AudioManager.StopSounds();
                AudioManager.PlayMusic("mainmenu");
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen();
            }
            else if (!AudioManager.isPlaying("level2bg"))
            {
                AudioManager.StopSounds();
                AudioManager.PlayMusic("mainmenu");
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen();
            }
            else
            {
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen(); 
            }
        }

        /// <summary>
        /// Handles "Retry" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        void RetrySelected(object sender, EventArgs e)
        {
            if (CurLevel == "level1")
            {
                if (!AudioManager.isPlaying("level1BG") )
                {
                    AudioManager.StopSounds();
                    AudioManager.PlayMusic("level1BG");
                    ScreenManager.AddScreen(new LevelOneScreen(), null); 
                }
            }
            else if (CurLevel == "level2")
            {
                if (!AudioManager.isPlaying("level2bg"))
                {
                    AudioManager.StopSounds();
                    ScreenManager.AddScreen(new LevelTwoScreen(), null);
                    AudioManager.PlayMusic("level2bg");
                }
            }
        }
        #endregion
    }
}
