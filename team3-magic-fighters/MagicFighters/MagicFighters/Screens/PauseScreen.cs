// ----------------------------------------------------------------------------------
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
// PauseScreen.cs
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
#endregion

namespace MagicFighters
{
    class PauseScreen : MenuScreen
    {
        #region Fields
        GameScreen backgroundScreen;
        #endregion

        #region Initialization
        public PauseScreen(GameScreen backgroundScreen,List<Object /*TODO: change to player type*/> players)
            : base(String.Empty)
        {
            IsPopup = true;

            this.backgroundScreen = backgroundScreen;

            //TODO: Create the menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Return");
            MenuEntry exitMenuEntry = new MenuEntry("Quit Game");

            //TODO: Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //TODO: Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

          

            //TODO: Preserve the old state of the game
           

            //TODO: Pause the game logic progress
           

            AudioManager.PauseResumeSounds(false);
        }
        #endregion

        #region Overrides
        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

             //TODO: Set menu location
           
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
             //TODO: Reset player active
           

           //TODO: play sound for menu items

            backgroundScreen.ExitScreen();
            ExitScreen();
        }

        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        /// 
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            AudioManager.StopSounds();
            ScreenManager.AddScreen(new MainMenuScreen(), null);
            ExitScreen();


        }
        #endregion
    }
}
