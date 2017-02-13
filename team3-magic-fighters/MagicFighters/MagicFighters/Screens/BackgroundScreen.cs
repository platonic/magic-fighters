// ----------------------------------------------------------------------------------
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
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
        #endregion

        #region Initialization
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        #endregion

        #region Loading
        public override void LoadContent()
        {
             //TODO: Change background path

                  //background = Load<Texture2D>("blanka");
                  title = Load<Texture2D>("MF_Title");

        }
        #endregion

        #region Render        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //TODO: Draw
               //  spriteBatch.Draw(background, new Vector2(0, 20), null,
               //       new Color(255, 255, 255, TransitionAlpha), 0, new Vector2(0, 0), 3.0f, SpriteEffects.None, 0);

                 spriteBatch.Draw(title, new Vector2(0, 10), null,
                    new Color(255, 255, 255, TransitionAlpha), 25, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
           

            spriteBatch.End();
        }
        #endregion
    }
}
