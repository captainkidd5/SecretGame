using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.StageFolder;
using SecretProject.Class.UI;
using System;

using TiledSharp;
//using XMLDataLib;

using System.Collections.Generic;

using System.Runtime.Serialization;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using SecretProject.Class.SoundStuff;
using SecretProject.Class.TextureStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SavingStuff;
using Microsoft.Xna.Framework.Content;

using XMLData.DialogueStuff;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ShopStuff;
using XMLData.RouteStuff;
using XMLData.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding;
using static SecretProject.Class.UI.CheckList;
using SecretProject.Class.EventStuff;
using Microsoft.Xna.Framework.Audio;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Weather;



//TODO: Make enum for player actions, items, world items etc so that strings aren't used
// fix player clipping around when performing action
// diagonal movement
// inside of house
// change screen edge stuff from hardcode
//make screen width/height stuff better
//placeable objects needs two new layers so stuff underneat is preserved.
//Tile random generation
//Work on NPC collision detection
//set IDs for worlditems

namespace SecretProject
{

    public enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public enum SecondaryDir
    {
        Down,
        Up,
        Left,
        Right,
        None
    }

    public enum Stages
    {
        Town = 0,
        ElixirHouse = 1,
        JulianHouse = 2,
        OverWorld = 3,
        DobbinHouse = 4,
        PlayerHouse = 5,
        GeneralStore = 6,
        KayaHouse = 7,
        Cafe = 8,
        CaveWorld = 9,
        MainMenu = 50,
        Exit = 55,
        

    }
    
    public enum WeatherType
    {
        None = 0,
        Sunny = 1,
        Rainy = 2
    }


    public class Game1 : Game
    {
        #region FIELDS

        public static bool EnablePlayerCollisions = true;
        public static bool EnableCutScenes = true;

        public static bool IsFirstTimeStartup;

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager MainMenuContentManager;

        //STAGES
        public static MainMenu mainMenu;
        //public static NormalStage Iliad;
        public static Town Town;
        public static TmxStageBase ElixirHouse;


        public static TmxStageBase JulianHouse;
        public static TmxStageBase DobbinHouse;
        public static World OverWorld;
        public static TmxStageBase PlayerHouse;
        public static TmxStageBase GeneralStore;
        public static TmxStageBase KayaHouse;
        public static TmxStageBase Cafe;
        public static World CaveWorld;
        public static List<ILocation> AllStages;
        public static int CurrentStage;
        public static int PreviousStage = 0;
        public static bool freeze = false;

        //SOUND
        public static SoundBoard SoundManager;

        //INPUT

        public static MouseManager myMouseManager;
        public static bool isMyMouseVisible = true;

        public static KeyboardState OldKeyBoardState;
        public static KeyboardState NewKeyBoardState;

        //Camera
        public static Camera2D cam;
        public static bool ToggleFullScreen = false;

        //Initialize Starting Stage
        public static Stages gameStages = Stages.MainMenu;

        //screen stuff
        public static Rectangle ScreenRectangle = new Rectangle(0, 0, 0, 0);
        public static int ScreenHeight { get { return ScreenRectangle.Height; } }
        public static int ScreenWidth { get { return ScreenRectangle.Width; } }


        //UI
        public UserInterface userInterface;

        public static DebugWindow DebugWindow;

        public static CheckList SanctuaryCheckList;

        //TEXTURES
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Player Player { get; set; }
        public Texture2D MainCharacterTexture { get; set; }
        public static Texture2D ItemAtlas;
        public static TextureBook AllTextures;

        public static Texture2D LineTexture;



        //TOOLS

        public static Utility Utility;
        public static Procedural Procedural;
        public static float FrameRate = 0f;
        public static List<ActionTimer> AllActions;

        public static Texture2D RectangleOutlineTexture;

        //CLOCK
        public static Clock GlobalClock;

        //ITEMS
        public static ItemBank ItemVault;

        //XMLDATA

