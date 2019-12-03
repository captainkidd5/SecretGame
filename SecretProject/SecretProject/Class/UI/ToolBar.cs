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
    }

    public class ToolBar
    {

        public Button InGameMenu { get; set; }
        public Button OpenInventory { get; set; }
        public Button ScrollTree { get; set; }


        public Texture2D ToolBarButton { get; set; }

        public List<Button> AllNonInventoryButtons { get; set; }


        public Rectangle BackGroundTextureRectangle { get; set; }


        public buttonIsClicked toolBarState = buttonIsClicked.none;
        GraphicsDevice graphicsDevice;
        ContentManager content;

        public Vector2 BackGroundTexturePosition { get; set; }


        TextBuilder TextBuilder;

        public bool IsActive { get; set; } = true;



        public ToolBar(GraphicsDevice graphicsDevice, BackPack backPack, ContentManager content)
        {
            this.IsActive = true;
            BackGroundTexturePosition = new Vector2(320, 635);


            this.graphicsDevice = graphicsDevice;
   
            this.content = content;

            //--------------------------------------
            //Initialize Textur


            InGameMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(80, 80, 64, 64), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .2f, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal);
            OpenInventory = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(192, 16, 32, 32), graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .25f, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal);
            ScrollTree = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(192, 16, 32, 32), graphicsDevice, new Vector2(200, 645), CursorType.Normal);






            AllNonInventoryButtons = new List<Button>()
            {
                OpenInventory,
                InGameMenu,
                ScrollTree,

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
            OpenInventory.isClicked = false;
            for (int i = 0; i < AllNonInventoryButtons.Count; i++)
            {
                AllNonInventoryButtons[i].Update(mouse);
            }

            if (InGameMenu.isClicked)
            {

                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {


                TextBuilder.Draw(spriteBatch, .75f);



                OpenInventory.Draw(spriteBatch, Game1.AllTextures.MenuText, "Inv", OpenInventory.Position, Color.CornflowerBlue, .69f, .7f);
                InGameMenu.Draw(spriteBatch, Game1.AllTextures.MenuText, "Menu", InGameMenu.Position, Color.CornflowerBlue, .69f, .7f);


            }
        }

    }


}
