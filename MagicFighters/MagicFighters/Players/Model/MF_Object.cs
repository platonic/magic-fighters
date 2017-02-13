// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Object.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//Last Modified : 9/22/2012    By: Lisandro Martinez
//Last Modified : 9/26/2012    By: Lisandro Martinez
//Last Modified : 9/27/2012    By: Lisandro Martinez    Comments: Added the new state and organized class
//-----------------------------------------------------------------------------
#endregion
using System;
using MagicFighters.Players.Model;
using MagicFighters.GameStates;


namespace MagicFighters.Model
{
    abstract class MF_Object : Microsoft.Xna.Framework.DrawableGameComponent
    {

        #region Fields
        /// <summary>
        /// The size of the object
        /// </summary>
        private Microsoft.Xna.Framework.Vector2 _Size;
        /// <summary>
        /// The player direction
        /// </summary>
        protected Microsoft.Xna.Framework.Vector2 Direction;

        /// <summary>
        /// Holds all the player's animations
        /// </summary>
        protected MobileSprite Animations;

        /// <summary>
        /// Gets or set the player's position
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Position
        {
            get { return Animations.Position; }
            set { Animations.Position = value; }
        }
        /// <summary>
        /// Easyly manage animation states and player states
        /// </summary>
        protected MF_PlayerStates CurrentAnimationState
        {
            get { return CurrentState; }
            set
            {

                CurrentState = value;
                Animations.Sprite.CurrentAnimation = CurrentState.ToString();
            }
        }
        #endregion
        #region IMF_Player Properties

        //-----------------------------------------------------------------------------
        //gets moving properties for the direction
        //-----------------------------------------------------------------------------
        protected int MOVE_UP { get { return -1; } }
        protected int MOVE_DOWN { get { return 1; } }
        protected int MOVE_LEFT { get { return -1; } }
        protected int MOVE_RIGHT { get { return 1; } }
        protected int MOVE_NONE { get { return 0; } }
        protected int HIT_RIGHT { get { return 0; } }
        protected int HIT_LEFT { get { return -1; } }
        //-----------------------------------------------------------------------------


