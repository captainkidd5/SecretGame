using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public class CharacterCreationMenu
    {
        public SaveSlot CurrentSaveSlot { get; set; }
        public string StartButtonString { get; set; }
        public string PlayerName { get; set; }
        public float Scale { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Rectangle CharacterPortraitWindow { get; set; }
        public TypingWindow TypingWindow { get; set; }

        public Button StartNewGameButton { get; set; }
        public CharacterCreationMenu(GraphicsDevice graphics,SaveSlot saveSlot, Vector2 position)
        {
            this.CurrentSaveSlot = saveSlot;
            this.PlayerName = string.Empty;
            this.StartButtonString = "GO!";
            this.Scale = 3f;
            this.Graphics = graphics;
            this.Position = position;
            this.BackGroundSourceRectangle = new Rectangle(832, 496, 192, 160);
            this.CharacterPortraitWindow = new Rectangle(896, 432, 64, 64);
            this.TypingWindow = new TypingWindow(graphics, position);

            this.StartNewGameButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 656, 64,22), this.Graphics, new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width /2 * this.Scale - 64 / 2 * this.Scale, this.Position.Y + this.BackGroundSourceRectangle.Height *this.Scale - 22 /2 * this.Scale + 32), Controls.CursorType.Normal, 3f, null);
        }
        public void Update(GameTime gameTime)
        {
            Game1.Player.position = this.Position;
            this.StartNewGameButton.Update(Game1.myMouseManager);
            this.TypingWindow.Update(gameTime);
            this.PlayerName = TypingWindow.EnteredString;

            if(this.StartNewGameButton.isClicked)
            {
                if(this.PlayerName == string.Empty)
                {
                    Game1.mainMenu.AddAlert(AlertType.Normal, AlertSize.Medium, Game1.Utility.CenterRectangleOnScreen(new Rectangle(0, 0, 64, 32), 2f), "Please enter a name");
                    return;
                }
                Action negativeAction = new Action(Game1.mainMenu.ReturnToDefaultState);
                Action action = new Action(EnterGame);

                Game1.mainMenu.AddAlert(AlertType.Confirmation, AlertSize.Large, Game1.Utility.centerScreen, "Start new game?", action, negativeAction);
            }
        }

        public void EnterGame()
        {
            Game1.Player.Name = char.ToUpper(this.PlayerName[0]) + this.PlayerName.Substring(1).ToLower();
            this.CurrentSaveSlot.StartNewSave();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Game1.Player.Draw(spriteBatch, 1f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardTextDepth - .04f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width /2 * this.Scale - this.CharacterPortraitWindow.Width /2 * Scale,
                this.Position.Y + this.BackGroundSourceRectangle.Height/ 2 * this.Scale - this.CharacterPortraitWindow.Height / 2 * Scale),this.CharacterPortraitWindow, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            this.TypingWindow.Draw(spriteBatch);
            this.StartNewGameButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.StartButtonString, StartNewGameButton.Position, StartNewGameButton.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, this.Scale);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.PlayerName, new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width / 2 * this.Scale - this.CharacterPortraitWindow.Width / 2 * Scale,
                this.Position.Y + this.BackGroundSourceRectangle.Height / 2 * this.Scale - this.CharacterPortraitWindow.Height / 2 * Scale -32), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }
    }
}
