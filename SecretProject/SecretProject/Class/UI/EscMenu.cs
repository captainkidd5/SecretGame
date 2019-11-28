using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;

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

        public EscMenu(GraphicsDevice graphicsDevice, ContentManager content)
        {
            mySave = new SaveLoadManager();



            Scale = 2f;

            BackGroundSourceRectangle = new Rectangle(64, 416, 128, 224);

           
            this.IsActive = false;
            this.FreezesGame = true;
            this.Tabs = new List<CategoryTab>()
            {
                new CategoryTab("Esc", graphicsDevice,new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height - 25 * Scale),
                new Rectangle(64,392, 32,25),2f),
                new CategoryTab("Settings", graphicsDevice,new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width + 64, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height - 25 * Scale),
                new Rectangle(64,392, 32,25),2f),

            };

            Tabs[0].Pages.Add(new MainEscPage(graphicsDevice,BackGroundSourceRectangle));
            Tabs[1].Pages.Add(new SettingsPage(graphicsDevice, BackGroundSourceRectangle,Scale));

        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Game1.isMyMouseVisible = true;

            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Button.Update(Game1.myMouseManager);
                if (Tabs[i].Button.isClicked)
                {
                    this.ActiveTab = i;

                }
                if (ActiveTab == i)
                {
                    Tabs[i].IsActive = true;
                    Tabs[i].ButtonColorMultiplier = 1f;
                }
                else
                {
                    Tabs[i].IsActive = false;
                    Tabs[i].ButtonColorMultiplier = .5f;
                }
            }
            Tabs[ActiveTab].Update(gameTime);
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(Game1.Utility.CenterScreenX -200, Game1.Utility.CenterScreenY -200 , 224,304),
            //  new Rectangle(576, 48, 224, 304),Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .69f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(Game1.PresentationParameters.BackBufferWidth / 2 - BackGroundSourceRectangle.Width, Game1.PresentationParameters.BackBufferHeight / 2 - BackGroundSourceRectangle.Height),
                BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            for (int i = 0; i < Tabs.Count; i++)
            {
                
                Tabs[i].Button.DrawNormal(spriteBatch, Tabs[i].Button.Position, Tabs[i].Button.BackGroundSourceRectangle, Color.White * Tabs[i].ButtonColorMultiplier, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }

            Tabs[ActiveTab].Draw(spriteBatch, BackGroundSourceRectangle, Scale, false);


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


        public MainEscPage(GraphicsDevice graphicsDevice, Rectangle backgroundSourceRectangle)
        {
            ReturnButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2, Game1.Utility.CenterScreenY - 150), CursorType.Normal);

            SettingsButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2, Game1.Utility.CenterScreenY - 90), CursorType.Normal);

            MenuButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2, Game1.Utility.CenterScreenY), CursorType.Normal);
            ToggleFullScreenButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2, Game1.Utility.CenterScreenY + 90), CursorType.Normal);

            MenuText = "Exit Game";
            SettingsText = "Save Game";
            ReturnText = "Return";
            ToggleFullScreenButtonText = "FullScreen Mode";



            Buttons = new List<Button>() { MenuButton, SettingsButton, ReturnButton, ToggleFullScreenButton };
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Update(Game1.myMouseManager);
            }
            if (MenuButton.isClicked)
            {
                Game1.mainMenu.LoadBackGround();
                Game1.gameStages = Stages.MainMenu;
            }

            if (ReturnButton.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
               // isTextChanged = false;
            }

            if (SettingsButton.isClicked)
            {
              //  mySave.Save();
               // isTextChanged = true;
            }

            if (ToggleFullScreenButton.isClicked)
            {
                Game1.FullScreenToggle();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MenuButton.Draw(spriteBatch, Game1.AllTextures.MenuText, MenuText, MenuButton.FontLocation, Color.BlueViolet, Game1.Utility.StandardButtonDepth + .01f, Game1.Utility.StandardButtonDepth + .02f);
            ReturnButton.Draw(spriteBatch, Game1.AllTextures.MenuText, ReturnText, ReturnButton.FontLocation, Color.BlueViolet, Game1.Utility.StandardButtonDepth + .01f, Game1.Utility.StandardButtonDepth + .02f);
            SettingsButton.Draw(spriteBatch, Game1.AllTextures.MenuText, SettingsText, SettingsButton.FontLocation, Color.BlueViolet, Game1.Utility.StandardButtonDepth + .01f, Game1.Utility.StandardButtonDepth + .02f);
            ToggleFullScreenButton.Draw(spriteBatch, Game1.AllTextures.MenuText, ToggleFullScreenButtonText, ToggleFullScreenButton.FontLocation, Color.BlueViolet, Game1.Utility.StandardButtonDepth + .01f, Game1.Utility.StandardButtonDepth + .02f);

        }


    }

    class SettingsPage : IPage
    {
        public SliderBar VolumeSetting { get; set; }
        public SettingsPage(GraphicsDevice graphics,Rectangle backgroundSourceRectangle, float scale)
        {
            VolumeSetting = new SliderBar(graphics, new Vector2(Game1.Utility.centerScreen.X - backgroundSourceRectangle.Width / 2 - 48, Game1.Utility.CenterScreenY - backgroundSourceRectangle.Height/4), scale);
        }

        public void Update(GameTime gameTime)
        {
           Game1.SoundManager.GameVolume = VolumeSetting.Update(Game1.SoundManager.GameVolume);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Volume",new Vector2( VolumeSetting.SliderBackgroundPosition.X + 16 * VolumeSetting.Scale, VolumeSetting.SliderBackgroundPosition.Y - 80 * VolumeSetting.Scale), Color.Black, 0f, Game1.Utility.Origin, VolumeSetting.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            VolumeSetting.Draw(spriteBatch, "Music: ");
        }

        
    }
}
