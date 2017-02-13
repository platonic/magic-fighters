
#region File Description
//-----------------------------------------------------------------------------
//MainMenuScreen.cs
//Author        : William McHugh
//Comments      : Optimized by William McHugh
//Date          : 09/23/12
//Last Modified : 09/23/12    By: William McHugh
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using MagicFighters.Screens.Model;
#endregion

namespace MagicFighters.Screens
{
    class OptionScreen : MenuScreen
    {
        #region Fields
        //y location of the on/off button
        const float MUSIC_ON = 300;
        const float MUSIC_OFF = 350; 
        const float EXIT_SCR = 200;
        
        
        Color COLORINFO = new Color(255f,255f,0f);//default color
        //create menu entries
        MenuEntry Sound = new MenuEntry("Turn Music On/Off");
        MenuEntry On = new MenuEntry("Sound ON");
        MenuEntry Off = new MenuEntry("Sound OFF");
        MenuEntry ExitOptionScreen = new MenuEntry("Exit");
        GameScreen backgroundScreen;
        #endregion

        #region Overrides

        #endregion
        #region Initialization

        public OptionScreen(GameScreen backgroundScreen)
            : base("Options")
        {
            IsPopup = true;

            this.backgroundScreen = backgroundScreen;

            
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            //hook up menu event handlers
            //sound selected here
            ExitOptionScreen.Selected += OnCancel;
            On.Selected += SoundOnSelected;
            Off.Selected += SoundOffSelected;

            //add entries to the menu

            MenuEntries.Add(ExitOptionScreen);
            MenuEntries.Add(On);
            MenuEntries.Add(Off);
        }
        #endregion

        #region Event Handlers for Option Items
        ///<summary>
        ///Handlles the sound on off item selection
        ///</summary>
        ///

        protected override void OnCancel(PlayerIndex playerIndex)
        {          
            backgroundScreen.ExitScreen();
            ExitScreen();
        }

        void SoundOnSelected(object sender, EventArgs e)
        {
            ///<summary>
            ///If the player Chooses ON and Music is NOT PLAYING
            ///enable all music
            ///else don't do anything, the user is special.
            ///</summary>
            ///            
            if (!AudioManager.isPlaying("mainmenu"))
                AudioManager.PlayMusic("mainmenu");
        }

        void SoundOffSelected(object sender, EventArgs e)
        {
            ///If the player chooses OFF and Music is PLAYING
            ///disable all music
            ///

            AudioManager.StopSounds();
        }
        #endregion

       
    }
}
