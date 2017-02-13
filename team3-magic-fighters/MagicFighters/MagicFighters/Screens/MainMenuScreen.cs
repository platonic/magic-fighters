// ----------------------------------------------------------------------------------
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
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
#endregion

namespace MagicFighters
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        public MainMenuScreen()
            : base(String.Empty)
        {
            IsPopup = true;

            //TODO: Create the menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Play");
            MenuEntry characterSelect = new MenuEntry("Instructions Screen");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            //TODO: Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            //characterSelect.Selected += CharacterSelectEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //TODO: Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(characterSelect);
            MenuEntries.Add(exitMenuEntry);
        }
        #endregion

        #region Overrides
        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

             //TODO:change meny positions
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
            ScreenManager.AddScreen(new InstructionsScreen(), null);
        }

        void CharacterSelectEntrySelected(object sender, EventArgs e)
        {
             //ScreenManager.AddScreen(new CharacterSelectScreen(), null);
        }

        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        /// 
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
            ScreenManager.Game.Exit();
        }

        /// <summary>
        /// Handles "Select Background Music" menu item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectBackgroundMusicMenuEntrySelected(object sender, EventArgs e)
        {
            var backgroundScreen = new BackgroundScreen();

            ScreenManager.AddScreen(backgroundScreen, null);
            ScreenManager.AddScreen(new MusicSelectionScreen(backgroundScreen), null);
        }

		/// <summary>
		/// Handler "Share" menu item selection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ShareMenuEntrySelected(object sender, EventArgs e)
		{
			//TODO: Execute a task defined in the game class
			//( (MagicFighters.MagicFightersGame)( ScreenManager.Game ) ).ExecuteTask();
		}
        #endregion
    }
}
