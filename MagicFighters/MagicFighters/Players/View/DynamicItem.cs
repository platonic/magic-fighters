// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//DynamicItems.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/21/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicFighters.View;
using Microsoft.Xna.Framework;
using MagicFighters.ViewModel;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Model;

namespace MagicFighters.Players.View
{
    class DynamicItem : MF_Object
    {
        public Player player = null;
        public MF_EditorPlayer CurLoadedItem = null;
        protected Texture2D Texture = null;
        public Vector2 vDirection
        {
            get
            {
                return Direction;
            }
            set
            {
                Direction = value;
            }
        }

        public Vector2 vSpeed
        {
            get
            {
                return Speed;
            }
            set
            {
                Speed = value;
            }
        }

        public DynamicItem(Game game, TileMap level, SpriteBatch screenSpriteBatch, Player _player)
            : base(game, level, screenSpriteBatch)
        {
            player = _player;
            spriteBatch = screenSpriteBatch;
            curGame = (MagicFightersGame)game;
            CurrentLevel = level;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (CurLoadedItem != null)
                Texture = curGame.Content.Load<Texture2D>(CurLoadedItem.FilePath);


            base.LoadContent();
        }
        public void Init(MF_EditorPlayer item)
        {
            CurLoadedItem = item;
            Animations = new MobileSprite(Texture);

            if (CurLoadedItem != null)
                Position = new Vector2(CurLoadedItem.Position.X, CurLoadedItem.Position.Y);

            Initialize();

        }
        public override void Update(GameTime gameTime)
        {
            Position += Direction * Speed;
            if (!isObjectVisible())
            {
                Visible = false; 
            }
            base.Update(gameTime);
        }
        public void Load(string filePath)
        {
            Animations = new MobileSprite(null);
            Texture = curGame.Content.Load<Texture2D>(filePath);
        }
        public void Draw(Color c)
        {
            spriteBatch.Draw(Texture, Position, c);
        }
        public override void Draw(GameTime gameTime)
        {
            if (CurLoadedItem == null)
                return;

            if (CurLoadedItem.Size == null)
            {
                //- CurrentLevel.Camera.Pos
                spriteBatch.Draw(Texture, Position, Color.White);

            }
            else if (CurLoadedItem.Size.H <= 0 || CurLoadedItem.Size.W <= 0)
            {
                // - CurrentLevel.Camera.Pos
                spriteBatch.Draw(Texture, Position, Color.White);

            }
            else
            {
                // - CurrentLevel.Camera.Pos.X
                spriteBatch.Draw(Texture, new Rectangle((int)(Position.X), (int)(Position.Y),// - CurrentLevel.Camera.Pos.Y
                    CurLoadedItem.Size.W, CurLoadedItem.Size.H), Color.White);
            }

            base.Draw(gameTime);
        }



    }
}