        public DialogueHolder ElixirDialogue;
        public DialogueHolder DobbinDialogue;
        public DialogueHolder SnawDialogue;
        public DialogueHolder KayaDialogue;
        public DialogueHolder JulianDialogue;
        public DialogueHolder SarahDialogue;

        public RouteSchedule DobbinRouteSchedule;
        public RouteSchedule ElixirRouteSchedule;
        public RouteSchedule KayaRouteSchedule;
        public RouteSchedule JulianRouteSchedule;
        public RouteSchedule SarahRouteSchedule;
        public static List<RouteSchedule> AllSchedules;
        public static ItemHolder AllItems;

        public static CropHolder AllCrops;

        public static DialogueLibrary DialogueLibrary;

        public static CookingGuide AllCookingRecipes;

        //SHOPS AND MENUS
        public static List<IShop> AllShops { get; set; }
        public static List<ProgressBook> AllProgressBooks { get; set; }

        //RENDERTARGETS
        public RenderTarget2D MainTarget;
        public RenderTarget2D LightsTarget;


        public static PresentationParameters PresentationParameters;

        //event handlers
        //Events
        public static List<IEvent> AllEvents;
        public static IEvent CurrentEvent;

        //NPCS
        public static Elixir Elixir;
        public static Dobbin Dobbin;
        public static Kaya Kaya;

        public static Character Snaw;
        public static Julian Julian;
        public static Sarah Sarah;
        public static List<Character> AllCharacters;

        //PORTALS
        public static Graph PortalGraph;

        //WEATHER
        public static WeatherType CurrentWeather;
        public static Dictionary<WeatherType, IWeather> AllWeather;

        public static bool IsEventActive;

        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            HomeContentManager = new ContentManager(Content.ServiceProvider);
            MainMenuContentManager = new ContentManager(Content.ServiceProvider);
            Content.RootDirectory = "Content";
            HomeContentManager.RootDirectory = "Content";
            MainMenuContentManager.RootDirectory = "Content";

            //set window dimensions
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            IsFixedTimeStep = false;

        }
        #endregion

        #region INITIALIZE
        protected override void Initialize()
        {
            ScreenRectangle.Width = graphics.PreferredBackBufferWidth;
            ScreenRectangle.Height = graphics.PreferredBackBufferHeight;
            //seed parameter
            Utility = new Utility(1);
            //CAMERA
            cam = new Camera2D(GraphicsDevice.Viewport);
            //MOUSE

            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager = new MouseManager(cam, graphics.GraphicsDevice);

            

            


            //SCREEN


            AllActions = new List<ActionTimer>();

            base.Initialize();
        }
        #endregion



        public static ILocation GetCurrentStage()
        {
            switch (gameStages)
            {

                case Stages.OverWorld:
                    return OverWorld;


                case Stages.Town:
                    return Town;


                case Stages.ElixirHouse:
                    return ElixirHouse;
                case Stages.JulianHouse:
                    return JulianHouse;
                case Stages.DobbinHouse:
                    return DobbinHouse;
                case Stages.PlayerHouse:
                    return PlayerHouse;
               case Stages.GeneralStore:
                    return GeneralStore;
                case Stages.KayaHouse:
                    return KayaHouse;
                case Stages.Cafe:
                    return Cafe;
                case Stages.CaveWorld:
                    return CaveWorld;

                default:
                    return null;

            }
        }

        ////Town = 0,
        //Pass = 1,
        //Center = 2,
        //World = 3,
        //Sanctuary = 4,
        //ElixirShop = 5,
        //julianshop = 6
        public static ILocation GetStageFromInt(Stages stage)
        {
            switch (stage)
            {
                case Stages.Town:
                    return Town;
 
                case Stages.OverWorld:
                    return OverWorld;

                case Stages.ElixirHouse:
                    return ElixirHouse;
                case Stages.JulianHouse:
                    return JulianHouse;
                case Stages.DobbinHouse:
                    return DobbinHouse;
                case Stages.PlayerHouse:
                    return PlayerHouse;
                case Stages.GeneralStore:
                    return GeneralStore;
                case Stages.KayaHouse:
                    return KayaHouse;
                case Stages.Cafe:
                    return Cafe;
                case Stages.CaveWorld:
                    return CaveWorld;
                default:
                    return null;

            }

        }

