using System;

namespace MagicFighters.TileEngine.Model

{
     public class MF_TileVector2
     {
          public float X { get; set; }
          public float Y { get; set; }
     }
     public class Rect
     {
          public MF_TileVector2 Min { get; set; }
          public MF_TileVector2 Max { get; set; }
     }
     public class Sphere
     {
          public int Radious { get; set; }
          public MF_TileVector2 Center { get; set; }
     }
     public class TileAnimation
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

          public bool isSelected { get; set; }
          public int[] ImagesID { get; set; }
          public int soundIndex { get; set; }

          public bool hasAnimation { get; set; }
          public bool hasCollision { get; set; }
          public bool hasSound { get; set; }
          public bool hasAction { get; set; }

          public bool hasCollisonRectangle { get; set; }
          public bool hasCollisonSphere { get; set; }
          public bool hasCollisonPoints { get; set; }

          public Rect CollisonRectangle { get; set; }
          public Sphere CollisonSphere { get; set; }
          public MF_TileVector2[] CollisionPoints { get; set; }

          public TileAnimation[] Animations { get; set; }


          public MF_TileVector2 Position { get; set; }

          public MF_TileVector2 Speed { get; set; }

     }
}
