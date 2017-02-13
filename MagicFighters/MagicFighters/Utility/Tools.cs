using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicFighters
{
    public enum DirectionY
    {
        Up = -1,
        None = 0,
        Down = 1
    }

    public enum DirectionX
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    public struct CorrectionVector2
    {
        public DirectionX DirectionX;
        public DirectionY DirectionY;
        public float X;
        public float Y;
    }
}
