// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//InstructionsScreen.cs
//Author        : William McHugh
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/20/2012    By: Lisandro Martinez
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

namespace MagicFighters
{
    class InstructionsScreen : MenuScreen
    {
        #region Fields

        //instructions settings and information
        const float STARTINGYPOSITION = 150;//starting y position
        const float INSTRUCTION_OFFSET = 10;//y offset
        const float XPERCENTAGE = 0.5f;//percentage of the screen alignment 0 = Left and 1 Right of the screen
        const float EXIT_Y_LOCATION = 550;//y position of the Exit button
        Color COLORINFO = new Color(255f, 150f, 0f);//default color for the instructions
        MF_Instruction[] Informations;

        // Create our menu entries.
        MenuEntry cancelMenuEntry = new MenuEntry("Cancel");
        SpriteFont font;
        GameScreen backgroundScreen;
        
        #endregion

        #region Initialization
        public InstructionsScreen(GameScreen backgroundScreen)
            : base("Instructions")
        {

            IsPopup = true;

            this.backgroundScreen = backgroundScreen;

            MenuEntry ExitInstructionScreen = new MenuEntry("Exit");
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            // Hook up menu event handlers.
            cancelMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(cancelMenuEntry);

        }
        #endregion

        #region Event Handlers for Menu Items
        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        protected override void OnCancel(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {

            if (AudioManager.isPlaying("instructions"))
            {
                AudioManager.StopSounds();
                AudioManager.PlaySound("mainmenu", true);
                backgroundScreen.ExitScreen();
                ExitScreen();
            }
            else
            {
                AudioManager.StopSounds();
                backgroundScreen.ExitScreen();
                ExitScreen();
            }

            
        }
        #endregion

        #region Loading
        public override void LoadContent()
        {

            //load the font
            font = Load<SpriteFont>("MainFont");

            //Initialize the instructions
            Informations = new MF_Instruction[]
             {
                 new MF_Instruction(150,200,"Textures/Instructions/keyboard_arrow_keys",Color.White,ScreenManager.SpriteBatch,
                     new string [] 
                     {
                         "Arrow keys: move left, right and crouch",
                         "Left: Walk Left",
                         "Right: Walk Right",
                         "Down: Crouch",
                     },ScreenManager.Game,30),
                new MF_Instruction(150,350,"Textures/Instructions/keyboard_spacebar",Color.White,ScreenManager.SpriteBatch,
                     new string [] 
                     {
                         "Press space to jump",
                     },ScreenManager.Game,30),
                      new MF_Instruction(150,400,"Textures/Instructions/keyboard_asdf",Color.White,ScreenManager.SpriteBatch,
                     new string [] 
                     {
                         "A: High punch",
                         "S: High kick",
                         "D: Grab",
                         "F: Perform special move.(When available)",
                     },ScreenManager.Game,30,true),
                
             };

            foreach (var info in Informations)
                info.Initialize();

            base.LoadContent();
        }
        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        #region Handle input
        public override void HandleInput(InputState input)
        {
            PlayerIndex p;
            if (input.IsMenuCancel(PlayerIndex.One, out p))
            {
                ExitScreen();
            }

            base.HandleInput(input);
        }
        #endregion

        #region Render
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            //draw all the instructions
            foreach (var info in Informations)
                info.Draw(gameTime);
            //draw the Exit button
            spriteBatch.Begin();
            {
                Vector2 size = Vector2.Zero;
                size = font.MeasureString(cancelMenuEntry.Text);
                cancelMenuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - size.X) * XPERCENTAGE, EXIT_Y_LOCATION);
                cancelMenuEntry.Draw(this, true, gameTime);
            }
            spriteBatch.End();
        }
        #endregion

        
    }
}
