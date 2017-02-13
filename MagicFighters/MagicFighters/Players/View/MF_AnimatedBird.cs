using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicFighters.ViewModel;
using Microsoft.Xna.Framework.Graphics;

namespace MagicFighters.Players.View
{
    class MF_AnimatedBird :DynamicItem
    {
        public MF_AnimatedBird(Game game, TileMap level, SpriteBatch batch)
            : base(game, level, batch, null)
        {

        }
        protected override void LoadContent()
        {
            
            Animations = new MobileSprite(curGame.Content.Load<Texture2D>("Level1Tileset/Items/AnimBrid1"));
            Animations.Position = new Vector2(this.CurLoadedItem.Position.X,this.CurLoadedItem.Position.Y);
            Animations.Sprite.AddAnimation("flyingR", 0, 0, 64, 57, 2, 0.6f);
            Animations.Sprite.AddAnimation("flyingL", 0, 0, 64, 57, 2, 0.6f, true);
            Animations.Sprite.CurrentAnimation = "flyingR";
            Direction = new Vector2(MOVE_RIGHT, MOVE_NONE);
            Speed = new Vector2(2, 2);
           // Size = new Vector2(64, 57);
           // base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            if (!isObjectVisible())
                return;

                Animations.Draw(spriteBatch);

                //base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {         
            //Animations.Position += Direction * Speed;
            FlyingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            flyAI();
            if (FlyingTimer >= 2)
            {
                FlyingTimer = 0;
            }
            Animations.Update(gameTime);            
           // base.Update(gameTime);
        }
        float FlyingTimer = 0;
       
        public void flyAI()
        {
            if (Animations.Sprite.CurrentAnimation == "flyingL")
            {
                Animations.Position += Direction * 0.1f;
            }
            else if (Animations.Sprite.CurrentAnimation == "flyingR")
            {
                Animations.Position += Direction * Speed; 
            }
                      
            if (FlyingTimer >= 2)
            {               
                if(Animations.Sprite.CurrentAnimation == "flyingL")
                    Animations.Sprite.CurrentAnimation = "flyingR";
                else
                    Animations.Sprite.CurrentAnimation = "flyingL";
                    Direction = -Direction;
            }
        }
    }
}
