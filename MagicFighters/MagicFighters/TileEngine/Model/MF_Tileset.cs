using System;

namespace MagicFighters.Model
{
     public class MF_Tileset : MF_TileBase
     {
          public MF_Tile[] Tiles { get; set; }

          /// <summary>
          /// Global Width information
          /// </summary>
          public float Width { get; set; }
          /// <summary>
          /// Global Height information
          /// </summary>
          public float Height { get; set; }
         /// <summary>
         /// Global offset information
         /// </summary>
          public MF_Rect Offset { get; set; }
     }
}
