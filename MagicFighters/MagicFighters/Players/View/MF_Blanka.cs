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

namespace MagicFighters.View
{
    class MF_Blanka : Enemy
    {
        #region Properties

        const float BLANKA_SPEED = 2;
        const float JUMP_HIGHT = 100;
        const float ANIMATION_RATIO = 0.04f;
        public float BlankaDamage = 10;
        public float BlankaHP { get { return HP; } set { HP = value; } }
        double count = 0;
        double DeadLapsedSeconds;
        double ElectrocuteLapsedSeconds;
        Vector2 StartingPosition = Vector2.Zero;
        #endregion

     
        #region Initialization
        //Blankas Contstructor
        public MF_Blanka(Game game, TileMap level, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch) { }

        public void InitAnimation(float x, float y)
        {
            //Load the Blanka Sprite Sheet
            Texture = curGame.Content.Load<Texture2D>("Textures/Players/Blanka/BlankaAnimations");
            Animations = new MobileSprite(Texture);

            //BLANKA IDLE
            //4 frames for Blankas Idle
            List<Rectangle> BlankaIdle = new List<Rectangle>()
            {
                new Rectangle(12,34,57,83),
                new Rectangle(84,32,56,85),
                new Rectangle(155,30,56,87),
                new Rectangle(226,32,56,85),

            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleR.ToString(), BlankaIdle, BLANKA_SPEED * ANIMATION_RATIO + 0.2f);
            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleL.ToString(), BlankaIdle, BLANKA_SPEED * ANIMATION_RATIO + 0.2f, true);

            //BLANKA WALK
            //5 Frames for Blankas walk
            List<Rectangle> BlankaWalk = new List<Rectangle>()
            {
                new Rectangle(297,42,59,71),
                new Rectangle(377,42,48,71),
                new Rectangle(450,42,45,71),
                new Rectangle(522,48,45,71),
                new Rectangle(587,45,52,72),
            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkR.ToString(), BlankaWalk, BLANKA_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkL.ToString(), BlankaWalk, BLANKA_SPEED * ANIMATION_RATIO, true);

            //BLANKA JUMP
            //4 frames for Blanka Jump
            List<Rectangle> BlankaJump = new List<Rectangle>()
            {
                //new Rectangle(659,54,51,63),
                new Rectangle(781,13,43,105),
                //new Rectangle(836,19,44,64),
                new Rectangle(890,13,44,105),
            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpUpR.ToString(), BlankaJump, BLANKA_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpUpL.ToString(), BlankaJump, BLANKA_SPEED * ANIMATION_RATIO, true);

            //BLANKA HURT
            //3 Frames for Blka heavy hit
            List<Rectangle> BlankaHeavyHit = new List<Rectangle>()
            {
                new Rectangle(311,1179,56,75),
                new Rectangle(376,1185,61,69),
                new Rectangle(443,1190,61,64),
            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.HitL.ToString(), BlankaHeavyHit, BLANKA_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.HitR.ToString(), BlankaHeavyHit, BLANKA_SPEED * ANIMATION_RATIO, true);

            //BLANKA ELECTROCUTE
            //8 Frames for Blanka Electrocute
            List<Rectangle> BlankaElectrocute = new List<Rectangle>()
            {
                new Rectangle(7,727,54,64),
                new Rectangle(70,728,51,63),
                new Rectangle(132,727,54,64),
                new Rectangle(195,728,51,63),
                new Rectangle(256,718,72,73),
                new Rectangle(339,735,49,56),
                new Rectangle(397,718,72,73),
                new Rectangle(480,735,49,56),
            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.ElectrocuteR.ToString(), BlankaElectrocute, BLANKA_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.ElectrocuteL.ToString(), BlankaElectrocute, BLANKA_SPEED * ANIMATION_RATIO, true);

            //Set the current animation to Idle Left
            Animations.Sprite.CurrentAnimation = MF_PlayerStates.IdleL.ToString();
            Animations.Position = new Vector2(x, y);
            Animations.Sprite.AutoRotate = false;
            Animations.IsPathing = false;
            Animations.IsMoving = false;

            CurrentState = MF_PlayerStates.IdleL;
            Visible = true;
            Name = "Blanka";
        }

