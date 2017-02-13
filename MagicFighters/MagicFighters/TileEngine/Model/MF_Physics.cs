using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace MagicFighters.TileEngine.Model
{
    public class MF_Physics
    {
        public class PointF
        {
            public static PointF Empty { get { return new PointF(0, 0); } }
            public PointF() { }
            public PointF(float x, float y) { X = x; Y = y; }
            public float X;
            public float Y;
        }
        public class Tile
        {
            public Tile() { }
            public Tile(Rectangle rect, Texture2D texture) { Texture = texture; Rect = rect; }
            public Tile(BoundingSphere sphere, Texture2D texture) { Texture = texture; Sphere = sphere; }
            public Tile(BoundingBox box, Texture2D texture) { Texture = texture; Box = box; }
            public Tile(Ray ray, Texture2D texture) { Texture = texture; Ray = ray; }
            public Tile(Vector2 position, Texture2D texture) { Texture = texture; Position = position; Rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
            public Ray? Ray { get; protected set; }
            public Rectangle? Rect { get; protected set; }
            public BoundingSphere? Sphere { get; protected set; }
            public BoundingBox? Box { get; protected set; }
            public Texture2D Texture { get; protected set; }
            public Vector2 Position { get; protected set; }
        }
        public static Texture2D CreateLineTexture(GraphicsDevice graphicsDevice, int lineThickness, Color color)
        {
            Texture2D texture2D = new Texture2D(graphicsDevice, 2, lineThickness + 2, false, SurfaceFormat.Color);

            //Texture2D texture2D = new Texture2D(graphicsDevice, 2, lineWidth + 2);
            int count = 2 * (lineThickness + 2);
            Color[] colorArray = new Color[count];
            colorArray[0] = Color.Transparent;
            colorArray[1] = Color.Transparent;

            for (int i = 0; i < count; i++)
            {
                int y = i / 2;
                colorArray[i] = color;
            }

            colorArray[count - 2] = Color.White;
            colorArray[count - 1] = Color.White;
            texture2D.SetData<Color>(colorArray);
            return texture2D;
        }


        static double MyEpsilon = 0.00001;

        private static float[] OverlapIntervals(float ub1, float ub2)
        {
            float l = Math.Min(ub1, ub2);
            float r = Math.Max(ub1, ub2);
            float A = Math.Max(0, l);
            float B = Math.Min(1, r);
            if (A > B) // no intersection
                return new float[] { };
            else if (A == B)
                return new float[] { A };
            else // if (A < B)
                return new float[] { A, B };
        }

        // IMPORTANT: a1 and a2 cannot be the same, e.g. a1--a2 is a true segment, not a point
        // b1/b2 may be the same (b1--b2 is a point)
        private static PointF[] OneD_Intersection(PointF a1, PointF a2, PointF b1, PointF b2)
        {
            //float ua1 = 0.0f; // by definition
            //float ua2 = 1.0f; // by definition
            float ub1, ub2;

            float denomx = a2.X - a1.X;
            float denomy = a2.Y - a1.Y;

            if (Math.Abs(denomx) > Math.Abs(denomy))
            {
                ub1 = (b1.X - a1.X) / denomx;
                ub2 = (b2.X - a1.X) / denomx;
            }
            else
            {
                ub1 = (b1.Y - a1.Y) / denomy;
                ub2 = (b2.Y - a1.Y) / denomy;
            }

            List<PointF> ret = new List<PointF>();
            float[] interval = OverlapIntervals(ub1, ub2);
            foreach (float f in interval)
            {
                float x = a2.X * f + a1.X * (1.0f - f);
                float y = a2.Y * f + a1.Y * (1.0f - f);
                PointF p = new PointF(x, y);
                ret.Add(p);
            }
            return ret.ToArray();
        }

        private static bool PointOnLine(PointF p, PointF a1, PointF a2)
        {
            float dummyU = 0.0f;
            double d = DistFromSeg(p, a1, a2, MyEpsilon, ref dummyU);
            return d < MyEpsilon;
        }

        private static double DistFromSeg(PointF p, PointF q0, PointF q1, double radius, ref float u)
        {
            // formula here:
            //http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
            // where x0,y0 = p
            //       x1,y1 = q0
            //       x2,y2 = q1
            double dx21 = q1.X - q0.X;
            double dy21 = q1.Y - q0.Y;
            double dx10 = q0.X - p.X;
            double dy10 = q0.Y - p.Y;
            double segLength = Math.Sqrt(dx21 * dx21 + dy21 * dy21);
            if (segLength < MyEpsilon)
                throw new Exception("Expected line segment, not point.");
            double num = Math.Abs(dx21 * dy10 - dx10 * dy21);
            double d = num / segLength;
            return d;
        }
        #region Static Method used by entire game for calculating seperation amount when two rects collide
        public static Vector2 CalcualteMinimumTranslationDistance(Rectangle Rect1, Rectangle Rect2)
        {
            Vector2 result = Vector2.Zero;
            float difference = 0.0f;
            float minimumTranslationDistance = 0.0f;  //The absolute minimum distance we'll need to separate our colliding object.  
            int axis = 0; // Axis stores the value of X or Y.  X = 0, Y = 1.  
            int side = 0; // Side stores the value of left (-1) or right (+1).  

            // Left  
            difference = Rect1.Right - Rect2.Left;
            minimumTranslationDistance = difference;
            axis = 0;
            side = -1;

            // Right  
            difference = Rect2.Right - Rect1.Left;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 0;
                side = 1;
            }

            // Down  
            difference = Rect1.Bottom - Rect2.Top;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 1;
                side = -1;
            }

            // Up  
            difference = Rect2.Bottom - Rect1.Top;
            if (difference < minimumTranslationDistance)
            {
                minimumTranslationDistance = difference;
                axis = 1;
                side = 1;
            }

            if (axis == 1) //Y Axis  
            {
                result.Y = (float)side * minimumTranslationDistance;
            }
            else
            {   //X Axis  
                result.X = (float)side * minimumTranslationDistance;
            }

            return result;
        }
        #endregion 
        // this is the general case. Really really general
        public static PointF[] Intersection(PointF a1, PointF a2, PointF b1, PointF b2)
        {
            if (a1.Equals(a2) && b1.Equals(b2))
            {
                // both "segments" are points, return either point
                if (a1.Equals(b1))
                    return new PointF[] { a1 };
                else // both "segments" are different points, return empty set
                    return new PointF[] { };
            }
            else if (b1.Equals(b2)) // b is a point, a is a segment
            {
                if (PointOnLine(b1, a1, a2))
                    return new PointF[] { b1 };
                else
                    return new PointF[] { };
            }
            else if (a1.Equals(a2)) // a is a point, b is a segment
            {
                if (PointOnLine(a1, b1, b2))
                    return new PointF[] { a1 };
                else
                    return new PointF[] { };
            }

            // at this point we know both a and b are actual segments

            float ua_t = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
            float ub_t = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
            float u_b = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);

            // Infinite lines intersect somewhere
            if (!(-MyEpsilon < u_b && u_b < MyEpsilon))   // e.g. u_b != 0.0
            {
                float ua = ua_t / u_b;
                float ub = ub_t / u_b;
                if (0.0f <= ua && ua <= 1.0f && 0.0f <= ub && ub <= 1.0f)
                {
                    // Intersection
                    return new PointF[] {
                    new PointF(a1.X + ua * (a2.X - a1.X),
                        a1.Y + ua * (a2.Y - a1.Y)) };
                }
                else
                {
                    // No Intersection
                    return new PointF[] { };
                }
            }
            else // lines (not just segments) are parallel or the same line
            {
                // Coincident
                // find the common overlapping section of the lines
                // first find the distance (squared) from one point (a1) to each point
                if ((-MyEpsilon < ua_t && ua_t < MyEpsilon)
                   || (-MyEpsilon < ub_t && ub_t < MyEpsilon))
                {
                    if (a1.Equals(a2)) // danger!
                        return OneD_Intersection(b1, b2, a1, a2);
                    else // safe
                        return OneD_Intersection(a1, a2, b1, b2);
                }
                else
                {

                    // Parallel
                    return new PointF[] { };
                }
            }
        }
        public static PointF FindLineIntersection(PointF start1, PointF end1, PointF start2, PointF end2)
        {
            float denom = ((end1.X - start1.X) * (end2.Y - start2.Y)) - ((end1.Y - start1.Y) * (end2.X - start2.X));

            //  AB & CD are parallel 
            if (denom == 0)
                return PointF.Empty;

            float numer = ((start1.Y - start2.Y) * (end2.X - start2.X)) - ((start1.X - start2.X) * (end2.Y - start2.Y));

            float r = numer / denom;

            float numer2 = ((start1.Y - start2.Y) * (end1.X - start1.X)) - ((start1.X - start2.X) * (end1.Y - start1.Y));

            float s = numer2 / denom;

            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return PointF.Empty;

            // Find intersection point
            PointF result = new PointF();
            result.X = start1.X + (r * (end1.X - start1.X));
            result.Y = start1.Y + (r * (end1.Y - start1.Y));

            return result;
        }

        float SignedTriangleArea(Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            return (pointA.X - pointC.X) * (pointC.Y - pointB.Y) - (pointC.Y - pointA.Y) * (pointB.X - pointC.X);
        }
        bool TestSegmentsIntersection(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
        {
            float a1 = SignedTriangleArea(pointA, pointB, pointD);
            float a2 = SignedTriangleArea(pointA, pointB, pointC);

            if (a1 * a2 < 0.0f)
            {
                float a3 = SignedTriangleArea(pointC, pointD, pointA);
                float a4 = a3 + a2 - a1;

                if (a3 * a4 < 0.0f)
                    return true;

                return false;
            }
            return false;

        }

        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream)
        {
            Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i != data.Length; ++i)
                data[i] = Color.FromNonPremultiplied(data[i].ToVector4());
            texture.SetData(data);
            return texture;

            /*
                // Load image using GDI because Texture2D.FromStream doesn't support BMP
                using (Image image = Image.FromStream(stream))
                {
                // Now create a MemoryStream which will be passed to Texture2D after converting to PNG internally
                using (MemoryStream ms = new MemoryStream())
                {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                texture = Texture2D.FromStream(_graphicsDevice, ms);
                }
                }
             */
        }

    }//class MF_Physics
}
