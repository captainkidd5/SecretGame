using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
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

        //change clothing buttons
        public Button HairFoward { get; set; }
        public Button HairBackward { get; set; }

        public Button ShirtForward { get; set; }
        public Button ShirtBackward { get; set; }

        public Button PantsForward { get; set; }
        public Button PantsBackward { get; set; }

        public Button ShoesForward { get; set; }
        public Button ShoesBackward { get; set; }

        public List<Button> CustomizationButtons { get; set; }

        public Vector2 PlayerPortraitDrawLocation { get; set; }

        public bool WasClothingChanged { get; private set; }
        public CharacterCreationMenu(GraphicsDevice graphics,SaveSlot saveSlot, Vector2 position)
        {
            this.CurrentSaveSlot = saveSlot;
            this.PlayerName = string.Empty;
            this.StartButtonString = "GO!";
            this.Scale = 3f;
            this.Graphics = graphics;
            this.Position = position;
            this.BackGroundSourceRectangle = new Rectangle(832, 496, 192, 192);
            this.CharacterPortraitWindow = new Rectangle(896, 416, 64, 80);


            
            this.TypingWindow = new TypingWindow(graphics, new Vector2(this.Position.X + 16 * this.Scale, this.Position.Y + this.BackGroundSourceRectangle.Height * this.Scale));
            this.StartNewGameButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 720, 64, 22),
                this.Graphics, new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width / 2 * this.Scale - 64 / 2 * this.Scale, this.Position.Y + this.BackGroundSourceRectangle.Height * this.Scale + TypingWindow.BackGroundSourceRectangle.Height * Scale), Controls.CursorType.Normal, 3f, null);

            Rectangle forwardRectangle = new Rectangle(384, 528, 32, 16);
            Rectangle backWardRectangle = new Rectangle(304, 528, 32, 16);

            this.PlayerPortraitDrawLocation = new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width / 2 * this.Scale - this.CharacterPortraitWindow.Width / 2 * Scale,
                this.Position.Y + this.BackGroundSourceRectangle.Height / 3 * this.Scale - this.CharacterPortraitWindow.Height / 2 * Scale);
            Vector2 customizationButtonPosition = new Vector2(PlayerPortraitDrawLocation.X + this.CharacterPortraitWindow.Width / 4 * Scale, PlayerPortraitDrawLocation.Y + this.CharacterPortraitWindow.Height  * this.Scale);
          
            this.HairFoward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y), CursorType.Normal, this.Scale);
            this.HairBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y), CursorType.Normal, this.Scale);

            this.ShirtForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 64), CursorType.Normal, this.Scale);
            this.ShirtBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 64), CursorType.Normal, this.Scale);

            this.PantsForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 128), CursorType.Normal, this.Scale);
            this.PantsBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 128), CursorType.Normal, this.Scale);

            this.ShoesForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 192), CursorType.Normal, this.Scale);
            this.ShoesBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 192), CursorType.Normal, this.Scale);

            this.CustomizationButtons = new List<Button>()
            {
                HairFoward,
                HairBackward,
                ShirtForward,
                ShirtBackward,
                PantsForward,
                PantsBackward,
                ShoesForward,
                ShoesBackward
            };




        }
        public void Update(GameTime gameTime)
        {
            this.WasClothingChanged = false;
            this.StartNewGameButton.Update(Game1.MouseManager);
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

            for(int i =0; i < this.CustomizationButtons.Count; i++)
            {
                this.CustomizationButtons[i].Update(Game1.MouseManager);
            }
           
            if (HairFoward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Hair, this.PlayerPortraitDrawLocation);
                WasClothingChanged = true;
            }
            else if (HairBackward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Hair, this.PlayerPortraitDrawLocation, true);
                WasClothingChanged = true;
            }
            else if (ShirtForward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Shirt, this.PlayerPortraitDrawLocation);
                WasClothingChanged = true;
            }
            else if (ShirtBackward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Shirt, this.PlayerPortraitDrawLocation, true);
                WasClothingChanged = true;
            }
            else if (PantsForward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Pants, this.PlayerPortraitDrawLocation);
                WasClothingChanged = true;
            }
            else if (PantsBackward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Pants, this.PlayerPortraitDrawLocation, true);
                WasClothingChanged = true;
            }
            else if (ShoesForward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Shoes, this.PlayerPortraitDrawLocation);
                WasClothingChanged = true;
            }
            else if (ShoesBackward.isClicked)
            {
                Game1.Player.PlayerWardrobe.CycleClothing(Playable.ClothingLayer.Shoes, this.PlayerPortraitDrawLocation, true);
                WasClothingChanged = true;
            }
            if(WasClothingChanged)
            {
                Game1.Player.PlayerWardrobe.UpdateMovementAnimations(this.PlayerPortraitDrawLocation, true);
            }
        }

        public void EnterGame()
        {
            Game1.Player.Name = char.ToUpper(this.PlayerName[0]) + this.PlayerName.Substring(1).ToLower();
            this.CurrentSaveSlot.StartNewSave();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i =0; i < Game1.Player.PlayerWardrobe.BasicMovementAnimations.GetLength(1); i++)
            {
                Game1.Player.PlayerWardrobe.BasicMovementAnimations[0, i].DrawScalableAnimation(spriteBatch, new Vector2(this.PlayerPortraitDrawLocation.X + 54, this.PlayerPortraitDrawLocation.Y + 16), .9f - .01f * i,0f,5f);
            }

            for (int i = 0; i < this.CustomizationButtons.Count; i++)
            {
                this.CustomizationButtons[i].DrawNormal(spriteBatch, CustomizationButtons[i].Position, CustomizationButtons[i].BackGroundSourceRectangle, CustomizationButtons[i].Color,
                0f, Game1.Utility.Origin, CustomizationButtons[i].HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth - .04f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,this.PlayerPortraitDrawLocation,this.CharacterPortraitWindow, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            this.TypingWindow.Draw(spriteBatch);
            this.StartNewGameButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.StartButtonString, StartNewGameButton.Position, StartNewGameButton.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, this.Scale);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.PlayerName, new Vector2(this.PlayerPortraitDrawLocation.X + 16, this.PlayerPortraitDrawLocation.Y + CharacterPortraitWindow.Height * Scale - 48), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth + .04f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Hair", new Vector2(HairFoward.Position.X - 96, HairFoward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Shirt", new Vector2(ShirtForward.Position.X - 96, ShirtForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Pants", new Vector2(PantsForward.Position.X - 96, PantsForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Shoes", new Vector2(ShoesForward.Position.X - 96, ShoesForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }
    }
}
