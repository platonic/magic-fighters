using System;

namespace MagicFighters.TileEngine.Model
{
     public class MF_Tileset : MF_TileBase
     {
          public MF_Tile[] Tiles { get; set; }

          /// <summary>
          /// Global Width information
          /// </summary>
          public int Width { get; set; }
          /// <summary>
          /// Global Height information
          /// </summary>
          public int Height { get; set; }
     }
}
