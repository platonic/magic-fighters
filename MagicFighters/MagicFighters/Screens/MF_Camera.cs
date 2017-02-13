// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Camera.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MagicFighters.Screens
{
    public class MF_Camera
    {
        public Matrix viewMatrix;
        private Vector2 Position;
        public Viewport viewport;

        public MF_Camera(Rectangle clientRect)
        {
            viewport = new Viewport((int)Position.X, (int)Position.Y, clientRect.Right, clientRect.Bottom);
            UpdateViewMatrix();
        }

        public Vector2 Pos
        {
            get
            {
                return Position;
            }

            set
            {
                Position = value;
                UpdateViewMatrix();
            }
        }
        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
        }
        //public void CamRestrict()
        //{
        //if(cam.Position.X worldBoundry.Width)
        //cam.Position.X = worldBoundry.Width;
        //if(cam.Position.Y worldBoundry.Height)
        //            {cam.Position.Y = worldBoundry.Height)
        //}

        //}
        private void UpdateViewMatrix()
        {
            viewMatrix = Matrix.CreateTranslation(viewport.X - Position.X, viewport.Y - Position.Y, 0.0f);
        }
    }
}
