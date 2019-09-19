﻿using System;
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
using static SecretProject.Class.UI.CheckList;

namespace SecretProject.Class.UI
{
    public enum OpenShop
    {
        None = 0,
        ToolShop = 1,
        DobbinShop = 2,
        JulianShop = 3
        //LodgeInteior = 1,
        //Iliad = 2,
        //Exit = 3,
        //Sea = 4,
        //RoyalDock = 5
    }

    public enum ExclusiveInterfaceItem
    {
        None = 0,
        EscMenu = 1,
        ShopMenu = 2,
        CraftingMenu = 3,
        SanctuaryCheckList = 4
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

        public int TileSelectorX { get; set; } = 0;
        public int TileSelectorY { get; set; } = 0;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;

        public CraftingMenu CraftingMenu { get; set; }

        public ScrollTree ScrollTree { get; set; }
        public bool IsAnyChestOpen { get; set; }
        public string OpenChestKey { get; set; }
        public HealthBar PlayerHealthBar { get; set; }
        public StaminaBar PlayerStaminaBar { get; set; }
        public ExclusiveInterfaceItem CurrentOpenInterfaceItem;

        //keyboard



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
            TextBuilder = new TextBuilder("", 50f, 10f);
            this.Player = player;
            CraftingMenu = new CraftingMenu();
            CraftingMenu.LoadContent(content, GraphicsDevice);
            ScrollTree = new ScrollTree(graphicsDevice);

            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            PlayerHealthBar = new HealthBar();
            this.PlayerStaminaBar = new StaminaBar(graphicsDevice,Game1.Player.Stamina, .2f);

        }


        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            IsAnyChestOpen = false;
            foreach (KeyValuePair<string, Chest> chest in Game1.GetCurrentStage().AllChests)
            {
                if (chest.Value.IsUpdating)
                {
                    chest.Value.Update(gameTime, mouse);
                    this.IsAnyChestOpen = true;
                    this.OpenChestKey = chest.Key;
                }

            }
            if (BottomBar.IsActive)
            {
                BottomBar.Update(gameTime, inventory, mouse);
            }

            switch(CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.None:
                    if(!TextBuilder.FreezeStage)
                    {
                        Game1.freeze = false;
                    }
                    
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
                    CraftingMenu.Update(gameTime, mouse);
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

            }
            
           

            TextBuilder.Update(gameTime);



            if(this.ScrollTree.IsActive)
            {
                ScrollTree.Update(gameTime, Game1.Player.Wisdom);
            }

           

                
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

            }


            if (BottomBar.IsActive)
            {
                BottomBar.Draw(spriteBatch);
            }
            
            if(ScrollTree.IsActive)
            {
                ScrollTree.Draw(spriteBatch);
            }

            TextBuilder.Draw(spriteBatch, .71f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            foreach (KeyValuePair<string, Chest> chest in Game1.GetCurrentStage().AllChests)
            {
                if (chest.Value.IsUpdating)
                {
                    chest.Value.Draw(spriteBatch);
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
