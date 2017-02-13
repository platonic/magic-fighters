// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//Player.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion

using System;
using MagicFighters.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MagicFighters.ViewModel;
using System.Collections.Generic;
using MagicFighters.Players.View;
using MagicFighters.TileEngine.Model;
using MagicFighters.GameStates;


namespace MagicFighters.View
{
    abstract class Player : MF_Object
    {
        #region Fields
        //-------------------------------------------------------------------------------------------
        //Moving information
        protected bool isWalkingLeft = false;
        protected bool isWalkingRight = false;
        protected bool isWalkingUp = false;
        protected bool isWalkingDown = false;
        protected bool isColliding = false;
        //-------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------
        /// <summary>
        /// Player's texture for drawing or animation
        /// </summary>
        protected Texture2D Texture;

        /// <summary>
        /// Any effect that needs to be applied to the texture
        /// </summary>
        protected SpriteEffects spriteEffects;
        #endregion

        #region Properties

        public float GetSetHP
        {
            get { return HP; }
            set
            {
                HP = value;
            }
        }

        public float GetSetScore
        {
            get { return Score; }
            set
            {
                Score = value;
            }
        }
        public MF_PlayerStates GetSetCurrentState
        {
            get { return CurrentState; }
            set
            {
                CurrentState = value;
            }
        }
        public bool isJumpState
        {
            get
            {
                return (CurrentState == MF_PlayerStates.Jump ||
                    CurrentState == MF_PlayerStates.JumpL ||
                    CurrentState == MF_PlayerStates.JumpR ||
                    CurrentState == MF_PlayerStates.JumpFW ||
                    CurrentState == MF_PlayerStates.JumpUp ||
                    CurrentState == MF_PlayerStates.JumpUpL ||
                    CurrentState == MF_PlayerStates.JumpUpR ||
                    CurrentState == MF_PlayerStates.JumpDown ||
                    CurrentState == MF_PlayerStates.JumpDownL ||
                    CurrentState == MF_PlayerStates.JumpDownR ||
                    CurrentState == MF_PlayerStates.JumpBW);
            }
        }

        public bool isHitState
        {
            get
            {
                return(CurrentState == MF_PlayerStates.Hit ||
                    CurrentState == MF_PlayerStates.HitL ||
                    CurrentState == MF_PlayerStates.HitR ||
                    CurrentState == MF_PlayerStates.PunchLH ||
                    CurrentState == MF_PlayerStates.PunchRH ||
                    CurrentState == MF_PlayerStates.Electrocute ||
                    CurrentState == MF_PlayerStates.ElectrocuteL ||
                    CurrentState == MF_PlayerStates.ElectrocuteR ||
                    CurrentState == MF_PlayerStates.Fireball ||
                    CurrentState == MF_PlayerStates.FireballL ||
                    CurrentState == MF_PlayerStates.FireballR);
            }
        }
        //-------------------------------------------------------------------------------------------
        //Menu bar information
        public MF_HudBar MPBar { get; set; }
        public MF_HudBar HPBar { get; set; }
        public MF_HudBar ScoreBar { get; set; }
        //-------------------------------------------------------------------------------------------

