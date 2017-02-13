// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Instruction.cs
//Author        : Lisandro Martinez
//Comments      : 
//Date          : 9/01/2012
//Last Modified : 9/20/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicFighters.Screens.Model
{
    class MF_Instruction : DrawableGameComponent
    {
        #region Fields
        Vector2 Position;
        Texture2D Texture;
        string[] InfoLines;
        SpriteBatch spriteBatch;
        MagicFightersGame curGame;
        string ImagePath;
        Color TextColor;
        SpriteFont font;
        float xOffset;
        bool WrapText;
        #endregion

        #region Initialization

        /// <summary>
        /// Initialize the instructions information
        /// </summary>
        /// <param name="X">X position including image</param>
        /// <param name="Y">X position including image</param>
        /// <param name="image">Picture on the left(text is wrap around it)</param>
        /// <param name="textColor"></param>
        /// <param name="batch">Sprite Batch</param>
        /// <param name="infoLines">string information around the texture</param>
        /// <param name="game">current game</param>
        /// <param name="offset">distance from image</param>
        /// <param name="wrap">whether to wrap text around the image or not</param>
        public MF_Instruction(float X, float Y, string image,Color textColor, SpriteBatch batch,string[] infoLines, Game game,float offset = 0,bool wrap = false)
            : base(game)
        {
            TextColor = textColor;
            Position = new Vector2(X, Y);
            ImagePath = image;
            InfoLines = infoLines;
            curGame = (MagicFightersGame)game;
            spriteBatch = batch;
            WrapText = wrap;
            xOffset = offset;
        }

        protected override void LoadContent()
        {

            if(!String.IsNullOrEmpty(ImagePath ))
                Texture = curGame.Content.Load<Texture2D>(ImagePath);

            font = curGame.Content.Load<SpriteFont>("GameFonts/InformationFont");

            base.LoadContent();
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            {
                //draw the image at the spesified location
                if(Texture != null)
                    spriteBatch.Draw(Texture, Position, Color.White);

                //start at the end of the picture
                float x = Position.X + (Texture != null ? Texture.Width : 0);
                float y = Position.Y;
                //draw each line of text around the image
                foreach (var text in InfoLines)
                {
                    //add the offset if the text is on the side of the image
                    if (Texture != null)
                        if (y < Texture.Height + Position.Y)
                            x = Position.X + xOffset + (Texture != null ? Texture.Width : 0);
                  
                    Vector2 size = font.MeasureString(text);//get the text size
                    spriteBatch.DrawString(font, text, new Vector2(x, y), TextColor);

                    y += size.Y + 10;//set the text y position

                    if (Texture != null)
                    {
                        //if we reach the end of the image then reset the y position of the text to
                        //the beginning of the image
                        if (WrapText && y >= Texture.Height + Position.Y)
                            x = Position.X;
                    }

                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