        public static Stages GetCurrentStageInt()
        {
            switch (gameStages)
            {
                case Stages.Town:
                    return Stages.Town;


                case Stages.OverWorld:
                    return Stages.OverWorld;



                case Stages.ElixirHouse:
                    return Stages.ElixirHouse;
                case Stages.JulianHouse:
                    return Stages.JulianHouse;
                case Stages.DobbinHouse:
                    return Stages.DobbinHouse;

                case Stages.PlayerHouse:
                    return Stages.PlayerHouse;
                case Stages.GeneralStore:
                    return Stages.GeneralStore;
                case Stages.KayaHouse:
                    return Stages.KayaHouse;
                case Stages.Cafe:
                    return Stages.Cafe;
                case Stages.CaveWorld:
                    return Stages.CaveWorld;

                default:
                    return Stages.Town;

            }

        }

        

        #region LOADCONTENT
        protected override void LoadContent()
        {
            PresentationParameters = GraphicsDevice.PresentationParameters;
            MainTarget = new RenderTarget2D(GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            LightsTarget = new RenderTarget2D(GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            
            //ORDER MATTERS!!!
            ElixirDialogue = Content.Load<DialogueHolder>("Dialogue/ElixirDialogue");
            DobbinDialogue = Content.Load<DialogueHolder>("Dialogue/DobbinDialogue");
            SnawDialogue = Content.Load<DialogueHolder>("Dialogue/SnawDialogue");
            KayaDialogue = Content.Load<DialogueHolder>("Dialogue/KayaDialogue");
            JulianDialogue = Content.Load<DialogueHolder>("Dialogue/JulianDialogue");
            SarahDialogue = Content.Load<DialogueHolder>("Dialogue/SarahDialogue");

            DobbinRouteSchedule = Content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = Content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            KayaRouteSchedule = Content.Load<RouteSchedule>("Route/KayaRouteSchedule");
            JulianRouteSchedule = Content.Load<RouteSchedule>("Route/JulianRouteSchedule");
            SarahRouteSchedule = Content.Load<RouteSchedule>("Route/SarahRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule, KayaRouteSchedule, JulianRouteSchedule, SarahRouteSchedule };
            for(int i =0; i < AllSchedules.Count; i++)
            {
                foreach(Route route in AllSchedules[i].Routes)
                {
                    route.ProcessStageToEndAt();
                }
            }
            AllCrops = Content.Load<CropHolder>("Crop/CropStuff");

            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue, KayaDialogue, JulianDialogue,SarahDialogue };
            foreach(DialogueHolder holder in tempListHolder)
            {
                holder.RemoveAllNewLines();
            }
            DialogueLibrary = new DialogueLibrary(tempListHolder);
            AllCookingRecipes = Content.Load<CookingGuide>("Item/Cooking/CookingGuide");
            //TEXTURES
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AllTextures = new TextureBook(Content, spriteBatch);



            //  testItem = Content.Load&lt;XMLDataLib.Item&gt;("Level1");

            //SOUND
            SoundManager = new SoundBoard(this, Content);


            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");
            //PLAYERS

            LoadPlayer();
            //UI

            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet, GraphicsDevice);

            //ITEMS
            ItemVault = new ItemBank();


            AllItems = Content.Load<ItemHolder>("Item/ItemHolder");
            Procedural = new Procedural();

            Player.UserInterface = new UserInterface(Player, graphics.GraphicsDevice, Content, cam) { graphics = graphics.GraphicsDevice };
            SanctuaryCheckList = new CheckList(graphics.GraphicsDevice, new Vector2(200, 50),
                new List<CheckListRequirement>()
                {new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("SuperBulb",1792, 1, "plant", false)
                });


            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, MainMenuContentManager, myMouseManager, Player.UserInterface);
            Town = new Town("Town", LocationType.Exterior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 1)
            { StageIdentifier = (int)Stages.Town};

            OverWorld = new World("OverWorld", LocationType.Exterior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.OverWorld };



            ElixirHouse = new TmxStageBase("ElixirHouse",LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/elixirShop.tmx", 1, 0) { StageIdentifier = (int)Stages.ElixirHouse };
            JulianHouse = new TmxStageBase("JulianHouse", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/JulianShop.tmx", 1, 0) { StageIdentifier = (int)Stages.JulianHouse };
            DobbinHouse = new TmxStageBase("DobbinHouse", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/DobbinHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.DobbinHouse };
            PlayerHouse = new TmxStageBase("PlayerHouse", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/PlayerHouseSmall.tmx", 1, 0) { StageIdentifier = (int)Stages.PlayerHouse };
            GeneralStore = new TmxStageBase("GeneralStore", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/GeneralStore.tmx", 1, 0) { StageIdentifier = (int)Stages.GeneralStore };
            KayaHouse = new TmxStageBase("KayaHouse", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/KayaHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.KayaHouse };
            Cafe = new TmxStageBase("Cafe", LocationType.Interior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/Cafe.tmx", 1, 0) { StageIdentifier = (int)Stages.Cafe };
            CaveWorld = new World("CaveWorld", LocationType.Exterior, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.CaveWorld };


            GlobalClock = new Clock();



            AllStages = new List<ILocation>() {  Town,  OverWorld, ElixirHouse, JulianHouse,DobbinHouse, PlayerHouse, GeneralStore,KayaHouse,Cafe };
            PortalGraph = new Graph(AllStages.Count);

            


            Shop ToolShop = new Shop(graphics.GraphicsDevice, 1, "ToolShop", new ShopMenu("ToolShopInventory", graphics.GraphicsDevice, 25));
            for(int i =0; i < AllItems.AllItems.Count; i++)
            {
                ToolShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }
            

            Shop DobbinShop = new Shop(graphics.GraphicsDevice, 2, "DobbinShop", new ShopMenu("DobbinShopInventory", graphics.GraphicsDevice, 5));
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                DobbinShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }


            Shop JulianShop = new Shop(graphics.GraphicsDevice, 3, "JulianShop", new ShopMenu("JulianShopInventory", graphics.GraphicsDevice, 10));
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                JulianShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }

            Shop ElixirShop = new Shop(graphics.GraphicsDevice, 4, "ElixirShop", new ShopMenu("ElixirShopInventory", graphics.GraphicsDevice, 10));
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                ElixirShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }

            Shop KayaShop = new Shop(graphics.GraphicsDevice, 5, "KayaShop", new ShopMenu("KayaShopInventory", graphics.GraphicsDevice, 10));
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                KayaShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }
            AllShops = new List<IShop>()
            {
                ToolShop,
                DobbinShop,
                JulianShop,
                ElixirShop,
                KayaShop
            };
           
            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(120, null));
           
            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(160, null));
            for (int i = 0; i < 99; i++)
            {
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1162, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1401, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(480, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1164, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1202, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1055, null));
            }
            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(640, null));
            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(520, null));



            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });

            Elixir = new Elixir("Elixer", new Vector2(35, 23), graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, AllSchedules[1], AllTextures.ElixirPortrait) { FrameToSet = 0 };
            Dobbin = new Dobbin("Dobbin", new Vector2(26, 28), graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, AllSchedules[0], AllTextures.DobbinPortrait) { FrameToSet = 0 };
            Kaya = new Kaya("Kaya", new Vector2(26, 18), graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, AllSchedules[2]) { FrameToSet = 0 };
            Snaw = new Character("Snaw", new Vector2(60, 40), graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                3)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                0, 0, 72, 96, 3, .3f, new Vector2(1280, 500)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 3,
                CurrentStageLocation = Stages.Town,
                FrameToSet = 3,
                IsBasicNPC = true
            };
            Julian = new Julian("Julian", new Vector2(28, 22), graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, AllSchedules[3], AllTextures.JulianPortrait) { FrameToSet = 0 };
            Sarah = new Sarah("Sarah", new Vector2(40, 21), graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, AllSchedules[4], AllTextures.SarahPortrait) { FrameToSet = 0 };
            AllCharacters = new List<Character>()
            {
                Elixir,
                Dobbin,
                Kaya,
                Snaw,
                Julian, 
                Sarah
            };

            ProgressBook JulianProgressBook = new ProgressBook(Julian,Content, graphics.GraphicsDevice, 1);
            ProgressBook ElixirProgressBook = new ProgressBook(Elixir,Content, graphics.GraphicsDevice, 2);
            AllProgressBooks = new List<ProgressBook>()
            {
                JulianProgressBook,
                ElixirProgressBook
            };

            foreach (ILocation stage in AllStages)
            {
                foreach (Character character in AllCharacters)
                {
                    if (character.CurrentStageLocation == (Stages)stage.StageIdentifier)
                    {
                        stage.CharactersPresent.Add(character);
                    }
                }



            }

            
            AllEvents = new List<IEvent>()
            {
               // new IntroduceSanctuary(),
                new IntroduceJulianShop(GraphicsDevice),
                new IntroScene(GraphicsDevice)
            };
            IsEventActive = false;

            RectangleOutlineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            RectangleOutlineTexture.SetData(new Color[] { Color.Red });

            AllWeather = new Dictionary<WeatherType, IWeather>()
            {
                {WeatherType.Sunny, new Sunny() },
                {WeatherType.Rainy, new Rainy(GraphicsDevice) }
            };

            CurrentWeather = WeatherType.Sunny;

           
        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion

        //check portal from previous and current stage and set the player to the new position specified. Must be called after loading content.



        public static void SwitchStage(Stages currentStage, Stages stageToSwitchTo, Portal portal = null)
        {
            Game1.Player.UserInterface.IsTransitioning = true;

            Game1.Player.UserInterface.TransitionSpeed = .05f;
            Game1.Player.UserInterface.TransitionTimer.TargetTime = 2f;
            GetStageFromInt(currentStage).UnloadContent();
            gameStages = (Stages)stageToSwitchTo;
            if(gameStages == Stages.OverWorld)
            {
                Game1.Player.LockBounds = false;
            }
            else
            {
                Game1.Player.LockBounds = true;
            }
            if (!GetStageFromInt(stageToSwitchTo).IsLoaded)
            {

                GetStageFromInt(stageToSwitchTo).LoadContent(cam, AllSchedules);
            }

             // List<Portal> testPortal = GetCurrentStage().AllPortals;
            if (portal != null)
            {
              //  ILocation location = GetCurrentStage();
               // List<Portal> newStageTestPortals = GetCurrentStage().AllPortals;
                Portal tempPortal = GetCurrentStage().AllPortals.Find(z => z.From == portal.To && z.To == portal.From);
                float x = tempPortal.PortalStart.X;
                float width = tempPortal.PortalStart.Width / 2;
                float y = tempPortal.PortalStart.Y;
                float safteyX = tempPortal.SafteyOffSetX;
                float safteyY = tempPortal.SafteyOffSetY;
                Player.position = new Vector2(x + width + safteyX, y + safteyY);
                //Player.UpdateMovementAnimationsOnce(gameTime);

            }
        }


        public static void FullScreenToggle()
        {

            ToggleFullScreen = true;
        }

        #region UPDATE
        protected override void Update(GameTime gameTime)
        {
           // IsEventActive = false;
            OldKeyBoardState = NewKeyBoardState;
            NewKeyBoardState = Keyboard.GetState();
            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager.Update(gameTime);
            DebugWindow.Update(gameTime);

            //SOUND
            MediaPlayer.IsRepeating = true;
           // SoundManager.PlaySong();
          //  SoundManager.CurrentSongInstance.Volume = SoundManager.GameVolume;
            //KEYBOARD

            if (ToggleFullScreen)
            {
                graphics.ToggleFullScreen();
                ToggleFullScreen = false;
            }
           
            if (!IsEventActive)
            {

                switch (gameStages)
                {
                    case Stages.MainMenu:

                        mainMenu.Update(gameTime, myMouseManager, this);
                        break;

                    case Stages.OverWorld:

                        OverWorld.Update(gameTime, myMouseManager, Player);
                        break;


                    case Stages.Town:

                        Town.Update(gameTime, myMouseManager, Player);
                        break;


                    case Stages.ElixirHouse:
                        ElixirHouse.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.JulianHouse:
                        JulianHouse.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.DobbinHouse:
                        DobbinHouse.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.PlayerHouse:
                        PlayerHouse.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.GeneralStore:
                        GeneralStore.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.KayaHouse:
                        KayaHouse.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.Cafe:
                        Cafe.Update(gameTime, myMouseManager, Player);
                        break;

                }


            }
            if (EnableCutScenes && !IsEventActive)
            {
                foreach (IEvent e in AllEvents)
                {
                    if (e.DayToTrigger == GlobalClock.TotalDays && e.StageToTrigger == (int)GetCurrentStageInt() && !e.IsCompleted)
                    {
                        int num = (int)GetCurrentStageInt();
                        if (!e.IsActive)
                        {
                            //e.Start();
                            IsEventActive = true;
                            CurrentEvent = e;
                            CurrentEvent.Start();
                        }


                    }
                }
            }
            if (CurrentEvent != null)
            {
                CurrentEvent.Update(gameTime);
            }



            if (!myMouseManager.ToggleGeneralInteraction)
            {
                this.IsMouseVisible = true;
            }

            base.Update(gameTime);
        }
        #endregion

        #region DRAW
        protected override void Draw(GameTime gameTime)
        {

            

            switch (gameStages)
            {
                case Stages.MainMenu:
                    //GraphicsDevice.Clear(Color.Black);
                    GraphicsDevice.Clear(Color.DeepSkyBlue);
                    mainMenu.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;

                case Stages.OverWorld:
                    GraphicsDevice.Clear(Color.Black);
                    OverWorld.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.Town:
                    GraphicsDevice.Clear(Color.Black);
                    Town.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.ElixirHouse:
                    GraphicsDevice.Clear(Color.Black);
                    ElixirHouse.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.JulianHouse:
                    GraphicsDevice.Clear(Color.Black);
                    JulianHouse.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.DobbinHouse:
                    GraphicsDevice.Clear(Color.Black);
                    DobbinHouse.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.PlayerHouse:
                    GraphicsDevice.Clear(Color.Black);
                    PlayerHouse.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.GeneralStore:
                    GraphicsDevice.Clear(Color.Black);
                    GeneralStore.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.KayaHouse:
                    GraphicsDevice.Clear(Color.Black);
                    KayaHouse.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.Cafe:
                    GraphicsDevice.Clear(Color.Black);
                    Cafe.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

            }
            if (CurrentEvent != null)
            {
                CurrentEvent.Draw(spriteBatch);
            }
            Game1.DebugWindow.Draw(spriteBatch);

            base.Draw(gameTime);
        }
        #endregion

        public void LoadPlayer()
        {
            Player = new Player("joe", new Vector2(800, 800), AllTextures.PlayerBase, 4, 5, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true, IsDrawn = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);

            //meaning hair of direction forward:
            Player.animations[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerHair, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };//, Color = Color.Black };
            Player.animations[0, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShirt, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[0, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerPants, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[0, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShoes, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[0, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerBase, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };


            //up
            Player.animations[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerHair, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };
            Player.animations[1, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShirt, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[1, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerPants, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[1, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShoes, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[1, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerBase, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };

            //Left
            Player.animations[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerHair, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f, Flip = true };
            Player.animations[2, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShirt, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f, Flip = true };
            Player.animations[2, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerPants, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f, Flip = true };
            Player.animations[2, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShoes, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f, Flip = true };
            Player.animations[2, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerBase, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f, Flip = true };

            //Right
            Player.animations[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerHair, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };
            Player.animations[3, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShirt, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[3, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerPants, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[3, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerShoes, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[3, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerBase, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };

            //MiningDown
            Player.Mining[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000012f };
            Player.Mining[0, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000011f };
            Player.Mining[0, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000010f };
            Player.Mining[0, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000009f };
            Player.Mining[0, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000008f };
            Player.Mining[0, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .000000007f };

            //MiningUP
            Player.Mining[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000012f };
            Player.Mining[1, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000011f };
            Player.Mining[1, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000010f };
            Player.Mining[1, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000009f };
            Player.Mining[1, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000008f };
            Player.Mining[1, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .000000007f };

            //MiningLeft
            Player.Mining[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 256, 0, 48, 48, 5, .1f, Game1.Player.position, - 32,-16) { LayerDepth = .00000012f, Flip = true };
            Player.Mining[2, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000011f, Flip = true };
            Player.Mining[2, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000010f, Flip = true };
            Player.Mining[2, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000009f, Flip = true };
            Player.Mining[2, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000008f, Flip = true };
            Player.Mining[2, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .000000007f, Flip = true };

            //MiningRight
            Player.Mining[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 256, 0, 48, 48, 5, .1f, Game1.Player.position,0, -16) { LayerDepth = .00000012f };
            Player.Mining[3, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000011f };
            Player.Mining[3, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000010f };
            Player.Mining[3, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000009f };
            Player.Mining[3, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000008f };
            Player.Mining[3, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .000000007f };


            //SwipingDown
           // Player.Swiping[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[0, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[0, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[0, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[0, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };


            //SwipingUp
           // Player.Swiping[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[1, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[1, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[1, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[1, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };

            //SwipingLeft
           // Player.Swiping[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f, Flip = true };
            Player.Swiping[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f, Flip = true };
            Player.Swiping[2, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f, Flip = true };
            Player.Swiping[2, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f, Flip = true };
            Player.Swiping[2, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f, Flip = true };
            Player.Swiping[2, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f, Flip = true };

            //SwipingRight
          //  Player.Swiping[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[3, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[3, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[3, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[3, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };

            Player.PlayerActionAnimations = new Sprite[6];

            for (int i = 0; i < Player.Mining.GetLength(1); i++)
            {
                Player.PlayerActionAnimations[i] = Player.Mining[0, i];
            }

            Player.PlayerMovementAnimations = new Sprite[5];
            for (int i = 0; i < Player.animations.GetLength(1); i++)
            {
                Player.PlayerMovementAnimations[i] = Player.animations[0, i];
            }

            for (int i = 0; i < Player.PlayerMovementAnimations.GetLength(0); i++)
            {
                Player.PlayerMovementAnimations[i].SourceRectangle = new Rectangle((int)(Player.PlayerMovementAnimations[i].FirstFrameX + Player.PlayerMovementAnimations[i].FrameWidth * Player.PlayerMovementAnimations[i].CurrentFrame),
                    (int)Player.PlayerMovementAnimations[i].FirstFrameY, (int)Player.PlayerMovementAnimations[i].FrameWidth, (int)Player.PlayerMovementAnimations[i].FrameHeight);
                Player.PlayerMovementAnimations[i].DestinationRectangle = new Rectangle((int)Player.PlayerMovementAnimations[i].Position.X + Player.PlayerMovementAnimations[i].OffSetX,
                    (int)Player.PlayerMovementAnimations[i].Position.Y + Player.PlayerMovementAnimations[i].OffSetY, Player.PlayerMovementAnimations[i].FrameWidth, Player.PlayerMovementAnimations[i].FrameHeight);

            }
        }
        
    }

}