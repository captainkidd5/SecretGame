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
using SecretProject.Class.Stage;

namespace SecretProject.Class.UI
{
    //TODO: Switch statements on buttons
    public enum buttonIsClicked
    {
        none = 0,
        menu = 1,
        inv = 2,
    }

    class ToolBar
    {
        //--------------------------------------
        //Textures
        public Texture2D Background { get; set; }
        public Button InGameMenu { get; set; }
        public Button OpenInventory { get; set; }
        public Button InvSlot1 { get; set; }
        public Button InvSlot2 { get; set; }
        public Button InvSlot3 { get; set; }
        public Button InvSlot4 { get; set; }
        public Texture2D ToolBarButton { get; set; }
        public SpriteFont Font { get; set; }
        public List<Button> AllButtons { get; set; }
        public List<Button> AllSlots { get; set; }
        public MouseManager CustomMouse { get; set; }

        public Texture2D InvSlot1Texture { get; set; }
        public Texture2D InvSlot2Texture { get; set; }
        public Texture2D InvSlot3Texture { get; set; }
        public Texture2D InvSlot4Texture { get; set; }

        public Rectangle BackGroundTextureRectangle { get; set; }

        public bool MouseOverToolBar { get; set; }

        public buttonIsClicked toolBarState = buttonIsClicked.none;
        Game1 game;
        GraphicsDevice graphicsDevice;
        ContentManager content;

        public Vector2 BackGroundTexturePosition { get; set; }


        Inventory inventory;

        public bool Added { get; set; } = false;

        public ToolBar(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            BackGroundTexturePosition = new Vector2(320, 635);


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

            this.BackGroundTextureRectangle = new Rectangle((int)BackGroundTexturePosition.X, (int)BackGroundTexturePosition.Y, Background.Width, Background.Height);

            //


            //--------------------------------------
            //Initialize Buttons
            InGameMenu = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(367, 635));
            OpenInventory = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(433, 635));
            InvSlot1 = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(500, 635)) { ItemCounter = 0 };
            InvSlot2 = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(577, 635)) { ItemCounter = 0 };
            InvSlot3 = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(630, 635)) { ItemCounter = 0 };
            InvSlot4 = new Button(ToolBarButton, graphicsDevice, CustomMouse, new Vector2(690, 635)) { ItemCounter = 0 };

            //--------------------------------------
            //Button List Stuff
            AllButtons = new List<Button>();
            AllSlots = new List<Button>()
            {
                InvSlot1,
                InvSlot2,
                InvSlot3,
                InvSlot4
            };
            AllButtons.Add(OpenInventory);
            AllButtons.Add(InGameMenu);


  
        }


        

        public void Update(GameTime gameTime, Inventory inventory)
        {

            //--------------------------------------
            //Update Buttons

            this.inventory = inventory;

            for(int i = 0; i < 4; i ++)
            {
                if (inventory.currentInventory.ElementAt(i) == null)
                {
                    AllSlots[i].ItemCounter = 0;
                }
                else
                {
                    AllSlots[i].ItemCounter = inventory.currentInventory.ElementAt(i).SlotItems.Count;
                }
                
                if(AllSlots[i].ItemCounter != 0)
                {
                    AllSlots[i].Texture = inventory.currentInventory.ElementAt(i).SlotItems[0].Texture;
                }
                else
                {
                    AllSlots[i].Texture = ToolBarButton;
                }

                AllSlots[i].Update();
            }


            foreach(Button button in AllButtons)
            {
                button.Update();


            }
            if(CustomMouse.IsHovering(BackGroundTextureRectangle))
            {
                MouseOverToolBar = true;
            }
            if(!CustomMouse.IsHovering(BackGroundTextureRectangle))
            {
                MouseOverToolBar = false;
            }


            if (InGameMenu.isClicked)
            {
                UserInterface.IsEscMenu = !UserInterface.IsEscMenu;
            }
            else if (OpenInventory.isClicked)
            {

            }
            else if (InvSlot1.isClickedAndHeld)
            {             
                if (Added == false)
                {
                    Iliad.allSprites.Add(new SpriteFolder.Sprite(graphicsDevice, content, Game1.Player.Inventory.currentInventory.ElementAt(0).SlotItems[0].Texture, CustomMouse.WorldMousePosition, false) { IsBeingDragged = true });
                    Added = true;
                }
            }

            //--------------------------------------
            //Switch GameStages on click

            OpenInventory.isClicked = false;
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            //--------------------------------------
            //Draw Background
            spriteBatch.Draw(Background, BackGroundTexturePosition);

            //--------------------------------------
            //Draw Buttons
            OpenInventory.Draw(spriteBatch, Font, "Inv", new Vector2(450, 660), Color.CornflowerBlue);
            InGameMenu.Draw(spriteBatch, Font, "Menu", new Vector2(377, 660), Color.CornflowerBlue);
            InvSlot1.Draw(spriteBatch, Font, InvSlot1.ItemCounter.ToString(), new Vector2(543, 670), Color.DarkRed);
            InvSlot2.Draw(spriteBatch, Font, InvSlot2.ItemCounter.ToString(), new Vector2(600, 670), Color.DarkRed);
            InvSlot3.Draw(spriteBatch, Font, InvSlot3.ItemCounter.ToString(), new Vector2(660, 670), Color.DarkRed);
            InvSlot4.Draw(spriteBatch, Font, InvSlot4.ItemCounter.ToString(), new Vector2(720, 670), Color.DarkRed);
           

            // spriteBatch.End();
        }
    }
}
