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
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicFighters.View;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MagicFighters.ViewModel;
using MagicFighters.TileEngine.Model;
using MagicFighters.GameStates;
using MagicFighters.Players.View;
using MagicFighters.Model;
//namespace MagicFighters.Players.View;



namespace MagicFighters.View
{
    class MF_Bot : Enemy
    {
        #region Properties

        const float ANIMATION_RATIO = 0.04f;
        bool isBombDropped; 
        Vector2 StartingPosition = Vector2.Zero;
        DynamicItem Bomb { get; set; }
        #endregion

        #region Initialization
        //Bots Constructor
        public MF_Bot(MF_EditorPlayer info, Game game, TileMap level, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch)
        {
            var sprite = curGame.Content.Load<Texture2D>("Level1Tileset\\Items\\" + info.Name);
            Name = info.Name;
            Animations = new MobileSprite(sprite);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkL.ToString(), 0, 0, info.Size.W, info.Size.H, 1, 1);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkR.ToString(), 0, 0, info.Size.W, info.Size.H, 1, 1, true);
            
            //Set the current animation to Walk Left
            Animations.Sprite.CurrentAnimation = MF_PlayerStates.WalkL.ToString();
        }

        public void Initialize(Player player)
        {
         
            Player = player;
            Bomb = new DynamicItem(curGame, CurrentLevel, spriteBatch, Player);
            //Bomb.Initialize();
            Bomb.Load("Textures/Bots/bomb_t");
            Bomb.Visible = false;
            Bomb.vDirection = new Vector2(MOVE_NONE, MOVE_DOWN);
            Bomb.vSpeed = new Vector2(0, 2);
            //Name = "Bot";
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        #region Update

        protected override void ResetState()
        {
            CurrentLevel.LevelGravity = 0;
        }

        protected override void HandleState()
        {

            if (HPBar != null)
                HPBar.Visible = false;
           

            float distanceFromPlayer = Math.Abs(this.Position.X - Player.Position.X);

            if (distanceFromPlayer <= 100)
            {
                if (this.Position.X > Player.Position.X - 50 ||
                    this.Position.X < Player.Position.X + 50)
                {
                    if (Bomb.Visible == true)
                    {
                        //fly left
                        if ((Player.Position.X > this.Position.X))
                            this.CurrentState = MF_PlayerStates.WalkR;
                        //fly right
                        if ((Player.Position.X < this.Position.X))
                            this.CurrentState = MF_PlayerStates.WalkL;
                    }
                    else
                        isBombDropped = false;
              
                    if (isBombDropped == false)
                    {
                        Bomb.Visible = true;
                        Bomb.Position = Position;
                        isBombDropped = true;
                   }
                }
            }
        }
        protected override void UpdateState(GameTime gameTime)
        {
            if (Bomb.Visible)
            {
                if ((Bomb.BoundingBox.Intersects(Player.BoundingBox)))
                {
                    Bomb.Visible = false;
                    Player.GetSetHP -= 10;

                    if (Player.GetSetHP <= 0)
                    {
                        Player.GetSetCurrentState = MF_PlayerStates.Dead;
                    }
                }

                var floor = this.CurrentLevel.GetFloorCollisionBounding(Bomb.BoundingBox);

                if (floor != null)
                {
                    Bomb.Visible = false;
                }

                var tile = this.CurrentLevel.GetTileCollisionBounding(Bomb.BoundingBox);

                if (tile != null)
                {
                    Bomb.Visible = false;
                }

                switch (CurrentState)
                {
                    //flying aka walking
                    case MF_PlayerStates.WalkL:
                        Set(new Vector2(MOVE_LEFT, MOVE_NONE), MF_PlayerStates.WalkL, new Vector2(3, 0));
                        break;
                    case MF_PlayerStates.WalkR:
                        Set(new Vector2(MOVE_RIGHT, MOVE_NONE), MF_PlayerStates.WalkR, new Vector2(3, 0));
                        break;
                    //floating aka idle
                    case MF_PlayerStates.IdleL:
                        Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleL, new Vector2(0, 0));
                        break;
                    case MF_PlayerStates.IdleR:
                        Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleR, new Vector2(0, 0));
                        break;
                    //drop bomb
                    case MF_PlayerStates.dropBomb:
                        Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.dropBomb, new Vector2(0, 0));
                        break;
                }
                Bomb.Update(gameTime);
            }
        }

        #endregion
        #region Draw
        //Draws Blanka on the screen
        public override void Draw(GameTime gameTime)
        {
            if (Bomb != null && Bomb.Visible)
            {
                if (Name == "flyingbot1")
                    Bomb.Draw(Color.Red);
                else
                    Bomb.Draw(Color.Green);
            }
            base.Draw(gameTime);
        }
        #endregion
    }
}
