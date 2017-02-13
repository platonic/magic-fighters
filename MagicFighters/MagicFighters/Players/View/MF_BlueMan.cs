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
//Last Modified : 9/27/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicFighters.ViewModel;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.TileEngine.Model;
using MagicFighters.GameStates;
using MagicFighters.ParticleEngine;
using MagicFighters.ParticleEngine.Helpers;

namespace MagicFighters.View
{
    class MF_BlueMan : Enemy
    {

        #region Fields
        ParticleComponent particleComponent;
        Emitter emitter;
        double DeadLapsedSeconds;
        double PunchingLapsedSeconds;
        #endregion

        #region Initialize
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">the curent game</param>
        /// <param name="level">current level</param>
        /// <param name="screenSpriteBatch">screen spritebatch</param>
        public MF_BlueMan(Game game, TileMap level, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch){}
        /// <summary>
        /// Initialises blueman and the player he needs to know about
        /// </summary>
        /// <param name="player"></param>
        public void Initialize(Player player)
        {
            HP = 100;
            Player = player;
            particleComponent = new ParticleComponent(spriteBatch, curGame);
            InitAnimation(650, 240);

            //the current visivility states
            Visible = true;
            Name = "";
            Initialize();
           
        }
       
        /// <summary>
        /// Initialises the animations
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        private void InitAnimation(float x, float y)
        {



            PunchingLapsedSeconds = -1;

            int frameH = 78;
            //Size = new Vector2(72, frameH);
            //setup blueman animations
            Texture = curGame.Content.Load<Texture2D>("Textures/Players/BlueMan");
            
            Animations = new MobileSprite(Texture);

            //load particles
            emitter = new Emitter()
            {
                Active = true,
                TextureList = new List<Texture2D>() { curGame.Content.Load<Texture2D>("Textures/Players/BlueManDead") },
                Position = new Vector2(x, y),
                RandomEmissionInterval = new RandomMinMax(8.0d),
                ParticleLifeTime = 100,
                ParticleDirection = new RandomMinMax(0, 1),
                ParticleSpeed = new RandomMinMax(0.5f, 2.0f),
                ParticleRotation = new RandomMinMax(0, 0),
                RotationSpeed = new RandomMinMax(0),
                ParticleFader = new ParticleFader(true, true, 350),
                ParticleScaler = new ParticleScaler(true, 1.0f),
                 
            };
            particleComponent.particleEmitterList.Add(emitter);


            List<Rectangle> WalkingRects = new List<Rectangle>()
            {
                new Rectangle(154,0,78,frameH),
                new Rectangle(232,0,60,frameH),
                new Rectangle(292,0,70,frameH),              
            };
            List<Rectangle> PunchRects = new List<Rectangle>()
            {
                new Rectangle(232,0,60,frameH),
                new Rectangle(72,0,82,frameH),
            };
            //punching animation
            Animations.Sprite.AddAnimation(MF_PlayerStates.PunchRH.ToString(), PunchRects, 0.5f);
            Animations.Sprite.AddAnimation(MF_PlayerStates.PunchLH.ToString(), PunchRects, 0.5f, true);

            //walking animation
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkR.ToString(), WalkingRects, 0.5f);
            Animations.Sprite.AddAnimation(MF_PlayerStates.WalkL.ToString(), WalkingRects, 0.5f, true);

            //idle animation
            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleL.ToString(), 232, 0, 60, frameH, 1, 0.5f, true);
            Animations.Sprite.AddAnimation(MF_PlayerStates.IdleR.ToString(), 232, 0, 60, frameH, 1, 0.5f);
            
            //dead animation
            Animations.Sprite.AddAnimation(MF_PlayerStates.Dead.ToString(), 0, 40, 72, 38, 1, 0.5f);

            Animations.Sprite.CurrentAnimation = MF_PlayerStates.Idle.ToString();

            Animations.Position = new Vector2(x, y);
            Animations.Sprite.AutoRotate = false;
            Animations.IsPathing = false;
            Animations.IsMoving = false;
            CurrentState = MF_PlayerStates.IdleL;
           
            Position = new Vector2(x, y);
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        #endregion

        #region Update
        protected override void ResetState()
        {
            //set default properties
            CurrentLevel.LevelGravity = 1.38f;

        }
       
