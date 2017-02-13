// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//StaticItems.cs
//Author        : Lisandro Martinez
//Comments      : 
//Date          : 9/01/2012
//Last Modified : 9/21/2012    By: Lisandro Martinez
//Last Modified : 9/26/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicFighters.View;
using Microsoft.Xna.Framework;
using MagicFighters.ViewModel;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Model;
using System.Xml.Linq;
using MagicFighters.TileEngine.Model;

namespace MagicFighters.Players.View
{
    class StaticItem : MF_Object
    {
        #region Fields
        public Player player = null;
        public MF_EditorPlayer CurLoadedItem = null;
        Texture2D Texture = null;
        SpriteFont font;
        
#if DEBUG
        Texture2D debugRectangleTexture;
#endif
        #endregion
        #region Properties
        public Vector2 vPosition
        {
            get { return base.Position; }
        }
        public Vector2 vSize
        {
            get { return base.Size; }
           // set { Size = vSize; }
        }
        #endregion
        #region Initialize

        public StaticItem(Game game, TileMap level, 
            SpriteBatch screenSpriteBatch, Player _player)
            : base(game, level,screenSpriteBatch)
        {
            player = _player;
            curGame = (MagicFightersGame)game;
            CurrentLevel = level;
        }

        public override void Initialize()
        {
           
            base.Initialize();
        }

        protected override void LoadContent()
        {
#if DEBUG
            debugRectangleTexture = MF_Physics.CreateLineTexture(curGame.GraphicsDevice, 10, Color.Black);
#endif

            if(CurLoadedItem != null)
                Texture = curGame.Content.Load<Texture2D>(CurLoadedItem.FilePath);
            font = curGame.Content.Load<SpriteFont>("MainFont");
            base.LoadContent();
        }
        public void LoadFromXML(string itemName)
        {
             Dialog = new Model.MF_Dialogue(this, spriteBatch, curGame);
             Dialog.LoadXMLDialog(itemName);

            
           

                    string action = Dialog.Action;
                    string field = Dialog.Field;

                    switch (field)
                    {
                        case "MP":
                            {
                                switch (action)
                                {
                                    case "FireBall":
                                    case "+":
                                        CurLoadedItem.MP = MP = Dialog.Value;
                                        break;
                                    case "-":
                                        CurLoadedItem.MP = MP = -Dialog.Value;
                                        break;
                                }

                            }
                            break;
                        case "HP":
                            switch (action)
                            {
                                case "+":
                                    CurLoadedItem.HP = HP = Dialog.Value;
                                    break;
                                case "-":
                                    CurLoadedItem.HP = HP = -Dialog.Value;
                                    break;

                            }
                            break;
                        case "Inventory":
                            switch (action)
                            {
                                case "+":
                                    break;
                                case "-":
                                    break;

                            }
                            break;
                    }

        }

        public void Init(MF_EditorPlayer item)
        {


            CurLoadedItem = item;
            Animations = new MobileSprite(Texture);

            if (CurLoadedItem != null)
            {
                //Size = new Vector2(CurLoadedItem.Size.W, CurLoadedItem.Size.H);
                Position = new Vector2(CurLoadedItem.Position.X, CurLoadedItem.Position.Y);
            }



            Initialize();

        }
        #endregion

        #region Update
        public void Collected()
        {
            Visible = false;
            Dialog.ResetDisplayTimer();
            Dialog.Display = true;
        }
       
        public override void Update(GameTime gameTime)
        {
            if (Dialog != null)
            {
                if(Dialog.Display)
                    Dialog.Update(gameTime);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(GameTime gameTime)
        {
            if (CurLoadedItem == null)
                return;



            if (Dialog != null)
            {
                if (Dialog.Display)
                {
                    Dialog.Draw(new Vector2(0, -100), Color.Red);
                }

                if (Dialog.DisplayTimer > 10)
                {
                    Dialog.Display = false;
                    Dialog.Enabled = false;
                    Dialog.ResetDisplayTimer();
                }

            }

            if (!Visible)
                return;

#if DEBUG
            spriteBatch.DrawString(font, CurLoadedItem.Name, Position, Color.White);
            spriteBatch.Draw(debugRectangleTexture, new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y), Color.White);
#endif
            if (CurLoadedItem.Size == null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);

            }
            else if (CurLoadedItem.Size.H <= 0 || CurLoadedItem.Size.W <= 0)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
            else
            {
                
                spriteBatch.Draw(Texture, new Rectangle((int)(Position.X ), (int)(Position.Y ),//- CurrentLevel.Camera.Pos.Y
                    CurLoadedItem.Size.W, CurLoadedItem.Size.H), Color.White);
            }

            base.Draw(gameTime);
        }
        #endregion



    }
}
