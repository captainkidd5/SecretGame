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
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.UI
{
    //TODO: Switch statements on buttons
    public enum buttonIsClicked
    {
        none = 0,
        menu = 1,
        inv = 2,
    }

    class ToolBar : IGeneral
    {
        //--------------------------------------
        //Textures
        public Texture2D Background { get; set; }
        public Button InGameMenu { get; set; }
        public Button OpenInventory { get; set; }
        public Button InvSlot1 { get; set; }
        public Texture2D ToolBarButton { get; set; }
        public SpriteFont Font { get; set; }
        public List<Button> AllButtons { get; set; }
        public MouseManager CustomMouse { get; set; }

        public Texture2D InvSlot1Texture { get; set; }

        public buttonIsClicked toolBarState = buttonIsClicked.none;
        Game1 game;
        GraphicsDevice graphicsDevice;
        ContentManager content;

        private int itemCounter1;
        public int ItemCounter1 { get { return itemCounter1; } set { itemCounter1 = value; } }

        Inventory inventory;


        public ToolBar(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, Inventory inventory)
        {

            this.CustomMouse = mouse;

            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            //--------------------------------------
            //initialize SpriteFonts
            Font = content.Load<SpriteFont>("SpriteFont/MenuText");

            //--------------------------------------
            //Initialize Textures
            this.ToolBarButton = content.Load<Texture2D>("Button/ToolBarButton");
            this.Background = content.Load<Texture2D>("Button/ToolBar");

            //
            ItemCounter1 = 0;

            //--------------------------------------
            //Initialize Buttons
            InGameMenu = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(367, 635));
            OpenInventory = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(433, 635));
            InvSlot1 = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(500, 635));

            //--------------------------------------
            //Button List Stuff
            AllButtons = new List<Button>();
            AllButtons.Add(OpenInventory);
            AllButtons.Add(InGameMenu);

            this.inventory = inventory;
  
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            //--------------------------------------
            //Draw Background
            spriteBatch.Draw(Background, new Vector2(320, 635));

            //--------------------------------------
            //Draw Buttons
            OpenInventory.Draw(spriteBatch, Font, "Inv", new Vector2(450, 660), Color.CornflowerBlue);
            InGameMenu.Draw(spriteBatch, Font, "Menu", new Vector2(377, 660), Color.CornflowerBlue);
            InvSlot1.Draw(spriteBatch, Font, ItemCounter1.ToString(), new Vector2(543, 670), Color.CadetBlue);

           // spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

            //--------------------------------------
            //Update Buttons

            InGameMenu.Update();
            OpenInventory.Update();
            this.itemCounter1 = inventory.ItemCount;
            /*
            switch (toolBarState)
            {
                case toolBarButtons.none:

                    break;


                case toolBarButtons.menu:
                    _inGameMenu.Update();
                    break;

                case toolBarButtons.inv:
                    _openInventory.Update(); 
                    break;

                default:

                    break;
            }
            */

            
            
            if (InGameMenu.isClicked)
            {
                UserInterface.IsEscMenu = !UserInterface.IsEscMenu;
            }
            else if(OpenInventory.isClicked)
            {

            }

            



            //--------------------------------------
            //Switch GameStages on click

            OpenInventory.isClicked = false;
            
        }
    }
}
