using System;

namespace MagicFighters.Model
{
    
    public class MF_Collision
    {
        public enum MF_CollisionType
        {
            LINE,
            POINT,
            TRIANGLE,
            RECTANGLE,
            SPHERE,
        }
        MF_CollisionType Collisiontype { get; set; }
        public MF_Collision() { }
        public MF_Collision(MF_TileVector2 point, MF_TileVector2 OwnerPosition, float Rotation = 0
            , bool hasOwner = true, MF_TileVector2 Offset = null)
        {
            this.Offset = Offset;
            this.OwnerPosition = OwnerPosition;
            this.Rotation = Rotation;
            this.hasOwner = hasOwner;
            CollisionPoints = new MF_TileVector2[]
            {
                point,
            };
        }
        public MF_Collision(MF_TileVector2 point1, MF_TileVector2 point2, MF_TileVector2 OwnerPosition, float Rotation = 0
            , bool hasOwner = true, MF_TileVector2 Offset = null)
        {
            this.Offset = Offset;
            this.OwnerPosition = OwnerPosition;
            this.Rotation = Rotation;
            this.hasOwner = hasOwner;
            CollisionPoints = new MF_TileVector2[]
            {
                point1,
                point2,
            };
        }
        public MF_Collision(MF_TileVector2 point1, MF_TileVector2 point2, MF_TileVector2 point3, MF_TileVector2 OwnerPosition, float Rotation = 0
            , bool hasOwner = true, MF_TileVector2 Offset = null)
        {
            this.Offset = Offset;
            this.OwnerPosition = OwnerPosition;
            this.Rotation = Rotation;
            this.hasOwner = hasOwner;
            CollisionPoints = new MF_TileVector2[]
            {
                point1,
                point2,
                point3,
            };
        }
        /// <summary>
        /// LINE = 2POINTS
        /// POINT = 1POINT
        /// TRIANGLE = 3POINTS
        /// RECTANGLE = 4POINTS
        /// SHPERE = 2POINTS Point[0] = Center and Point[1].X = Radious
        /// </summary>
        public MF_TileVector2[] CollisionPoints { get; set; }
        public MF_TileVector2 OwnerPosition { get; set; }
        public MF_TileVector2 Offset { get; set; }
        public bool hasOwner { get; set; }
        public float Rotation { get; set; }

       
    }
}
