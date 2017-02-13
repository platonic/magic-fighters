// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------
#region File Description
//-----------------------------------------------------------------------------
//LevelOneScreen.cs
//Author        : William McHugh
//Comments      : Optimized by Lisandro Martinez
//Date          : 09/29/2012
//Last Modified : 9/29/2012    By: Lisandro Martinez
//Last Modified : 9/29/2012    By: Lisandro Martinez/William McHugh
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
using MagicFighters.Screens;
using Microsoft.Xna.Framework.Media;
#endregion
namespace MagicFighters
{
    class LevelTwoScreen:GameScreen
    {
        #region Fields

        LevelTwoTileMap level2 = null;
        MF_Camera camera = null;
        bool gameOver, isGamePaused;
        Texture2D KenHealthBar;
        Vector2 KenHealthBarPosition;
        float KenHealthBarXOffset = -20;
        float KenHealthBarYOffset = -100;
        #endregion

        #region Properties
        public MF_Camera Camera { get { return camera; } }
        #endregion

        #region Initialization
        public LevelTwoScreen()
        {
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
            //get instances first
            Start();

            //load information
            level2.Initialize();
            KenHealthBar = ScreenManager.ContentManager.Load<Texture2D>("PlayerHealthBar");
            level2.InitializePlayersHud();


        }
        #endregion

        #region Update
        /// <summary>
        /// Runs one frame of update for the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            switch (level2.CurrentGameState)
            {
                case GameStates.MF_GameStates.GameOver:
                    {
                        gameOver = true;
                        ExitScreen();
                        if (AudioManager.isPlaying("level2bg"))
                        {
                            AudioManager.PauseResumeSounds(true);

                            AudioManager.StopSound("level2bg");
                            AudioManager.PlaySound("gameover", false);
                        }

                        var background = new BackgroundScreen();
                        ScreenManager.AddScreen(background, null);
                        ScreenManager.AddScreen(new GameOverScreen(background, null, "level2"), PlayerIndex.One);
                    }
                    break;
                case GameStates.MF_GameStates.LevelOver:
                    {
                        ExitScreen();
                        AudioManager.StopSound("level2bg");
                        AudioManager.PlayMusic("kenending");

                       // var background = new BackgroundScreen();
                        ScreenManager.AddScreen(new KenEndingScreen("Magic Fighters"), PlayerIndex.One);
                    }
                    break;
                case GameStates.MF_GameStates.Playing:
                    {
                        level2.Update(gameTime);
                        level2.UpdatePlayersHud(gameTime);
                    }
                    break;
            }


            //if (level2.CurrentGameState == GameStates.MF_GameStates.GameOver)
            //{
            //    gameOver = true;
            //    ExitScreen();
            //    if (AudioManager.isPlaying("level2BG"))
            //    {
            //        AudioManager.PauseResumeSounds(true);

            //        AudioManager.StopSound("level2BG");
            //        AudioManager.PlaySound("gameover", false);
            //    }

            //    var background = new BackgroundScreen();
            //    ScreenManager.AddScreen(background, null);
            //    ScreenManager.AddScreen(new GameOverScreen(background, null,"level2"), PlayerIndex.One);
            //}
            //else
            //{
            //    if (!isGamePaused)
            //    {
            //        level2.Update(gameTime);
            //        level2.UpdatePlayersHud(gameTime);
            //    }
            //}
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

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.viewMatrix);
            {
                DrawBackground(gameTime);
                level2.Draw(gameTime);
                DrawPlayers(gameTime);
                DrawHud(gameTime);
            }
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);

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
                if (input.IsPauseGame(PlayerIndex.One))
                {
                    FinishCurrentGame();
                }

                return;
            }

            if (input.IsPauseGame(PlayerIndex.One))
            {
                PauseCurrentGame();
            }
            else
            {
                isGamePaused = false;
                level2.HandleInput();
            }
        }
        #endregion

        #region Update Helpers

        #endregion

        #region Draw Helpers
        /// <summary>
        /// Draws the players
        /// </summary>
        void DrawPlayers(GameTime gameTime)
        {
            if (!gameOver)
                level2.DrawPlayers(gameTime);                      
        }
        /// <summary>
        /// Draw the current background. 
        /// Use ScreenManager.SpriteBatch.Draw
        /// </summary>
        private void DrawBackground(GameTime gameTime)
        {
            // Clear the background
            ScreenManager.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            level2.DrawBackgroundItems(gameTime);
        }

        /// <summary>
        /// Draw the HUD, which consists of the score elements and the GAME OVER tag.
        /// </summary>
        void DrawHud(GameTime gameTime)
        {
            if (gameOver){ }
            else
                level2.DrawPlayersHud(gameTime);
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
            if (AudioManager.isPlaying("level2BG"))
            {
                AudioManager.PauseResumeSounds(true);

                AudioManager.StopSound("level2BG");
                AudioManager.PlaySound("pause", false);
            }
            isGamePaused = true;
            var pauseMenuBackground = new BackgroundScreen();
            ScreenManager.AddScreen(pauseMenuBackground, null);
            ScreenManager.AddScreen(new PauseScreen(pauseMenuBackground, null), PlayerIndex.One);
        }
        #endregion

        #region Gameplay Helpers
        /// <summary>
        /// Starts a new game session, setting all game states to initial values.
        /// </summary>
        void Start()
        {
            // Set initial properties
            camera = new MF_Camera(new Rectangle(0, 0, 0, 0));
            level2 = new LevelTwoTileMap(ScreenManager.Game, camera, ScreenManager.SpriteBatch);
            camera.viewport = new Viewport(0, 0, ((MagicFightersGame)ScreenManager.Game).ScreenWidth, ((MagicFightersGame)ScreenManager.Game).ScreenHeight);
            KenHealthBarPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + KenHealthBarXOffset, ScreenManager.GraphicsDevice.Viewport.Height + KenHealthBarYOffset);
        }
        #endregion
    }
}
