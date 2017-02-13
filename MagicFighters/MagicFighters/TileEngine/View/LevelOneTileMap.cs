// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//LevelOneTileMap.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicFighters.ViewModel;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Screens;
using MagicFighters.Model;
using MagicFighters.Players.View;
using MagicFighters.View;
using System.Windows.Forms;

namespace MagicFighters.TileEngine
{
    class LevelOneTileMap : TileMap
    {
        MF_Ken Ken = null;

        public LevelOneTileMap(Game game, MF_Camera cam, SpriteBatch batch)
            : base(game, cam, batch) { }

        public override void Initialize()
        {
            LeftMargin = 0;
            TopMargin = 0;
            RightMargin = 0;
            BottomMargin = 0;

            camera.Pos = new Vector2(0, 0);
            tileTextures = new Dictionary<string, Texture2D>();
                       
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Players.Clear();

            if (!LoadMap("Level1Tileset/tilemap.mfm", "Level1Tileset"))
            {
                MessageBox.Show("Map could not be loaded.");
            }

            MF_BlueMan BlueMan = null;
            MF_BlueMan BlueMan1 = null;
            MF_BlueMan BlueMan2 = null;
            MF_BlueMan BlueMan3 = null;
            MF_BlueMan BlueMan4 = null;
            MF_BlueMan BlueMan5 = null;
            MF_BlueMan BlueMan6 = null;
            MF_BlueMan BlueMan7 = null;
            MF_BlueMan BlueMan8 = null;
            MF_BlueMan BlueMan9 = null;
            MF_Blanka Blanka = null;

            BlueMan = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan1 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan2 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan3 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan4 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan5 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan6 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan7 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan8 = new MF_BlueMan(curGame, this, spriteBatch);
            BlueMan9 = new MF_BlueMan(curGame, this, spriteBatch);

            Blanka = new MF_Blanka(curGame, this, spriteBatch);


            Ken = new MF_Ken(curGame, this, new List<Enemy>() { BlueMan, Blanka }, spriteBatch);
            BlueMan.Initialize(Ken);
            BlueMan1.Initialize(Ken);
            BlueMan2.Initialize(Ken);
            BlueMan3.Initialize(Ken);
            BlueMan4.Initialize(Ken);
            BlueMan5.Initialize(Ken);
            BlueMan6.Initialize(Ken);
            BlueMan7.Initialize(Ken);
            BlueMan8.Initialize(Ken);
            BlueMan9.Initialize(Ken);

            Blanka.Initialize(Ken);

            BlueMan1.Position = new Vector2(1879,257);
            BlueMan2.Position = new Vector2(2329, 257);
            BlueMan3.Position = new Vector2(3059, 349);
            BlueMan4.Position = new Vector2(5154, 363);
            BlueMan5.Position = new Vector2(7054, 415);
            BlueMan6.Position = new Vector2(7714, 415);
            BlueMan7.Position = new Vector2(8821, 234);
            BlueMan8.Position = new Vector2(9906, 415);
            BlueMan9.Position = new Vector2(11424, 257);
                                                                        
            foreach (var item in BotTypeItems)
                item.Initialize(Ken);

            Ken.Initialize();

            Players.Add(Blanka);
            Players.Add(BlueMan);
            Players.Add(BlueMan1);
            Players.Add(BlueMan2);
            Players.Add(BlueMan3);
            Players.Add(BlueMan4);
            Players.Add(BlueMan5);
            Players.Add(BlueMan6);
            Players.Add(BlueMan7);
            Players.Add(BlueMan8);
            Players.Add(BlueMan9);
            Players.Add(Ken);

            //Ken only cares about certain items type
            Ken.InitStaticItems((from w in this.MiscItems
                                 where w.CurLoadedItem.PlayerType == "CollectorType" ||
                                 w.CurLoadedItem.PlayerType == "ColectorType" ||        //TODO: TOFIX: workaround, correct for tile editor mistake
                                 w.CurLoadedItem.PlayerType == "VegetableType" ||
                                 w.CurLoadedItem.PlayerType == "ItemType"
                                 select w).ToList());

            //find Ken's position and reposition him
            if (tilemap.Players != null && tilemap.Players.Length > 0)
            {
                foreach (var p in tilemap.Players)
                {
                    if (p.Name == "Ken")
                    {
                        if (p.Position != null)
                            Ken.Position = new Vector2(p.Position.X, p.Position.Y);
                        break;
                    }
                    else if (p.Name == "Anim_bigMonster")
                    {

                    }
                    else if (p.Name == "Blanka")
                    {
                        if (p.Position != null)
                        {
                            Blanka.Position = new Vector2(p.Position.X, p.Position.Y);
                            break;
                        }

                    }
                }
            }

            base.LoadContent();         
        }

        /// <summary>
        /// Input helper
        /// </summary>
        public void HandleInput()
        {
           
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
      
    }
}
