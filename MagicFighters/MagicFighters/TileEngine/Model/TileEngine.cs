using System;
using Polenter.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Model;
using MagicFighters.View;

namespace MagicFighters.TileEngine
{
    public class MF_TileEngine
    {
        #region Constructor
        public MF_TileEngine()
        {
        }
        #endregion//Constructor

        #region Properties


        #endregion//Properties

        #region Functions

        /// <summary>
        /// Returns a TileMap from a tilemap.mfm file, null otherwise
        /// </summary>
        /// <param name="tilemapFile">the location of the tilemap including the extention</param>
        /// <returns>TileMap</returns>
        //public TileMap Load(string tilemapFile)
        //{
        //    if (String.IsNullOrEmpty(tilemapFile))
        //        return null;


        //    var serializer = new SharpSerializer(true);

        //    if (System.IO.File.Exists(tilemapFile))
        //    {
        //        var stream = new FileStream(tilemapFile, FileMode.Open, FileAccess.Read);
        //        MF_TileMap tilemap = (MF_TileMap)serializer.Deserialize(stream);

        //        //Make sure we have a valid tilemap from the file
        //        if (tilemap == null)
        //            return null;

        //        TileMap tmp = new TileMap();
        //        tmp.TileWidth = (int)tilemap.Tileset.Width;
        //        tmp.TileHeight = (int)tilemap.Tileset.Height;

        //        // tmp.camera.Pos = Vector2.Zero;

        //        tmp.TilesWide = tilemap.Tiles.GetLength(1);
        //        tmp.TilesHigh = tilemap.Tiles.GetLength(0);

        //        tmp.BottomMargin = (int)tilemap.Tileset.Height * tmp.TilesHigh;
        //        tmp.RightMargin = (int)tilemap.Tileset.Width * tmp.TilesWide;

        //        if (tilemap.PlayerStartPosition != null)
        //        {
        //            tmp.PlayerStartPosition = new Vector2(tilemap.PlayerStartPosition.X - tmp.TileWidth, tilemap.PlayerStartPosition.Y);
        //        }
        //        if (tilemap.hasBackgroundImage && tilemap.BackgroundImageNames != null && tilemap.BackgroundImageNames.Length > 0)
        //            tmp.BackgroundName = tilemap.BackgroundImageNames[0];

        //        tmp.map = tilemap.Tiles;
        //        tmp.TileSet = tilemap.Tileset;
        //        return tmp;
        //    }
        //    return null;
        //}
        #endregion//Functions
    }
}
