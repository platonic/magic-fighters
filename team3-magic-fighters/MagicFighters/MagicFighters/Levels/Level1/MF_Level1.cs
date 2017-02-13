using System;
using Microsoft.Xna.Framework;

namespace MagicFighters.Levels.Level1
{
     internal class MF_Level1 : DrawableGameComponent
     {
          MagicFightersGame curGame = null;
          Vector2 CameraPosition = Vector2.Zero;
          #region Initialization
          public MF_Level1(Game game)
               : base(game)
          {
               curGame = (MagicFightersGame)game;
          }
          public override void Initialize()
          {
               CameraPosition = new Vector2();
          }
          #endregion
     }
}
