using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicFighters.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace MagicFighters.Players.Model
{
    class MF_Dialogue : DrawableGameComponent
    {
        #region Fields
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D Texture;
        MF_Object OwnerObject;
        MagicFightersGame curGame;
        public Vector2 Position;
        public Vector2 Offset;
        public bool ShowSpeechBuble = true;
        double displayTimer = 0;
      

        #endregion
        #region Properties
        public double DisplayTimer { get { return displayTimer; } }
        public bool Display {get;set;}
        public string Title { get; set; }
        public string Comments { get; set; }
        public float Value { get; set; }
        public string Action { get; set; }
        public string Field { get; set; }
        #endregion
        #region Initialize
        public MF_Dialogue( MF_Object mfObject, SpriteBatch batch, Game game)
            : base(game)
        {
            Display = false;
            OwnerObject = mfObject;
            spriteBatch = batch;
            curGame = (MagicFightersGame)game;
            
        }
        public void LoadXMLDialog(int index, string itemType)
        {

            //load Dialogues information from the XML file
            XDocument doc = XDocument.Load("Content/Dialogue.xml");
            XName name = XName.Get("Item");
            var dialogues = doc.Document.Descendants(name);

            foreach (var item in dialogues)
            {
                itemType = item.Attribute("ItemType").Value;
                int i = -1;
               if(int.TryParse(item.Attribute("Index").Value,out i))
                if (itemType == "NPCType" && i == index)
                {
                    Title = item.Attribute("Title").Value;
                    Comments = item.Attribute("Comments").Value;
                    float v;
                    float.TryParse(item.Attribute("Value").Value, out v);
                    Value = v;
                    Action = item.Attribute("Action").Value;
                    Field = item.Attribute("Field").Value;
                    LoadContent();
                    break;
                }
            }
        }

        public void LoadXMLDialog(string itemName)
        {

            //load Dialogues information from the XML file
            XDocument doc = XDocument.Load("Content/Dialogue.xml");
            XName name = XName.Get("Item");
            var dialogues = doc.Document.Descendants(name);

            foreach (var item in dialogues)
            {
                string itemname = item.Attribute("Name").Value;
               
                    if (itemName == itemname)
                    {
                        Title = item.Attribute("Title").Value;
                        Comments = item.Attribute("Comments").Value;
                        float v;
                        float.TryParse(item.Attribute("Value").Value, out v);
                        Value = v;
                        Action = item.Attribute("Action").Value;
                        Field = item.Attribute("Field").Value;
                        LoadContent();
                        break;
                    }
            }
        }
        protected override void LoadContent()
        {
           
            font = curGame.Content.Load<SpriteFont>("GameFonts/InformationFont");
            Texture = curGame.Content.Load<Texture2D>("Textures/Dialogue/speech-bubble");
            base.LoadContent();
        }
        #endregion

        #region Update
        /// <summary>
        /// Resets the display timer
        /// </summary>
        public void ResetDisplayTimer() { displayTimer = 0; }
        public override void Update(GameTime gameTime)
        {
            displayTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (OwnerObject.Position != null)
                Position = OwnerObject.Position - new Vector2(10, 0);

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        private List<String> GetWrappedText(String text,Rectangle rect)
        {

            List<String> list = new List<string>();
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length()
                > rect.Y + rect.Width / 2)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }
                line += word + ' ';
                list.Add(line);
            }

            return list;
        }
        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                    textheight += size.Y;
                }
            }

            return sb.ToString();
        }
        public void Draw(Vector2 offset,Color color)
        {
            if (String.IsNullOrEmpty(Comments))
                return;

            string text = WrapText(font, Comments, 150);
            spriteBatch.DrawString(font, text, Position + offset, color);
        }
        float textheight = 0;
        public override void Draw(GameTime gameTime)
        {

           
            if (!Display || font == null)
                return;

            Vector2 size = font.MeasureString(" ");
            string text = WrapText(font, Comments, 150);
            if(ShowSpeechBuble)
                spriteBatch.Draw(Texture, new Rectangle((int)(Position.X - Texture.Width), (int)Position.Y - ((int)textheight * 2), 150, (int)(textheight * 2.5)), Color.Black);
            //spriteBatch.DrawString(font, Title, Position+  new Vector2(20, (size.Y + 30)) , Color.White);
            spriteBatch.DrawString(font, text, (Position + new Vector2(20 - Texture.Width, (size.Y + 20) - ((int)textheight * 2)) + Offset), Color.Black);
            textheight = 0;

            base.Draw(gameTime);
        }
        #endregion


    }
}
