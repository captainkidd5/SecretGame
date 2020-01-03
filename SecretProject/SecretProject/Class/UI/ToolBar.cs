using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;

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
            BackGroundTexturePosition = new Vector2(320, 635);


            this.graphicsDevice = graphicsDevice;
   
            this.content = content;

            //--------------------------------------
            //Initialize Textur


            InGameMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(720, 128, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            { ItemSourceRectangleToDraw = new Rectangle(16,128,32,32)};
            OpenCraftingMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(720, 128, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f - 64, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            { ItemSourceRectangleToDraw = new Rectangle(20, 96, 32, 32) };
            OpenSanctuaryMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(720, 128, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f - 128, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal, 2f)
            {
                ItemSourceRectangleToDraw = new Rectangle(48, 96, 32, 32)
            };
            GoldIcon = new Rectangle(16, 320, 32, 32);
            GoldIconPosition = new Vector2(backPack.SmallPosition.X + backPack.SmallBackgroundSourceRectangle.Width * backPack.Scale + 80, backPack.SmallPosition.Y);
            GoldIconBackGroundSourceRectangle = new Rectangle(128, 688, 80, 32);

            Scale = 2f;




            AllNonInventoryButtons = new List<Button>()
            {

                InGameMenu,
                OpenCraftingMenu,
                OpenSanctuaryMenu

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
            for (int i = 0; i < AllNonInventoryButtons.Count; i++)
            {
                AllNonInventoryButtons[i].Update(mouse);
            }

            if (InGameMenu.isClicked)
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
            if(OpenCraftingMenu.isClicked)
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
            if(OpenSanctuaryMenu.isClicked)
            {
                if(Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
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

        public void Draw(SpriteBatch spriteBatch, int goldAmt)
        {
            if (this.IsActive)
            {


                TextBuilder.Draw(spriteBatch, .75f);


                InGameMenu.Draw(spriteBatch, InGameMenu.ItemSourceRectangleToDraw, InGameMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", InGameMenu.Position, Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth + .01f,true);

                OpenCraftingMenu.Draw(spriteBatch, OpenCraftingMenu.ItemSourceRectangleToDraw, OpenCraftingMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", OpenCraftingMenu.Position, Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth + .01f, true);


                OpenSanctuaryMenu.Draw(spriteBatch, OpenSanctuaryMenu.ItemSourceRectangleToDraw, OpenSanctuaryMenu.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
                    "", OpenSanctuaryMenu.Position, Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth + .01f, true);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, GoldIconPosition, GoldIconBackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(GoldIconPosition.X + 112, GoldIconPosition.Y), GoldIcon, Color.White, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, goldAmt.ToString(), new Vector2(GoldIconPosition.X + 16, GoldIconPosition.Y + 16), Color.White, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);

            }
        }

    }


}
