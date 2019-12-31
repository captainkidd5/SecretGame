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
using SecretProject.Class.ShopStuff;
using SecretProject.Class.Transportation;
using SecretProject.Class.UI.SanctuaryStuff;
using SecretProject.Class.Universal;
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
        CompletionHub = 7

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
        public IShop CurrentShop{ get; set; }

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

        private UserInterface()
        {

        }
        public UserInterface(Player player, GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam)
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            BackPack = new BackPack(graphicsDevice, Game1.Player.Inventory);
            BottomBar = new ToolBar(graphicsDevice, BackPack, content);
            Esc = new EscMenu(graphicsDevice, content);
            this.cam = cam;
            TextBuilder = new TextBuilder("", .5f, 10f);
            this.Player = player;
            CraftingMenu = new CraftingMenu(content, graphicsDevice);
            //CraftingMenu.LoadContent(content, GraphicsDevice);



            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            PlayerHealthBar = new HealthBar();
            this.PlayerStaminaBar = new StaminaBar(graphicsDevice, Game1.Player.Stamina, .2f);
            WarpGate = new WarpGate(graphicsDevice);
            TileSelector = new TileSelector();

            InfoBox = new InfoPopUp("Text Not Assigned", new Rectangle(1024, 64, 112, 48));
            Notes = new InfoPopUp("Text Not Assigned", new Rectangle(624, 272, 160, 224)) { Color = Color.Black };
            
            this.CurrentOpenProgressBook = CurrentOpenProgressBook.None;
            CompletionHub = new CompletionHub(graphicsDevice, content);
            this.AllRisingText = new List<RisingText>();


            this.BlackTransitionTexture = Game1.Utility.GetColoredRectangle(GraphicsDevice, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight, Color.Black);
            BlackTransitionColorMultiplier = 1f;
            TransitionSpeed = .05f;
            this.TransitionTimer = new SimpleTimer(2f);

        }


        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            InfoBox.IsActive = false;
          //  Notes.IsActive = false;
            IsAnyStorageItemOpen = false;
            if (CurrentAccessedStorableItem != null)
            {
                if (CurrentAccessedStorableItem.IsUpdating)
                {
                    CurrentAccessedStorableItem.Update(gameTime);
                    this.IsAnyStorageItemOpen = true;
                }
            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F3)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F3)))
            {

                Game1.EnablePlayerCollisions = !Game1.EnablePlayerCollisions;
            }
            if (BottomBar.IsActive)
            {
                InfoBox.Update(gameTime);
                BottomBar.Update(gameTime, inventory, mouse);
            }


            switch (CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.None:

                    if((Game1.OldKeyBoardState.IsKeyDown(Keys.F6)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F6)))
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
                        if (!TextBuilder.FreezeStage)
                    {
                        Game1.freeze = false;
                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Tab)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Tab)))
                    {
                        if (BackPack.Expanded)
                        {
                            BackPack.Expanded = false;
                        }
                        else
                        {
                            BackPack.Expanded = true;

                        }

                    }


                    Esc.isTextChanged = false;
                    PlayerStaminaBar.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {

                        ActivateShop(OpenShop.ToolShop);
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.T)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.T)))
                    {
                        TextBuilder.IsActive = !TextBuilder.IsActive;
                        TextBuilder.UseTextBox = true;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.N)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.N)))
                    {
                        if(!Notes.IsActive)
                        {
                            Notes.FitText("This is a sample text to see how long the message can get before getting absolutely booled on", 1);
                        }
                        Notes.IsActive = !Notes.IsActive;
                        Notes.WindowPosition = Game1.Utility.centerScreen;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.B)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.B)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CraftingMenu;
                    }

                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Z)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Z)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.CompletionHub;
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

                            Game1.isMyMouseVisible = true;
                            Game1.freeze = true;
                            CurrentShop.Update(gameTime, mouse);
                 
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)))
                    {
  
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
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Tab)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Tab)))
                    {
                        BackPack.Expanded = !BackPack.Expanded;
                    }
                    break;
                case ExclusiveInterfaceItem.SanctuaryCheckList:
                    Game1.SanctuaryCheckList.Update(gameTime, mouse);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;




                case ExclusiveInterfaceItem.WarpGate:
                    Game1.freeze = true;
                    WarpGate.Update(gameTime);
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

                    }
                    break;

                case ExclusiveInterfaceItem.CompletionHub:
                    if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Z)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Z)))
                    {
                        this.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
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

            BackPack.Update(gameTime);
            for (int i = 0; i < this.AllRisingText.Count; i++)
            {
                AllRisingText[i].Update(gameTime, AllRisingText);
            }

            TextBuilder.Update(gameTime);

            if (this.IsTransitioning)
            {
                BeginTransitionCycle(gameTime);
            }


        }

        public void BeginTransitionCycle(GameTime gameTime)
        {
            if (TransitionTimer.Time <= TransitionTimer.TargetTime)
            {
                this.BlackTransitionColorMultiplier -= TransitionSpeed;
            }
            else
            {
                this.BlackTransitionColorMultiplier += TransitionSpeed;
            }
            if (!TransitionTimer.Run(gameTime))
            {
                this.IsTransitioning = true;

            }
            else
            {
                BlackTransitionColorMultiplier = 1f;
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
            CurrentShop = Game1.AllShops.Find(x => x.ID == (int)shopID);
            CurrentShop.IsActive = true;
            Player.UserInterface.CurrentOpenShop = shopID;
            CurrentOpenInterfaceItem = ExclusiveInterfaceItem.ShopMenu;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            Game1.GlobalClock.Draw(spriteBatch);
            Game1.myMouseManager.Draw(spriteBatch, 1f);
            if (!CinematicMode)
            {


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
                        CurrentShop.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.CraftingMenu:
                        CraftingMenu.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.SanctuaryCheckList:
                        Game1.SanctuaryCheckList.Draw(spriteBatch);
                        break;



                    case ExclusiveInterfaceItem.WarpGate:
                        WarpGate.Draw(spriteBatch);
                        break;
                    case ExclusiveInterfaceItem.CompletionHub:
                        CompletionHub.Draw(spriteBatch);
                        break;

                        //case ExclusiveInterfaceItem.CookingMenu:
                        //    CookingMenu.Draw(spriteBatch);
                        //    break;
                }

                if (BottomBar.IsActive)
                {
                    BottomBar.Draw(spriteBatch, Game1.Player.Inventory.Money);
                    InfoBox.Draw(spriteBatch);
                    Notes.Draw(spriteBatch);
                }

                BackPack.Draw(spriteBatch);
            }
            TextBuilder.Draw(spriteBatch, .71f);

          

            if (CurrentAccessedStorableItem != null)
            {
                if (CurrentAccessedStorableItem.IsUpdating)
                {
                    CurrentAccessedStorableItem.Draw(spriteBatch);
                }
            }





            if (IsTransitioning)
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
