﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public enum CycleDirection
    {
        Backward = -1,
        Forward = 1
    }
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

        public Button ShoeColorForward { get; set; }
        public Button ShoeColorBackward { get; set; }

        public Button SkinToneForward { get; set; }
        public Button SkinToneBackWard { get; set; }

        public Button HairColorForward { get; set; }
        public Button HairColorBackward { get; set; }

        public Button EyeColorForward { get; set; }
        public Button EyeColorBackward { get; set; }

        public List<Button> CustomizationButtons { get; set; }

        public Vector2 PlayerPortraitDrawLocation { get; set; }

        public bool WasClothingChanged { get; private set; }

        public Vector2 PlayerPosition { get; set; }
        public CharacterCreationMenu(GraphicsDevice graphics,SaveSlot saveSlot)
        {
            this.CurrentSaveSlot = saveSlot;
            this.PlayerName = string.Empty;
            this.StartButtonString = "GO!";
            this.Scale = 3f;
            this.Graphics = graphics;
            
            this.BackGroundSourceRectangle = new Rectangle(832, 496, 192, 192);
            this.CharacterPortraitWindow = new Rectangle(896, 416, 64, 80);
            this.Position = Game1.Utility.CenterRectangleInRectangle(Game1.ScreenRectangle, this.BackGroundSourceRectangle, Game1.Utility.Origin, 1f, this.Scale);



            this.TypingWindow = new TypingWindow(graphics, new Vector2(this.Position.X + 16 * this.Scale, this.Position.Y + this.BackGroundSourceRectangle.Height * this.Scale));
            this.StartNewGameButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 720, 64, 22),
                this.Graphics, new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width / 2 * this.Scale - 64 / 2 * this.Scale, this.Position.Y + this.BackGroundSourceRectangle.Height * this.Scale + TypingWindow.BackGroundSourceRectangle.Height * Scale), Controls.CursorType.Normal, 3f, null);

            Rectangle forwardRectangle = new Rectangle(384, 528, 32, 16);
            Rectangle backWardRectangle = new Rectangle(304, 528, 32, 16);

            this.PlayerPortraitDrawLocation = new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width / 2 * this.Scale - this.CharacterPortraitWindow.Width / 2 * Scale,
                this.Position.Y + this.BackGroundSourceRectangle.Height / 3 * this.Scale - this.CharacterPortraitWindow.Height / 2 * Scale);
            Vector2 customizationButtonPosition = new Vector2(PlayerPortraitDrawLocation.X + this.CharacterPortraitWindow.Width / 4 * Scale, PlayerPortraitDrawLocation.Y + this.CharacterPortraitWindow.Height  * this.Scale);

            this.PlayerPosition = new Vector2(PlayerPortraitDrawLocation.X + 48 , PlayerPortraitDrawLocation.Y);
            this.HairFoward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y), CursorType.Normal, this.Scale - 1);
            this.HairBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y), CursorType.Normal, this.Scale - 1);

            this.ShirtForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 32), CursorType.Normal, this.Scale - 1);
            this.ShirtBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 32), CursorType.Normal, this.Scale - 1);

            this.PantsForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 64), CursorType.Normal, this.Scale - 1);
            this.PantsBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 64), CursorType.Normal, this.Scale - 1);

            this.ShoeColorForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 96), CursorType.Normal, this.Scale - 1);
            this.ShoeColorBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 96), CursorType.Normal, this.Scale - 1);

            this.SkinToneForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
              new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 128), CursorType.Normal, this.Scale - 1);
            this.SkinToneBackWard = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 128), CursorType.Normal, this.Scale - 1);

            this.HairColorForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
             new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 160), CursorType.Normal, this.Scale - 1);
            this.HairColorBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 160), CursorType.Normal, this.Scale - 1);

            this.EyeColorForward = new Button(Game1.AllTextures.UserInterfaceTileSet, forwardRectangle, this.Graphics,
             new Vector2(customizationButtonPosition.X + 96, customizationButtonPosition.Y + 192), CursorType.Normal, this.Scale - 1);
            this.EyeColorBackward = new Button(Game1.AllTextures.UserInterfaceTileSet, backWardRectangle, this.Graphics,
               new Vector2(customizationButtonPosition.X - 96, customizationButtonPosition.Y + 192), CursorType.Normal, this.Scale - 1);

            this.CustomizationButtons = new List<Button>()
            {
                HairFoward,
                HairBackward,
                ShirtForward,
                ShirtBackward,
                PantsForward,
                PantsBackward,
                ShoeColorForward,
                ShoeColorBackward,
                SkinToneForward,
                SkinToneBackWard,
                HairColorForward,
                HairColorBackward,
                EyeColorForward,
                EyeColorBackward,
            };




        }
        public void Update(GameTime gameTime)
        {
            this.WasClothingChanged = false;
            this.StartNewGameButton.Update();
            this.TypingWindow.Update(gameTime);
            this.PlayerName = TypingWindow.EnteredString;
            Game1.Player.Wardrobe.UpdateForCreationMenu();
            Game1.Player.Wardrobe.UpdateAnimations(gameTime, this.PlayerPosition, Dir.Down, false);

            if (this.StartNewGameButton.isClicked)
            {
                if(this.PlayerName == string.Empty)
                {
                    Game1.mainMenu.AddAlert(AlertType.Normal, Game1.Utility.CenterRectangleOnScreen(new Rectangle(0, 0, 64, 32), 2f), "Please enter a name");
                    return;
                }
                Action negativeAction = new Action(Game1.mainMenu.ReturnToDefaultState);
                Action action = new Action(EnterGame);

                Game1.mainMenu.AddAlert(AlertType.Confirmation, Game1.Utility.centerScreen, "Start new game?", action, negativeAction);
            }

            for(int i =0; i < this.CustomizationButtons.Count; i++)
            {
                this.CustomizationButtons[i].Update();
            }
           
            if (HairFoward.isClicked)
            {
                Game1.Player.Wardrobe.Hair.Cycle(CycleDirection.Forward);
                WasClothingChanged = true;
            }
            else if (HairBackward.isClicked)
            {
                Game1.Player.Wardrobe.Hair.Cycle(CycleDirection.Backward);
                WasClothingChanged = true;
            }
            else if (ShirtForward.isClicked)
            {
                Game1.Player.Wardrobe.ShirtPiece.Cycle(CycleDirection.Forward);
                WasClothingChanged = true;
            }
            else if (ShirtBackward.isClicked)
            {

                Game1.Player.Wardrobe.ShirtPiece.Cycle(CycleDirection.Backward);
                WasClothingChanged = true;
            }
            else if (PantsForward.isClicked)
            {
           //     Game1.Player.Wardrobe.PantsPiece.Cycle(CycleDirection.Forward);

               // WasClothingChanged = true;
            }
            else if (PantsBackward.isClicked)
            {
               // Game1.Player.Wardrobe.PantsPiece.Cycle(CycleDirection.Backward);
              //  WasClothingChanged = true;
            }
            else if (ShoeColorForward.isClicked)
            {
                Game1.Player.Wardrobe.ShoesPiece.ChangeShoeColor(CycleDirection.Forward);
             //   Game1.Player.Wardrobe.ShoesPiece.Cycle(CycleDirection.Forward);
              //  WasClothingChanged = true;
            }
            else if (ShoeColorBackward.isClicked)
            {
                // Game1.Player.Wardrobe.ShoesPiece.Cycle(CycleDirection.Backward);
                //  WasClothingChanged = true;
                Game1.Player.Wardrobe.ShoesPiece.ChangeShoeColor(CycleDirection.Backward);
            }
            else if(SkinToneForward.isClicked)
            {
                Game1.Player.Wardrobe.ChangeSkin(CycleDirection.Forward);
            }
            else if(SkinToneBackWard.isClicked)
            {
                Game1.Player.Wardrobe.ChangeSkin(CycleDirection.Backward);
            }
            else if(HairColorForward.isClicked)
            {
                Game1.Player.Wardrobe.ChangeHairColor(CycleDirection.Forward);
            }
            else if(HairColorBackward.isClicked)
            {
                Game1.Player.Wardrobe.ChangeHairColor(CycleDirection.Backward);
            }
            else if(EyeColorForward.isClicked)
            {
                Game1.Player.Wardrobe.EyePiece.ChangeEyeColor(CycleDirection.Forward);
            }
            else if (EyeColorBackward.isClicked)
            {
                Game1.Player.Wardrobe.EyePiece.ChangeEyeColor(CycleDirection.Backward);
            }
            if (WasClothingChanged)
            {
               // Game1.Player.Wardrobe.UpdateMovementAnimations(this.PlayerPortraitDrawLocation, true, Dir.Down);
            }
        }

        public void EnterGame()
        {
            Game1.Player.Name = char.ToUpper(this.PlayerName[0]) + this.PlayerName.Substring(1).ToLower();
            Game1.Player.Wardrobe.SetScale(1);
            this.CurrentSaveSlot.StartNewSave();
        }
        public void Draw(SpriteBatch spriteBatch)
        {


            Game1.Player.Wardrobe.DrawForCreationMenu(spriteBatch);

            for (int i = 0; i < this.CustomizationButtons.Count; i++)
            {
                this.CustomizationButtons[i].DrawNormal(spriteBatch, CustomizationButtons[i].Position, CustomizationButtons[i].BackGroundSourceRectangle, CustomizationButtons[i].Color,
                0f, Game1.Utility.Origin, CustomizationButtons[i].HitBoxScale, SpriteEffects.None,Utility.StandardButtonDepth);
            }

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardTextDepth - .04f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,this.PlayerPortraitDrawLocation,this.CharacterPortraitWindow, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardTextDepth);
            this.TypingWindow.Draw(spriteBatch);
            this.StartNewGameButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.StartButtonString, StartNewGameButton.Position, StartNewGameButton.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, this.Scale);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.PlayerName, new Vector2(this.PlayerPortraitDrawLocation.X + 16, this.PlayerPortraitDrawLocation.Y + CharacterPortraitWindow.Height * Scale - 48), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth + .04f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Hair", new Vector2(HairFoward.Position.X - 96, HairFoward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Shirt", new Vector2(ShirtForward.Position.X - 96, ShirtForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Pants", new Vector2(PantsForward.Position.X - 96, PantsForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Shoes", new Vector2(ShoeColorForward.Position.X - 96, ShoeColorForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Skin", new Vector2(SkinToneForward.Position.X - 96, SkinToneForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Hair Color", new Vector2(HairColorForward.Position.X - 96, HairColorForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Eye Color", new Vector2(EyeColorForward.Position.X - 96, EyeColorForward.Position.Y + 4), Color.White, 0f, Game1.Utility.Origin, this.Scale - 1, SpriteEffects.None, Utility.StandardTextDepth);
        }
    }
}
