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
using SecretProject.Class.Transportation;
using static SecretProject.Class.UI.CheckList;

namespace SecretProject.Class.UI
{
    public enum OpenShop
    {
        None = 0,
        ToolShop = 1,
        DobbinShop = 2,
        JulianShop = 3,
        ElixirShop = 4,
        KayaShop = 5
        //LodgeInteior = 1,
        //Iliad = 2,
        //Exit = 3,
        //Sea = 4,
        //RoyalDock = 5
    }
    public enum CurrentOpenProgressBook
    {
        None = 0,
        Julian = 1,
        Elixir = 2,
    }
    public enum ExclusiveInterfaceItem
    {
        None = 0,
        EscMenu = 1,
        ShopMenu = 2,
        CraftingMenu = 3,
        SanctuaryCheckList = 4,
        WarpGate = 5,
        ProgressBook = 6,

    }
    public class UserInterface
    {
        ContentManager content;
        public bool IsShopMenu { get; set; }

        public bool DrawTileSelector { get; set; } = true;

        public GraphicsDevice GraphicsDevice { get; set; }
        public Game1 Game { get; set; }
        public EscMenu Esc { get; set; }
        //public ShopMenu ShopMenu { get; set; }
        internal ToolBar BottomBar { get; set; }

        public Camera2D cam;
        public GraphicsDevice graphics { get; set; }
        public TileSelector TileSelector;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;

        public CraftingMenu CraftingMenu { get; set; }



        public bool IsAnyChestOpen { get; set; }
        public string OpenChestKey { get; set; }
        public HealthBar PlayerHealthBar { get; set; }
        public StaminaBar PlayerStaminaBar { get; set; }
        public WarpGate WarpGate { get; set; }
        public ExclusiveInterfaceItem CurrentOpenInterfaceItem;
        public CurrentOpenProgressBook CurrentOpenProgressBook;


        //keyboard

        public InfoPopUp InfoBox { get; set; }

        public List<RisingText> AllRisingText { get; set; }



        private UserInterface()
        {

        }
        public UserInterface(Player player, GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam )
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            
            BottomBar = new ToolBar( graphicsDevice, content);
            Esc = new EscMenu(graphicsDevice, content);
            this.cam = cam;
            TextBuilder = new TextBuilder("", .5f, 10f);
            this.Player = player;
            CraftingMenu = new CraftingMenu(content, graphicsDevice);
            //CraftingMenu.LoadContent(content, GraphicsDevice);


            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            PlayerHealthBar = new HealthBar();
            this.PlayerStaminaBar = new StaminaBar(graphicsDevice,Game1.Player.Stamina, .2f);
            WarpGate = new WarpGate(graphicsDevice);
            TileSelector = new TileSelector();

            InfoBox = new InfoPopUp("Text Not Assigned");
            this.CurrentOpenProgressBook = CurrentOpenProgressBook.None;
            this.AllRisingText = new List<RisingText>();

        }


        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            InfoBox.IsActive = false;
            IsAnyChestOpen = false;
            if(Game1.GetCurrentStage().AllTiles != null)
            {
                foreach (KeyValuePair<string, IStorableItem> storeableItem in Game1.GetCurrentStage().AllTiles.StoreableItems)
                {
                    if (storeableItem.Value.IsUpdating)
                    {
                        storeableItem.Value.Update(gameTime);
                        this.IsAnyChestOpen = true;
                        this.OpenChestKey = storeableItem.Key;
                    }

                }
            }
            
            if (BottomBar.IsActive)
            {
                InfoBox.Update(gameTime);
                BottomBar.Update(gameTime, inventory, mouse);
            }

