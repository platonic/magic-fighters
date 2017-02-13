// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//TileMap.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//Last Modified : 9/27/2012    By: Lisandro Martinez Comments: Fixed DEBUG Mode and load and update tile
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Model;
using Microsoft.Xna.Framework.Content;
using MagicFighters.Screens;
using Polenter.Serialization;
using MagicFighters.Players.View;
using MagicFighters.TileEngine.Model;
using MagicFighters.View;
using MagicFighters.TileEngine.View;
using System.Windows.Forms;
using MagicFighters.ParticleSystem;
using MagicFighters.ParticleEngine;
using MagicFighters.ParticleEngine.Helpers;
using MagicFighters.GameStates;
namespace MagicFighters.ViewModel
{
    abstract class TileMap : DrawableGameComponent
    {
        #region DEBUG Content

#if DEBUG
        MagicFighters.TileEngine.Model.MF_Line PlayerLine = null;
        Texture2D CollisionRectangleTexture;
#endif
        #endregion

        static string[] ITEM_TYPES =
        {
            "PlayerType",
            "BlankaType",
            "EnemyType",
            "NPCType",
            "BirdType",
            "SpikesType",
            "FruitType",
            "BackgroundType",
            "VegetableType",
            "ColectorType",
            "FireType",
            "BotType",
            "HouseType",
            "LampType",
            "ItemType",
        };
        public MF_GameStates CurrentGameState;
        public List<DrawableGameComponent> Players;
        protected MF_Camera camera;
        public MF_Camera Camera
        {
            get { return camera; }
        }
        protected Game curGame;
        public TileMap(Game game, MF_Camera cam, SpriteBatch batch)
            : base(game)
        {
            CurrentGameState = MF_GameStates.Playing;
            camera = cam;
            curGame = game;
            LevelGravity = 0.96875f;
            spriteBatch = batch;

            Players = new List<DrawableGameComponent>();

        }
        public MF_Tile[] CollisionTiles { get; set; }
        public MF_EditorPlayer[] PlayerTiles { get; set; }
        public Vector2 PlayerStartPosition { get; set; }
        public float LevelGravity { get; set; }
        public MF_TileMap tilemap;
        public int TileHeight { get { return tilemap == null ? 0 : (int)tilemap.Tileset.Height; } }
        public int TileWidth { get { return tilemap == null ? 0 : (int)tilemap.Tileset.Width; } }
        public int TilesWide { get { return map == null ? 0 : map.GetLength(1); } }
        public int TilesHigh { get { return map == null ? 0 : map.GetLength(0); } }

        public int Width
        {
            get { return map == null ? 0 : map.GetLength(1) * TilesWide; }
        }
        public int Height
        {
            get { return map == null ? 0 : map.GetLength(0) * TilesHigh; }
        }

        protected int LeftMargin { get; set; }
        protected int TopMargin { get; set; }
        protected int RightMargin { get; set; }
        protected int BottomMargin { get; set; }
        protected List<StaticItem> BackgroundItems = new List<StaticItem>();
        protected List<DynamicItem> PlayerTypeItems = new List<DynamicItem>();
        protected List<DynamicItem> BirdTypeItems = new List<DynamicItem>();
        protected List<DynamicItem> BlankaTypeItems = new List<DynamicItem>();
        protected List<DynamicItem> EnemyTypeItems = new List<DynamicItem>();
        protected List<MF_Bot> BotTypeItems = new List<MF_Bot>();
        protected List<DynamicItem> NPCTypeItems = new List<DynamicItem>();
        protected List<StaticItem> MiscItems = new List<StaticItem>();
        private List<MF_Tile> CollidableTiles = new List<MF_Tile>();

        /// <summary>
        /// Drawing helper property
        /// </summary>
        protected SpriteBatch spriteBatch;

        //public Vector2 LevelPosition;
        protected Rectangle worldBoundry
        {
            get
            {
                return new Rectangle((int)camera.Pos.X, (int)camera.Pos.Y,
                    (int)camera.Pos.X + Width, (int)camera.Pos.Y + Height);
            }
        }

        protected Vector2 centre = new Vector2(8.0f, 56.0f);
        protected Vector2 TargetPosition { get; set; }
        protected MF_Tile[,] map = null;

        protected Texture2D Background = null;
        protected BackgroundImage[] BackgroundNames;
        protected Dictionary<string, Texture2D> tileTextures = null;

        protected MF_Tileset TileSet = null;

        protected void ScrollCamera()
        {
            foreach (var p in Players)
                if (p.GetType() == typeof(MF_Ken))
                {
                    TargetPosition = ((MF_Ken)p).Position;
                    break;
                }

            camera.Pos = new Vector2(TargetPosition.X - (camera.viewport.Width * 0.5f), 0);
            // Update the camera position, but prevent scrolling off the ends of the level.
            float maxCameraPosition = Width;

            camera.Pos = new Vector2(MathHelper.Clamp(camera.Pos.X, 0.0f, maxCameraPosition - camera.viewport.Width), 0);
        }
    

        public override void Update(GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case MF_GameStates.FightMode:
                    break;
                case MF_GameStates.GameOver:
                    {
                        
                    }
                    break;
                case MF_GameStates.Initialize:
                    break;
                case MF_GameStates.LevelOver:
                    break;
                case MF_GameStates.LostLife:
                    break;
                case MF_GameStates.Playing:
                    {
                        UpdateVisibleTiles();
                        UpdateBirds(gameTime);
                        UpdateMiscItems(gameTime);
                        particleComponent.Update(gameTime);
                        
                        foreach (var item in BotTypeItems)
                            item.Update(gameTime);

                        ScrollCamera();

                        foreach (var player in Players)
                        {
                            player.Update(gameTime);
                        }

                    }
                    break;
                case MF_GameStates.StoryMode:
                    break;
            }
            
