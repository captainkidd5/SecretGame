using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI
{
    //TODO: Switch statements on buttons
    public enum buttonIsClicked
    {
        none = 0,
        menu = 1,
        inv = 2,
        scrollTree = 3,
        CraftingMenu = 4
    }

    public class ToolBar
    {

        public Button InGameMenu { get; set; }
        public Button OpenCraftingMenu { get; set; }
        public Button OpenSanctuaryMenu { get; set; }


        public Texture2D ToolBarButton { get; set; }

        public List<Button> AllNonInventoryButtons { get; set; }


        public Rectangle BackGroundTextureRectangle { get; set; }


        public buttonIsClicked toolBarState = buttonIsClicked.none;
        GraphicsDevice graphicsDevice;
        ContentManager content;

        public Vector2 BackGroundTexturePosition { get; set; }


        TextBuilder TextBuilder;

        public bool IsActive { get; set; } = true;

        public Rectangle GoldIcon { get; set; }
        public Rectangle GoldIconBackGroundSourceRectangle { get; set; }
        public Vector2 GoldIconPosition { get; set; }
        public float Scale { get; set; }

        public ToolBar(GraphicsDevice graphicsDevice, BackPack backPack, ContentManager content)
        {
            this.IsActive = true;
            this.BackGroundTexturePosition = new Vector2(320, 635);


            this.graphicsDevice = graphicsDevice;

            this.content = content;

            //--------------------------------------
            //Initialize Textur


            this.InGameMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            { ItemSourceRectangleToDraw = new Rectangle(16, 128, 32, 32) };
            this.OpenCraftingMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f - 64, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            { ItemSourceRectangleToDraw = new Rectangle(20, 96, 32, 32) };
            this.OpenSanctuaryMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f - 128, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            {
                ItemSourceRectangleToDraw = new Rectangle(48, 96, 32, 32)
            };
            this.GoldIcon = new Rectangle(16, 320, 32, 32);
            this.GoldIconPosition = new Vector2(backPack.SmallPosition.X + backPack.SmallBackgroundSourceRectangle.Width * backPack.Scale + 80, backPack.SmallPosition.Y);
            this.GoldIconBackGroundSourceRectangle = new Rectangle(128, 688, 80, 32);

            this.Scale = 2f;




            this.AllNonInventoryButtons = new List<Button>()
            {

                this.InGameMenu,
                this.OpenCraftingMenu,
                this.OpenSanctuaryMenu

            };

            TextBuilder = new TextBuilder("", .01f, 5);

        }


        public void Update(GameTime gameTime, Inventory inventory, MouseManager mouse)
        {


            TextBuilder.Update(gameTime);


            UpdateNonInventoryButtons(mouse);

        }

        public void UpdateNonInventoryButtons(MouseManager mouse)
        {
            for (int i = 0; i < this.AllNonInventoryButtons.Count; i++)
            {
                this.AllNonInventoryButtons[i].Update(mouse);
            }
            if (InGameMenu.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                //Game1.Player.UserInterface.InfoBox.FitText("Settings (esc)", 2f);
                //Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(InGameMenu.Position.X, InGameMenu.Position.Y - 128);

                if (this.InGameMenu.isClicked)
                {

                    if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
                    {
                        Game1.SoundManager.PlayOpenUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;
                    }
                    else
                    {
                        Game1.SoundManager.PlayCloseUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                }
            }
            if (this.OpenCraftingMenu.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.FitText("Crafting Menu (b)", 2f);
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(OpenCraftingMenu.Position.X, OpenCraftingMenu.Position.Y - 128);

                if (this.OpenCraftingMenu.isClicked)
                {
                    if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
                    {
                        Game1.SoundManager.PlayOpenUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CraftingMenu;
                    }
                    else
                    {
                        Game1.SoundManager.PlayCloseUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                }
            }
            if (this.OpenSanctuaryMenu.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.FitText("Sanctuary Log (z)", 2f);
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(OpenSanctuaryMenu.Position.X, OpenSanctuaryMenu.Position.Y - 128);

                if (this.OpenSanctuaryMenu.isClicked)
                {
                    if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
                    {
                        Game1.SoundManager.PlayOpenUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CompletionHub;
                    }
                    else
                    {
                        Game1.SoundManager.PlayCloseUI();
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, int goldAmt)
        {
            if (this.IsActive)
            {


                TextBuilder.Draw(spriteBatch, .75f);


                this.InGameMenu.Draw(spriteBatch, this.InGameMenu.ItemSourceRectangleToDraw, this.InGameMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", this.InGameMenu.Position, Color.White, 2f, 2f, Utility.StandardButtonDepth + .01f, true);

                this.OpenCraftingMenu.Draw(spriteBatch, this.OpenCraftingMenu.ItemSourceRectangleToDraw, this.OpenCraftingMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", this.OpenCraftingMenu.Position, Color.White, 2f, 2f, Utility.StandardButtonDepth + .01f, true);


                this.OpenSanctuaryMenu.Draw(spriteBatch, this.OpenSanctuaryMenu.ItemSourceRectangleToDraw, this.OpenSanctuaryMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", this.OpenSanctuaryMenu.Position, Color.White, 2f, 2f, Utility.StandardButtonDepth + .01f, true);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.GoldIconPosition, this.GoldIconBackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.GoldIconPosition.X + 112, this.GoldIconPosition.Y), this.GoldIcon, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, goldAmt.ToString(), new Vector2(this.GoldIconPosition.X + 16, this.GoldIconPosition.Y + 16), Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);

            }
        }

    }


}