            switch(CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.None:
                    if(!TextBuilder.FreezeStage)
                    {
                        Game1.freeze = false;
                    }
                    BottomBar.IsActive = true;
                    
                    Esc.isTextChanged = false;
                    PlayerStaminaBar.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {
                        this.IsShopMenu = true;
                        ActivateShop(OpenShop.ToolShop);
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.T)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.T)))
                    {
                        TextBuilder.IsActive = !TextBuilder.IsActive;
                        TextBuilder.UseTextBox = true;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.B)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.B)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CraftingMenu;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.L)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.L)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ProgressBook;
                    }
                    break;
                case ExclusiveInterfaceItem.EscMenu:
                    Esc.Update(gameTime, mouse);

                    Game1.freeze = true;
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                case ExclusiveInterfaceItem.ShopMenu:
                    for (int i = 0; i < Game1.AllShops.Count; i++)
                    {
                        if (Game1.AllShops[i].IsActive)
                        {
                            Game1.isMyMouseVisible = true;
                            Game1.freeze = true;
                            Game1.AllShops[i].Update(gameTime, mouse);
                        }
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {
                        this.IsShopMenu = false;
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    break;
                case ExclusiveInterfaceItem.CraftingMenu:
                    CraftingMenu.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.B)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.B)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    break;
                case ExclusiveInterfaceItem.SanctuaryCheckList:
                    Game1.SanctuaryCheckList.Update(gameTime, mouse);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;

                case ExclusiveInterfaceItem.ProgressBook:
                    Game1.freeze = true;
                    for(int i =0; i < Game1.AllProgressBooks.Count; i++)
                    {
                        if(Game1.AllProgressBooks[i].ID == (int)CurrentOpenProgressBook)
                        {
                            Game1.AllProgressBooks[i].Update(gameTime);
                        }
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.L)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.L)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    //BottomBar.IsActive = false;
                    break;


                case ExclusiveInterfaceItem.WarpGate:
                    Game1.freeze = true;
                    WarpGate.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                    
                //case ExclusiveInterfaceItem.CookingMenu:
                //    //Game1.freeze = true;
                //    CookingMenu.Update(gameTime);
                //    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                //    {
                //        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                //    }
                //    break;

            }
            
           for(int i =0; i < this.AllRisingText.Count; i++)
            {
                AllRisingText[i].Update(gameTime, AllRisingText);
            }

            TextBuilder.Update(gameTime);

           
        }
        
        public void ActivateProgressBook(CurrentOpenProgressBook progressBookID)
        {
            this.CurrentOpenProgressBook = progressBookID;
            this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ProgressBook;
        }

        public void ActivateShop(OpenShop shopID)
        {

            for(int i = 0; i < Game1.AllShops.Count; i++)
            {
                Game1.AllShops[i].IsActive = false;
            }
            Game1.AllShops.Find(x => x.ID == (int)shopID).IsActive = IsShopMenu;
            Player.UserInterface.CurrentOpenShop = shopID;
            if (!IsShopMenu)
            {
                Player.UserInterface.CurrentOpenShop = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            Game1.myMouseManager.Draw(spriteBatch, 1f);
            switch (CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.None:
                    Game1.freeze = false;
                    Esc.isTextChanged = false;
                    PlayerHealthBar.Draw(spriteBatch, Game1.Player.Health);
                    PlayerStaminaBar.Draw(spriteBatch);
                    break;
                case ExclusiveInterfaceItem.EscMenu:
                    Esc.Draw(spriteBatch);
                    break;
                case ExclusiveInterfaceItem.ShopMenu:
                    for (int i = 0; i < Game1.AllShops.Count; i++)
                    {
                        if (Game1.AllShops[i].IsActive)
                        {
                            Game1.AllShops[i].Draw(spriteBatch);
                        }
                    }
                    break;
                case ExclusiveInterfaceItem.CraftingMenu:
                    CraftingMenu.Draw(spriteBatch);
                    break;
                case ExclusiveInterfaceItem.SanctuaryCheckList:
                    Game1.SanctuaryCheckList.Draw(spriteBatch);
                    break;

                case ExclusiveInterfaceItem.ProgressBook:
                    for (int i = 0; i < Game1.AllProgressBooks.Count; i++)
                    {
                        if (Game1.AllProgressBooks[i].ID == (int)CurrentOpenProgressBook)
                        {
                            Game1.AllProgressBooks[i].Draw(spriteBatch);
                        }
                    }
                    break;

                case ExclusiveInterfaceItem.WarpGate:
                    WarpGate.Draw(spriteBatch);
                    break;

                //case ExclusiveInterfaceItem.CookingMenu:
                //    CookingMenu.Draw(spriteBatch);
                //    break;
            }

            if (BottomBar.IsActive)
            {
                BottomBar.Draw(spriteBatch);
                InfoBox.Draw(spriteBatch);
            }
            


            TextBuilder.Draw(spriteBatch, .71f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            if(Game1.GetCurrentStage().AllTiles != null)
            {
                foreach (KeyValuePair<string, IStorableItem> storeableItem in Game1.GetCurrentStage().AllTiles.StoreableItems)
                {
                    if (storeableItem.Value.IsUpdating)
                    {
                        storeableItem.Value.Draw(spriteBatch);
                    }

                }
            }
            

            

            spriteBatch.End();

        }

        public void HandleSceneChanged(object sender, EventArgs eventArgs)
        {
            this.TextBuilder.Reset();
            this.TextBuilder.StringToWrite = Game1.GetCurrentStage().StageName;
            this.TextBuilder.Scale = 4f;
            this.TextBuilder.Color = Color.Black;
            this.TextBuilder.IsActive = true;
            
        }

        
        

    }

    
}
