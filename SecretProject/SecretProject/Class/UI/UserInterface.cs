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
using SecretProject.Class.UI.AlertStuff;
using SecretProject.Class.UI.CraftingStuff;
using SecretProject.Class.UI.QuestStuff;
using SecretProject.Class.UI.SanctuaryStuff;
using SecretProject.Class.UI.StaminaStuff;
using SecretProject.Class.UI.Transitions;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        SarahShop = 7

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
        CompletionHub = 7,
        CommandConsole = 8,
        QuestLog = 9,
        WorldQuestMenu = 10

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

        private Camera2D cam;
        public GraphicsDevice graphics { get; set; }
        public TileSelector TileSelector;


        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;
        public IShop CurrentShop { get; set; }

       // public CraftingMenu CraftingMenu { get; set; }

        public CraftingWindow CraftingWindow { get; set; }

        public BackPack BackPack { get; set; }

        public LoadingScreen LoadingScreen { get; set; }

        public bool IsAnyStorageItemOpen { get; set; }


        public IStorableItemBuilding CurrentAccessedStorableItem { get; set; }

        public HealthBar PlayerHealthBar { get; set; }
        public StaminaBar StaminaBar { get; set; }
        public ExclusiveInterfaceItem CurrentOpenInterfaceItem;
        public CurrentOpenProgressBook CurrentOpenProgressBook;
        public CompletionHub CompletionHub;
        public CommandConsole CommandConsole { get; set; }
        public QuestLog QuestLog { get; set; }

        public WorldQuestMenu WorldQuestMenu { get; set; }



        public InfoPopUp InfoBox { get; set; }
        public InfoPopUp Notes { get; set; }

        public List<RisingText> AllRisingText { get; set; }

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
            //this.CraftingMenu = new CraftingMenu(content, graphicsDevice);
            this.CraftingWindow = new CraftingWindow(content, graphicsDevice);
            //CraftingMenu.LoadContent(content, GraphicsDevice);
            this.QuestLog = new QuestLog(graphicsDevice);

            this.WorldQuestMenu = new WorldQuestMenu(graphicsDevice);

            this.LoadingScreen = new LoadingScreen(graphicsDevice);

            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            this.PlayerHealthBar = new HealthBar();
            this.StaminaBar = new StaminaBar(graphicsDevice, new Vector2(Game1.PresentationParameters.BackBufferWidth * .9f, Game1.PresentationParameters.BackBufferHeight * .2f), 7);
            TileSelector = new TileSelector();

            this.InfoBox = new InfoPopUp("Text Not Assigned", Game1.Utility.Origin);
            this.Notes = new InfoPopUp("Text Not Assigned", Game1.Utility.Origin) { Color = Color.Black };

            CurrentOpenProgressBook = CurrentOpenProgressBook.None;
            CompletionHub = new CompletionHub(graphicsDevice, content);
            this.CommandConsole = new CommandConsole(this.GraphicsDevice);
            this.AllRisingText = new List<RisingText>();



            this.AllUISprites = new List<UISprite>();

            this.AllAlerts = new List<Alert>();

        }

        public void SwitchCurrentAccessedStorageItem(IStorableItemBuilding building)
        {
            if (this.CurrentAccessedStorableItem != null)
            {
                if(building != this.CurrentAccessedStorableItem)
                {
                    this.CurrentAccessedStorableItem.Deactivate();
                }
                   
                
                
            }

            this.CurrentAccessedStorableItem = building;
        }

        public void AddAlert(AlertType type, Vector2 position, string text, Action positiveAction = null, Action negativeAction = null)
        {
            switch (type)
            {
                case AlertType.Confirmation:
                    this.AllAlerts.Add(new ConfirmationAlert(positiveAction, negativeAction, this.GraphicsDevice, position, text));
                    break;
                default:
                    this.AllAlerts.Add(new Alert(this.GraphicsDevice, position, text));
                    break;
            }

            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.Alert1);
        }

        public void Update(GameTime gameTime, Inventory inventory)
        {
            this.InfoBox.IsActive = false;
            //  Notes.IsActive = false;
            this.IsAnyStorageItemOpen = false;
            this.BottomBar.IsActive = true;
            this.BackPack.Update(gameTime);
            if (this.CurrentAccessedStorableItem != null)
            {
                if (this.CurrentAccessedStorableItem.IsUpdating)
                {
                    this.CurrentAccessedStorableItem.Update(gameTime);
                    this.IsAnyStorageItemOpen = true;
                }
            }
            if (Game1.KeyboardManager.WasKeyPressed(Keys.F3))
            {

                Game1.EnablePlayerCollisions = !Game1.EnablePlayerCollisions;
            }
            if (this.BottomBar.IsActive)
            {

                this.BottomBar.Update(gameTime, inventory, Game1.MouseManager);
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

                    if (Game1.KeyboardManager.WasKeyPressed(Keys.F6))
                    {
                        Game1.cam.Zoom++;
                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.F5))
                    {
                        Game1.cam.Zoom--;
                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.F3))
                    {
                        Game1.DebugWindow.IsActivated = !Game1.DebugWindow.IsActivated;
                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.F10))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CommandConsole;
                    }
                    //if (!this.TextBuilder.FreezeStage)
                    //{
                    //    Game1.freeze = false;
                    //}
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Tab))
                    {
                        if (this.BackPack.Expanded)
                        {   
     
                                Game1.SoundManager.PlayCloseUI();
                            
                            this.BackPack.Expanded = false;
                        }
                        else
                        {
                            Game1.SoundManager.PlayOpenUI();
                            this.BackPack.Expanded = true;

                        }

                    }


                    this.Esc.isTextChanged = false;

                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                    {
                        Game1.SoundManager.PlayOpenUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;

                    }
                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.P))
                    {
                        Game1.SoundManager.PlayOpenUI();
                        ActivateShop(OpenShop.ToolShop);
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

                    }

                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.F3))
                    {
                        this.TextBuilder.IsActive = !this.TextBuilder.IsActive;
                        this.TextBuilder.UseTextBox = true;
                    }

                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.N))
                    {
                        Game1.SoundManager.PlayOpenUI();
                        if (!this.Notes.IsActive)
                        {
                            this.Notes.FitText("This is a sample text to see how long the message can get before getting absolutely booled on", 1);
                        }
                        this.Notes.IsActive = !this.Notes.IsActive;
                        this.Notes.WindowPosition = Game1.Utility.centerScreen;
                    }

                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.B))
                    {
                        Game1.SoundManager.PlayOpenUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CraftingMenu;
                    }

                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.Z))
                    {
                        Game1.SoundManager.PlayOpenUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CompletionHub;
                    }
                    else if (Game1.KeyboardManager.WasKeyPressed(Keys.L))
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PageRuffleOpen, true, .1f);
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.QuestLog;
                    }
                    break;
                case ExclusiveInterfaceItem.EscMenu:

                    this.Esc.Update(gameTime, Game1.MouseManager);

                    Game1.freeze = true;
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                        {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                case ExclusiveInterfaceItem.ShopMenu:

                    Game1.isMyMouseVisible = true;
                    Game1.freeze = true;
                    this.CurrentShop.Update(gameTime, Game1.MouseManager);

                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.P))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    break;
                case ExclusiveInterfaceItem.CraftingMenu:
                    this.CraftingWindow.Update(gameTime);
                   // this.CraftingMenu.Update(gameTime);
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.B))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Tab))
                    {
                        this.BackPack.Expanded = !this.BackPack.Expanded;
                    }
                    break;
                case ExclusiveInterfaceItem.SanctuaryCheckList:
                    Game1.SanctuaryCheckList.Update(gameTime, Game1.MouseManager);
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;
                case ExclusiveInterfaceItem.CommandConsole:
                    CommandConsole.Update(gameTime);
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.F10))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;

                case ExclusiveInterfaceItem.CompletionHub:
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Z))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    CompletionHub.Update(gameTime);
                    break;

                case ExclusiveInterfaceItem.QuestLog:
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.L))
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PageRuffleClose, true, .1f);
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                    }
                    QuestLog.Update(gameTime);
                    break;

                case ExclusiveInterfaceItem.WorldQuestMenu:
                    Game1.freeze = true;

                    WorldQuestMenu.Update(gameTime);
                    if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                    {
                        Game1.SoundManager.PlayCloseUI();
                        CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

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

          
            for (int i = 0; i < this.AllRisingText.Count; i++)
            {
                this.AllRisingText[i].Update(gameTime, this.AllRisingText);
            }

            this.TextBuilder.Update(gameTime);

            if (this.LoadingScreen.IsTransitioning)
            {
                Task.Run(() => LoadingScreen.BlackTransition(gameTime));
            }

            this.StaminaBar.Update(gameTime);


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
            Game1.MouseManager.Draw(spriteBatch, 1f);
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
                        this.StaminaBar.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.EscMenu:
                        this.Esc.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.ShopMenu:
                        this.CurrentShop.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.CraftingMenu:
                        //this.CraftingMenu.Draw(spriteBatch);
                        this.CraftingWindow.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.SanctuaryCheckList:
                        Game1.SanctuaryCheckList.Draw(spriteBatch);
                        break;

                    case ExclusiveInterfaceItem.CompletionHub:
                        CompletionHub.Draw(spriteBatch);
                        break;

                    //case ExclusiveInterfaceItem.CookingMenu:
                    //    CookingMenu.Draw(spriteBatch);
                    //    break;

                    case ExclusiveInterfaceItem.CommandConsole:
                        spriteBatch.End();
                        CommandConsole.Draw(spriteBatch);
                        spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
                        break;
                    case ExclusiveInterfaceItem.QuestLog:
                        QuestLog.Draw(spriteBatch);
                        break;

                    case ExclusiveInterfaceItem.WorldQuestMenu:
                        this.WorldQuestMenu.Draw(spriteBatch);
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





            if (LoadingScreen.IsTransitioning)
            {
                LoadingScreen.DrawTransitionTexture(spriteBatch);
            }

            this.StaminaBar.Draw(spriteBatch);

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
