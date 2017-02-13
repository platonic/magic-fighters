// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_Level1.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
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
