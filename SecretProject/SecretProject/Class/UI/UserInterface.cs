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
using SecretProject.Class.ShopStuff;
using SecretProject.Class.Transportation;
using SecretProject.Class.UI.SanctuaryStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.UI
{
    public enum OpenShop
    {
        None = 0,
        ToolShop = 1,
        DobbinShop = 2,
        JulianShop = 3,
        ElixirShop = 4,
        KayaShop = 5,
        BusinessSnailShop = 6,
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
        CompletionHub = 7,
        CommandConsole

    }
    public class UserInterface
    {
        ContentManager content;

        public bool CinematicMode { get; set; } = false;
        public bool DrawTileSelector { get; set; } = true;

        public GraphicsDevice GraphicsDevice { get; set; }
        public Game1 Game { get; set; }
        public EscMenu Esc { get; set; }

        internal ToolBar BottomBar { get; set; }

        public Camera2D cam;
        public GraphicsDevice graphics { get; set; }
        public TileSelector TileSelector;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;
        public IShop CurrentShop { get; set; }

        public CraftingMenu CraftingMenu { get; set; }

        public BackPack BackPack { get; set; }



        public bool IsAnyStorageItemOpen { get; set; }


        public IStorableItemBuilding CurrentAccessedStorableItem { get; set; }

        public HealthBar PlayerHealthBar { get; set; }
        public StaminaBar PlayerStaminaBar { get; set; }
        public WarpGate WarpGate { get; set; }
        public ExclusiveInterfaceItem CurrentOpenInterfaceItem;
        public CurrentOpenProgressBook CurrentOpenProgressBook;
        public CompletionHub CompletionHub;
        public CommandConsole CommandConsole { get; set; }


        //keyboard

        public InfoPopUp InfoBox { get; set; }
        public InfoPopUp Notes { get; set; }

        public List<RisingText> AllRisingText { get; set; }


        //transition
        public Texture2D BlackTransitionTexture { get; set; }
        public bool IsTransitioning { get; set; }
        public SimpleTimer TransitionTimer { get; set; }
        public float BlackTransitionColorMultiplier { get; set; }

        public float TransitionSpeed { get; set; }

        public List<UISprite> AllUISprites { get; set; }
        public List<Alert> AllAlerts { get; set; }

        private UserInterface()
        {

        }
        public UserInterface(Player player, GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam)
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            this.BackPack = new BackPack(graphicsDevice, Game1.Player.Inventory);
            this.BottomBar = new ToolBar(graphicsDevice, this.BackPack, content);
            this.Esc = new EscMenu(graphicsDevice, content);
            this.cam = cam;
            this.TextBuilder = new TextBuilder("", .5f, 10f);
            this.Player = player;
            this.CraftingMenu = new CraftingMenu(content, graphicsDevice);
            //CraftingMenu.LoadContent(content, GraphicsDevice);



            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            this.PlayerHealthBar = new HealthBar();
            this.PlayerStaminaBar = new StaminaBar(graphicsDevice, Game1.Player.Stamina, .2f);
            this.WarpGate = new WarpGate(graphicsDevice);
            TileSelector = new TileSelector();

            this.InfoBox = new InfoPopUp("Text Not Assigned", new Rectangle(1024, 64, 112, 48));
            this.Notes = new InfoPopUp("Text Not Assigned", new Rectangle(624, 272, 160, 224)) { Color = Color.Black };

            CurrentOpenProgressBook = CurrentOpenProgressBook.None;
            CompletionHub = new CompletionHub(graphicsDevice, content);
            this.CommandConsole = new CommandConsole(new Vector2(Game1.Utility.CenterScreenX - 50, 50));
            this.AllRisingText = new List<RisingText>();


            this.BlackTransitionTexture = Game1.Utility.GetColoredRectangle(this.GraphicsDevice, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight, Color.Black);
            this.BlackTransitionColorMultiplier = 1f;
            this.TransitionSpeed = .05f;
            this.TransitionTimer = new SimpleTimer(2f);
            this.AllUISprites = new List<UISprite>();

            this.AllAlerts = new List<Alert>();
        }

        public void SwitchCurrentAccessedStorageItem(IStorableItemBuilding building)
        {
            if (this.CurrentAccessedStorableItem != null)
            {
                this.CurrentAccessedStorableItem.Deactivate();
            }

            this.CurrentAccessedStorableItem = building;
        }

        public void AddAlert(AlertSize size, Vector2 position, string text)
        {
            this.AllAlerts.Add(new Alert(this.GraphicsDevice, size, position, text));
        }

        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            this.InfoBox.IsActive = false;
            //  Notes.IsActive = false;
            this.IsAnyStorageItemOpen = false;
            this.BottomBar.IsActive = true;
            if (this.CurrentAccessedStorableItem != null)
            {
                if (this.CurrentAccessedStorableItem.IsUpdating)
                {
                    this.CurrentAccessedStorableItem.Update(gameTime);
                    this.IsAnyStorageItemOpen = true;
                }
            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F3)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F3)))
            {

                Game1.EnablePlayerCollisions = !Game1.EnablePlayerCollisions;
            }
            if (this.BottomBar.IsActive)
            {
                this.InfoBox.Update(gameTime);
                this.BottomBar.Update(gameTime, inventory, mouse);
            }

            for (int i = 0; i < AllUISprites.Count; i++)
            {
                AllUISprites[i].Update(gameTime);
            }

            for (int i = 0; i < AllAlerts.Count; i++)
            {
                AllAlerts[i].Update(gameTime, AllAlerts);
            }

            switch (CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.None:

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F6)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F6)))
                    {
                        Game1.cam.Zoom++;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F5)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F5)))
                    {
                        Game1.cam.Zoom--;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F3)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F3)))
                    {
                        Game1.DebugWindow.IsActivated = !Game1.DebugWindow.IsActivated;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Enter)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Enter)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CommandConsole;
                    }
                    if (!this.TextBuilder.FreezeStage)
                    {
                        Game1.freeze = false;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Tab)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Tab)))
                    {
                        if (this.BackPack.Expanded)
                        {
                            this.BackPack.Expanded = false;
                        }
                        else
                        {
                            this.BackPack.Expanded = true;

                        }

                    }


                    this.Esc.isTextChanged = false;
                    this.PlayerStaminaBar.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {

                        ActivateShop(OpenShop.ToolShop);
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.T)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.T)))
                    {
                        this.TextBuilder.IsActive = !this.TextBuilder.IsActive;
                        this.TextBuilder.UseTextBox = true;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.N)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.N)))
                    {
                        if (!this.Notes.IsActive)
                        {
                            this.Notes.FitText("This is a sample text to see how long the message can get before getting absolutely booled on", 1);
                        }
                        this.Notes.IsActive = !this.Notes.IsActive;
                        this.Notes.WindowPosition = Game1.Utility.centerScreen;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.B)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.B)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CraftingMenu;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Z)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Z)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CompletionHub;
                    }
                    break;
                case ExclusiveInterfaceItem.EscMenu:
                    this.Esc.Update(gameTime, mouse);

                    Game1.freeze = true;
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                case ExclusiveInterfaceItem.ShopMenu:

                    Game1.isMyMouseVisible = true;
                    Game1.freeze = true;
                    this.CurrentShop.Update(gameTime, mouse);

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {

                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    break;
                case ExclusiveInterfaceItem.CraftingMenu:
                    this.CraftingMenu.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.B)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.B)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Tab)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Tab)))
                    {
                        this.BackPack.Expanded = !this.BackPack.Expanded;
                    }
                    break;
                case ExclusiveInterfaceItem.SanctuaryCheckList:
                    Game1.SanctuaryCheckList.Update(gameTime, mouse);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                case ExclusiveInterfaceItem.CommandConsole:
                    CommandConsole.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Enter)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Enter)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;




                case ExclusiveInterfaceItem.WarpGate:
                    Game1.freeze = true;
                    this.WarpGate.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;

                case ExclusiveInterfaceItem.CompletionHub:
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Z)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Z)))
                    {
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    CompletionHub.Update(gameTime);
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

            this.BackPack.Update(gameTime);
            for (int i = 0; i < this.AllRisingText.Count; i++)
            {
                this.AllRisingText[i].Update(gameTime, this.AllRisingText);
            }

            this.TextBuilder.Update(gameTime);

            if (this.IsTransitioning)
            {
                BeginTransitionCycle(gameTime);
            }


        }

        public void BeginTransitionCycle(GameTime gameTime)
        {
            if (this.TransitionTimer.Time <= this.TransitionTimer.TargetTime)
            {
                this.BlackTransitionColorMultiplier -= this.TransitionSpeed;
            }
            else
            {
                this.BlackTransitionColorMultiplier += this.TransitionSpeed;
            }
            if (!this.TransitionTimer.Run(gameTime))
            {
                this.IsTransitioning = true;

            }
            else
            {
                this.BlackTransitionColorMultiplier = 1f;
                this.IsTransitioning = false;
            }
        }

        public void DrawTransitionTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.BlackTransitionTexture, Game1.Utility.Origin, null, Color.White * this.BlackTransitionColorMultiplier, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f); ;
        }


        public void ActivateShop(OpenShop shopID)
        {

            for (int i = 0; i < Game1.AllShops.Count; i++)
            {
                Game1.AllShops[i].IsActive = false;
            }
            this.CurrentShop = Game1.AllShops.Find(x => x.ID == (int)shopID);
            this.CurrentShop.IsActive = true;
            this.Player.UserInterface.CurrentOpenShop = shopID;
            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            Game1.GlobalClock.Draw(spriteBatch);
            Game1.myMouseManager.Draw(spriteBatch, 1f);
            for (int i = 0; i < AllUISprites.Count; i++)
            {
                AllUISprites[i].Draw(spriteBatch);
            }
            for (int i = 0; i < AllAlerts.Count; i++)
            {
                AllAlerts[i].Draw(spriteBatch);
            }
            if (!this.CinematicMode)
            {


                switch (CurrentOpenInterfaceItem)
                {
                    case ExclusiveInterfaceItem.None:
                        Game1.freeze = false;
                        this.Esc.isTextChanged = false;
                        this.PlayerHealthBar.Draw(spriteBatch, Game1.Player.Health);
                        this.PlayerStaminaBar.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.EscMenu:
                        this.Esc.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.ShopMenu:
                        this.CurrentShop.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.CraftingMenu:
                        this.CraftingMenu.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.SanctuaryCheckList:
                        Game1.SanctuaryCheckList.Draw(spriteBatch);
                        break;



                    case ExclusiveInterfaceItem.WarpGate:
                        this.WarpGate.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.CompletionHub:
                        CompletionHub.Draw(spriteBatch);
                        break;

                    //case ExclusiveInterfaceItem.CookingMenu:
                    //    CookingMenu.Draw(spriteBatch);
                    //    break;

                    case ExclusiveInterfaceItem.CommandConsole:
                        CommandConsole.Draw(spriteBatch);
                        break;
                }

                if (this.BottomBar.IsActive)
                {
                    this.BottomBar.Draw(spriteBatch, Game1.Player.Inventory.Money);
                    this.InfoBox.Draw(spriteBatch);
                    this.Notes.Draw(spriteBatch);
                }

                this.BackPack.Draw(spriteBatch);
            }
            this.TextBuilder.Draw(spriteBatch, .71f);

            for (int i = 0; i < this.AllRisingText.Count; i++)
            {
                this.AllRisingText[i].Draw(spriteBatch);
            }

            if (this.CurrentAccessedStorableItem != null)
            {
                if (this.CurrentAccessedStorableItem.IsUpdating)
                {
                    this.CurrentAccessedStorableItem.Draw(spriteBatch);
                }
            }





            if (this.IsTransitioning)
            {
                DrawTransitionTexture(spriteBatch);
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
