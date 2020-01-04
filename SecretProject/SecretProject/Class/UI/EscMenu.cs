﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.MenuStuff
{
    public class EscMenu : IExclusiveInterfaceComponent
    {
        public bool isTextChanged = false;
        public int ActiveTab { get; set; }

        //  SaveLoadManager saveManager;
        SaveLoadManager mySave;
        private Rectangle BackGroundSourceRectangle;
        private List<CategoryTab> Tabs { get; set; }
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }
        private float Scale { get; set; }
        public Vector2 Position { get; set; }

        public EscMenu(GraphicsDevice graphicsDevice, ContentManager content)
        {
            mySave = new SaveLoadManager();



            this.Scale = 2f;

            BackGroundSourceRectangle = new Rectangle(64, 416, 128, 224);
            this.Position = new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height);


            this.IsActive = false;
            this.FreezesGame = true;
            this.Tabs = new List<CategoryTab>()
            {
                new CategoryTab("Esc", graphicsDevice,new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height - 25 * this.Scale),
                new Rectangle(64,392, 32,25),2f),
                new CategoryTab("Settings", graphicsDevice,new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width + 64, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height - 25 * this.Scale),
                new Rectangle(64,392, 32,25),2f),

            };

            this.Tabs[0].Pages.Add(new MainEscPage(graphicsDevice, BackGroundSourceRectangle, this.Position));
            this.Tabs[1].Pages.Add(new SettingsPage(graphicsDevice, BackGroundSourceRectangle, this.Scale));

        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Game1.isMyMouseVisible = true;

            for (int i = 0; i < this.Tabs.Count; i++)
            {
                this.Tabs[i].Button.Update(Game1.myMouseManager);
                if (this.Tabs[i].Button.isClicked)
                {
                    this.ActiveTab = i;

                }
                if (this.ActiveTab == i)
                {
                    this.Tabs[i].IsActive = true;
                    this.Tabs[i].ButtonColorMultiplier = 1f;
                }
                else
                {
                    this.Tabs[i].IsActive = false;
                    this.Tabs[i].ButtonColorMultiplier = .5f;
                }
            }
            this.Tabs[this.ActiveTab].Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(Game1.Utility.CenterScreenX -200, Game1.Utility.CenterScreenY -200 , 224,304),
            //  new Rectangle(576, 48, 224, 304),Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .69f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position,
                BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth);

            for (int i = 0; i < this.Tabs.Count; i++)
            {

                this.Tabs[i].Button.DrawNormal(spriteBatch, this.Tabs[i].Button.Position, this.Tabs[i].Button.BackGroundSourceRectangle, Color.White * this.Tabs[i].ButtonColorMultiplier, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
            }

            this.Tabs[this.ActiveTab].Draw(spriteBatch, BackGroundSourceRectangle, this.Scale, false);


        }


    }

    class MainEscPage : IPage
    {
        internal Button MenuButton { get; set; }
        internal Button SettingsButton { get; set; }
        internal Button ReturnButton { get; set; }
        internal Button ToggleFullScreenButton { get; set; }
        List<Button> Buttons { get; set; }

        private string ReturnText { get; set; }
        private string SettingsText { get; set; }
        private string MenuText { get; set; }
        private string ToggleFullScreenButtonText;

        public Vector2 Position { get; private set; }


        public MainEscPage(GraphicsDevice graphicsDevice, Rectangle backgroundSourceRectangle, Vector2 Position)
        {
            this.ReturnButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(432, 16, 80, 48), graphicsDevice, new Vector2(Position.X + backgroundSourceRectangle.Width / 3, Position.Y + 16 * 2), CursorType.Normal, 2f);

            this.SettingsButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(432, 16, 80, 48), graphicsDevice, new Vector2(Position.X + backgroundSourceRectangle.Width / 3, Position.Y + 64 * 2), CursorType.Normal, 2f);

            this.MenuButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(432, 16, 80, 48), graphicsDevice, new Vector2(Position.X + backgroundSourceRectangle.Width / 3, Position.Y + 112 * 2), CursorType.Normal, 2f);
            this.ToggleFullScreenButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(432, 16, 80, 48), graphicsDevice, new Vector2(Position.X + backgroundSourceRectangle.Width / 3, Position.Y + 160 * 2), CursorType.Normal, 2f);

            this.MenuText = "Exit Game";
            this.SettingsText = "Save Game";
            this.ReturnText = "Return";
            ToggleFullScreenButtonText = "FullScreen Mode";



            this.Buttons = new List<Button>() { this.MenuButton, this.SettingsButton, this.ReturnButton, this.ToggleFullScreenButton };
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Buttons.Count; i++)
            {
                this.Buttons[i].Update(Game1.myMouseManager);
            }
            if (this.MenuButton.isClicked)
            {
                Game1.mainMenu.LoadBackGround();
                Game1.gameStages = Stages.MainMenu;
            }

            if (this.ReturnButton.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                // isTextChanged = false;
            }

            if (this.SettingsButton.isClicked)
            {
                //  mySave.Save();
                // isTextChanged = true;
            }

            if (this.ToggleFullScreenButton.isClicked)
            {
                Game1.FullScreenToggle();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.MenuButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.MenuText, this.MenuButton.FontLocation, Color.White, Utility.StandardButtonDepth + .01f, Utility.StandardButtonDepth + .02f, 2f);
            this.ReturnButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.ReturnText, this.ReturnButton.FontLocation, Color.White, Utility.StandardButtonDepth + .01f, Utility.StandardButtonDepth + .02f, 2f);
            this.SettingsButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.SettingsText, this.SettingsButton.FontLocation, Color.White, Utility.StandardButtonDepth + .01f, Utility.StandardButtonDepth + .02f, 2f);
            this.ToggleFullScreenButton.Draw(spriteBatch, Game1.AllTextures.MenuText, ToggleFullScreenButtonText, this.ToggleFullScreenButton.FontLocation, Color.White, Utility.StandardButtonDepth + .01f, Utility.StandardButtonDepth + .02f, 2f);

        }


    }

    class SettingsPage : IPage
    {
        public SliderBar VolumeSetting { get; set; }
        public SettingsPage(GraphicsDevice graphics, Rectangle backgroundSourceRectangle, float scale)
        {
            this.VolumeSetting = new SliderBar(graphics, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2 - 48, Game1.Utility.CenterScreenY - backgroundSourceRectangle.Height / 4), scale);
        }

        public void Update(GameTime gameTime)
        {
            Game1.SoundManager.GameVolume = this.VolumeSetting.Update(Game1.SoundManager.GameVolume);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Volume", new Vector2(this.VolumeSetting.SliderBackgroundPosition.X + 16 * this.VolumeSetting.Scale, this.VolumeSetting.SliderBackgroundPosition.Y - 80 * this.VolumeSetting.Scale), Color.Black, 0f, Game1.Utility.Origin, this.VolumeSetting.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
            this.VolumeSetting.Draw(spriteBatch, "Music: ");
        }


    }
}
