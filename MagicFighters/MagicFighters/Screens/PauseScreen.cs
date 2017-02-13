//---------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//PauseCreen.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
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
    class PauseScreen : MenuScreen
    {
        #region Fields
        GameScreen backgroundScreen;
        #endregion

        #region Initialization
        public PauseScreen(GameScreen backgroundScreen,List<MF_Object> players)
            : base(String.Empty)
        {
            
            IsPopup = true;

            this.backgroundScreen = backgroundScreen;

            //Create the menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Return");
            MenuEntry exitMenuEntry = new MenuEntry("Quit Game");

            //Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
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

        #region Event Handlers for Menu Items
        /// <summary>
        /// Handles "Return" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            if (!AudioManager.isPlaying("level1BG"))
            {
                AudioManager.PauseResumeSounds(true);
                AudioManager.PlaySound("level1BG");
                backgroundScreen.ExitScreen();
                ExitScreen();
            }
            else
            {
                backgroundScreen.ExitScreen();
                ExitScreen();
            }
        }

        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        /// 
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (!AudioManager.isPlaying("level1BG"))
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
        #endregion
    }
}
