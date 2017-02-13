using System;

namespace MagicFighters.Model
{
    public enum MF_TileType
    {
        Tile,

        CollisionChild, 
        
        Decal,
        
        CollisionBound,
        
        Animation,

        Player,

        Floor,
    }
    public class MF_TileVector2
    {
        public MF_TileVector2() { }
        public MF_TileVector2(int x, int y) { X = x; Y = y; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class MF_Size
    {
        public MF_Size() { }
        public MF_Size(int w, int h) { W = w; h = H; }
        public int W { get; set; }
        public int H { get; set; }
    }
    public class MF_Rect
    {
        public MF_Rect() { }
        public MF_Rect(int x, int y, int w, int h) { X = x; Y = y; W = w; H = h; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
    public class MF_Sphere
    {
        public MF_Sphere() { }
        public MF_Sphere(int r, int x, int y) { Radious = r; X = x; Y = y; }
        public int Radious { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

    }
    public class MF_TileAnimation
    {
        public int ImageID { get; set; }

        public MF_TileVector2 StartPosition { get; set; }
        public MF_TileVector2 EndPosition { get; set; }
        public MF_TileVector2 Speed { get; set; }


        /// <summary>
        /// If false will rotate only one time
        /// </summary>
        public bool RotateAllways { get; set; }
        public int Rotation { get; set; }

        /// <summary>
        /// If false will only play once
        /// </summary>
        public bool Repeat { get; set; }
    }

    public class MF_Tile : MF_TileBase
    {

        public MF_Tile()
        {

           // isCollisionBound = false;
           // isPlayer = false;
            isSelected = false;
            hasDecal = false;
            Decal = null;
            soundName = null;
            isVisible = true;

            hasAnimation = false;
            hasCollision = false;
            hasSound = false;
            hasAction = false;

            hasCollisonRectangle = false;
            hasCollisonSphere = false;
            hasCollisonPoints = false;

            //isFloor = false;

            //CollisonRectangle = null;
            //CollisonSphere = null;
            //CollisionPoints = null;

            Animations = null;


            Position = null;
            Offset = null;
            Size = null;

            Speed = null;
        }
        public MF_Tile(MF_Tile tile)
        {
            TileType = tile.TileType;

           // isCollisionChild = tile.isCollisionChild;
           // isCollisionBound = tile.isCollisionBound;
           // isPlayer = tile.isPlayer;

            Name = tile.Name;
            base.LifeSpan = tile.LifeSpan;

            isSelected = tile.isSelected;
            hasDecal = tile.hasDecal;
            
            //isDecal = tile.isDecal;
            Decal = tile.Decal;

            soundName = tile.soundName;

            isVisible = tile.isVisible;

            hasAnimation = tile.hasAnimation;
            hasCollision = tile.hasCollision;
            hasSound = tile.hasSound;
            hasAction = tile.hasAction;

            hasCollisonRectangle = tile.hasCollisonRectangle;
            hasCollisonSphere = tile.hasCollisonSphere;
            hasCollisonPoints = tile.hasCollisonPoints;

           // isFloor = tile.isFloor;

            //CollisonRectangle = tile.CollisonRectangle;
            //CollisonSphere = tile.CollisonSphere;
            //CollisionPoints = tile.CollisionPoints;

            Animations = tile.Animations;


            Position = tile.Position;
            Offset = tile.Offset;
            Size = tile.Size;

            Speed = tile.Speed;
        }
        public MF_TileType TileType { get; set; }
        //public bool isCollisionChild { get; set; }
        //public bool isCollisionBound { get; set; }
        //public bool isPlayer { get; set; }
        public bool isSelected { get; set; }
        public bool hasDecal { get; set; }
        //public bool isDecal { get; set; }
        //public bool isFloor { get; set; }
        public MF_Tile Decal { get; set; }
        public string soundName { get; set; }

        public bool hasAnimation { get; set; }
        public bool hasCollision { get; set; }
        public bool hasSound { get; set; }
        public bool hasAction { get; set; }

        public bool hasCollisonRectangle { get; set; }
        public bool hasCollisonSphere { get; set; }
        public bool hasCollisonPoints { get; set; }
        public float Rotation { get; set; }

        //public MF_Rect CollisonRectangle { get; set; }
        //public MF_Sphere CollisonSphere { get; set; }
        //public MF_TileVector2[] CollisionPoints { get; set; }

        public MF_TileAnimation[] Animations { get; set; }
        public MF_Collision[] Collisions { get; set; }


        public MF_TileVector2 Position { get; set; }
        public MF_Rect Offset { get; set; }
        public MF_Size Size { get; set; }

        public MF_TileVector2 Speed { get; set; }

    }
}
