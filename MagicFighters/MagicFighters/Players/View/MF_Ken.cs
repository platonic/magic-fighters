// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Ken.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//Last Modified : 9/26/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
#define NOT_PERPIXEL

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
using MagicFighters.Players.Model;
using MagicFighters.TileEngine;
namespace MagicFighters.Players.View
{
    class MF_Ken : Player
    {
        #region Fields
        KeyboardState CurrentKeyboardState = new KeyboardState();
        Vector2 StartingPosition = Vector2.Zero;
        Vector2 PrevPosition = Vector2.Zero;
        KeyboardState mPreviousKeyboardState;
        const float KEN_SPEED = 5;
        const float ANIMATION_RATIO = 0.04f;
        const float JUMP_HIGHT = 100;
        List<Enemy> Enemies = null;
        private SpriteFont font = null;
        private Texture2D CollisionRectangleTexture;
        private MF_Dialogue KenMessage;
        private string Message;
        private MF_BlueMan BlueMan;
        #endregion
        #region Properties

        bool isBlueManAlive
        {
            get
            {
                if (BlueMan == null)
                    return false;
                return BlueMan.GetState != MF_PlayerStates.Dead;
            }
        }
        

        #endregion
        #region Initialization

        /// <summary>
        /// Initialize the items ken can pick up
        /// </summary>
        /// <param name="items"></param>
        public void InitStaticItems(List<StaticItem> items)
        {
            Items = items;
        }
        /// <summary>
        /// Initialize all ken animations based on MF_PlayerStates
        /// </summary>
        public void InitAnimation()
        {
          
             
            //setup the animations
            Texture = curGame.Content.Load<Texture2D>("KenAnimations");
            Animations = new MobileSprite(Texture);

            int frameW = 50;
            int frameH = 100;
            //Size = new Vector2(frameW, frameH);

            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleR.ToString(), 0, 0, frameW, frameH, 4, KEN_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkR.ToString(), 200, 0, frameW, frameH, 5, KEN_SPEED * ANIMATION_RATIO);

            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleL.ToString(), 0, 0, frameW, frameH, 4, KEN_SPEED * ANIMATION_RATIO, true);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkL.ToString(), 200, 0, frameW, frameH, 5, KEN_SPEED * ANIMATION_RATIO, true);

            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpL.ToString(), 456, 0, 40, frameH, 7, KEN_SPEED * ANIMATION_RATIO, true);
            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpR.ToString(), 456, 0, 40, frameH, 7, KEN_SPEED * ANIMATION_RATIO);

            List<Rectangle> highpunchrects = new List<Rectangle>()
            {
                new Rectangle(224,112,46,frameH),
                new Rectangle(283,112,64,frameH),
                new Rectangle(360,112,45,frameH),
               
            };
            Animations.Sprite.AddAnimation(MF_PlayerStates.PunchRH.ToString(), highpunchrects,0.08f);
            Animations.Sprite.AddAnimation(MF_PlayerStates.PunchLH.ToString(), highpunchrects,0.08f, true);

            List<Rectangle> rects = new List<Rectangle>()
            {
                new Rectangle(747,0,42,frameH),
                new Rectangle(790,0,43,frameH),
                new Rectangle(833,0,64,frameH),
                new Rectangle(897,0,42,frameH),
                new Rectangle(939,0,78,frameH),
                new Rectangle(1017,0,50,frameH),
                new Rectangle(1067,0,45,frameH),
               
            };
            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpFW.ToString(), rects, KEN_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.JumpBW.ToString(), rects, KEN_SPEED * ANIMATION_RATIO, true);
            Animations.Sprite.AddAnimation(MF_PlayerStates.CrouchR.ToString(), 1158, 43, 46, 54, 1, KEN_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.CrouchL.ToString(), 1158, 43, 46, 54, 1, KEN_SPEED * ANIMATION_RATIO, true);

            List<Rectangle> KenHurt = new List<Rectangle>()
            {
                new Rectangle(165,779,39,78),
                new Rectangle(217,780,39,78),
                new Rectangle(270,780,43,78),
                new Rectangle(326,777,50,81),

            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.HitL.ToString(), KenHurt, KEN_SPEED * ANIMATION_RATIO);
            Animations.Sprite.AddAnimation(MF_PlayerStates.HitR.ToString(), KenHurt, KEN_SPEED * ANIMATION_RATIO, true);

            List<Rectangle> Fireball = new List<Rectangle>()
            {
                new Rectangle(892,548,25,27),
                new Rectangle(922,549,32,23),

            };

            Animations.Sprite.AddAnimation(MF_PlayerStates.FireballL.ToString(), Fireball, KEN_SPEED * ANIMATION_RATIO * 0.1f);
            Animations.Sprite.AddAnimation(MF_PlayerStates.FireballR.ToString(), Fireball, KEN_SPEED * ANIMATION_RATIO * 0.1f, false);
            Animations.Sprite.CurrentAnimation = MF_PlayerStates.IdleR.ToString();
            Animations.Sprite.AutoRotate = false;
            Animations.IsPathing = false;
            Animations.IsMoving = false;

            CurrentState = MF_PlayerStates.IdleR;
            Visible = true;
            Name = "Ken";


            KenMessage = new MF_Dialogue(this, spriteBatch, curGame);
            KenMessage.Comments = "You have to kill Blue Man before you can move forward.";
            KenMessage.Initialize();
            KenMessage.Position = new Vector2(400, 400);
            KenMessage.Offset = new Vector2(Size.X, 30);
            KenMessage.ShowSpeechBuble = false;
            
        }

        public MF_Ken(Game game, TileMap level, List<Enemy> enemies, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.GetType() == typeof(MF_BlueMan))
                    BlueMan = (MF_BlueMan)enemy;
            }
            Enemies = enemies;
        }



        public override void Initialize()
        {
            HP = 100;
            MP = 100;
            Score = 0;
            MPBar = new MF_HudBar(CurrentLevel.Camera, spriteBatch, curGame, new Vector2(0, 450), new Vector2(34, 0), 10, "MP:", Color.Red, (int)MP);
            HPBar = new MF_HudBar(CurrentLevel.Camera, spriteBatch, curGame, new Vector2(0, 465), new Vector2(34, 0), 10, "HP:", Color.Green, (int)HP);
            ScoreBar = new MF_HudBar(CurrentLevel.Camera, spriteBatch, curGame, new Vector2(0, 480), new Vector2(10, 0), 10, "Score:", Color.Beige, (int)Score);
            CollisionRectangleTexture = curGame.Content.Load<Texture2D>("rectangle");
            font = curGame.Content.Load<SpriteFont>("DebugSpriteFont");
            Direction.X = MOVE_RIGHT;
            InitAnimation();

            base.Initialize();
        }
        #endregion

        #region Handle Input




        #endregion

        #region Update

        protected override void ResetState()
        {
           
            CurrentLevel.LevelGravity = 3.38f;
            isPlayerCollidingWithTheFloor = false;


            if (CurrentState == MF_PlayerStates.Dead)
                return;

            if (isJumpState)
            {
               UpdateJump();
            }
            else
            {
                if (Direction.X == MOVE_LEFT)
                {
                    CurrentState = MF_PlayerStates.IdleL;
                }
                else if (Direction.X == MOVE_RIGHT)
                {
                    CurrentState = MF_PlayerStates.IdleR;
                }

            }
        }

        protected override void HandleState()
        {
            mPreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentState == MF_PlayerStates.Dead)
                return;

            if (!isPlayerJumping)
            {
                /// <summary>
                /// Function processes the user input
                /// </summary>
                if (CurrentKeyboardState.IsKeyDown(Keys.Left))
                {
                    CurrentState = MF_PlayerStates.WalkL;
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Right))
                {
                    CurrentState = MF_PlayerStates.WalkR;
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Down))
                {
                    switch (CurrentState)
                    {
                        case MF_PlayerStates.IdleL:
                            CurrentState = MF_PlayerStates.CrouchL;
                            break;
                        case MF_PlayerStates.IdleR:
                            CurrentState = MF_PlayerStates.CrouchR;
                            break;
                    }

                }

                if (CurrentKeyboardState.IsKeyDown(Keys.A))
                {
                    switch (CurrentState)
                    {
                        case MF_PlayerStates.WalkL:
                        case MF_PlayerStates.IdleL:
                            CurrentState = MF_PlayerStates.PunchLH;
                            break;
                        case MF_PlayerStates.WalkR:
                        case MF_PlayerStates.IdleR:
                            CurrentState = MF_PlayerStates.PunchRH;
                            break;
                    }
                }
            }
            else
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.Left))
                {
                    Direction.X = MOVE_LEFT;
                    Speed = new Vector2(KEN_SPEED, KEN_SPEED);
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Right))
                {
                    Direction.X = MOVE_RIGHT;
                    Speed = new Vector2(KEN_SPEED, KEN_SPEED);

                }

               
            }

            if (!isJumpState && CurrentKeyboardState.IsKeyDown(Keys.Space) == true &&
                   mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                Jump(KEN_SPEED);
            }

