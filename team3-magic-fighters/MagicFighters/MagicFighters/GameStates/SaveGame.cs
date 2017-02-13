using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;


namespace MagicFighters.GameStates
{
    class SaveState
    {
        public string PlayerName;
        public Vector2 AvatarPosition;
        public int Level;
        public int Score;
    }
}
