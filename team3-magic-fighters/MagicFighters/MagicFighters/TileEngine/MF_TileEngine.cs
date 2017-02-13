using System;
using MagicFighters.TileEngine.Model;
using Polenter.Serialization;
using System.IO;
using Microsoft.Xna.Framework;

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

          public MF_TileMap Load(string srs)
          {
               var serializer = new SharpSerializer(true);
               var stream = new FileStream(srs, FileMode.Open, FileAccess.Read);
               MF_TileMap tilemap = (MF_TileMap)serializer.Deserialize(stream);
               return tilemap;
          }

          public bool Draw(DrawableGameComponent level)
          {
               
               return true;
          }


          #endregion//Functions
     }
}
