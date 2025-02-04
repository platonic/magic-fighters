﻿// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//Animation.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MagicFighters
{
    class Animation
    {
        #region Fields
        // Animation variables
        Texture2D animatedCharacter;
        Point sheetSize;     
		Point currentFrame;
		public Point FrameSize { get; set; }
		public Vector2 Offset { get; set; }

        public int FrameCount
        {
            get { return sheetSize.X * sheetSize.Y; }
        }


        /// <summary>
        /// Returns or sets the current animation frame.
        /// </summary>
        public int FrameIndex
        {
            get
            {
                return sheetSize.X * currentFrame.Y + currentFrame.X;
            }
            set
            {
                if (value >= sheetSize.X * sheetSize.Y + 1)
                {
                    throw new InvalidOperationException(
                        "Specified frame index exeeds available frames");
                }

                currentFrame.Y = value / sheetSize.X;
                currentFrame.X = value % sheetSize.X;
            }
        }

        public bool IsActive { get; private set; }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor of an animation class
        /// </summary>
        /// <param name="frameSheet">Texture with animation frames sheet</param>
        /// <param name="size">Single frame size</param>
        /// <param name="frameSheetSize">The whole frame sheet size</param>
        /// <param name="interval">Interval between progressing to the next frame</param>
		public Animation(Texture2D frameSheet, Point size, Point frameSheetSize)
		{
			animatedCharacter = frameSheet;
			FrameSize = size;
			sheetSize = frameSheetSize;
			Offset = Vector2.Zero;
		}
        #endregion

        #region Update and Render
        /// <summary>
        /// Updates the animaton progress
        /// </summary>
        public void Update()
        {
            if (IsActive)
            {
                if (FrameIndex >= FrameCount - 1)
                {                  
                    IsActive = false;
                    FrameIndex = FrameCount - 1; // Stop at last frame 
                }
                else
                {
                    // Remember that updating "currentFrame" will also
                    // update the FrameIndex property.

                    currentFrame.X++;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        currentFrame.Y++;
                    }
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }             
            }           
        }

        /// <summary>
        /// Rendering of the animation
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch in which current 
        /// frame will be rendered</param>
        /// <param name="position">The position of current frame</param>
        /// <param name="spriteEffect">SpriteEffect to apply on 
        /// current frame</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffect)
        {
            Draw(spriteBatch, position, 1.0f, spriteEffect);
        }

        /// <summary>
        /// Rendering of the animation
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch in which current frame
        /// will be rendered</param>
        /// <param name="position">The position of the current frame</param>
        /// <param name="scale">Scale factor to apply on the current frame</param>
        /// <param name="spriteEffect">SpriteEffect to apply on the 
        /// current frame</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
			spriteBatch.Draw(animatedCharacter, position + Offset, new Rectangle(
					FrameSize.X * currentFrame.X,
					FrameSize.Y * currentFrame.Y,
					FrameSize.X,
					FrameSize.Y),
                  Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0);
        }

        /// <summary>
        /// Causes the animation to start playing from a specified frame index
        /// </summary>
        /// <param name="frameIndex"></param>
        public void PlayFromFrameIndex(int frameIndex)
        {
            FrameIndex = frameIndex;
            IsActive = true;
        }
        #endregion
    }
}