            base.Update(gameTime);
        }

        protected void AddTileTexture(Texture2D texture)
        {
            //tileTextures.Add(texture);
        }
        protected void SetViewPos(Vector2 pos)
        {
            // Only move towards the specified position, don't move directly there.
            centre.X += (pos.X - centre.X) * 0.1f;
            centre.Y += (pos.Y - centre.Y) * 0.1f;
        }

        private float scale = 1.5f;
        /// <summary>
        /// Returns the screen position matching the given map position.
        /// </summary>
        protected Vector2 WorldToScreen(Vector2 pos, float screenWidth, float screenHeight, float tileWidth, float tileHeight)
        {
            pos -= centre;
            pos.X *= scale * tileWidth;
            pos.Y *= scale * tileHeight;
            pos.X += screenWidth / 2;
            pos.Y += screenHeight / 2;
            return pos;
        }

        /// <summary>
        /// Returns the world position matching the given screen position.
        /// </summary>
        protected Vector2 ScreenToWorld(Vector2 pos, float screenWidth, float screenHeight, float tileWidth, float tileHeight)
        {
            pos.X -= screenWidth / 2;
            pos.Y -= screenHeight / 2;
            pos.X /= scale * tileWidth;
            pos.Y /= scale * tileHeight;
            pos += centre;
            return pos;
        }
        /// <summary>
        /// Move the vector representing the tilemap position
        /// </summary>
        /// <param name="cameraDirection"></param>
        protected void MoveLevel(Vector2 cameraDirection)
        {
            camera.Pos += cameraDirection;

            if (camera.Pos.X < LeftMargin)
                camera.Pos += new Vector2(LeftMargin, 0);
            if (camera.Pos.Y < TopMargin)
                camera.Pos += new Vector2(0, TopMargin);
            if (camera.Pos.X > (Width - TilesWide) * TileWidth + RightMargin)
                camera.Pos += new Vector2((Width - TilesWide) * TileWidth + 25, 0);
            if (camera.Pos.Y > (Height - TilesHigh) * TileHeight + BottomMargin)
                camera.Pos += new Vector2(0, (Height - TilesHigh) * TileHeight + BottomMargin);
        }

        private BoundingBox GetTileBoundingBox(int ix, int iy)
        {
            float x = ((ix * TileWidth) - camera.Pos.X);
            float y = ((iy * TileHeight) - camera.Pos.Y);
            return new BoundingBox(new Vector3(x, y, 0),
              new Vector3(x + TileWidth, y + TileHeight, 0));
        }


        private void Draw(string tileName, int x, int y, int w, int h, Color color)
        {
            if (!string.IsNullOrEmpty(tileName) && tileTextures.Keys.Contains(tileName) && tileTextures[tileName] != null)
            {
                spriteBatch.Draw(tileTextures[tileName], new Rectangle(x, y, w, h), color);
            }
        }

        #region Draw Individual items

        private void DrawBackgrounds()
        {
            //tilemap
            if (tilemap.Backgrounds != null)
            {
                foreach (var background in tilemap.Backgrounds)
                {
                    if (background.isBackgroundImageStreched)
                    {
                        Draw(background.Name, 0, 0, camera.viewport.Width, camera.viewport.Height, Color.White);
                    }
                    else
                    {
                        if (background.isBackgroundImageTiledX)
                        {
                            Draw(background.Name, 0, 0, (int)background.Width, (int)background.Height, Color.White);
                        }
                        if (background.isBackgroundImageTiledY)
                        {
                            Draw(background.Name, 0, 0, (int)background.Width, (int)background.Height, Color.White);
                        }
                    }

                }
            }
            //if (!string.IsNullOrEmpty(BackgroundName))
            //    spriteBatch.Draw(tileTextures[BackgroundName], new Rectangle((int)camera.Pos.X, (int)camera.Pos.Y,
            //        camera.viewport.Width, camera.viewport.Height), Color.White);

        }
        public void UpdateBirds(GameTime gameTime)
        {
            if (BirdTypeItems != null && BirdTypeItems.Count > 0)
            {
                foreach (var item in BirdTypeItems)
                {
                    item.Update(gameTime);
                }
            }
        }
        public void DrawBirdItems(GameTime gameTime)
        {
            if (BirdTypeItems != null && BirdTypeItems.Count > 0)
            {
                foreach (var item in BirdTypeItems)
                {
                    item.Draw(gameTime);
                }
            }
        }

        public void DrawBackgroundItems(GameTime gameTime, bool firstOnly = true)
        {
            if (BackgroundItems != null && BackgroundItems.Count > 0)
            {
                if (firstOnly)
                {
                    BackgroundItems[0].Position = camera.Pos;
                    BackgroundItems[0].Draw(gameTime);
                }
                else
                {
                    foreach (var item in BackgroundItems)
                    {
                        item.Draw(gameTime);
                    }
                }
                //get background index from camera view
                // int index = (int)(camera.Pos.X / 1646);
                //BackgroundItems[index + 1].Draw(gameTime);
                //BackgroundItems[index + 2].Draw(gameTime);

                //foreach (var item in BackgroundItems)
                //{
                //  //  if(item.isObjectVisible())
                //        item.Draw(gameTime);
                //}


            }
        }

