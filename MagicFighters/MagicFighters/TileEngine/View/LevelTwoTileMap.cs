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
    class LevelTwoTileMap : TileMap
    {
        MF_Ken Ken = null;

        Vector2 KenGirl = new Vector2(13290, 157);
        Texture2D kenGirlSprite;

       
        public LevelTwoTileMap(Game game, MF_Camera cam, SpriteBatch batch)
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

            kenGirlSprite = curGame.Content.Load<Texture2D>("Textures/Players/seena");
           
            if (!LoadMap("Level2Tileset/tilemap.mfm", "Level2Tileset"))
            {
                MessageBox.Show("Map could not be loaded.");
            }

            Ken = new MF_Ken(curGame, this, new List<Enemy>() {  }, spriteBatch);
            Ken.Initialize();

            foreach (var item in BotTypeItems)
                item.Initialize(Ken);

            Players.Add(Ken);

            //Ken only cares about certain items type
            Ken.InitStaticItems((from w in this.MiscItems
                                 where w.CurLoadedItem.PlayerType == "CollectorType" ||
                                 w.CurLoadedItem.PlayerType == "ColectorType" ||        //TODO: TOFIX: workaround, correct for tile editor mistake
                                 w.CurLoadedItem.PlayerType == "VegetableType" ||
                                 w.CurLoadedItem.PlayerType == "ItemType" ||
                                 w.CurLoadedItem.PlayerType == "FruitType" || 
                                 w.CurLoadedItem.PlayerType == "LampType" 
                                 
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
                }
            }
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(kenGirlSprite, KenGirl, Color.White);
            base.Draw(gameTime);
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
