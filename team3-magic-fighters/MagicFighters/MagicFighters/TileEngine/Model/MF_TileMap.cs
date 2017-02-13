using System;

namespace MagicFighters.TileEngine.Model
{
     public class MF_TileMap : MF_TileBase
     {
          public bool hasMultipleLayers { get; set; }
          public int CurrentLayerIndex { get; set; }
          public MF_TileMap[] Layers { get; set; }

          public int Width { get; set; }
          public int Height { get; set; }

          public bool hasBackgroundImage { get; set; }
          public int[] BackgroundImageIndex { get; set; }

          public bool isBackgroundImageStreched { get; set; }
          public bool isBackgroundImageTiledX { get; set; }
          public bool isBackgroundImageTiledY { get; set; }

          public bool hasTiles { get; set; }
          public MF_Tile[] Tiles { get; set; }

          //Holds the list of tiles including the actual textures information
          public MF_Tileset Tileset { get; set; }
     }
}