        void DrawMiscItems(GameTime gameTime)
        {
            if (MiscItems != null && MiscItems.Count > 0)
            {
                for (int i = 0; i < MiscItems.Count; i++)
                {
                    MiscItems[i].Draw(gameTime);
                }
            }
        }
        void UpdateMiscItems(GameTime gameTime)
        {
            if (MiscItems != null && MiscItems.Count > 0)
            {
                for (int i = 0; i < MiscItems.Count; i++)
                {
                    MiscItems[i].Update(gameTime);
                }
            }
        }
        void DrawEnemycItems(GameTime gameTime)
        {
#if DEBUG
            if (EnemyTypeItems != null && EnemyTypeItems.Count > 0)
            {
                foreach (var item in EnemyTypeItems)
                {
                    item.Draw(gameTime);
                }
            }
#endif
        }
        void DrawNPCItems(GameTime gameTime)
        {
            if (NPCTypeItems != null && NPCTypeItems.Count > 0)
            {
                foreach (var item in NPCTypeItems)
                {
                    item.Draw(gameTime);
                    item.Dialog.Update(gameTime);
                    item.Dialog.Draw(gameTime);
                }
            }
        }

        public void DrawPlayers(GameTime gameTime)
        {
            foreach (var player in Players)
            {
                if(player.Visible)
                    player.Draw(gameTime);
            }
        }
        #endregion
        #region HUD
        public void DrawPlayersHud(GameTime gameTime)
        {
            foreach (var player in Players)
            {
                if (player.GetType().BaseType == typeof(Player))
                {
                    if (((Player)player).HPBar != null)
                        ((Player)player).HPBar.Draw(gameTime);
                    if (((Player)player).MPBar != null)
                        ((Player)player).MPBar.Draw(gameTime);
                    if (((Player)player).ScoreBar != null)
                        ((Player)player).ScoreBar.Draw(gameTime);
                }
            }
        }
        public void UpdatePlayersHud(GameTime gameTime)
        {
            foreach (var player in Players)
            {
                if (player.GetType().BaseType == typeof(Player))
                {
                    if (((Player)player).HPBar != null)
                    {

                        ((Player)player).HPBar.Update(gameTime);
                    }
                    if (((Player)player).MPBar != null)
                    {
                        ((Player)player).MPBar.Update(gameTime);
                    }
                    if (((Player)player).ScoreBar != null)
                    {
                        ((Player)player).ScoreBar.Update(gameTime);
                    }
                }

            }
        }
        public void InitializePlayersHud()
        {
            foreach (var player in Players)
            {
                if (player.GetType().BaseType == typeof(Player))
                {
                    if (((Player)player).HPBar != null)
                        ((Player)player).HPBar.Initialize();
                    if (((Player)player).MPBar != null)
                        ((Player)player).MPBar.Initialize();
                    if (((Player)player).ScoreBar != null)
                        ((Player)player).ScoreBar.Initialize();
                }

            }
        }
        #endregion
        private void DrawVisibleItemsOnly()
        {

            int left = (int)Math.Floor(camera.Pos.X / TileWidth);
            int right = (left + spriteBatch.GraphicsDevice.Viewport.Width / TileWidth) + 10;
            right = Math.Min(right, TilesWide - 1);

            for (var x = left; x <= right; ++x)
            {
                for (var y = 0; y < TilesHigh; y++)
                {
                    var tile = map[y, x];

                    if (tile == null) continue;

                    //draw all the tiles
                    Vector2 p = new Vector2(x, y) * new Vector2(TileWidth, TileHeight) + new Vector2(tile.Offset.X, tile.Offset.Y);
                    int xpos = (int)p.X;
                    int ypos = (int)p.Y;
                    Vector2 position = new Vector2(x, y) * new Vector2(TileWidth, TileHeight) + new Vector2(tile.Offset.X, tile.Offset.Y);
                    var texture = tileTextures[tile.Name];
                    spriteBatch.Draw(texture, position, Color.White);



#if DEBUG
                    DrawTileBounding(tile, x, y);
                    //DrawTile(spriteBatch, tile, x, y);

                    //see if we need to draw a Decal Image
                    //if (tile.Decal != null)
                    //    DrawTile(spriteBatch, tile.Decal, x, y);

                    MF_Ken ken = ((MF_Ken)Players[0]);

                    Rectangle? r = GetTileCollisionRect(ken.GetRectangle);

                    if (r != null)
                    {
                        spriteBatch.Draw(CollisionRectangleTexture, r.Value, Color.White);
                    }


                    DrawTileBounding(tile, x, y);
#endif

                }
            }
        }
        private void DrawVisibleTiles()
        {
            foreach (var tile in VisibleTiles)
            {
                spriteBatch.Draw(tile.Texture, tile.Position, Color.White);

#if DEBUG

                 DrawTileBounding(tile);
#endif

            }
        }
        ParticleComponent particleComponent;
       
