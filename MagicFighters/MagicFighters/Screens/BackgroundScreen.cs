// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//BackgroundScreen.cs
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameStateManagement;
#endregion

namespace MagicFighters
{
    class BackgroundScreen : GameScreen
    {
        #region Fields
         Texture2D background;
         Texture2D title;
         public string Background = "Bison_Background";
        #endregion

        #region Initialization
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        public BackgroundScreen(string texture)
        {
            Background = texture;
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        #endregion

        #region Loading
        public override void LoadContent()
        {
            background = Load<Texture2D>(Background);
                  title = Load<Texture2D>("MF_Title");

        }
        #endregion

        #region Render        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            
                 spriteBatch.Draw(background, new Vector2(0, 0), null,
                      new Color(255, 255, 255, TransitionAlpha), 0, new Vector2(0, 0), 3.6f, SpriteEffects.None, 0);

                 spriteBatch.Draw(title, new Vector2(0, 10), null,
                    new Color(255, 255, 255, TransitionAlpha), 25, new Vector2(0, 0), 0.22f, SpriteEffects.None, 0);
           

            spriteBatch.End();
        }
        #endregion
    }
}
