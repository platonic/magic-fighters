// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//CreditScreen.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using MagicFighters.Screens.Model;
#endregion

namespace MagicFighters
{
    class CreditsScreen : MenuScreen 
    {
        #region Fields

        const float STARTINGYPOSITION = 150;
        const float OFFSET = 10;
        const float OFFSET2 = 50;
        const float XPERCENTAGE = 0.5f;
        const float EXIT_Y_LOCATION = 500;
        Color COLORINFO1 = Color.Yellow;
        Color COLORINFO2 = Color.Green;
        Color CurColor;
        const float Speed = 0.5f;
        Vector2 Direction = new Vector2(0, -1);
        Vector2 Position = Vector2.Zero;
        public bool showTextureBackground = false;
        Texture2D background;
        // Create our menu entries.
        MenuEntry cancelMenuEntry = new MenuEntry("Cancel");
        SpriteFont font;
        GameScreen backgroundScreen;

        private List<Credit> Credits;
        #endregion

        #region Initialization
        public CreditsScreen(GameScreen backgroundScreen)
            : base("Instructions")
        {
            Credits = new List<Credit>();
            CurColor = COLORINFO1;
            IsPopup = true;
           

            this.backgroundScreen = backgroundScreen;

           
            MenuEntry ExitInstructionScreen = new MenuEntry("Exit");
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Hook up menu event handlers.
            cancelMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(cancelMenuEntry);
            
        }
        #endregion

        #region Event Handlers for Menu Items
        /// <summary>
        /// Handles "Exit" menu item selection
        /// </summary>
        protected override void OnCancel(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {
            if (AudioManager.isPlaying("credits"))
            {
                AudioManager.StopSounds();
                AudioManager.PlayMusic("mainmenu");
                backgroundScreen.ExitScreen();
                ExitScreen();
            }
            else
            {
                AudioManager.StopSounds();
                backgroundScreen.ExitScreen();
                ExitScreen();
            }
        }
        #endregion

        #region Loading
        public override void LoadContent()
        {
             font = Load<SpriteFont>("MainFont");

             background = ScreenManager.ContentManager.Load<Texture2D>("Textures/Players/KenEnding/20");
             Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * XPERCENTAGE,
                          ScreenManager.GraphicsDevice.Viewport.Height - 200);

            //load credits information from the XML file
             XDocument doc = XDocument.Load("Content/Credits.xml");
             XName name = XName.Get("Credit");
             var credits = doc.Document.Descendants(name);


             Vector2 size = Vector2.Zero;
           
            // Loop over all credits in the XML file
             foreach (var credit in credits)
             {
                 Vector2 position = Vector2.Zero;
                string Name = credit.Attribute("Name").Value;
                string Title = credit.Attribute("Title").Value;
                FlowDirection FlowDirection = Screens.Model.FlowDirection.BottomCenter;
                string tmp = credit.Attribute("FlowDirection").Value;

                 string Comments  = "";
                 if(credit.Attribute("Comments") != null)
                     Comments = credit.Attribute("Comments").Value;
                switch (tmp)
                {
                    case "BottomCenter":
                        FlowDirection = Screens.Model.FlowDirection.BottomCenter;
                        position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * XPERCENTAGE,
                            ScreenManager.GraphicsDevice.Viewport.Height);
                        break;
                }

                bool isRotating;

                bool.TryParse(credit.Attribute("IsRotating").Value, out isRotating);

                Credits.Add(new Credit(position,Comments, Name, Title, FlowDirection, isRotating));
             }

            base.LoadContent();
        }
        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
          if(Position.Y >= -1450)
            Position += Direction * Speed;
          else
              Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * XPERCENTAGE,
                         ScreenManager.GraphicsDevice.Viewport.Height - 200);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        #region Handle input
        public override void HandleInput(InputState input)
        {
            PlayerIndex p;
            if (input.IsMenuCancel(PlayerIndex.One, out p))
            {
                ExitScreen();
            }
           
            base.HandleInput(input);
        }
        #endregion

        #region Render
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            {

                if(showTextureBackground)
                    spriteBatch.Draw(background, new Rectangle(0, 0,
                    ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);

                Vector2 size = Vector2.Zero;
                float y = STARTINGYPOSITION;
                float add = OFFSET2;
                int i = 0;
                foreach (var credit in Credits)
                {
                    size = font.MeasureString(credit.Name);
                    y += size.Y;
                    spriteBatch.DrawString(font, credit.Name, Position + new Vector2(-size.X, size.Y + y) * new Vector2(XPERCENTAGE, 1), CurColor);

                    size = font.MeasureString(credit.Title);
                    y += size.Y;
                    spriteBatch.DrawString(font, credit.Title, Position + new Vector2(-size.X, size.Y + y + OFFSET) * new Vector2(XPERCENTAGE, 1), CurColor);

                    size = font.MeasureString(credit.Comments);
                    y += size.Y;
                    spriteBatch.DrawString(font, credit.Comments, Position + new Vector2(-size.X, size.Y + y + OFFSET + 20) * new Vector2(XPERCENTAGE, 1), CurColor);

                    if (i % 2 == 0)
                        CurColor = COLORINFO1;
                    else
                        CurColor = COLORINFO2;

                    y += OFFSET + add;
                    i++;
                }
                
                size = font.MeasureString(cancelMenuEntry.Text);
                cancelMenuEntry.Position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - (size.X + 50)), EXIT_Y_LOCATION);
                cancelMenuEntry.Draw(this, true, gameTime);
            }
            spriteBatch.End();
        }
        #endregion
    }
}