        public override void Draw(GameTime gameTime)
        {
            if (tilemap == null)
                return;



            DrawVisibleTiles();

            DrawMiscItems(gameTime);
            DrawNPCItems(gameTime);

            DrawEnemycItems(gameTime);

            foreach (var item in BotTypeItems)
                item.Draw(gameTime);

            DrawBirdItems(gameTime);

            particleComponent.Draw(gameTime);



#if DEBUG

            //-------------------------------------------------------------------------------------------------------------------------
            //Draw player Bounding & Ray
            //-------------------------------------------------------------------------------------------------------------------------
            foreach (var p in Players)
            {
                if (p.GetType().BaseType == typeof(Player) || p.GetType().BaseType.BaseType == typeof(Player) || p.GetType() == typeof(Player))
                {
                    Player pr = (Player)p;

                    //draw the bounding box
                    spriteBatch.Draw(CollisionRectangleTexture, new Rectangle((int)pr.GetPlayerBoundingBox().Min.X,
                        (int)pr.GetPlayerBoundingBox().Min.Y,
                        (int)(pr.GetPlayerBoundingBox().Max.X - pr.Position.X),
                        (int)(pr.GetPlayerBoundingBox().Max.Y - pr.Position.Y)), 
                        Color.White);

                    //draw the bottom center Ray
                    if (pr.CenterBottomSensor() != null)
                        PlayerLine.Draw(spriteBatch, new Vector2(pr.CenterBottomSensor().Value.Position.X, pr.CenterBottomSensor().Value.Position.Y),
                        new Vector2(pr.CenterBottomSensor().Value.Position.X, pr.CenterBottomSensor().Value.Position.Y) *
                        new Vector2(pr.CenterBottomSensor().Value.Direction.X, pr.CenterBottomSensor().Value.Direction.Y));

                }
            }
            //-------------------------------------------------------------------------------------------------------------------------
#endif
            base.Draw(gameTime);
        }
#if DEBUG
        protected void DrawTileBounding(MF_Physics.Tile tile)
        {
            if (tile != null)
            {
                //draw the bounding box
                if (tile.Rect != null)
                    spriteBatch.Draw(CollisionRectangleTexture, tile.Rect.Value, Color.White);
            }
        }

        protected void DrawTileBounding(MF_Tile tile, int x, int y)
        {
            //   if (tile != null && tile.isCollisionBound)
            {
                int left = (int)(x * tile.Size.W);
                int top = (int)(y * tile.Size.H);

                spriteBatch.Draw(CollisionRectangleTexture, new Rectangle(left, top, tile.Size.W, tile.Size.H), Color.Black);
            }
            //   if (tile != null && tile.isCollisionBound)
            {
                int left = (int)(x * tile.Size.W);
                int top = (int)(y * tile.Size.H);

                spriteBatch.Draw(CollisionRectangleTexture, new Rectangle(left, top, tile.Size.W, tile.Size.H), Color.Black);
            }
        }

        public void DrawCollisionBounds()
        {
            foreach (var tile in CollisionTiles)
            {
                int x = (int)(tile.Position.X);
                int y = (int)(tile.Position.Y);
                if (tile.Size != null)
                    spriteBatch.Draw(CollisionRectangleTexture, new Rectangle(x, y, tile.Size.W, tile.Size.H), Color.Black);
            }



        }
#endif
        private void DrawTile(SpriteBatch batch, MF_Tile tile, int x, int y)
        {
            var texture = tileTextures[tile.Name];

            int left = (int)(x * tile.Size.W - (int)camera.Pos.X);
            int top = (int)(y * tile.Size.H - (int)camera.Pos.Y);

            //Draw the tile at the spesified offeted in the tile editor
            if (tile.Offset != null)
            {

                batch.Draw(texture, new Rectangle(left + (int)(tile.Offset.X), top + (int)(tile.Offset.Y),
                     (int)tile.Size.W, (int)tile.Size.H), Color.White);
            }
            else // just draw the tile at the location
            {
                batch.Draw(texture, new Rectangle(left + (int)(tile.Offset.X), top,
                      (int)tile.Size.W, (int)tile.Size.H), Color.White);
            }
        }
        public System.Collections.IEnumerable GetFloorRectangles()
        {

            foreach (var tile in GetFloorBounding())
            {

                float x = ((BoundingBox)tile).Min.X;
                float y = ((BoundingBox)tile).Min.Y;
                float w = ((BoundingBox)tile).Max.Y;
                float h = ((BoundingBox)tile).Max.Y;

                yield return new Rectangle((int)x, (int)y, (int)w, (int)h);
            }
        }
        public System.Collections.IEnumerable GetEditorPlayer()
        {
            return tilemap.Players;
        }
        public System.Collections.IEnumerable GetFloorBounding()
        {

            foreach (var tile in tilemap.Collision)
            {


                float x = tile.Position.X;//- camera.Pos.X
                float y = tile.Position.Y;// - camera.Pos.Y

                float w = tile.Size == null ? 100 : tile.Size.W;
                float h = tile.Size == null ? 34 : tile.Size.H;

                yield return new BoundingBox(new Vector3(x, y, 0), new Vector3(x + w, y + h, 0));
            }
        }



        public bool RayIntersectFloor(Ray? r)
        {
            if (r == null)
                return false;

            foreach (var tile in tilemap.Collision)
            {


                float x = tile.Position.X;//- camera.Pos.X
                float y = tile.Position.Y;// - camera.Pos.Y

                float w = tile.Size == null ? 100 : tile.Size.W;
                float h = tile.Size == null ? 34 : tile.Size.H;

                BoundingBox box = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + w, y + h, 0));

