// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_HudBar.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//Last Modified : 9/26/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using MagicFighters.Screens;
using MagicFighters.Model;

namespace MagicFighters.Players.View
{
    class MF_HudBar: Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Fields

        Texture2D HealthBarTexture;
        float textbaroffset = 10;
        float healthbarxoffset = 10;
        float healthbaryoffset = 0;
        MagicFightersGame curGame = null;
        SpriteBatch SpriteBatch = null;
        string name;
        Color color;
        int _value;
        Vector2 position;
        Vector2 cameraposition;
        MF_Camera camera;
        SpriteFont font;
        int Percentage;
        #endregion

        #region Properties

        public float TextBarOffset { get { return textbaroffset; } set { textbaroffset = value; } }
        public MF_Camera Camera { get { return camera; } set { camera = value; } }
        //public ScreenManager ScreenManager { get { return screenManager; } set { screenManager = value; } }
        public float HealthBarXOffset { get { return healthbarxoffset; } set { healthbarxoffset = value; } }
        public float HealthBarYOffset { get { return healthbaryoffset; } set { healthbaryoffset = value; } }

        public string Name { get { return name; } set { name = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public int Value { get { return _value; } set { _value = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        Vector2 CameraPosition { get { return cameraposition; } set { cameraposition = value; } }
        #endregion

        public MF_HudBar(MF_Camera camera,SpriteBatch spritebatch,Game game, Vector2 position,Vector2 offset,float textbaroffset, string name, Color color, int value, int percentage = 100)
            : base(game)
        {
            TextBarOffset = textbaroffset;
            HealthBarXOffset = offset.X;
            HealthBarYOffset = offset.Y;
            Camera = camera;
            curGame = (MagicFightersGame)game;
            SpriteBatch = spritebatch;
            Name = name;
            Color = color;
            Value = value;
            Position = position;
            Percentage = percentage;
        }

        public MF_HudBar(SpriteBatch spritebatch, Game game, Vector2 position, Vector2 offset, float textbaroffset, string name, Color color, int value, int percentage = 100)
            : base(game)
        {
            TextBarOffset = textbaroffset;
            HealthBarXOffset = offset.X;
            HealthBarYOffset = offset.Y;
            curGame = (MagicFightersGame)game;
            SpriteBatch = spritebatch;
            Name = name;
            Color = color;
            Value = value;
            Position = position;
            Percentage = percentage;
        }

        /// <summary>
        /// A helper to draw shadowed text.
        /// </summary>
        void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(font, text,
                new Vector2(position.X + 1, position.Y + 1), Color.Black);
            SpriteBatch.DrawString(font, text, position, color);
        }

        /// <summary>
        /// A helper to draw shadowed text.
        /// </summary>
        void DrawString(SpriteFont font, string text, Vector2 position, Color color, float fontScale)
        {
            SpriteBatch.DrawString(font, text,
                new Vector2(position.X + 1, position.Y + 1),
                Color.Black, 0, new Vector2(0, font.LineSpacing * 0.5f),
                fontScale, SpriteEffects.None, 0);
            SpriteBatch.DrawString(font, text, position,
                color, 0, new Vector2(0, font.LineSpacing * 0.5f),
                fontScale, SpriteEffects.None, 0);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            HealthBarTexture = curGame.Content.Load<Texture2D>("PlayerHealthBar");
            font = curGame.Content.Load<SpriteFont>("HudSpriteFont");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            if(Camera == null)
                CameraPosition = Vector2.Zero;
            else
                CameraPosition = Camera.Pos;

            DrawString(font, Name, Position + CameraPosition + new Vector2(TextBarOffset, HealthBarYOffset), Color);

            Vector2  size = font.MeasureString(Name);

            int x = (int)((Position.X + CameraPosition.X) + HealthBarXOffset + size.X);
            int y = (int)((Position.Y + CameraPosition.Y ) + HealthBarYOffset+5);
            int w = HealthBarTexture.Width;
            int h = 10;

            SpriteBatch.Draw(HealthBarTexture, new Rectangle(x, y, w, h), new Rectangle(0, 12, w, h), Color.Gray);

            SpriteBatch.Draw(HealthBarTexture, new Rectangle(x, y, (int)(HealthBarTexture.Width * ((double)Value / Percentage)),
                h), new Rectangle(0, 12, w, h), Color);

            //Draw the box around the health bar
            SpriteBatch.Draw(HealthBarTexture, new Rectangle(x, y, w, h), new Rectangle(0, 0, w, h), Color.White);
            
            base.Draw(gameTime);
        }

        public void Update(int val)
        {
            Value = val;
        }
        public void Update(int val,MF_Object obj)
        {
            Value = val;
            Position = obj.Position - new Vector2(obj.Rectangle.Value.Width * 0.5f, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
