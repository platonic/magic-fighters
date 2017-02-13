// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MagicFightersGame.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;

namespace MagicFighters
{
     /// <summary>
     /// This is the main type for your game
     /// </summary>
     public class MagicFightersGame : Microsoft.Xna.Framework.Game
     {
          public static int MAX_SONGS = 5;


          GraphicsDeviceManager graphics;
          ScreenManager screenManager;
          public static int DEFAULT_SCREEN_WIDTH = 800;
          public static int DEFAULT_SCREEN_HEIGHT = 600;
          public int ScreenWidth = DEFAULT_SCREEN_WIDTH;
          public int ScreenHeight = DEFAULT_SCREEN_HEIGHT;
         /// <summary>
         /// Change the screen resolution
         /// </summary>
         /// <param name="w">Width</param>
         /// <param name="h">Hight</param>
         /// <param name="fullScreen">true for full screen false otherwise</param>
         public void SetResolution(int w, int h, bool fullScreen = false)
         {
             if (graphics == null)
                 return;

             graphics.PreferredBackBufferWidth = w;
             graphics.PreferredBackBufferHeight = h;
             this.graphics.IsFullScreen = fullScreen;
             graphics.ApplyChanges();
         }
          public MagicFightersGame()
          {
               graphics = new GraphicsDeviceManager(this);
              
              //Initialize the Audio Manager
               AudioManager.Initialize(this);

              //set default screen resolution
               SetResolution(ScreenWidth, ScreenHeight);

              //set default content folder
               Content.RootDirectory = "Content";

               //Create a new instance of the Screen Manager
               screenManager = new ScreenManager(this);
               Components.Add(screenManager);

              //add main meny screen
               screenManager.AddScreen(new BackgroundScreen(), null);
               screenManager.AddScreen(new MainMenuScreen(), null);

             

  
          }

          /// <summary>
          /// LoadContent will be called once per game and is the place to load
          /// all of your content.
          /// </summary>
          protected override void LoadContent()
          {
              AudioManager.LoadSounds();
              AudioManager.PlayMusic("mainmenu");

              base.LoadContent();
          }

     }
}