        //-----------------------------------------------------------------------------
        //Set helper functions
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Helper to set the object direction, current animation and speed
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="animation"></param>
        /// <param name="speed"></param>
        protected void Set(Microsoft.Xna.Framework.Vector2 direction,
            MF_PlayerStates animation,
            Microsoft.Xna.Framework.Vector2 speed)
        {
            this.Direction = direction;
            if(Animations.Sprite.CurrentAnimation != animation.ToString())
                Animations.Sprite.CurrentAnimation = animation.ToString();
            Speed = speed;
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// gets or set the current object state
        /// </summary>
        protected MF_PlayerStates CurrentState {get;set;}

        /// <summary>
        /// gets or set the current level
        /// </summary>
        protected MagicFighters.ViewModel.TileMap CurrentLevel  {get;set;}

        /// <summary>
        /// gets or set the Dialog pertaining to this object
        /// </summary>
        public MF_Dialogue Dialog { get; set; }

        /// <summary>
        /// Defines the PlayerType
        /// </summary>
        protected PlayerType playerType { get; set; }
        /// <summary>
        /// Defines the PlayerType
        /// </summary>
        protected AIType aiType { get; set; }
        
        
        ///// <summary>
        ///// Defines if the player is alive
        ///// </summary>
        //protected bool IsAlive { get; set; }
        ///// <summary>
        ///// Defines if the player is active
        ///// </summary>
        //protected bool IsActive { get; set; }
        
        /// <summary>
        /// Player's Magic Points
        /// Decreases when a skill is used
        /// </summary>
        protected float MP { get; set; }
        /// <summary>
        /// Player's Hit Points
        /// Decrease every time the player receives damage
        /// </summary>
        protected float HP { get; set; }
        /// <summary>
        /// Player scores when an enemy is defeated
        /// </summary>
        protected float Score { get; set; }
        /// <summary>
        /// Current game
        /// </summary>
        protected MagicFightersGame curGame {get;set;}
        /// <summary>
        /// Drawing helper property
        /// </summary>
        protected Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch {get;set;}
        /// <summary>
        /// How much pull will the player have when jumping
        /// </summary>
        protected float JumpGravity { get; set; }

        /// <summary>
        /// Defines if the player animation is running or not
        /// </summary>
        protected bool AnimationRunning { get; set; }

        /// <summary>
        /// Defines if the player is jumping
        /// </summary>
        protected bool isPlayerJumping { get; set; }

        /// <summary>
        /// Defines if the player is colliding with the floor
        /// </summary>
        protected bool isPlayerCollidingWithTheFloor { get; set; }

        /// <summary>
        /// The player speed
        /// </summary>
        protected Microsoft.Xna.Framework.Vector2 Speed { get; set; }

        /// <summary>
        /// Holds all the player's animations
        /// </summary>
        public MobileSprite GetAnimations { get { return Animations; } }

        /// <summary>
        /// The name of the player
        /// </summary>
        protected string Name { get; set; }
        

        /// <summary>
        /// Get a sensor ray on the left bottom of the player, for collision
        /// </summary>
        /// <returns></returns>
        protected Microsoft.Xna.Framework.Ray? LeftBottomSensor()
        {
            if (Position == null || Size == null)
                return null;
            float x = Position.X;
            float y = Position.Y + (int)Size.Y;

            return new Microsoft.Xna.Framework.Ray(
                new Microsoft.Xna.Framework.Vector3(x,y,0),new Microsoft.Xna.Framework.Vector3(0,800,0));
        }
        /// <summary>
        /// Get a sensor ray on the right bottom of the player, for collision
        /// </summary>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Ray? RightBottomSensor()
        {
            if (Position == null || Size == null)
                return null;
            float x = Position.X + Size.X;
            float y = Position.Y + (int)Size.Y;

            return new Microsoft.Xna.Framework.Ray(
                new Microsoft.Xna.Framework.Vector3(x, y, 0), new Microsoft.Xna.Framework.Vector3(0, 800, 0));
        }
        /// <summary>
        /// Get a sensor ray on the center bottom of the player, for collision
        /// </summary>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Ray? CenterBottomSensor()
        {
            if (Position == null || Size == null)
                return null;
            float x = Position.X + (Size.X * 0.5f);
            float y = Position.Y + (int)Size.Y;

            return new Microsoft.Xna.Framework.Ray(
                new Microsoft.Xna.Framework.Vector3(x, y, 0), new Microsoft.Xna.Framework.Vector3(0, y+800, 0));
        }

        /// <summary>
        /// gets the object Rectangle based on the position and the size
        /// </summary>
        public Microsoft.Xna.Framework.Rectangle? Rectangle
        {
            get
            {
                if (Position == null || Size == null)
                    return null;

                return new Microsoft.Xna.Framework.Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        /// <summary>
        /// gets the object BoundingBox based on the position and the size
        /// </summary>
        public Microsoft.Xna.Framework.BoundingBox BoundingBox
        {
            get
            {
                float x = Position.X;
                float y = Position.Y;
                float W = Size.X;
                float H = Size.Y;

                Microsoft.Xna.Framework.Vector3 min = new Microsoft.Xna.Framework.Vector3(x, y, 0);
                Microsoft.Xna.Framework.Vector3 max = new Microsoft.Xna.Framework.Vector3(x+W, y+H, 0);

                return new Microsoft.Xna.Framework.BoundingBox(min, max);
            }
        }

        /// <summary>
        /// gets or set the size of the player
        /// </summary>
        protected Microsoft.Xna.Framework.Vector2 Size 
        { 
            get 
            {
                if (Animations != null && Animations.Sprite != null &&
                    Animations.Sprite.CurrentFrameAnimation != null)
                {
                    return new Microsoft.Xna.Framework.Vector2(Animations.Sprite.CurrentFrameAnimation.FrameWidth,
                        Animations.Sprite.CurrentFrameAnimation.FrameHeight);
                }
                return _Size; 
            } 
            
            //set { _Size = value; } 
        }

        /// <summary>
        /// gets the size of the player based on the current animation rectangle size
        /// </summary>
        protected Microsoft.Xna.Framework.Vector2 CurrentAnimationSize 
        { 
            get 
            { 
                return new Microsoft.Xna.Framework.Vector2(Animations.Sprite.CurrentFrameAnimation.FrameWidth,
            Animations.Sprite.CurrentFrameAnimation.FrameHeight); 
            }  
        }

        /// <summary>
        /// gets the size of the player based on the current animation rectangle size
        /// </summary>
        protected Microsoft.Xna.Framework.Rectangle CurrentAnimationRectangle
        {
            get
            {
                return Animations.Sprite.CurrentFrameAnimation.FrameRectangle;
            }
        }

        /// <summary>
        /// checks if the object is withing the screen viewport
        /// </summary>
        /// <returns>true if the object x + suze.x is not withing the screen viewport.width + its x position false otherwise</returns>
        protected bool isObjectVisible()
        {
            if (CurrentLevel != null && CurrentLevel.Camera != null && Position != null)
            {
                if ((Position.X + Size.X) <= CurrentLevel.Camera.Pos.X || 
                    Position.X >= CurrentLevel.Camera.Pos.X + CurrentLevel.Camera.viewport.Width)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Initialization

        public MF_Object(Microsoft.Xna.Framework.Game game,
           MagicFighters.ViewModel.TileMap curLevel, 
            Microsoft.Xna.Framework.Graphics.SpriteBatch screenSpriteBatch)
            : base(game)
        {
            CurrentLevel = curLevel;
            curGame = (MagicFightersGame)game;
            spriteBatch = screenSpriteBatch;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        #region Update and Render
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
           base.Draw(gameTime);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
           base.Update(gameTime);
        }

        #endregion
    }
}
