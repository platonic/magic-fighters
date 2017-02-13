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
using MagicFighters.TileEngine;
#endregion

namespace MagicFighters
{
    class GameplayScreen : GameScreen
    {
         #region Fields
        
         //TODO: Gameplay members
        static int SCREEN_WIDTH = 800;
        static int SCREEN_HEIGHT = 60;
        GraphicsDeviceManager graphics;
        MF_TileEngine tileEngine = new MF_TileEngine();
        TileMap level1 = null;
        Player player = null;
        Camera camera = null;


         bool gameOver;
         Random random;

         //TODO: Helper members
         
         #endregion

        #region Initialization
        public GameplayScreen()
        {
             random = new Random();

             //graphics.PreferredBackBufferWidth = SCREEN_WIDTH; // tilemap.Width * 101;
             //graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;// tilemap.Height * 171;
             //// graphics.IsFullScreen = true;
             //graphics.ApplyChanges();

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
             //Load<Texture2D>("blanca");
            //TODO: Load font
            
             //Load Level 1
             level1 = tileEngine.Load("Content/Level1Tileset/tilemap.mfm");
             player = new Player(level1, "ken Idle", new Vector2(0, 0), SpriteEffects.None);
             if (level1 != null)
             {


                 level1.LoadRectangleTexture(GetScreenManager().ContentManager);

                 if (level1.PlayerStartPosition != null)
                     player.InitAnimation(level1.PlayerStartPosition.X,
                         level1.PlayerStartPosition.Y, GetScreenManager().ContentManager, "ken walking Left_Right");

                 //graphics.PreferredBackBufferWidth = (int)(level1.TilesWide * level1.TileWidth);
                 SCREEN_HEIGHT = (int)(level1.TilesHigh * level1.TileHeight);

                 //graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
                 camera = new Camera(new Rectangle(0, 0, 800, SCREEN_HEIGHT));
                 //graphics.ApplyChanges();
               
                 //load all the textures
                 level1.LoadAllTextures("Level1Tileset", GetScreenManager().ContentManager);

             }

            //Load health bar
            // spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);
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
            // Allows the game to exit
           // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
           //     this.Exit();
            // player.StartWalkingFromLastIdlePosition();

            //TODO: Add your update logic here
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Left))
            {
                //if (!level1.LevelIntersect(player))
                player.WalkLeft(2);
            }
            if (k.IsKeyDown(Keys.Right))
            {
                //if(!level1.LevelIntersect(player))
                player.WalkRight(2);
            }
            if (k.IsKeyDown(Keys.Space))
            {
                //if(!level1.LevelIntersect(player))
                player.Jump(4);
            }
            if (k.IsKeyDown(Keys.Up))
            {
                player.MoveUP(2);
                // level1.MoveCamera(new Vector2(0, 5));
            }
            if (k.IsKeyDown(Keys.Down))
            {
                player.MoveDown(2);
                // level1.MoveCamera(new Vector2(0, -5));
            }

            //if (k.IsKeyDown(Keys.Escape))
            //{
            //    Exit();
            //}
            //test intersect


            level1.LevelIntersect(player, null);

            level1.ScrollCamera(player, 800, level1.TilesHigh * level1.TileHeight);

            player.Update(gameTime);

           // base.Update(gameTime);


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
           // ScreenManager.SpriteBatch.Begin();

            //TODO: Render all parts of the screen
           // GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            // spriteBatch.Begin();
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.viewMatrix);
            // spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState, Camera2D.Transform);
            // tilemap.Draw(spriteBatch);
            level1.Draw(player, ScreenManager.SpriteBatch, SCREEN_WIDTH, SCREEN_HEIGHT);
            //spriteBatch.Draw(player.texture, player.position, Color.White);
            player.Draw(gameTime, ScreenManager.SpriteBatch);
            level1.LevelIntersect(player, ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);


           // ScreenManager.SpriteBatch.End();
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
            
            ScreenManager.AddScreen(new PauseScreen(pauseMenuBackground,null), null);
        }
        #endregion

        #region Return to Main Menu
        private void returnToMainMenu()
        {
            
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
