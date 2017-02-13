// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//Enemy.cs
//Author        : William McHugh
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/18/2012    By: Lisandro Martinez
//              : 09/23/2012   By: William McHugh
//              : 09/26/2012   By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion

#define PERPIXEL
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.ViewModel;
using System.Collections.Generic;
using MagicFighters.TileEngine.Model;
using MagicFighters.GameStates;

namespace MagicFighters.View
{
    abstract class Enemy : Player
    {
        public Player Player = null;

        public Enemy(Game game, TileMap level, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch)
        {
          
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            HPBar = new Players.View.MF_HudBar(spriteBatch, curGame, Position, new Vector2(0,-30), -5, Name, Color.LightGray, 100);
            HPBar.Initialize();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(HPBar != null)
                HPBar.Draw(gameTime);

            Animations.Draw(spriteBatch);
           
            base.Draw(gameTime);
        }


        protected override void HandleDammage() { }
      
       
             
    }
}