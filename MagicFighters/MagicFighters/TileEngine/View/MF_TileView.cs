using System;
using Microsoft.Xna.Framework.Graphics;
using MagicFighters.Model;
using Microsoft.Xna.Framework;


namespace MagicFighters.TileEngine.View
{
     class MF_TileView : MF_Tile
     {
         public MF_TileView() { }
         public MF_TileView(MF_Tile tile) 
         {
             if (tile == null)
                 return;
             this.Animations = tile.Animations;
             this.Collisions = tile.Collisions;
             this.Decal = tile.Decal;
             this.Description = tile.Description;
             this.hasAction = tile.hasAction;
             this.hasAnimation = tile.hasAnimation;
             this.hasCollision = tile.hasCollision;
             this.hasCollisonPoints = tile.hasCollisonPoints;
             this.hasCollisonRectangle = tile.hasCollisonRectangle;
             this.hasCollisonSphere = tile.hasCollisonSphere;
             this.hasDecal = tile.hasDecal;
             this.hasSound = tile.hasSound;
             this.ID = tile.ID;
             this.isSelected = tile.isSelected;
             this.isVisible = tile.isVisible;
             this.LifeSpan = tile.LifeSpan;
             this.Name = tile.Name;
             this.Offset = tile.Offset;
             this.Position = tile.Position;
             this.Rotation = tile.Rotation;
             this.Size = tile.Size;
             this.soundName = tile.soundName;
             this.Speed = tile.Speed;
             this.TileType = tile.TileType;
         }
         public MF_TileView(MF_Tile tile, Texture2D texture)
         {
             this.Animations = tile.Animations;
             this.Collisions = tile.Collisions;
             this.Decal = tile.Decal;
             this.Description = tile.Description;
             this.hasAction = tile.hasAction;
             this.hasAnimation = tile.hasAnimation;
             this.hasCollision = tile.hasCollision;
             this.hasCollisonPoints = tile.hasCollisonPoints;
             this.hasCollisonRectangle = tile.hasCollisonRectangle;
             this.hasCollisonSphere = tile.hasCollisonSphere;
             this.hasDecal = tile.hasDecal;
             this.hasSound = tile.hasSound;
             this.ID = tile.ID;
             this.isSelected = tile.isSelected;
             this.isVisible = tile.isVisible;
             this.LifeSpan = tile.LifeSpan;
             this.Name = tile.Name;
             this.Offset = tile.Offset;
             this.Position = tile.Position;
             this.Rotation = tile.Rotation;
             this.Size = tile.Size;
             this.soundName = tile.soundName;
             this.Speed = tile.Speed;
             this.TileType = tile.TileType;
             this.Texture = texture;

         }
          public Texture2D Texture { get; set; }

          public Rectangle BoundingRect
          {
              get
              {
                  int x = (Position.X * Size.W)+ (Offset!=null? Offset.X:0);
                  int y = (Position.Y * Size.H) + (Offset !=null?Offset.Y:0);
                  return new Rectangle(x, y, Size.W, Size.H);
              }
          }
          public BoundingBox BoundingBox
          {
              get
              {
                  Vector3 min = new Vector3(BoundingRect.X,BoundingRect.Y,0);
                  Vector3 max = new Vector3(BoundingRect.X + BoundingRect.Width, BoundingRect.Y + BoundingRect.Height, 0); ;
                  return new BoundingBox(min,max);
              }
          }

          public bool Update(double x, double y)
          {
              
               return false;
          }
          public bool Draw()
          {


               return false;
          }
     }
}
