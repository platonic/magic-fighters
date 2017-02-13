// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_TileMap.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;

namespace MagicFighters.Model
{
    public class BackgroundImage : MF_TileBase
    {
        public bool isBackgroundImageStreched { get; set; }
        public bool isBackgroundImageTiledX { get; set; }
        public bool isBackgroundImageTiledY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public BackgroundImage()
        {
            isBackgroundImageStreched = false;
            isBackgroundImageTiledX = false;
            isBackgroundImageTiledY = false;
        }
    }
     public class MF_TileMap : MF_TileBase
     {
         public MF_TileMap()
         {

             Width = 0;
             Height = 0;
             CurrentLayerIndex = 0;
             TilesWide = 0;
             TilesHigh = 0;
             hasBackgroundImage = false;
            
             hasMultipleLayers = false;
             hasTiles = false;
             Tiles = null;
             Backgrounds = null;
             Layers = null;
             PlayerStartPosition = null;
             Tileset = null;
         }
          public bool hasMultipleLayers { get; set; }
          public int CurrentLayerIndex { get; set; }
          public MF_TileMap[] Layers { get; set; }

          public float Width { get; set; }
          public float Height { get; set; }

          public int TilesWide { get; set; }
          public int TilesHigh { get; set; }

          public bool hasBackgroundImage { get; set; }
          public BackgroundImage[] Backgrounds { get; set; }
                 
          public bool hasTiles { get; set; }
          public MF_Tile[,] Tiles { get; set; }
         // public MF_Tile[] PlayersList { get; set; }
          public MF_Tile[] Collision { get; set; }
          public MF_Tile[] CollidableTiles { get; set; }

          public MF_TileVector2 PlayerStartPosition { get; set; }

          //Holds the list of tiles including the actual textures information
          public MF_Tileset Tileset { get; set; }

          public MF_EditorPlayer[] Players { get; set; }
     }
}