        public void Initialize(Player player)
        {
            Player = player;
            Initialize();
        }
        public override void Initialize()
        {

            BlankaHP = 100;

            if (CurrentLevel != null && CurrentLevel.PlayerStartPosition != null)
            { InitAnimation(CurrentLevel.PlayerStartPosition.X, CurrentLevel.PlayerStartPosition.Y); }
            else
                InitAnimation(0, 0);
           // Position = new Vector2(600, 300);
            base.Initialize();
        }
        #endregion

        protected void Jump(float speed)
        {
            switch (CurrentState)
            {
                case MF_PlayerStates.WalkL:
                    CurrentState = MF_PlayerStates.JumpUpL;
                    Speed = new Vector2(1,1 );
                    break;
                case MF_PlayerStates.WalkR:
                    CurrentState = MF_PlayerStates.JumpUpR;
                    Speed = new Vector2(1, 1);
                    break;
                case MF_PlayerStates.IdleL:
                    CurrentState = MF_PlayerStates.JumpUpL;
                    Speed = new Vector2(1, 1);
                    break;
            }
            isPlayerJumping = true;
            StartingPosition = Position;
            Direction.Y = MOVE_UP;
        }

        private void UpdateJump()
        {
            //see if our distance is over 100 then revert the direction
            if ((StartingPosition.Y - Position.Y) >= JUMP_HIGHT)
            {
                if (Direction.X == MOVE_LEFT)
                    CurrentState = MF_PlayerStates.JumpDownL;
                else if (Direction.X == MOVE_RIGHT)
                    CurrentState = MF_PlayerStates.JumpDownR;
                Direction.Y = MOVE_DOWN;
            }
            //if we reach the starting position then set back to the origianl position
            // and reset the current state
            if ((int)Position.Y >= (int)StartingPosition.Y)
            {
                isPlayerJumping = false;
                Position = new Vector2(Position.X, StartingPosition.Y);

                if (Direction.X == MOVE_LEFT)
                    CurrentState = MF_PlayerStates.IdleL;
                else if (Direction.X == MOVE_RIGHT)
                    CurrentState = MF_PlayerStates.IdleR;
            }
            if (Direction.Y == MOVE_DOWN && isColliding)
            {
                isPlayerJumping = false;
                if (Direction.X == MOVE_LEFT)
                    CurrentState = MF_PlayerStates.IdleL;
                else if (Direction.X == MOVE_RIGHT)
                    CurrentState = MF_PlayerStates.IdleR;
            }
        }
        protected override void ResetState()
        {
            CurrentLevel.LevelGravity = 3.38f;
            isPlayerCollidingWithTheFloor = false;

            if (CurrentState == MF_PlayerStates.Dead)
                return;

            if (!isHitState)
            {
                if (isJumpState)
                    UpdateJump();
                else
                {
                    if (Direction.X == MOVE_LEFT)
                        CurrentState = MF_PlayerStates.IdleL;
                    else if (Direction.X == MOVE_RIGHT)
                        CurrentState = MF_PlayerStates.IdleR;
                }
            }
            else
            {


                if (!isJumpState)
                {
                    if (ElectrocuteLapsedSeconds >= 2)
                    {
                        if (Direction.X == MOVE_LEFT)
                            CurrentState = MF_PlayerStates.IdleL;
                        else if (Direction.X == MOVE_RIGHT)
                            CurrentState = MF_PlayerStates.IdleR;
                        ElectrocuteLapsedSeconds = 0;
                    }
                }
            }
        }

        void handlePlayerPositionLeft()
        {
            if (Player.Position.X < this.Position.X)
            {
                this.CurrentState = MF_PlayerStates.WalkL;
            }
        }

        void handlePlayerPositionRight()
        {
            if (Player.Position.X > this.Position.X)
            {
                this.CurrentState = MF_PlayerStates.WalkR;
            }
        }