        public MF_PlayerStates GetState { get { return CurrentState; } }
        protected List<StaticItem> Items { get; set; }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X, 
                    (int)Position.Y, 
                    (int)Position.X + (int)Size.X, 
                    (int)Position.Y + (int)Size.Y);
            }
            
        }
        #endregion

        /// <summary>
        /// checks if there is collision with an item in the List of StaticItem Items
        /// </summary>
        /// <returns>the item the player is colliding with, null otherwise</returns>
        protected StaticItem CollidedWithItem()
        {
            if (Items != null && Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    //we meant to remove this item 
                    if (!Items[i].Enabled)
                    {
                        Items.RemoveAt(i);
                        continue;
                    }
                    if (!Items[i].Visible || Items[i].Rectangle == null)
                    {
                        continue;
                    }


                    if (Items[i].BoundingBox.Intersects(GetPlayerBoundingBox()))
                    {
                        return Items[i];
                    }
                }
            }
            return null;
        }


        public CorrectionVector2 GetCorrectionVector(Player target)
        {
            CorrectionVector2 ret = new CorrectionVector2();

            float x1 = Math.Abs(Bounds.Right - target.Bounds.Left);
            float x2 = Math.Abs(Bounds.Left - target.Bounds.Right);
            float y1 = Math.Abs(Bounds.Bottom - target.Bounds.Top);
            float y2 = Math.Abs(Bounds.Top - target.Bounds.Bottom);

            // calculate displacement along X-axis
            if (x1 < x2)
            {
                ret.X = x1;
                ret.DirectionX = DirectionX.Left;
            }
            else if (x1 > x2)
            {
                ret.X = x2;
                ret.DirectionX = DirectionX.Right;
            }

            // calculate displacement along Y-axis
            if (y1 < y2)
            {
                ret.Y = y1;
                ret.DirectionY = DirectionY.Up;
            }
            else if (y1 > y2)
            {
                ret.Y = y2;
                ret.DirectionY = DirectionY.Down;
            }

            return ret;
        }

        private void HandleSlopeCollisions(GameTime gameTime)
        {
            //collisionPaths = level.collisionPaths;
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (velocity.Y >= 0)
            //{
            //    if (attachedPathArray.size() > 0)
            //    {
            //        isOnGround = true;

            //        for (unsigned int i = 0; i < attachedPathArray.size(); i++) // convert to foreach
            //        {
            //            position.Y = attachedPathArray[i].InterpolateY(position.X);
            //            velocity.Y = 0;

            //            if (position.X < attachedPathArray[i].MinimumX || position.X > attachedPathArray[i].MaximumX)
            //            {
            //                attachedPathArray.remove(i);
            //                --i; // otherwise we would skip a path because the size just decreased
            //            }              
            //        }            
            //    }
            //    else
            //    {
            //        Vector2 footPosition = position;
            //        Vector2 expectedFootPosition = footPosition + velocity * elapsed;

            //        CollisionPath landablePath = null;
            //        float landablePosition = float.MaxValue;

            //        foreach (CollisionPath path in collisionPaths)
            //        {
            //            if (expectedFootPosition.X >= path.MinimumX && expectedFootPosition.X <= path.MaximumX)
            //            {
            //                float pathOldY = path.InterpolateY(footPosition.X);
            //                float pathNewY = path.InterpolateY(expectedFootPosition.X);

            //                if (footPosition.Y <= pathOldY && expectedFootPosition.Y >= pathNewY && pathNewY < landablePosition)
            //                {
            //                    landablePathArray.Add(path);
            //                    landablePosition = pathNewY;
            //                }
            //            }
            //        }

            //        if (landablePathArray.size() > 0)
            //        {
            //            velocity.Y = 0;
            //            footPosition.Y = landablePosition;
            //            attachedPath = landablePathArray[?]; // which one would you select in the array? up to you, I can't tell from your code


            //            position.Y = footPosition.Y;
            //        }
            //    }
            //}
            //else
            //{
            //    attachedPath = null;
            //}
        }
        public Rectangle GetRectangle
        {
            get
            {
                if (Rectangle == null)
                    throw new Exception("No rectangle initilized.");

                return Rectangle.Value;

            }
        }

        #region Initialization

        public Player(Game game, SpriteBatch screenSpriteBatch,
            TileMap currentlevel, string IdleTexture,
            Vector2 Position, SpriteEffects SpriteEffect)
            : base(game, currentlevel, screenSpriteBatch)
        {
            spriteEffects = SpriteEffect;
        }


        public Player(Game game, TileMap level, SpriteBatch screenSpriteBatch)
            : base(game, level, screenSpriteBatch)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion


        protected abstract void HandleDammage();

        protected abstract void HandleState();
        protected abstract void UpdateState(GameTime gameTime);
        protected abstract void ResetState();


        protected void HandleCollision(bool usePerPixel = false)
        {
            isColliding = false;

            if (usePerPixel)
            {
                Rectangle? rect = CurrentLevel.PerPixelObjecCollidedWithTile(this);

                if (rect != null)
                {
                    //Direction = new Vector2(MOVE_NONE, MOVE_NONE);
                    //Vector2 displacement =
                    //     Player.CalcualteMinimumTranslationDistance(GetPlayerRectangle(), rect.Value);
                    isColliding = isPlayerCollidingWithTheFloor = true;
                    // Position += displacement;
                    CurrentLevel.LevelGravity = 0;
                }


            }
            else
            {
                BoundingBox? b = CurrentLevel.GetFloorCollisionBounding(GetPlayerBoundingBox());
                if (b != null)
                {
                    // HandleCollisionReaction();

                    Rectangle r = new Rectangle(
                               (int)b.Value.Min.X,
                               (int)b.Value.Min.Y,
                               (int)b.Value.Max.X,
                               (int)b.Value.Max.Y);
                    HandleCollisionReaction(r);

                    //Vector2 displacement =
                    //  MF_Physics.CalcualteMinimumTranslationDistance(GetRectangle, floorRect);
                    //isColliding = isPlayerCollidingWithTheFloor = true;

                    //Position += displacement;
                    //CurrentLevel.LevelGravity = 0;
                }

            }
            {
                //---------------------------------------------------------------------------------------------
                //TODO: This code here until offset is fixed on perpixel collision
                //---------------------------------------------------------------------------------------------
                BoundingBox? b = CurrentLevel.GetTileCollisionBounding(this.GetPlayerBoundingBox());
                if (b != null)
                {
                    Rectangle r = new Rectangle(
                                  (int)b.Value.Min.X,
                                  (int)b.Value.Min.Y,
                                  (int)b.Value.Max.X,
                                  (int)b.Value.Max.Y);
                    HandleCollisionReaction(r);

                }
                //---------------------------------------------------------------------------------------------
            }
        }
        Vector2 CollisionPoint;
        void HandleCollisionReaction(Rectangle r)
        {
            //Position = CurrentLevel.LastCollisionVector;
            //isColliding = isPlayerCollidingWithTheFloor = true;
            //CurrentLevel.LevelGravity = 0;


            Vector2 displacement =
              MF_Physics.CalcualteMinimumTranslationDistance(GetRectangle, r);
            isColliding = isPlayerCollidingWithTheFloor = true;

            Position += displacement;
            CurrentLevel.LevelGravity = 0;

            //if (Direction.X == MOVE_LEFT)
            //{
            //}
            //else if (Direction.X == MOVE_RIGHT)
            //{
            //}

            //if (Direction.Y == MOVE_UP)
            //{
            //}
            //else if (Direction.Y == MOVE_DOWN)
            //{
            //    //Position -= new Vector2(0, -10);
            //}
        }
        /// <summary>
        /// update player's information based on the collected items
        /// </summary>
        void HandleItems()
        {
            var item = CollidedWithItem();

            if (item != null)
            {

                if (item.CurLoadedItem != null)
                {
                    //set the players hp and mp based on the item assigned values
                    HP += item.CurLoadedItem.HP;
                    MP += item.CurLoadedItem.MP;

                    //make sure the hp and the mp does not overflow
                    if (HP > 100) HP = 100;
                    if (MP > 100) MP = 100;
                    if (HP < 0) HP = 0;
                    if (MP < 0) MP = 0;
                }
                item.Collected();
            }
        }
        public override void Update(GameTime gameTime)
        {

            //---------------------------------------------------------------------------------------------
            //reset states
            //---------------------------------------------------------------------------------------------
            ResetState();


            //---------------------------------------------------------------------------------------------
            //Handle the player collision
            //---------------------------------------------------------------------------------------------
            HandleCollision();

            //---------------------------------------------------------------------------------------------
            //init states
            //---------------------------------------------------------------------------------------------
            HandleState();


            //---------------------------------------------------------------------------------------------
            //handle damage
            //---------------------------------------------------------------------------------------------
            HandleDammage();

            
            //---------------------------------------------------------------------------------------------
            //update player's information based on the collected items
            HandleItems();
            //---------------------------------------------------------------------------------------------


            //---------------------------------------------------------------------------------------------
            //update states
            //---------------------------------------------------------------------------------------------

            UpdateState(gameTime);

            

           


            //update hud bars
            if (HPBar != null)
            {
                if (this.GetType().BaseType == typeof(Enemy))
                    HPBar.Update((int)HP, this);
                else
                    HPBar.Update((int)HP);
            }
            if (MPBar != null)
            {
                if (this.GetType().BaseType == typeof(Enemy))
                    HPBar.Update((int)MP, this);
                else
                    MPBar.Update((int)MP);
            }
            if (ScoreBar != null)
            {
                if (this.GetType().BaseType == typeof(Enemy))
                    ScoreBar.Update((int)Score, this);
                else
                    ScoreBar.Update((int)Score);
            }


            //update location and animations
            UpdateAndAnimate(gameTime);



            base.Update(gameTime);
        }
        protected void UpdateAndAnimate(GameTime gameTime)
        {
            
                Position += Direction * Speed;// *(float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += new Vector2(0, CurrentLevel.LevelGravity);
           
            Animations.Update(gameTime);
        }
        protected void DrawAnimation()
        {
            if (Animations != null)
                Animations.Draw(spriteBatch);
        }

        /// <summary>
        /// Gets the player bounding box for collision
        /// </summary>
        /// <returns>Collision bounding box</returns>
        public BoundingBox GetPlayerBoundingBox()
        {
            if (Animations.Sprite.CurrentFrameAnimation != null)
            {
                //float x = Animations.Sprite.CurrentFrameAnimation.FrameRectangle.X;
                //float y = Animations.Sprite.CurrentFrameAnimation.FrameRectangle.X;
                float w = Animations.Sprite.CurrentFrameAnimation.FrameRectangle.Width;
                float h = Animations.Sprite.CurrentFrameAnimation.FrameRectangle.Height;

                return new BoundingBox(new Vector3(Position.X, Position.Y, 0),
                 new Vector3(Position.X + w, Position.Y + h, 0));
            }
            else

            return new BoundingBox(new Vector3(Position.X, Position.Y, 0),
              new Vector3(Position.X + Size.X, Position.Y + Size.Y, 0));
        }
        /// <summary>
        /// Gets the vector position of the tile's location for the current player position
        /// </summary>
        /// <returns>The vector position of the tile's location for the current player position</returns>
        public Vector2 GetPlayerScreenLocation()
        {
            if (CurrentLevel != null)
            {
                int x = (int)(Math.Abs(Position.X) / CurrentLevel.TileWidth) % CurrentLevel.Width;
                int y = (int)(Math.Abs(Position.Y) / CurrentLevel.TileHeight) % CurrentLevel.Height;

                return new Vector2(x, y);
            }
            return Vector2.Zero;
        }


    }//End Player Class
}