#if DEBUG

            if (CurrentKeyboardState.IsKeyDown(Keys.Up))
            {
                Direction.Y = MOVE_UP;
                Speed = new Vector2(Speed.X, KEN_SPEED);
            }
#endif

        }

        protected override void UpdateState(GameTime gameTime)
        {

            if (CurrentLevel.GetType() == typeof(LevelTwoTileMap) && Position.X >= 13000)
            {
                CurrentLevel.CurrentGameState = MF_GameStates.LevelOver;
            }


            KenMessage.Update(gameTime);
            switch (CurrentState)
            {
                //walking
                case MF_PlayerStates.WalkL:
                    Set(new Vector2(MOVE_LEFT, this.Direction.Y), MF_PlayerStates.WalkL, new Vector2(KEN_SPEED, KEN_SPEED));
                    break;
                case MF_PlayerStates.WalkR:
                    Set(new Vector2(MOVE_RIGHT, this.Direction.Y), MF_PlayerStates.WalkR, new Vector2(KEN_SPEED, KEN_SPEED));
                    break;
                //jumping
                case MF_PlayerStates.JumpUpL:
                    Set(new Vector2(this.Direction.X, this.Direction.Y), MF_PlayerStates.JumpL, Speed);
                    break;
                case MF_PlayerStates.JumpUpR:
                    Set(new Vector2(this.Direction.X, this.Direction.Y), MF_PlayerStates.JumpR, Speed);
                    break;
                case MF_PlayerStates.JumpDownL:
                    Set(new Vector2(this.Direction.X, MOVE_DOWN), MF_PlayerStates.JumpR, Speed);
                    break;
                case MF_PlayerStates.JumpDownR:
                    Set(new Vector2(this.Direction.X, MOVE_DOWN), MF_PlayerStates.JumpR, Speed);
                    break;
                //case MF_PlayerStates.JumpL:
                //    Set(new Vector2(this.Direction.X, MOVE_UP), MF_PlayerStates.JumpL, new Vector2(KEN_SPEED, KEN_SPEED));
                //    break;
                //case MF_PlayerStates.JumpR:
                //    Set(new Vector2(this.Direction.X, MOVE_UP), MF_PlayerStates.JumpR, new Vector2(KEN_SPEED, KEN_SPEED));
                //    break;
                case MF_PlayerStates.JumpFW:
                    Set(new Vector2(this.Direction.X, MOVE_UP), MF_PlayerStates.JumpFW, new Vector2(KEN_SPEED, KEN_SPEED));
                    break;
                case MF_PlayerStates.JumpBW:
                    Set(new Vector2(this.Direction.X, MOVE_UP), MF_PlayerStates.JumpBW, new Vector2(KEN_SPEED, KEN_SPEED));
                    break;
                //crouch
                case MF_PlayerStates.CrouchL:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.CrouchL, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.CrouchR:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.CrouchR, new Vector2(0, 0));
                    break;
                //punching
                case MF_PlayerStates.PunchLH:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.PunchLH, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.PunchRH:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.PunchRH, new Vector2(0, 0));
                    break;
                //idle
                case MF_PlayerStates.IdleL:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleL, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.IdleR:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleR, new Vector2(0, 0));
                    break;
                //Hit
                case MF_PlayerStates.HitL:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.HitL, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.HitR:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.HitR, new Vector2(0, 0));
                    break;
                    //dead
                case MF_PlayerStates.Dead:
                    Set(new Vector2(MOVE_NONE, MOVE_NONE), MF_PlayerStates.Dead, new Vector2(0, 0));
                    CurrentLevel.CurrentGameState = MF_GameStates.GameOver;
                    break;
            }

            if ((Position.Y + Size.Y) > CurrentLevel.ScreenBottom)
            {
                CurrentState = MF_PlayerStates.Dead;
            }

            if (Position.X < CurrentLevel.ScreenLeft || (Position.X + Size.X) > CurrentLevel.ScreenRight)
            {
                //Vector2 displacement = MF_Physics.CalcualteMinimumTranslationDistance(GetRectangle, new Rectangle((int)(CurrentLevel.ScreenLeft - 10), 0, 20, 800));
                Position += new Vector2(Speed.X + 0.1f * -Direction.X, 0);
            }

            if (isBlueManAlive && Position.X + Size.X >= 1293)
            {
                KenMessage.ResetDisplayTimer();
                KenMessage.Display = true;
                Position = new Vector2(1153, 232);
            }
           

            if (isBlueManAlive && Position.X  <= 773)
            {
                KenMessage.Display = false;
            }


        }


        /// <summary>
        /// Handle when the enemies dammage Ken
        /// </summary>
        protected override void HandleDammage()
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.GetState == MF_PlayerStates.Dead)
                    continue;
                
                BoundingBox enemyBB = enemy.GetPlayerBoundingBox();
                BoundingBox playerBB = GetPlayerBoundingBox();

                if (playerBB.Intersects(enemyBB))
                {
                    //only get damage from the enemy if it is in the heat state
                    if (enemy.isHitState)
                    {
                        if (this.Position.X < enemy.Position.X)
                        {
                            CurrentState = MF_PlayerStates.HitL;
                        }
                        else
                        {
                            CurrentState = MF_PlayerStates.HitR;

                        }

                        this.Position += new Vector2((Size.X+10) * -Direction.X, 0);
                        HP -= 10;
                        if (HP < 0)
                        {
                            CurrentState = MF_PlayerStates.Dead;
                            HP = 0;
                        }

                    }//if (isHitState(enemy))
                }
            }
        }


        protected void Jump(float speed)
        {
            switch (CurrentState)
            {
                case MF_PlayerStates.WalkL:
                    CurrentState = MF_PlayerStates.JumpBW;
                    Speed = new Vector2(KEN_SPEED, KEN_SPEED);
                    break;
                case MF_PlayerStates.WalkR:
                    CurrentState = MF_PlayerStates.JumpFW;
                    Speed = new Vector2(KEN_SPEED, KEN_SPEED);
                    break;
                case MF_PlayerStates.IdleL:
                    CurrentState = MF_PlayerStates.JumpUpL;
                    Speed = new Vector2(MOVE_NONE, KEN_SPEED);
                    break;
                case MF_PlayerStates.IdleR:
                    CurrentState = MF_PlayerStates.JumpUpR;
                    Speed = new Vector2(MOVE_NONE, KEN_SPEED);
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
                
                if(Direction.X == MOVE_LEFT)
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
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            if (font != null)//Debugging properties
            {
                spriteBatch.DrawString(font, "(" + Position.X + "," + Position.Y + ")", new Vector2(Position.X - 268, 0), Color.White);
                spriteBatch.DrawString(font, "(" + Speed.X + "," + Speed.Y + ")", new Vector2(Position.X - 268, 25), Color.White);
                spriteBatch.DrawString(font, "(" + Direction.X + "," + Direction.Y + ")", new Vector2(Position.X - 268, 50), Color.White);
                spriteBatch.DrawString(font, "State: (" + CurrentState.ToString() + ")", new Vector2(Position.X - 268, 75), Color.White);
                spriteBatch.DrawString(font, "Collision: (" + (isPlayerCollidingWithTheFloor ? "True" : "False") + ")", new Vector2(Position.X - 268, 100), Color.White);
            }
            CurrentLevel.DrawCollisionBounds();
            spriteBatch.Draw(CollisionRectangleTexture, new Rectangle((int)base.GetPlayerBoundingBox().Min.X, (int)base.GetPlayerBoundingBox().Min.Y,
                (int)Size.X, (int)Size.Y), Color.Red);

#endif

            KenMessage.Draw(gameTime);
            DrawAnimation();

        }
        #endregion
    }
}