        void handleJump()
        {
            if (count >= 3)
            {
                count = 0; 
                Jump(BLANKA_SPEED);
            }
        }
        protected override void HandleState()
        {

            if (CurrentState == MF_PlayerStates.Dead)
                return;

            double distanceFromPlayer = this.Position.X - Player.Position.X;

            if (Math.Abs(distanceFromPlayer) < 1000)
            {
                //if blanka is in the hit state
                if (isHitState)
                {
                    isPlayerJumping = false;

                    if (Player.Position.X < this.Position.X)
                        CurrentState = MF_PlayerStates.ElectrocuteL;
                    else
                        CurrentState = MF_PlayerStates.ElectrocuteR;
                    //ElectrocuteLapsedSeconds = 0;
                }
                else
                {
                    if (!isPlayerJumping)
                    {
                        handlePlayerPositionLeft();
                        handlePlayerPositionRight();
                    }
                    handleJump();
                }
            }
        }
        /// <summary>
        /// This function handles getting damage
        /// </summary>
        protected override void HandleDammage()
        {
            float distanceFromPlayer = Math.Abs(this.Position.X - Player.Position.X);

            //if we are touching the enemy and the enemy is not hitting us
            if (Player.BoundingBox.Intersects(BoundingBox))
            {
                //and we are performing a punch attack
                if (Player.isHitState)
                {
                    isPlayerJumping = false;
                    switch (CurrentState)
                    {
                        case MF_PlayerStates.WalkL:
                            this.CurrentState = MF_PlayerStates.ElectrocuteL;
                            Position += new Vector2(20, 0);
                            break;
                        case MF_PlayerStates.WalkR:
                            this.CurrentState = MF_PlayerStates.ElectrocuteR;
                            Position += new Vector2(-20, 0);
                            break;
                    }
                    this.Position += new Vector2((Size.X + 10) * -Direction.X, 0);
                    HP -= 10;
                    //have we kill blanka yet
                    if (HP <= 0)
                    {
                        DeadLapsedSeconds = 0;
                        CurrentState = MF_PlayerStates.Dead;
                        CurrentLevel.CurrentGameState = MF_GameStates.LevelOver;
                    }
                }
            }
        }
        protected override void UpdateState(GameTime gameTime)
        {
            if (!isPlayerJumping)
                count += (gameTime.ElapsedGameTime.TotalSeconds);

            if(CurrentState == MF_PlayerStates.ElectrocuteL || 
                CurrentState == MF_PlayerStates.ElectrocuteR)
                ElectrocuteLapsedSeconds += (gameTime.ElapsedGameTime.TotalSeconds);

            switch (CurrentState)
            {
                    //walking
                case MF_PlayerStates.WalkL:
                    Set(new Vector2(MOVE_LEFT, this.Direction.Y), MF_PlayerStates.WalkL, new Vector2(1, 1));
                    break;
                case MF_PlayerStates.WalkR:
                    Set(new Vector2(MOVE_RIGHT, this.Direction.Y), MF_PlayerStates.WalkR, new Vector2(1, 1));
                    break;

                    //idle
                case MF_PlayerStates.IdleL:
                    Set(new Vector2(Direction.X, MOVE_NONE), MF_PlayerStates.IdleL, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.IdleR:
                    Set(new Vector2(Direction.X, MOVE_NONE), MF_PlayerStates.IdleR, new Vector2(0, 0));
                    break;
                    //jumping
                case MF_PlayerStates.JumpUpR:
                    Set(new Vector2(MOVE_RIGHT, MOVE_UP), MF_PlayerStates.JumpUpR, new Vector2(1, 4));
                    break;
                case MF_PlayerStates.JumpUpL:
                    Set(new Vector2(MOVE_LEFT, MOVE_UP), MF_PlayerStates.JumpUpL, new Vector2(1, 4));
                    break;
                    //dead
                case MF_PlayerStates.Dead:
                    DeadLapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;
                    if (DeadLapsedSeconds >= 2)
                        Visible = false;
                    Set(new Vector2(MOVE_NONE, MOVE_NONE), MF_PlayerStates.Dead, new Vector2(0, 0));
                    break;

                    //electrocute
                case MF_PlayerStates.ElectrocuteR:
                    Set(new Vector2(Direction.X, Direction.Y), MF_PlayerStates.ElectrocuteR, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.ElectrocuteL:
                    Set(new Vector2(Direction.X, Direction.Y), MF_PlayerStates.ElectrocuteL, new Vector2(0, 0));
                    break;
            }
            //emitter.Position = new Vector2(Position.X + 35, Position.Y + 22);
            // particleComponent.Update(gameTime);

            CurrentLevel.LevelGravity = 3.38f;
        }
    }
}
