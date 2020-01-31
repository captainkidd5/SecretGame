using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.EventStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.Playable;
using SecretProject.Class.ShopStuff;
using SecretProject.Class.SoundStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TextureStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using SecretProject.Class.Weather;
//using XMLDataLib;

using System.Collections.Generic;
using XMLData.DialogueStuff;
using XMLData.ItemStuff;
using XMLData.ItemStuff.LootStuff;
using XMLData.RouteStuff;
using static SecretProject.Class.UI.CheckList;



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

        DobbinHouseUpper = 9,
        SanctuaryHub = 10,
        Forest = 11,
        ResearchStation = 12,
        CaveWorld = 13,
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
        public static bool EnableCutScenes = false;
        public static bool EnableMusic = false;

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
        public static TmxStageBase DobbinHouseUpper;
        public static TmxStageBase SanctuaryHub;
        public static SanctuaryBase Forest;
        public static TmxStageBase ResearchStation;

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
        public DialogueHolder BusinessSnailDialogue;
        public DialogueHolder MippinDialogue;
        public DialogueHolder NedDialogue;

        public RouteSchedule DobbinRouteSchedule;
        public RouteSchedule ElixirRouteSchedule;
        public RouteSchedule KayaRouteSchedule;
        public RouteSchedule JulianRouteSchedule;
        public RouteSchedule SarahRouteSchedule;
        public RouteSchedule MippinRouteSchedule;
        public RouteSchedule NedRouteSchedule;
        public static List<RouteSchedule> AllSchedules;
        public static ItemHolder AllItems;
        public static LootBank LootBank;
        public static CropHolder AllCrops;

        public static DialogueLibrary DialogueLibrary;

        public static CookingGuide AllCookingRecipes;

        public static SpawnHolder OverWorldSpawnHolder { get; set; }

        //SHOPS AND MENUS
        public static List<IShop> AllShops { get; set; }

        //RENDERTARGETS
        public RenderTarget2D MainTarget;
        public RenderTarget2D NightLightsTarget;
        public RenderTarget2D DayLightsTarget;


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
        public static Character BusinessSnail;
        public static Julian Julian;
        public static Sarah Sarah;
        public static Mippin Mippin;
        public static Ned Ned;
        public static List<Character> AllCharacters;

        //PORTALS
        public static Graph PortalGraph;

        //WEATHER
        public static WeatherType CurrentWeather;
        public static Dictionary<WeatherType, IWeather> AllWeather;

        public static bool IsEventActive;


        //SanctuaryTrackers
        public static SanctuaryTracker ForestTracker;

        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            HomeContentManager = new ContentManager(this.Content.ServiceProvider);
            MainMenuContentManager = new ContentManager(this.Content.ServiceProvider);
            this.Content.RootDirectory = "Content";
            HomeContentManager.RootDirectory = "Content";
            MainMenuContentManager.RootDirectory = "Content";

            //set window dimensions
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            this.IsFixedTimeStep = false;

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
            cam = new Camera2D(this.GraphicsDevice.Viewport);
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
                case Stages.DobbinHouseUpper:
                    return DobbinHouseUpper;
                case Stages.SanctuaryHub:
                    return SanctuaryHub;
                case Stages.Forest:
                    return Forest;
                case Stages.ResearchStation:
                    return ResearchStation;

                default:
                    return null;

            }
        }

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
                case Stages.DobbinHouseUpper:
                    return DobbinHouseUpper;
                case Stages.SanctuaryHub:
                    return SanctuaryHub;
                case Stages.Forest:
                    return Forest;
                case Stages.ResearchStation:
                    return ResearchStation;
                default:
                    return null;

            }

        }

        public static Stages GetCurrentStageInt()
        {
            return gameStages;
        }



        #region LOADCONTENT
        protected override void LoadContent()
        {
            PresentationParameters = this.GraphicsDevice.PresentationParameters;
            MainTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            NightLightsTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            DayLightsTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            //ORDER MATTERS!!!
            ElixirDialogue = this.Content.Load<DialogueHolder>("Dialogue/ElixirDialogue");
            DobbinDialogue = this.Content.Load<DialogueHolder>("Dialogue/DobbinDialogue");
            SnawDialogue = this.Content.Load<DialogueHolder>("Dialogue/SnawDialogue");
            KayaDialogue = this.Content.Load<DialogueHolder>("Dialogue/KayaDialogue");
            JulianDialogue = this.Content.Load<DialogueHolder>("Dialogue/JulianDialogue");
            SarahDialogue = this.Content.Load<DialogueHolder>("Dialogue/SarahDialogue");
            BusinessSnailDialogue = this.Content.Load<DialogueHolder>("Dialogue/BusinessSnailDialogue");
            MippinDialogue = this.Content.Load<DialogueHolder>("Dialogue/MippinDialogue");
            NedDialogue = this.Content.Load<DialogueHolder>("Dialogue/NedDialogue");

            DobbinRouteSchedule = this.Content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = this.Content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            KayaRouteSchedule = this.Content.Load<RouteSchedule>("Route/KayaRouteSchedule");
            JulianRouteSchedule = this.Content.Load<RouteSchedule>("Route/JulianRouteSchedule");
            SarahRouteSchedule = this.Content.Load<RouteSchedule>("Route/SarahRouteSchedule");
            MippinRouteSchedule = this.Content.Load<RouteSchedule>("Route/MippinRouteSchedule");
            NedRouteSchedule = this.Content.Load<RouteSchedule>("Route/NedRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule, KayaRouteSchedule, JulianRouteSchedule, SarahRouteSchedule, MippinRouteSchedule, NedRouteSchedule };
            for (int i = 0; i < AllSchedules.Count; i++)
            {
                foreach (Route route in AllSchedules[i].Routes)
                {
                    route.ProcessStageToEndAt();
                }
            }
            AllCrops = this.Content.Load<CropHolder>("Crop/CropStuff");

            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue, KayaDialogue, JulianDialogue, SarahDialogue, BusinessSnailDialogue, MippinDialogue, NedDialogue };
            foreach (DialogueHolder holder in tempListHolder)
            {
                holder.RemoveAllNewLines();
            }
            DialogueLibrary = new DialogueLibrary(tempListHolder);
            AllCookingRecipes = this.Content.Load<CookingGuide>("Item/Cooking/CookingGuide");
            //TEXTURES
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            AllTextures = new TextureBook(this.Content, spriteBatch);



            //  testItem = Content.Load&lt;XMLDataLib.Item&gt;("Level1");

            //SOUND
            SoundManager = new SoundBoard(this, this.Content);


            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");
            //PLAYERS

            LoadPlayer();
            //UI

            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet, this.GraphicsDevice);

            //ITEMS
            ItemVault = new ItemBank();

            LootBank = new LootBank(Content.Load<LootHolder>("Item/Loot/LootHolder"));


            AllItems = this.Content.Load<ItemHolder>("Item/ItemHolder");
            Procedural = new Procedural();

            Player.UserInterface = new UserInterface(Player, graphics.GraphicsDevice, this.Content, cam) { graphics = graphics.GraphicsDevice };
            SanctuaryCheckList = new CheckList(graphics.GraphicsDevice, new Vector2(200, 50),
                new List<CheckListRequirement>()
                {new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("SuperBulb",1792, 1, "plant", false)
                });


            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, MainMenuContentManager, myMouseManager, Player.UserInterface);
            Town = new Town("Town", LocationType.Exterior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 1)
            { StageIdentifier = (int)Stages.Town };

            OverWorld = new World("OverWorld", LocationType.Exterior, StageType.Procedural, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.OverWorld };



            ElixirHouse = new TmxStageBase("ElixirHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/elixirShop.tmx", 1, 0) { StageIdentifier = (int)Stages.ElixirHouse };
            JulianHouse = new TmxStageBase("JulianHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/JulianShop.tmx", 1, 0) { StageIdentifier = (int)Stages.JulianHouse };
            DobbinHouse = new TmxStageBase("DobbinHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/DobbinHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.DobbinHouse };
            PlayerHouse = new TmxStageBase("PlayerHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/PlayerHouseSmall.tmx", 1, 0) { StageIdentifier = (int)Stages.PlayerHouse };
            GeneralStore = new TmxStageBase("GeneralStore", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/GeneralStore.tmx", 1, 0) { StageIdentifier = (int)Stages.GeneralStore };
            KayaHouse = new TmxStageBase("KayaHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/KayaHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.KayaHouse };
            Cafe = new TmxStageBase("Cafe", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/Cafe.tmx", 1, 0) { StageIdentifier = (int)Stages.Cafe };
            CaveWorld = new World("CaveWorld", LocationType.Exterior, StageType.Procedural, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.CaveWorld };
            DobbinHouseUpper = new TmxStageBase("DobbinHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/DobbinHouseUpper.tmx", 1, 0) { StageIdentifier = (int)Stages.DobbinHouse };
            SanctuaryHub = new TmxStageBase("SanctuaryHub", LocationType.Exterior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/SanctuaryHub.tmx", 1, 0) { StageIdentifier = (int)Stages.SanctuaryHub };
            Forest = new SanctuaryBase("Forest", LocationType.Exterior, StageType.Sanctuary, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Forest.tmx", 1, 0) { StageIdentifier = (int)Stages.Forest };
            ResearchStation = new TmxStageBase("ResearchStation", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/ResearchStation.tmx", 1, 0) { StageIdentifier = (int)Stages.ResearchStation };


            GlobalClock = new Clock();



            AllStages = new List<ILocation>() { Town, OverWorld, ElixirHouse, JulianHouse, DobbinHouse, PlayerHouse, GeneralStore, KayaHouse, Cafe, DobbinHouseUpper, SanctuaryHub, Forest, ResearchStation };
            PortalGraph = new Graph(AllStages.Count);




            Shop ToolShop = new Shop(graphics.GraphicsDevice, 1, "ToolShop", new ShopMenu("ToolShopInventory", graphics.GraphicsDevice, 25));
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                ToolShop.ShopMenu.TryAddStock(AllItems.AllItems[i].ID, 5);
            }


            Shop DobbinShop = new Shop(graphics.GraphicsDevice, 2, "DobbinShop", new ShopMenu("DobbinShopInventory", graphics.GraphicsDevice, 5));

            //bloomberry seeds
            DobbinShop.ShopMenu.TryAddStock(748, 2);
            //bloodcorn seeds
            DobbinShop.ShopMenu.TryAddStock(750, 2);
            //brine bulb seeds 
            DobbinShop.ShopMenu.TryAddStock(752, 2);
            //soil
            DobbinShop.ShopMenu.TryAddStock(1006, 50);



            Shop JulianShop = new Shop(graphics.GraphicsDevice, 3, "JulianShop", new ShopMenu("JulianShopInventory", graphics.GraphicsDevice, 10));

            //steel axe
            JulianShop.ShopMenu.TryAddStock(1, 5);
            //steel hammer
            JulianShop.ShopMenu.TryAddStock(41, 5);
            //steel shovel
            JulianShop.ShopMenu.TryAddStock(121, 5);
            //stone sword
            JulianShop.ShopMenu.TryAddStock(160, 5);
            //steel ore
            JulianShop.ShopMenu.TryAddStock(640, 1);

            Shop ElixirShop = new Shop(graphics.GraphicsDevice, 4, "ElixirShop", new ShopMenu("ElixirShopInventory", graphics.GraphicsDevice, 10));

            //empty flask
            ElixirShop.ShopMenu.TryAddStock(28, 5);


            Shop KayaShop = new Shop(graphics.GraphicsDevice, 5, "KayaShop", new ShopMenu("KayaShopInventory", graphics.GraphicsDevice, 10));
            //capture crate
            KayaShop.ShopMenu.TryAddStock(333, 5);
            //specimin jar
            KayaShop.ShopMenu.TryAddStock(373, 5);


            Shop BuisnessSnailShop = new Shop(graphics.GraphicsDevice, 6, "BusinessSnailShop", new ShopMenu("BusinessSnailShopInventory", graphics.GraphicsDevice, 10));

            BuisnessSnailShop.ShopMenu.TryAddStock(601, 5);
            BuisnessSnailShop.ShopMenu.TryAddStock(1402, 1);

            AllShops = new List<IShop>()
            {
                ToolShop,
                DobbinShop,
                JulianShop,
                ElixirShop,
                KayaShop,
                BuisnessSnailShop
            };
            for (int i = 0; i < 30; i++)
            {
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1162, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1403, null));
                Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(334, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(335, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(334, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1162, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1095, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(1055, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(748, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(750, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(756, null));
                //Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(752, null));
            }

            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(40, null));
            Player.Inventory.TryAddItem(ItemVault.GenerateNewItem(162, null));



            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });

            Elixir = new Elixir("Elixer", new Vector2(35, 23), graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, AllSchedules[1], AllTextures.ElixirPortrait) { FrameToSet = 0 };
            Dobbin = new Dobbin("Dobbin", new Vector2(26, 28), graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, AllSchedules[0], AllTextures.DobbinPortrait) { FrameToSet = 0 };
            Kaya = new Kaya("Kaya", new Vector2(26, 18), graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, AllSchedules[2], AllTextures.KayaPortrait) { FrameToSet = 0 };
            Snaw = new Character("Snaw", new Vector2(121, 70), graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                3, AllTextures.SnawPortrait)
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
            BusinessSnail = new Character("Business Snail", new Vector2(34, 80), graphics.GraphicsDevice, Game1.AllTextures.BusinessSnail,
                1, AllTextures.BusinessSnailPortrait)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.BusinessSnail,
                0, 0, 32, 32, 1, 1f, new Vector2(1280, 600)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 7,
                CurrentStageLocation = Stages.Town,
                FrameToSet = 0,
                IsBasicNPC = true
            };

            Mippin = new Mippin("Mippin", new Vector2(40, 21), graphics.GraphicsDevice, Game1.AllTextures.Mippin, AllSchedules[5], AllTextures.MippinPortrait) { FrameToSet = 0 };
            Ned = new Ned("Ned", new Vector2(45, 110), graphics.GraphicsDevice, Game1.AllTextures.Ned, AllSchedules[6], AllTextures.NedPortrait) { FrameToSet = 0 };
            AllCharacters = new List<Character>()
            {
                Elixir,
                Dobbin,
                Kaya,
                Snaw,
                Julian,
                Sarah,
                BusinessSnail,
                Mippin,
                Ned
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
               // new IntroduceJulianShop(GraphicsDevice),
                new IntroScene(this.GraphicsDevice),
                new MeetJulian(this.GraphicsDevice)
            };
            IsEventActive = false;

            RectangleOutlineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            RectangleOutlineTexture.SetData(new Color[] { Color.Red });

            AllWeather = new Dictionary<WeatherType, IWeather>()
            {
                {WeatherType.Sunny, new Sunny() },
                {WeatherType.Rainy, new Rainy(this.GraphicsDevice) }
            };

            CurrentWeather = WeatherType.Sunny;

            ForestTracker = new SanctuaryTracker(Player.UserInterface.CompletionHub.AllGuides[0]);
            OverWorldSpawnHolder = new SpawnHolder();

        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion

        public static SanctuaryTracker GetSanctuaryTrackFromStage(Stages stage)
        {
            switch (stage)
            {
                case Stages.Forest:
                    return ForestTracker;

                default:
                    return ForestTracker;
            }


        }

        //check portal from previous and current stage and set the player to the new position specified. Must be called after loading content.



        public static void SwitchStage(Stages currentStage, Stages stageToSwitchTo, Portal portal = null)
        {
            Game1.Player.UserInterface.IsTransitioning = true;

            Game1.Player.UserInterface.TransitionSpeed = .05f;
            Game1.Player.UserInterface.TransitionTimer.TargetTime = 2f;
            GetStageFromInt(currentStage).UnloadContent();
            gameStages = (Stages)stageToSwitchTo;
            if (gameStages == Stages.OverWorld)
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
                if(tempPortal != null)
                {
                    float x = tempPortal.PortalStart.X;
                    float width = tempPortal.PortalStart.Width / 2;
                    float y = tempPortal.PortalStart.Y;
                    float safteyX = tempPortal.SafteyOffSetX;
                    float safteyY = tempPortal.SafteyOffSetY;
                    Player.position = new Vector2(x + width + safteyX, y + safteyY);
                }
                else if (Game1.GetCurrentStage() == Town)
                {
                    Player.Position = new Vector2(1232, 304);
                }
                else if (Game1.GetCurrentStage() == OverWorld)
                {
                    Player.Position = new Vector2(128, 128);
                }


                //Player.UpdateMovementAnimationsOnce(gameTime);

            }
            if (Game1.GetCurrentStage() == PlayerHouse)
            {
                Player.Position = new Vector2(473, 670);
            }
            Player.UpdateMovementAnimationsOnce();
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
            if (EnableMusic)
            {
                SoundManager.PlaySong();
                SoundManager.CurrentSongInstance.Volume = SoundManager.GameVolume;
            }

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
                    case Stages.DobbinHouseUpper:
                        DobbinHouseUpper.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.SanctuaryHub:
                        SanctuaryHub.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.Forest:
                        Forest.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.ResearchStation:
                        ResearchStation.Update(gameTime, myMouseManager, Player);
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
                    this.GraphicsDevice.Clear(Color.DeepSkyBlue);
                    mainMenu.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;

                case Stages.OverWorld:
                    this.GraphicsDevice.Clear(Color.Black);
                    OverWorld.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.Town:
                    this.GraphicsDevice.Clear(Color.Black);
                    Town.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.ElixirHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    ElixirHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget,DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.JulianHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    JulianHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.DobbinHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    DobbinHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.PlayerHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    PlayerHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.GeneralStore:
                    this.GraphicsDevice.Clear(Color.Black);
                    GeneralStore.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.KayaHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    KayaHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.Cafe:
                    this.GraphicsDevice.Clear(Color.Black);
                    Cafe.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.DobbinHouseUpper:
                    this.GraphicsDevice.Clear(Color.Black);
                    DobbinHouseUpper.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.SanctuaryHub:
                    this.GraphicsDevice.Clear(Color.Black);
                    SanctuaryHub.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.Forest:
                    this.GraphicsDevice.Clear(Color.Black);
                    Forest.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.ResearchStation:
                    this.GraphicsDevice.Clear(Color.Black);
                    ResearchStation.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, myMouseManager, Player);
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
            Player = new Player("joe", new Vector2(1175, 760), AllTextures.PlayerBase, 4, 5, this.Content, graphics.GraphicsDevice, myMouseManager) { Activate = true, IsDrawn = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);

            //meaning hair of direction forward:
            Player.animations[0, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerHair, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };//, Color = Color.Black };
            Player.animations[0, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShirt, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[0, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerPants, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[0, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShoes, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[0, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerBase, 0, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };


            //up
            Player.animations[1, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerHair, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };
            Player.animations[1, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShirt, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[1, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerPants, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[1, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShoes, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[1, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerBase, 192, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };

            //Left
            Player.animations[2, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerHair, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f, Flip = true };
            Player.animations[2, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShirt, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f, Flip = true };
            Player.animations[2, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerPants, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f, Flip = true };
            Player.animations[2, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShoes, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f, Flip = true };
            Player.animations[2, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerBase, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f, Flip = true };

            //Right
            Player.animations[3, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerHair, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000011f };
            Player.animations[3, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShirt, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000010f };
            Player.animations[3, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerPants, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000009f };
            Player.animations[3, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerShoes, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .00000008f };
            Player.animations[3, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PlayerBase, 96, 0, 16, 34, 6, .1f, Game1.Player.position) { LayerDepth = .000000007f };

            //MiningDown
            Player.Mining[0, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000012f };
            Player.Mining[0, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000011f };
            Player.Mining[0, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000010f };
            Player.Mining[0, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000009f };
            Player.Mining[0, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000008f };
            Player.Mining[0, 5] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 0, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .000000007f };

            //MiningUP
            Player.Mining[1, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000012f };
            Player.Mining[1, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000011f };
            Player.Mining[1, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000010f };
            Player.Mining[1, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000009f };
            Player.Mining[1, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .00000008f };
            Player.Mining[1, 5] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 480, 0, 48, 48, 5, .1f, Game1.Player.position, -16, -16) { LayerDepth = .000000007f };

            //MiningLeft
            Player.Mining[2, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000012f, Flip = true };
            Player.Mining[2, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000011f, Flip = true };
            Player.Mining[2, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000010f, Flip = true };
            Player.Mining[2, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000009f, Flip = true };
            Player.Mining[2, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .00000008f, Flip = true };
            Player.Mining[2, 5] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 256, 0, 48, 48, 5, .1f, Game1.Player.position, -32, -16) { LayerDepth = .000000007f, Flip = true };

            //MiningRight
            Player.Mining[3, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingToolAtlas, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000012f };
            Player.Mining[3, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerHair, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000011f };
            Player.Mining[3, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShirt, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000010f };
            Player.Mining[3, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerPants, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000009f };
            Player.Mining[3, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerShoes, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .00000008f };
            Player.Mining[3, 5] = new Sprite(this.GraphicsDevice, Game1.AllTextures.ChoppingPlayerBase, 256, 0, 48, 48, 5, .1f, Game1.Player.position, 0, -16) { LayerDepth = .000000007f };


            //SwipingDown
            // Player.Swiping[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[0, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[0, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[0, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[0, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[0, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 0, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };


            //SwipingUp
            // Player.Swiping[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[1, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[1, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[1, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[1, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[1, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 800, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };

            //SwipingLeft
            // Player.Swiping[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f, Flip = true };
            Player.Swiping[2, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f, Flip = true };
            Player.Swiping[2, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f, Flip = true };
            Player.Swiping[2, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f, Flip = true };
            Player.Swiping[2, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f, Flip = true };
            Player.Swiping[2, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f, Flip = true };

            //SwipingRight
            //  Player.Swiping[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.SwipingTestTool, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000012f };
            Player.Swiping[3, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerHair, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000011f };
            Player.Swiping[3, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShirt, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000010f };
            Player.Swiping[3, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerPants, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000009f };
            Player.Swiping[3, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerShoes, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .00000008f };
            Player.Swiping[3, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.SwipingPlayerBase, 400, 0, 80, 64, 5, .05f, Game1.Player.position, -32, -14) { LayerDepth = .000000007f };

            Player.PlayerActionAnimations = new Sprite[6];



            //PickUp..Down
            Player.PickUpItem[0, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBlondeHair, 0, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PickUpItem[0, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemRedShirt, 0, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PickUpItem[0, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBluePants, 0, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PickUpItem[0, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBrownShoes, 0, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PickUpItem[0, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBase, 0, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };

            //PickUp..Up
            Player.PickUpItem[1, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBlondeHair, 96, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PickUpItem[1, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemRedShirt, 96, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PickUpItem[1, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBluePants, 96, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PickUpItem[1, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBrownShoes, 96, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PickUpItem[1, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBase, 96, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };
            //PickUp..Left
            Player.PickUpItem[2, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBlondeHair, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f, Flip = true };
            Player.PickUpItem[2, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemRedShirt, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f, Flip = true };
            Player.PickUpItem[2, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBluePants, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f, Flip = true };
            Player.PickUpItem[2, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBrownShoes, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f, Flip = true };
            Player.PickUpItem[2, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBase, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f, Flip = true };
            //PickUp..Right
            Player.PickUpItem[3, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBlondeHair, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PickUpItem[3, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemRedShirt, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PickUpItem[3, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBluePants, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PickUpItem[3, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBrownShoes, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PickUpItem[3, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.PickUpItemBase, 48, 0, 16, 32, 3, .1f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };

            //PortalJump Down
            Player.PortalJump[0, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpHair, 0, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PortalJump[0, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShirt, 0, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PortalJump[0, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpPants, 0, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PortalJump[0, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShoes, 0, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PortalJump[0, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpBase, 0, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };

            //PortalJump Up
            Player.PortalJump[1, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpHair, 96, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PortalJump[1, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShirt, 96, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PortalJump[1, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpPants, 96, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PortalJump[1, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShoes, 96, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PortalJump[1, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpBase, 96, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };
            //PortalJump Left
            Player.PortalJump[2, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpHair, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f, Flip = true };
            Player.PortalJump[2, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShirt, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f, Flip = true };
            Player.PortalJump[2, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpPants, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f, Flip = true };
            Player.PortalJump[2, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShoes, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f, Flip = true };
            Player.PortalJump[2, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpBase, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f, Flip = true };
            //PortalJump Right
            Player.PortalJump[3, 0] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpHair, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000012f };
            Player.PortalJump[3, 1] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShirt, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000011f };
            Player.PortalJump[3, 2] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpPants, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000010f };
            Player.PortalJump[3, 3] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpShoes, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000009f };
            Player.PortalJump[3, 4] = new Sprite(this.GraphicsDevice, Game1.AllTextures.portalJumpBase, 48, 0, 16, 32, 3, .3f, Game1.Player.position, 0, 0) { LayerDepth = .00000008f };

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