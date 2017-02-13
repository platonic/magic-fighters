// ----------------------------------------------------------------------------------
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
#endregion

namespace MagicFighters
{
    class GameplayScreen : GameScreen
    {
         #region Fields
        
         //TODO: Gameplay members


         bool gameOver;
         Random random;

         //TODO: Helper members
         
         #endregion

        #region Initialization
        public GameplayScreen()
        {
             random = new Random();
        }
        #endregion

        #region Content Loading/Unloading
        /// <summary>
        /// Loads the game assets and initializes "players"
        /// </summary>
        public override void LoadContent()
        {
			base.LoadContent();

			LoadAssets();
        }
         /// <summary>
         /// use: For Loading Textures 
          ///          Load<Texture2D>("blanca");
        ///       For Loading Font
        ///              Load<SpriteFont>("path")
         /// </summary>
        public void LoadAssets()
        {
            //TODO: Load textures
             Load<Texture2D>("blanca");
            //TODO: Load font
           
            //TODO: Define initial position
           

            //TODO: Initialize players
            

            //TODO: Initialize NPCs definitions

             //If loading success
             if (true)//TODO: start condition here
			{
                    // Start the game
                    Start();
			}
			else
			{
				//TODO: something went wrong
			}

        }
        #endregion

        #region Update
        /// <summary>
        /// Runs one frame of update for the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           //TODO: gameplay screen logic here

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        #endregion

        #region Draw
        /// <summary>
        /// Draw the game world, effects, and HUD
        /// </summary>
        /// <param name="gameTime">The elapsed time since last Draw</param>
        public override void Draw(GameTime gameTime)
        {
			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ScreenManager.SpriteBatch.Begin();

            //TODO: Render all parts of the screen
           


            ScreenManager.SpriteBatch.End();
        }
        #endregion

        #region Input
        /// <summary>
        /// Input helper method provided by GameScreen.  Packages up the various input
        /// values for ease of use.
        /// </summary>
        /// <param name="input">The state of the gamepads</param>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (gameOver)
            {
                if (input.IsPauseGame(null))
                {
                    FinishCurrentGame();
                }

                return;
            }

            if (input.IsPauseGame(null))
            {
                PauseCurrentGame();
            }
            else if (true) //TODO: handle player input logic here
            {
               
            }
        }
        #endregion

         //Place update for individual parts. 
        #region Update Helpers
        
        #endregion

        #region Draw Helpers
        /// <summary>
        /// Draws the player
        /// </summary>
        void DrawPlayer(GameTime gameTime)
        {
             if (!gameOver)
             {
                  //TODO: Draw player
             }
        }

        /// <summary>
        /// Draws the AI
        /// </summary>
        void DrawComputer(GameTime gameTime)
        {
             if (!gameOver)
             {
                  //TODO: Draw AI 
             }
        }

        /// <summary>
        /// Draw the current background. 
        /// Use ScreenManager.SpriteBatch.Draw
        /// </summary>
        private void DrawBackground()
        {
            // Clear the background
            ScreenManager.Game.GraphicsDevice.Clear(Color.White);

             //TODO: Draw Background here

        }

        /// <summary>
        /// Draw the HUD, which consists of the score elements and the GAME OVER tag.
        /// </summary>
        void DrawHud()
        {
            if (gameOver)
            {
               
            }
            else
            {
                //TODO: Draw Huds
               
            }
        }

        /// <summary>
        /// A simple helper to draw shadowed text.
        /// </summary>
        void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            ScreenManager.SpriteBatch.DrawString(font, text,
                new Vector2(position.X + 1, position.Y + 1), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, text, position, color);
        }

        /// <summary>
        /// A simple helper to draw shadowed text.
        /// </summary>
        void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
        {
			ScreenManager.SpriteBatch.DrawString(font, text,
				new Vector2(position.X + 1, position.Y + 1),
				Color.Black, 0, new Vector2(0, font.LineSpacing / 2),
				fontScale, SpriteEffects.None, 0);
			ScreenManager.SpriteBatch.DrawString(font, text, position,
				color, 0, new Vector2(0, font.LineSpacing / 2),
				fontScale, SpriteEffects.None, 0);
        }
        #endregion

        #region Input Helpers
        /// <summary>
        /// Finish the current game
        /// </summary>
        private void FinishCurrentGame()
        {
            ExitScreen();
        }

        /// <summary>
        /// Pause the current game
        /// </summary>
        private void PauseCurrentGame()
        {
            var pauseMenuBackground = new BackgroundScreen();

             //TODO: Change State
         

            ScreenManager.AddScreen(pauseMenuBackground, null);

            ScreenManager.AddScreen(new PauseScreen(pauseMenuBackground,null /*TODO: Players List*/), null);
        }
        #endregion

        #region Gameplay Helpers
        /// <summary>
        /// Starts a new game session, setting all game states to initial values.
        /// </summary>
        void Start()
        {
            // Set initial properties
        }
        #endregion
    }
}