                if (box.Intersects(r.Value) != null)
                    return true;

            }
            return false;
        }
        public BoundingBox? GetFloorCollisionBounding(BoundingBox rect)
        {
            BoundingBox b = new BoundingBox();
            bool collision = false;
            foreach (var tile in tilemap.Collision)
            {


                float x = tile.Position.X;//- camera.Pos.X
                float y = tile.Position.Y;// - camera.Pos.Y

                float w = tile.Size == null ? 100 : tile.Size.W;
                float h = tile.Size == null ? 34 : tile.Size.H;

                b = new BoundingBox(new Vector3(x, y, 0), new Vector3(x + w, y + h, 0));

                if (b.Intersects(rect))
                {
                    collision = true;
                    LastCollisionVector = new Vector2(rect.Min.X, rect.Min.Y);
                    break;
                }
            }
            if (collision)
                return b;

            return null;
        }
        public Rectangle? GetTileCollisionRect(Rectangle rect)
        {
            Rectangle b = new Rectangle();
            bool collision = false;
            foreach (var tile in CollidableTiles)
            {
                if (tile.Collisions != null && tile.Collisions.Length > 0)
                {
                    if (tile.Collisions[0].CollisionPoints != null &&
                        tile.Collisions[0].CollisionPoints.Length == 2)
                    {
                        int x = (int)((tile.Collisions[0].CollisionPoints[0].X - tile.Collisions[0].OwnerPosition.X));// - camera.Pos.X
                        int y = (int)((tile.Collisions[0].CollisionPoints[0].Y));// - camera.Pos.Y
                        int w = tile.Collisions[0].CollisionPoints[1].X;// tile.Collisions[0].CollisionPoints[1].X;
                        int h = tile.Collisions[0].CollisionPoints[1].Y;// tile.Collisions[0].CollisionPoints[1].Y;


                        b = new Rectangle(x, y, w, h);

                        if (rect.Intersects(b))
                        {
                            collision = true;
                            LastCollisionVector = new Vector2(rect.X, rect.Y);
                            break;
                           
                        }
                    }

                }

            }

            if (collision)
                return b;

            return null;
        }
        public Vector2 LastCollisionVector;
        public BoundingBox? GetTileCollisionBounding(BoundingBox rect)
        {
            BoundingBox b = new BoundingBox();
            bool collision = false;
            foreach (var tile in CollidableTiles)
            {
                if (tile.Collisions != null && tile.Collisions.Length > 0)
                {
                    if (tile.Collisions[0].CollisionPoints != null &&
                        tile.Collisions[0].CollisionPoints.Length == 2)
                    {
                        int x = (int)((tile.Collisions[0].CollisionPoints[0].X - tile.Collisions[0].OwnerPosition.X));// - camera.Pos.X
                        int y = (int)((tile.Collisions[0].CollisionPoints[0].Y));// - camera.Pos.Y
                        int w = tile.Collisions[0].CollisionPoints[1].X;// tile.Collisions[0].CollisionPoints[1].X;
                        int h = tile.Collisions[0].CollisionPoints[1].Y;// tile.Collisions[0].CollisionPoints[1].Y;

                        Vector3 min = new Vector3(x, y, 0);
                        Vector3 max = new Vector3(x + w, y + h, 0);

                        b = new BoundingBox(min, max);

                        if (rect.Intersects(b))
                        {
                            LastCollisionVector = new Vector2(rect.Min.X, rect.Min.Y);
                            collision = true;
                            break;

                        }
                    }

                }

            }
            if (collision)
                return b;
            else
                return null;
        }
        protected Point ConvertObjectPositionToTileMapPosition(Vector2 obj)
        {
            return new Point((int)(obj.X / TileWidth), (int)(obj.Y / TileHeight));
        }
        public bool isObjectVisible(MF_Object obj)
        {
            Rectangle? objRect = obj.Rectangle;

            if (objRect != null)
            {
                return isRectangleVisible(objRect.Value);
            }
            return false;
        }

     

        protected bool isRectangleVisible(Rectangle rect)
        {
            Rectangle screenRect = new Rectangle(0, 0, Width, Height);
            return screenRect.Intersects(rect);
        }

        public float ScreenRight { get { return camera.Pos.X + camera.viewport.Width; } }
        public float ScreenBottom { get { return camera.Pos.Y + camera.viewport.Height; } }
        public float ScreenLeft { get { return camera.Pos.X; } }
        
        /// <summary>
        /// Gets the tile from a vector
        /// </summary>
        /// <param name="obj">the vector to find tile from</param>
        /// <returns>The file at the vector location null otherwise</returns>
        private MF_TileView GetTileFromVector(Vector2 pos)
        {
            Point tilePos = ConvertObjectPositionToTileMapPosition(pos);

            int y = tilePos.Y;
            int x = tilePos.X;

            if (y >= TilesHigh || x >= TilesWide || y < 0 || x < 0)
                return null;

            var tile = map[y, x];

            if (tile == null)
                return null;

            return new MF_TileView(tile);
        }
        /// <summary>
        /// Gets the tile from an object center ray
        /// </summary>
        /// <param name="obj">the object to find tile from</param>
        /// <returns>The file at the object center ray null otherwise</returns>
        private MF_TileView GetTileFromObjectCenterRay(MF_Object obj)
        {
            if (obj == null)
                return null;

            if (!isObjectVisible(obj))
                return null;

            Ray? r = obj.CenterBottomSensor();
            if (r == null)
                return null;
            return GetTileFromVector(new Vector2(r.Value.Position.X, r.Value.Position.Y));
        }
        protected MF_TileView ObjectCenterRayCollidingWithTileBoundingRect(MF_Object obj)
        {
            if (obj == null)
                return null;

            MF_TileView tile = GetTileFromVector(obj.Position);
            if (tile == null)
                return null;
            if (obj.Rectangle != null)
            {
                if (tile.BoundingRect.Intersects(obj.Rectangle.Value))
                    return tile;
            }
            return null;
        }
        protected MF_TileView RectCollidingWithTileBoundingRect(Rectangle rect)
        {

            MF_TileView tile = GetTileFromVector(new Vector2((rect.X + (rect.Width * 0.5f)), rect.Y + rect.Height));
            if (tile == null)
                return null;

            if (tile.BoundingRect.Intersects(rect))
                return tile;

            return null;
        }
        public Rectangle? PerPixelObjecCollidedWithTile(MF_Object obj)
        {
            if (obj == null)
                return null;

            //  if (!isObjectVisible(obj))
            //      return null;

            MF_TileView tile = RectCollidingWithTileBoundingRect(obj.Rectangle.Value);

            //see if bounding is colliding first
            if (tile != null)
            {
                Rectangle? rect = PerPixelTileCollision(new MF_TileView((MF_Tile)tile, tileTextures[tile.Name]), obj);
                if (rect != null)
                {
                    return rect;
                }

            }
            return null;
        }
        public bool isCollidingWithACollidleTile(BoundingBox box)
        {
            foreach (var tile in CollidableTiles)
            {
                if (tile.Collisions != null && tile.Collisions.Length > 0)
                {
                    if (tile.Collisions[0].CollisionPoints != null &&
                        tile.Collisions[0].CollisionPoints.Length == 2)
                    {
                        float x = tile.Collisions[0].CollisionPoints[0].X - camera.Pos.X;//
                        float y = tile.Collisions[0].CollisionPoints[0].Y - camera.Pos.Y;//
                        Vector3 min = new Vector3(x, y, 0);

                        Vector3 max = new Vector3(x + tile.Collisions[0].CollisionPoints[1].X,
                         y + tile.Collisions[0].CollisionPoints[1].Y, 0);

                        BoundingBox b = new BoundingBox(min, max);

                        if ((b.Intersects(box)))
                            return true;
                        else
                            continue;
                    }

                }

            }

            return false;
        }
        public MF_Physics.Tile TileOnPlayerCenterSensor(MF_Object player)
        {


            return null;
        }
        public bool PerPixelCollisionCenterSensorTile(MF_Object player)
        {


            return false;
        }
        public Rectangle? PerPixelTileCollision(MF_TileView tile, MF_Object obj)
        {
            Rectangle objRect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, obj.GetAnimations.Sprite.CurrentFrameAnimation.FrameRectangle.Width,
               obj.GetAnimations.Sprite.CurrentFrameAnimation.FrameRectangle.Height);
            var rect = PerPixelTileCollision(tile.BoundingRect, tile.Texture, objRect, obj.GetAnimations.Sprite.Texture);
            if (rect != null)
            {
                return rect;
            }
            return null;
        }
        protected Rectangle? PerPixelTileCollision(Rectangle tileRect, Texture2D tileTexture,
            Rectangle objRect, Texture2D objTexture)
        {

            // Get Color data of each Texture
            Color[] colorData1 = new Color[tileRect.Width * tileRect.Height];
            tileTexture.GetData<Color>(0, new Rectangle(0, 0, tileRect.Width, tileRect.Height), colorData1, 0, colorData1.Length);
            Color[] colorData2 = new Color[objRect.Width * objRect.Height];
            objTexture.GetData(0, new Rectangle(0, 0, objRect.Width, objRect.Height), colorData2, 0, colorData2.Length);


            return IntersectPixels(tileRect, colorData1, objRect, colorData2);
        }

        // extract pixel data from the texture
        public bool[,] GetOpaqueData(Texture2D texture, Rectangle rect, byte threshold)
        {
            int width = rect.Width, height = rect.Height;  // temp variables to save some typing

            // an array of booleans, one for each pixel
            // true = opaque (considered), false = transparent (ignored)
            bool[,] data = new bool[width, height];

            // an array to hold the pixel data from the texture
            Color[] pixels = new Color[texture.Width * texture.Height];

            texture.GetData<Color>(pixels);


            for (int y = 0; y < height; y++)      // for each row of pixel data
            {
                for (int x = 0; x < width; x++)   // for each column of pixel data
                {
                    // if the pixel's alpha exceeds our threshold,note it in our boolean array
                    if (pixels[rect.Left + x + (rect.Top + y) * texture.Width].A >= threshold)
                    {
                        data[x, y] = true;
                    }
                }
            }
            return data;                    // return our findings to the caller
        }

        public Rectangle? IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - (rectangleA.Top)) * rectangleA.Width];

                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {

                        // then an intersection has been found
                        return new Rectangle(x, y, right, bottom);
                    }
                }
            }

            // No intersection found
            return null;
        }
        // perform a pixel-by-pixel comparison
        protected Rectangle? PixelsTouch(Rectangle rect1, Vector2 loc1, Color[] data1,
                                           Rectangle rect2, Vector2 loc2, Color[] data2)
        {
            //Update rects with locations of sprites
            rect1.X = (int)Math.Round(loc1.X);
            rect1.Y = (int)Math.Round(loc1.Y);
            rect2.X = (int)Math.Round(loc2.X);
            rect2.Y = (int)Math.Round(loc2.Y);

            //Determine the intersection of the two rects
            Rectangle intersect = Rectangle.Empty;
            intersect.Y = Math.Max(rect1.Top, rect2.Top);
            intersect.X = Math.Max(rect1.Left, rect2.Left);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int right = Math.Min(rect1.Right, rect2.Right);
            intersect.Height = bottom - intersect.Y;
            intersect.Width = right - intersect.X;

            //Scan the intersected rectangle, pixel-by-pixel
            int x1 = intersect.X - rect1.X;
            int x2 = intersect.X - rect2.X;
            int y1 = intersect.Y - rect1.Y;
            int y2 = intersect.Y - rect2.Y;
            for (int y = 0; y < intersect.Height; y++)
            {
                for (int x = 0; x < intersect.Width; x++)
                {
                    // Get the color from each texture
                    Color a = data1[x * y + x];
                    Color b = data2[x * y + x];

                    //Are both pixels opaque?
                    if (a.A <= 127 && b.A <= 127)
                    {
                        return rect1;
                    }
                }
            }
            return null;
        }

        public bool PerPixelCollisionVisibleTiles(MF_Object player)
        {

            foreach (var tile in VisibleTiles)
            {

                if (tile.Rect == null)
                    continue;

                Rectangle tileRect = tile.Rect.Value;
                Rectangle playerRect = player.GetAnimations.Sprite.CurrentFrameAnimation.FrameRectangle;

                // Get Color data of each Texture
                Color[] bitsA = new Color[tile.Texture.Width * tile.Texture.Height];
                tile.Texture.GetData(bitsA);
                Color[] bitsB = new Color[player.GetAnimations.Sprite.Texture.Width * player.GetAnimations.Sprite.Texture.Height];
                player.GetAnimations.Sprite.Texture.GetData(bitsB);

                // Calculate the intersecting rectangle
                int x1 = Math.Max(tileRect.X, playerRect.X);
                int x2 = Math.Min(tileRect.X + tileRect.Width, playerRect.X + playerRect.Width);

                int y1 = Math.Max(tileRect.Y, playerRect.Y);
                int y2 = Math.Min(tileRect.Y + tileRect.Height, playerRect.Y + playerRect.Height);

                // For each single pixel in the intersecting rectangle
                for (int y = y1; y < y2; ++y)
                {
                    for (int x = x1; x < x2; ++x)
                    {
                        // Get the color from each texture
                        Color a = bitsA[(x - tileRect.X) + (y - tileRect.Y) * tileRect.Width];
                        Color b = bitsB[(x - tileRect.X) + (y - playerRect.Y) * playerRect.Width];

                        if (a.A != 0 && b.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                        {
                            return true;
                        }
                    }
                }


            }

            // If no collision occurred by now, we're clear.
            return false;
        }
        private List<MF_Physics.Tile> _VisibleTiles = new List<MF_Physics.Tile>();
        private List<MF_Physics.Tile> VisibleTiles
        {
            get { return _VisibleTiles; }
            set { _VisibleTiles = value; }
        }
        private void UpdateVisibleTiles()
        {
            VisibleTiles.Clear();


            int left = (int)Math.Floor(camera.Pos.X / TileWidth);
            int right = (left + spriteBatch.GraphicsDevice.Viewport.Width / TileWidth) + 10;
            right = Math.Min(right, TilesWide - 1);

            for (var x = left; x <= right; ++x)
            {
                for (var y = 0; y < TilesHigh; y++)
                {
                    var tile = map[y, x];

                    if (tile == null) continue;

                    //draw all the tiles
                    Vector2 p = new Vector2(x, y) * new Vector2(TileWidth, TileHeight) + new Vector2(tile.Offset.X, tile.Offset.Y);
                    int xpos = (int)p.X;
                    int ypos = (int)p.Y;
                    Vector2 position = new Vector2(x, y) * new Vector2(TileWidth, TileHeight) + new Vector2(tile.Offset.X, tile.Offset.Y);
                    var texture = tileTextures[tile.Name];

                    VisibleTiles.Add(new MF_Physics.Tile(position, texture));
                }
            }


        }

        public Rectangle GetTileWorldBounding(MF_Tile tile)
        {

            if (tile.hasCollision)
                return new Rectangle((int)(tile.Position.X - camera.Pos.X), (int)(tile.Position.Y - camera.Pos.Y), tile.Size.W, tile.Size.H);
            else
                return new Rectangle((int)((tile.Position.X * tile.Size.W) - camera.Pos.X), (int)((tile.Position.Y * tile.Size.H) - camera.Pos.Y), tile.Size.W, tile.Size.H);

        }

        protected bool LoadMap(string name, string level)
        {
            if (!Load("Content/" + name))
                return false;


            if (TileSet != null && TileSet.Tiles != null && TileSet.Tiles.Length > 0)
            {
                for (var i = 0; i < TileSet.Tiles.Length; i++)
                {
                    var tile = TileSet.Tiles[i];

                    if (tile == null) continue;

                    try
                    {
                        string filename = level + "/" + System.IO.Path.GetFileNameWithoutExtension(tile.Name);// tile.Name.Substring(0, tile.Name.IndexOf("."));

                        tileTextures.Add(tile.Name, curGame.Content.Load<Texture2D>(filename));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            ((MagicFightersGame)curGame).SetResolution(800, TilesHigh * TileHeight);

            base.LoadContent();
            return true;

            
        }

        void ClearObjectsList()
        {
            BackgroundItems.Clear();
            NPCTypeItems.Clear();
            EnemyTypeItems.Clear();
            BlankaTypeItems.Clear();
            MiscItems.Clear();
            BirdTypeItems.Clear();
        }

        protected bool Load(string FileLocation)
        {
            if (String.IsNullOrEmpty(FileLocation))
                return false;

            particleComponent = new ParticleComponent(spriteBatch,curGame);
            //curGame.Components.Add(particleComponent);
#if DEBUG
            PlayerLine = new TileEngine.Model.MF_Line(10, Color.White);
            PlayerLine.Load(curGame.GraphicsDevice);
            CollisionRectangleTexture = curGame.Content.Load<Texture2D>("rectangle");
#endif
            var serializer = new SharpSerializer(true);

            if (System.IO.File.Exists(FileLocation))
            {
                var stream = new System.IO.FileStream(FileLocation, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                tilemap = (MF_TileMap)serializer.Deserialize(stream);

                //Make sure we have a valid tilemap from the file
                if (tilemap == null)
                    return false;


                BottomMargin = (int)tilemap.Tileset.Height * TilesHigh;
                RightMargin = (int)tilemap.Tileset.Width * TilesWide;

                if (tilemap.PlayerStartPosition != null)
                {
                    PlayerStartPosition = new Vector2(tilemap.PlayerStartPosition.X - TileWidth, tilemap.PlayerStartPosition.Y);
                }
                if (tilemap.hasBackgroundImage && tilemap.Backgrounds != null && tilemap.Backgrounds.Length > 0)
                    BackgroundNames = tilemap.Backgrounds;

                map = tilemap.Tiles;

                CollidableTiles = tilemap.CollidableTiles.ToList();


                CollisionTiles = tilemap.Collision;

                ClearObjectsList();
               
                if (tilemap.Players != null && tilemap.Players.Length > 0)
                {
                    foreach (var item in tilemap.Players)
                    {
                        if (String.IsNullOrEmpty(item.FilePath) || String.IsNullOrWhiteSpace(item.FilePath)) continue;

                        switch (item.PlayerType)
                        {
                            case "BackgroundType":
                                StaticItem tmpBG = new StaticItem(curGame, this, spriteBatch, null);
                                item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                tmpBG.Init(item);
                                BackgroundItems.Add(tmpBG);
                                break;
                            case "PlayerType":
                                DynamicItem player = new DynamicItem(curGame, this, spriteBatch, null);
                                item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                player.Init(item);
                                PlayerTypeItems.Add(player);
                                break;
                            case "BirdType":
                                MF_AnimatedBird bird = new MF_AnimatedBird(curGame, this, spriteBatch);
                                item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                bird.Init(item);
                                BirdTypeItems.Add(bird);
                                break;
                            case "NPCType":
                                DynamicItem npc = new DynamicItem(curGame, this, spriteBatch, null);
                                npc.Dialog = new MagicFighters.Players.Model.MF_Dialogue(npc, spriteBatch, curGame);
                                npc.Dialog.LoadXMLDialog(NPCTypeItems.Count(), "NPCType");
                                
                                item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                npc.Init(item);
                                NPCTypeItems.Add(npc);
                                break;
                            case "EnemyType":
                                switch (item.Name)
                                {
                                    case "Anim_badwasp1":
                                        break;
                                    case "Anim_bigMonster"://blue man
                                        break;
                                }
                                //DynamicItem enmey = new DynamicItem(curGame, this, spriteBatch, null);
                                //item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                //enmey.Init(item);
                                //EnemyTypeItems.Add(enmey);
                                break;
                            case "BlankaType":
                                //DynamicItem blanka = new DynamicItem(curGame, this, spriteBatch, null);
                                //item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                //blanka.Init(item);
                                //BlankaTypeItems.Add(blanka);
                                break;
                            case "FireType":

                                int count = 1;

                                if (item.Size.W > 100)
                                {
                                    count = 12;
                                }
                                float x = item.Position.X;
                                for (int i = 0; i < count; i++)
                                {
                                   
                                    Emitter emitter = new Emitter()
                                    {
                                        Active = true,
                                        TextureList = new List<Texture2D>()
		                            {
			                            curGame.Content.Load<Texture2D>("Level1Tileset\\Items\\flame1"),
			                            curGame.Content.Load<Texture2D>("Level1Tileset\\Items\\flame2"),
		                             },
                                        Position = new Vector2(x, item.Position.Y + item.Size.H),
                                        RandomEmissionInterval = new RandomMinMax(8.0d),
                                        ParticleLifeTime = 1000,
                                        ParticleDirection = new RandomMinMax(item.Size.W, -1),
                                        ParticleSpeed = new RandomMinMax(0.5f, 2.0f),
                                        ParticleRotation = new RandomMinMax(0, 0),
                                        RotationSpeed = new RandomMinMax(0),
                                        ParticleFader = new ParticleFader(true, true, 350),
                                        ParticleScaler = new ParticleScaler(true, 1.0f)
                                    };
                                    particleComponent.particleEmitterList.Add(emitter);

                                    x += 100;
                                }
                                break;
                            case "BotType":
                                {
                                    BotTypeItems.Add(new MF_Bot(item, curGame, this, spriteBatch) { Position = new Vector2(item.Position.X,item.Position.Y) });
                                }
                                break;
                            default:
                                StaticItem misc = new StaticItem(curGame, this, spriteBatch, null);
                                item.FilePath = "Level1Tileset/Items/" + System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                                misc.Init(item);
                                
                                misc.LoadFromXML(item.Name);
                                misc.Enabled = true;
                                misc.Visible = true;
                                MiscItems.Add(misc);
                                break;

                        }
                       
                    }
                }
                PlayerTiles = tilemap.Players;


                TileSet = tilemap.Tileset;
            }
            return true;
            
        }


    }//end TileMap class

}
