// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Line.cs
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
using Microsoft.Xna.Framework.Graphics;

namespace MagicFighters.TileEngine.Model
{
    class MF_Line
    {

        private Vector2 origin;
        private Vector2 scale;
        private float rotation;
        Vector2 xVector = new Vector2(1, 0);
        private Texture2D lineTexture;

        public MF_Line()
        {
        }

        public MF_Line(int thickness, Color color)
        {
            this.color = color;
            this.thickness = thickness;
        }

        private int thickness = 1;
        public int Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        private Color color = Color.Black;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private float layer = 0;
        public float Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            lineTexture = MF_Physics.CreateLineTexture(graphicsDevice, thickness, color);
            origin = new Vector2(0, thickness / 2f + 1);
        }

        Vector2 difference;
        public void Draw(SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint)
        {
            Vector2.Subtract(ref endPoint, ref startPoint, out difference);
            CalculateRotation(difference);
            CalculateScale(difference);
            spriteBatch.Draw(lineTexture, startPoint, null, color, rotation, origin, scale, SpriteEffects.None, layer);
        }

        Vector2 normalizedDifference = Vector2.Zero;
        float theta = 0;
        private void CalculateRotation(Vector2 difference)
        {
            Vector2.Normalize(ref difference, out normalizedDifference);
            Vector2.Dot(ref xVector, ref normalizedDifference, out theta);

            theta = (float)Math.Acos(theta);
            if (difference.Y < 0) { theta = -theta; }
            rotation = theta;
        }

        private void CalculateScale(Vector2 difference)
        {
            float desiredLength;
            desiredLength = difference.Length();
            scale.X = desiredLength / lineTexture.Width;
            scale.Y = 1;
        }

        //test
        /*
          LineBrush lineBrush;
        Texture2D circleTexture;
        Vector2 point1, point2, point3, point4, intersectionPoint;
        bool intersection, coincident;
          point1 = Vector2.Zero;
            point2 = new Vector2(800, 600);
            point3 = new Vector2(0, 300);
            point4 = new Vector2(800, 300);
         *  lineBrush = new LineBrush(1, Color.White);
            lineBrush.Load(GraphicsDevice);
            circleTexture = DrawingHelper.CreateCircleTexture(GraphicsDevice, 5, Color.White);
         * 
         * 
         * lineBrush.Draw(spriteBatch, point1, point2);
            lineBrush.Draw(spriteBatch, point3, point4);

            Vector2 circleOrigin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
            spriteBatch.Draw(circleTexture, point1, null, Color.Red, 0, circleOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(circleTexture, point2, null, Color.Red, 0, circleOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(circleTexture, point3, null, Color.Red, 0, circleOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(circleTexture, point4, null, Color.Red, 0, circleOrigin, 1, SpriteEffects.None, 0);
            if (intersection)
            {
                spriteBatch.Draw(circleTexture, intersectionPoint, null, coincident ? Color.Green : Color.Yellow, 0, circleOrigin, 1, SpriteEffects.None, 0);
            }
         * 
         * 
         * private void ProcessIntersection()
        {
            float ua = (point4.X - point3.X) * (point1.Y - point3.Y) - (point4.Y - point3.Y) * (point1.X - point3.X);
            float ub = (point2.X - point1.X) * (point1.Y - point3.Y) - (point2.Y - point1.Y) * (point1.X - point3.X);
            float denominator = (point4.Y - point3.Y) * (point2.X - point1.X) - (point4.X - point3.X) * (point2.Y - point1.Y);

            intersection = coincident = false;

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    intersection = coincident = true;
                    intersectionPoint = (point1 + point2) / 2;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersection = true;
                    intersectionPoint.X = point1.X + ua * (point2.X - point1.X);
                    intersectionPoint.Y = point1.Y + ua * (point2.Y - point1.Y);
                }
            }
        }
         */

    }
}
