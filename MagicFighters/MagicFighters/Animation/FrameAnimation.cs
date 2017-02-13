// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//FrameAnimation.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicFighters
{
    class FrameAnimation : ICloneable
    {
        // The first frame of the Animation.  We will calculate other
        // frames on the fly based on this frame.
        private Rectangle rectInitialFrame;

        private bool _Flip = false;
        public bool Flip
        {
            get { return _Flip; }
            set { _Flip = value; }
        }

        // Number of frames in the Animation
        private int iFrameCount = 1;


        // The frame currently being displayed. 
        // This value ranges from 0 to iFrameCount-1
        private int iCurrentFrame = 0;


        // Amount of time (in seconds) to display each frame
        private float fFrameLength = 0.2f;


        // Amount of time that has passed since we last animated
        private float fFrameTimer = 0.0f;


        // The number of times this animation has been played
        private int iPlayCount = 0;


        // The animation that should be played after this animation
        private string sNextAnimation = null;

        /// 
        /// The number of frames the animation contains
        /// 
        public int FrameCount
        {
            get { return iFrameCount; }
            set { iFrameCount = value; }
        }

        /// 
        /// The time (in seconds) to display each frame
        /// 
        public float FrameLength
        {
            get { return fFrameLength; }
            set { fFrameLength = value; }
        }

        /// 
        /// The frame number currently being displayed
        /// 
        public int CurrentFrame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount - 1); }
        }

        public int FrameWidth
        {
            get { return rectInitialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return rectInitialFrame.Height; }
        }

        public delegate void FrameAnimationEventHandler(object sender);

        public event FrameAnimationEventHandler OnAnimationEnded;
        
        /// 
        /// The rectangle associated with the current
        /// animation frame.
        /// 
        public Rectangle FrameRectangle
        {
            get
            {

                if(ProvidedRectangles != null)
                {
                    return ProvidedRectangles[ProvidedRectanglesIndex];
                }
                return new Rectangle(
                    rectInitialFrame.X + (rectInitialFrame.Width * iCurrentFrame),
                    rectInitialFrame.Y, rectInitialFrame.Width, rectInitialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return iPlayCount; }
            set { iPlayCount = value; }
        }

        public string NextAnimation
        {
            get { return sNextAnimation; }
            set { sNextAnimation = value; }
        }
        public FrameAnimation(List<Rectangle> providedrectangles,float framelength, bool flip = false)
        {
            if (providedrectangles == null)
                return;
            ProvidedRectangles = providedrectangles;
            fFrameLength = framelength;
            rectInitialFrame = providedrectangles[0];
            iFrameCount = providedrectangles.Count;
            Flip = flip;
        }

        public FrameAnimation(Rectangle FirstFrame, int Frames, bool flip = false)
        {
            rectInitialFrame = FirstFrame;
            iFrameCount = Frames;
            Flip = flip;
        }

        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, bool flip = false)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            Flip = flip;
        }

        public FrameAnimation(int X, int Y, int Width, int Height, int Frames, float FrameLength, bool flip = false)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            Flip = flip;
        }

        public FrameAnimation(int X, int Y,
            int Width, int Height, int Frames,
            float FrameLength, string strNextAnimation, bool flip = false)
        {
            rectInitialFrame = new Rectangle(X, Y, Width, Height);
            iFrameCount = Frames;
            fFrameLength = FrameLength;
            sNextAnimation = strNextAnimation;
            Flip = flip;
        }
        List<Rectangle> ProvidedRectangles;
        public int ProvidedRectanglesIndex = 0;
        public void Update(GameTime gameTime)
        {
            fFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (fFrameTimer > fFrameLength)
            {

                ProvidedRectanglesIndex++;
                ProvidedRectanglesIndex %= iFrameCount;

                fFrameTimer = 0.0f;
                iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;
                if (iCurrentFrame == 0)
                {
                    iPlayCount = (int)MathHelper.Min(iPlayCount + 1, int.MaxValue);
                    ProvidedRectanglesIndex = 0;
                    
                    if (OnAnimationEnded != null)
                    {
                        OnAnimationEnded(this);
                    }

                }

               
            }
            
        }

        object ICloneable.Clone()
        {
            return new FrameAnimation(this.rectInitialFrame.X, this.rectInitialFrame.Y,
                                      this.rectInitialFrame.Width, this.rectInitialFrame.Height,
                                      this.iFrameCount, this.fFrameLength, sNextAnimation);
        }
    }
}
