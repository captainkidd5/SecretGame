using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Playable;

namespace SecretProject.Class.UI
{
    public enum OpenShop
    {
        None = 0,
        ToolShop = 1
        //LodgeInteior = 1,
        //Iliad = 2,
        //Exit = 3,
        //Sea = 4,
        //RoyalDock = 5
    }
    public class UserInterface
    {
        ContentManager content;

        static bool isEscMenu;

        public bool IsEscMenu { get { return isEscMenu; } set { isEscMenu = value; } }
        public bool IsShopMenu { get; set; }

        public bool DrawTileSelector { get; set; } = true;

        public GraphicsDevice GraphicsDevice { get; set; }
        public Game1 Game { get; set; }
        public EscMenu Esc { get; set; }
        //public ShopMenu ShopMenu { get; set; }
        internal ToolBar BottomBar { get; set; }

        public Camera2D cam;
        public GraphicsDevice graphics { get; set; }

        public int TileSelectorX { get; set; } = 0;
        public int TileSelectorY { get; set; } = 0;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;

        public CraftingMenu CraftingMenu { get; set; }

        //keyboard



        private UserInterface()
        {

        }
        public UserInterface(Player player, GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam )
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            isEscMenu = false;
            
            BottomBar = new ToolBar( graphicsDevice, content);
            Esc = new EscMenu(graphicsDevice, content);
            this.cam = cam;
            TextBuilder = new TextBuilder("", .1f, 5f);
            this.Player = player;
            CraftingMenu = new CraftingMenu(GraphicsDevice);
        }


        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            BottomBar.Update(gameTime, inventory, mouse);
            //if (!IsShopMenu)
            //{

            //}
            CraftingMenu.Update(gameTime, mouse);
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)) && !IsShopMenu)
            {
                isEscMenu = !isEscMenu;

                if (isEscMenu == false)
                {
                    Esc.isTextChanged = false;
                }
                    

            }


            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)) && !isEscMenu)
            {
                IsShopMenu = !IsShopMenu;
               Game1.AllShops.Find(x => x.ID == 1).IsActive = IsShopMenu;
                if(!IsShopMenu)
                {
                    Player.UserInterface.CurrentOpenShop = 0;
                }
                //Player.UserInterface.CurrentOpenShop = OpenShop.ToolShop;

                
            }

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.T)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.T)) && !isEscMenu)
            {
                TextBuilder.IsActive = !TextBuilder.IsActive;
                TextBuilder.UseTextBox = true;
            }


                TextBuilder.Update(gameTime);
            // if(TextBuilder.FreezeStage)
            //{
            //    Game1.freeze = true;
            //}


            if(IsShopMenu)
            {
                for (int i = 0; i < Game1.AllShops.Count; i++)
                {
                    if (Game1.AllShops[i].IsActive)
                    {
                        Game1.isMyMouseVisible = true;
                        //Game1.freeze = true;
                        Game1.AllShops[i].Update(gameTime, mouse);
                    }
                }
            }
            

            if (isEscMenu)
            {
                Esc.Update(gameTime, mouse);
                Game1.freeze = true;
            }
            if(!isEscMenu && !IsShopMenu && !TextBuilder.FreezeStage)
            {
                Game1.freeze = false;

            }
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);


            
            

            BottomBar.Draw(spriteBatch);
            if(isEscMenu)
            {
                Esc.Draw(spriteBatch);
            }

            if(IsShopMenu)
            {
                for (int i = 0; i < Game1.AllShops.Count; i++)
                {
                    if (Game1.AllShops[i].IsActive)
                    {
                        Game1.AllShops[i].Draw(spriteBatch);
                    }
                }
            }
            


                TextBuilder.Draw(spriteBatch, .71f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            CraftingMenu.Draw(spriteBatch);

            spriteBatch.End();

        }

        public void HandleSceneChanged(object sender, EventArgs eventArgs)
        {
            this.TextBuilder.ClearString();
            this.TextBuilder.StringToWrite = Game1.GetCurrentStage().StageName;
            this.TextBuilder.Scale = 4f;
            this.TextBuilder.Color = Color.White;
            this.TextBuilder.IsActive = true;
            
        }
    }

    
}