        /// <summary>
        /// Initialize Blue man state based on certain events
        /// </summary>
        protected override void HandleState()
        {
            if (this.CurrentState == MF_PlayerStates.Dead)
                return;


            float distanceFromPlayer = 1000;
            Random rand = new Random();

            //if player is on the left
            if (Player.Bounds.Width <= Bounds.X)
            {
                distanceFromPlayer = Math.Abs(Player.Bounds.Width - Bounds.X);

                //if we are not touching the player
                if (distanceFromPlayer >= 10)
                    this.CurrentState = MF_PlayerStates.WalkL;
                else if(Player.BoundingBox.Intersects(BoundingBox))
                {
                    this.CurrentState = MF_PlayerStates.IdleL;
                    /*
                    //ok we can punch the player but should we?
                    double randnum = 1.5f + rand.NextDouble() * (3 - 1.5f);
                    //min + (random.NextDouble() * (max - min));

                    if (PunchingLapsedSeconds > randnum || PunchingLapsedSeconds == -1)//always punch the first time
                    {
                        PunchingLapsedSeconds = 0;

                        if(this.CurrentState != MF_PlayerStates.PunchLH)
                            this.CurrentState = MF_PlayerStates.PunchLH;
                    }
                   else
                       this.CurrentState = MF_PlayerStates.IdleL;
                     * */
                }
                else
                    this.CurrentState = MF_PlayerStates.PunchLH;


            }
            else if (Player.Bounds.X >= Bounds.X)//if player is on the right
            {
                distanceFromPlayer = Math.Abs(Bounds.Width - Player.Bounds.X);

                //if we are not touching the player
                if (distanceFromPlayer >= 27)
                    this.CurrentState = MF_PlayerStates.WalkR;
                else if (Player.BoundingBox.Intersects(BoundingBox))
                {

                    this.CurrentState = MF_PlayerStates.IdleR;
                    
                    /*
                    //ok we can punch the player but should we?
                    double randdiv = (rand.Next(100, 200));
                    double randnum = rand.Next(1, 100);
                    double r = randnum / randdiv;

                    if (PunchingLapsedSeconds >= r ||
                        PunchingLapsedSeconds == -1)//always punch the first time
                    {
                        PunchingLapsedSeconds = 0;
                        this.CurrentState = MF_PlayerStates.PunchRH;
                    }*/
                }
                else
                    this.CurrentState = MF_PlayerStates.PunchRH;

            }

            if (distanceFromPlayer >= Size.X + 200)
             // set the idle state we are, waiting for the player
            {
                //which direction to set the idle state
                if (Player.Position.X < this.Position.X)
                    this.CurrentState = MF_PlayerStates.IdleL;
                else
                    this.CurrentState = MF_PlayerStates.IdleR;
            }

        }
        /// <summary>
        /// Updates Bluman properties based on the current state
        /// </summary>
        protected override void UpdateState(GameTime gameTime)
        {
            if (PunchingLapsedSeconds >= 0)
                PunchingLapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            
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
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleL, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.IdleR:
                    Set(new Vector2(this.Direction.X, MOVE_NONE), MF_PlayerStates.IdleR, new Vector2(0, 0));
                    break;
                    //punching
                case MF_PlayerStates.PunchLH:
                    Set(new Vector2(this.Direction.X, this.Direction.Y), MF_PlayerStates.PunchLH, new Vector2(0, 0));
                    break;
                case MF_PlayerStates.PunchRH:
                    Set(new Vector2(this.Direction.X, this.Direction.Y), MF_PlayerStates.PunchRH, new Vector2(0, 0));
                    break;
                    //dead
                case MF_PlayerStates.Dead:
                    DeadLapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;
                    if (DeadLapsedSeconds >= 2)
                         Visible = false;

                    Set(new Vector2(MOVE_NONE, MOVE_NONE), MF_PlayerStates.Dead, new Vector2(0, 0));
                    break;
            }
            emitter.Position = new Vector2(Position.X + 35, Position.Y + 22);
            particleComponent.Update(gameTime);
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
                    switch (CurrentState)
                    {
                        case MF_PlayerStates.WalkL:
                            this.CurrentState = MF_PlayerStates.IdleL;
                            Position += new Vector2(20, 0);
                            break;
                        case MF_PlayerStates.WalkR:
                            this.CurrentState = MF_PlayerStates.IdleR;
                            Position += new Vector2(-20, 0);
                            break;
                    }
                    this.Position += new Vector2((Size.X + 10) * -Direction.X, 0);
                    HP -= 34;
                    
                    //have we kill blueman yet
                    if (HP <= 0)
                    {
                        DeadLapsedSeconds = 0;
                        CurrentState = MF_PlayerStates.Dead;
                        if (GetSetScore <= 100)
                        {
                            Player.GetSetScore += 5;
                            if (GetSetScore >= 100)
                            {
                                GetSetScore = 100;
                            }
                        }
                        
                    }
                }
            }
        }
        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            if(CurrentState == MF_PlayerStates.Dead)
                particleComponent.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion
                      
    }    
}
