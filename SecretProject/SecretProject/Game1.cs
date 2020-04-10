﻿using Microsoft.Xna.Framework;
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
using SecretProject.Class.SavingStuff;
//using XMLDataLib;

using System.Collections.Generic;
using XMLData.DialogueStuff;
using XMLData.ItemStuff;
using XMLData.ItemStuff.LootStuff;
using XMLData.RouteStuff;
using static SecretProject.Class.UI.CheckList;
using XMLData.QuestStuff;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.CollisionDetection;



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
        None = -1,
        Down,
        Up,
        Left,
        Right
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
        MarcusHouse = 10,

        LightHouse = 11,
        UnderWorld = 12,
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
        public static bool EnablePlayerInvincibility = false;
        public static bool EnablePlayerCollisions = true;
        public static bool EnableCutScenes = false;
        public static bool EnableMusic = false;
        public static bool InfiniteArrows = false;
        public static bool GenerateChunkLandscape = true;
        public static bool AllowNaturalNPCSpawning = true;

        public static int NPCSpawnCountLimit = 40;

        public static bool IsFirstTimeStartup;

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager MainMenuContentManager;

        //STAGES
        public static MainMenu mainMenu;
        //public static NormalStage Iliad;
        public static TmxStageBase Town;
        public static TmxStageBase ElixirHouse;


        public static TmxStageBase JulianHouse;
        public static TmxStageBase DobbinHouse;
        public static World OverWorld;
        public static TmxStageBase PlayerHouse;
        public static TmxStageBase GeneralStore;
        public static TmxStageBase KayaHouse;
        public static TmxStageBase Cafe;
        public static World UnderWorld;
        public static TmxStageBase DobbinHouseUpper;
        public static TmxStageBase MarcusHouse;
        public static TmxStageBase LightHouse;


        public static List<ILocation> AllStages;
        public static ILocation CurrentStage;
        public static int PreviousStage = 0;
        public static bool freeze = false;

        //SOUND
        public static SoundBoard SoundManager;

        //INPUT

        public static MouseManager MouseManager;
        public static bool isMyMouseVisible = true;

        public static KeyboardManager KeyboardManager;


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

        //World Quests
        public static WorldQuestHolder WorldQuestHolder;

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
        public DialogueHolder TealDialogue;
        public DialogueHolder MarcusDialogue;
        public DialogueHolder CasparDialogue;

        public RouteSchedule DobbinRouteSchedule;
        public RouteSchedule ElixirRouteSchedule;
        public RouteSchedule KayaRouteSchedule;
        public RouteSchedule JulianRouteSchedule;
        public RouteSchedule SarahRouteSchedule;
        public RouteSchedule MippinRouteSchedule;
        public RouteSchedule NedRouteSchedule;
        public RouteSchedule TealRouteSchedule;
        public RouteSchedule MarcusRouteSchedule;
        public RouteSchedule CasparRouteSchedule;
        public static List<RouteSchedule> AllSchedules;

        public QuestHandler DobbinQuests;
        public QuestHandler ElixirQuests;
        public QuestHandler KayaQuests;
        public QuestHandler JulianQuests;
        public QuestHandler SarahQuests;
        public QuestHandler MippinQuests;
        public QuestHandler NedQuests;
        public QuestHandler TealQuests;
        public QuestHandler MarcusQuests;
        public QuestHandler SnawQuests;
        public QuestHandler BusinessSnailQuests;
        public QuestHandler CasparQuests;



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
        public static Character Elixir;
        public static Character Dobbin;
        public static Character Kaya;

        public static Character Snaw;
        public static Character BusinessSnail;
        public static Character Julian;
        public static Character Sarah;
        public static Character Mippin;
        public static Character Ned;
        public static Character Teal;
        public static Character Marcus;
        public static Character Caspar;
        public static List<Character> AllCharacters;

        //PORTALS
        public static Graph PortalGraph;

        //WEATHER
        public static WeatherType CurrentWeather;
        public static Dictionary<WeatherType, IWeather> AllWeather;

        public static bool IsEventActive;


        //SanctuaryTrackers
        public static SanctuaryTracker ForestTracker;

        //FILEIO
        public static SaveLoadManager SaveLoadManager;

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
            MouseManager = new MouseManager(cam, graphics.GraphicsDevice);






            //SCREEN


            AllActions = new List<ActionTimer>();

            base.Initialize();
        }
        #endregion



        public static ILocation GetCurrentStage()
        {
            return CurrentStage;
        }

        public static ILocation GetStageFromInt(Stages stage)
        {
            return (AllStages[(int)stage]);


        }

        public static Stages GetCurrentStageInt()
        {
            return gameStages;
        }



        #region LOADCONTENT
        protected override void LoadContent()
        {
            IsFirstTimeStartup = true;
            SaveLoadManager = new SaveLoadManager();
            PresentationParameters = this.GraphicsDevice.PresentationParameters;
            MainTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            NightLightsTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            DayLightsTarget = new RenderTarget2D(this.GraphicsDevice, PresentationParameters.BackBufferWidth, PresentationParameters.BackBufferHeight, false, PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            //ORDER MATTERS!!!

            LoadDialogue();

            AllCrops = this.Content.Load<CropHolder>("Crop/CropStuff");
            AllCookingRecipes = this.Content.Load<CookingGuide>("Item/Cooking/CookingGuide");
            //TEXTURES
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            AllTextures = new TextureBook(this.Content, spriteBatch);


            //CONTROLS
            KeyboardManager = new KeyboardManager();
            //  testItem = Content.Load&lt;XMLDataLib.Item&gt;("Level1");

            //SOUND
            SoundManager = new SoundBoard(this, this.Content);

            LoadQuests();

            LoadSchedules();

            LoadPlayer();


            DebugWindow = new DebugWindow(this.GraphicsDevice, AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet, this.GraphicsDevice);

            LoadItems();
            WorldQuestHolder = new WorldQuestHolder();


            Procedural = new Procedural();

            Player.UserInterface = new UserInterface(Player, graphics.GraphicsDevice, this.Content, cam) { graphics = graphics.GraphicsDevice };
            SanctuaryCheckList = new CheckList(graphics.GraphicsDevice, new Vector2(200, 50),
                new List<CheckListRequirement>()
                {new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("Potted ThunderBirch",1790, 1, "plant", false),
                new CheckListRequirement("SuperBulb",1792, 1, "plant", false)
                });

            GlobalClock = new Clock();
            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, MainMenuContentManager, MouseManager, Player.UserInterface);
            // Game1.SaveLoadManager.Load(graphics.GraphicsDevice, Game1.SaveLoadManager.MainMenuData, false);
            PortalGraph = new Graph(13);


            LoadStages();
            LoadShops();

            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });

            LoadCharacters();





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

        private void LoadDialogue()
        {
            ElixirDialogue = this.Content.Load<DialogueHolder>("Dialogue/ElixirDialogue");
            DobbinDialogue = this.Content.Load<DialogueHolder>("Dialogue/DobbinDialogue");
            SnawDialogue = this.Content.Load<DialogueHolder>("Dialogue/SnawDialogue");
            KayaDialogue = this.Content.Load<DialogueHolder>("Dialogue/KayaDialogue");
            JulianDialogue = this.Content.Load<DialogueHolder>("Dialogue/JulianDialogue");
            SarahDialogue = this.Content.Load<DialogueHolder>("Dialogue/SarahDialogue");
            BusinessSnailDialogue = this.Content.Load<DialogueHolder>("Dialogue/BusinessSnailDialogue");
            MippinDialogue = this.Content.Load<DialogueHolder>("Dialogue/MippinDialogue");
            NedDialogue = this.Content.Load<DialogueHolder>("Dialogue/NedDialogue");
            TealDialogue = this.Content.Load<DialogueHolder>("Dialogue/TealDialogue");
            MarcusDialogue = this.Content.Load<DialogueHolder>("Dialogue/MarcusDialogue");
            CasparDialogue = this.Content.Load<DialogueHolder>("Dialogue/CasparDialogue");




            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue, KayaDialogue,
                JulianDialogue, SarahDialogue,
                BusinessSnailDialogue, MippinDialogue, NedDialogue, TealDialogue, MarcusDialogue,CasparDialogue };
            foreach (DialogueHolder holder in tempListHolder)
            {
                holder.RemoveAllNewLines();
            }
            DialogueLibrary = new DialogueLibrary(tempListHolder);
        }
        private void LoadItems()
        {
            ItemVault = new ItemBank();
            AllItems = this.Content.Load<ItemHolder>("Item/ItemHolder");
            ItemVault.ItemDictionary = new Dictionary<int, ItemData>();
            for (int i = 0; i < AllItems.AllItems.Count; i++)
            {
                ItemVault.ItemDictionary.Add(AllItems.AllItems[i].ID, AllItems.AllItems[i]);
            }

            LootBank = new LootBank(Content.Load<LootHolder>("Item/Loot/LootHolder"));

        }
        private void LoadSchedules()
        {
            DobbinRouteSchedule = this.Content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = this.Content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            KayaRouteSchedule = this.Content.Load<RouteSchedule>("Route/KayaRouteSchedule");
            JulianRouteSchedule = this.Content.Load<RouteSchedule>("Route/JulianRouteSchedule");
            SarahRouteSchedule = this.Content.Load<RouteSchedule>("Route/SarahRouteSchedule");
            MippinRouteSchedule = this.Content.Load<RouteSchedule>("Route/MippinRouteSchedule");
            NedRouteSchedule = this.Content.Load<RouteSchedule>("Route/NedRouteSchedule");
            TealRouteSchedule = this.Content.Load<RouteSchedule>("Route/TealRouteSchedule");
            MarcusRouteSchedule = this.Content.Load<RouteSchedule>("Route/MarcusRouteSchedule");
            CasparRouteSchedule = this.Content.Load<RouteSchedule>("Route/CasparRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule, KayaRouteSchedule,
                JulianRouteSchedule, SarahRouteSchedule, MippinRouteSchedule, NedRouteSchedule, TealRouteSchedule, MarcusRouteSchedule, CasparRouteSchedule };
            for (int i = 0; i < AllSchedules.Count; i++)
            {
                foreach (Route route in AllSchedules[i].Routes)
                {
                    route.ProcessStageToEndAt();
                }
            }
        }
        private void LoadQuests()
        {
            DobbinQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/DobbinQuests"));
            ElixirQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/ElixirQuests"));
            KayaQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/KayaQuests"));
            JulianQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/JulianQuests"));
            MippinQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/MippinQuests"));
            TealQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/TealQuests"));
            MarcusQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/MarcusQuests"));
            NedQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/NedQuests"));
            SnawQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/SnawQuests"));
            BusinessSnailQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/BusinessSnailQuests"));
            CasparQuests = new QuestHandler(Content.Load<QuestHolder>("QuestStuff/CasparQuests"));
        }
        private void LoadStages()
        {
            Town = new TmxStageBase("Town", LocationType.Exterior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 1)
            { StageIdentifier = (int)Stages.Town };





            ElixirHouse = new TmxStageBase("ElixirHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/elixirShop.tmx", 1, 0) { StageIdentifier = (int)Stages.ElixirHouse };
            JulianHouse = new TmxStageBase("JulianHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/JulianShop.tmx", 1, 0) { StageIdentifier = (int)Stages.JulianHouse };
            OverWorld = new World("OverWorld", LocationType.Exterior, StageType.Procedural, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.OverWorld };
            DobbinHouse = new TmxStageBase("DobbinHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/DobbinHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.DobbinHouse };
            PlayerHouse = new TmxStageBase("PlayerHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/PlayerHouseSmall.tmx", 1, 0) { StageIdentifier = (int)Stages.PlayerHouse };
            GeneralStore = new TmxStageBase("GeneralStore", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/GeneralStore.tmx", 1, 0) { StageIdentifier = (int)Stages.GeneralStore };
            KayaHouse = new TmxStageBase("KayaHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/KayaHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.KayaHouse };
            Cafe = new TmxStageBase("Cafe", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/Cafe.tmx", 1, 0) { StageIdentifier = (int)Stages.Cafe };
            UnderWorld = new World("CaveWorld", LocationType.Exterior, StageType.Procedural, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.MasterTileSet, "Content/bin/DesktopGL/Map/Town.tmx", 1, 0) { StageIdentifier = (int)Stages.UnderWorld };
            DobbinHouseUpper = new TmxStageBase("DobbinHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/DobbinHouseUpper.tmx", 1, 0) { StageIdentifier = (int)Stages.DobbinHouse };
            MarcusHouse = new TmxStageBase("MarcusHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/MarcusHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.MarcusHouse };
            LightHouse = new TmxStageBase("LightHouse", LocationType.Interior, StageType.Standard, graphics.GraphicsDevice, HomeContentManager, 0, AllTextures.InteriorTileSet1, "Content/bin/DesktopGL/Map/LightHouse.tmx", 1, 0) { StageIdentifier = (int)Stages.LightHouse };






            AllStages = new List<ILocation>() { Town, ElixirHouse, JulianHouse, OverWorld, DobbinHouse, PlayerHouse, GeneralStore, KayaHouse, Cafe, DobbinHouseUpper, MarcusHouse, LightHouse, UnderWorld };

        }
        private void LoadShops()
        {

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

            Shop SarahShop = new Shop(graphics.GraphicsDevice, 7, "SarahShop", new ShopMenu("SarahShopInventory", graphics.GraphicsDevice, 10));
            SarahShop.ShopMenu.TryAddStock(1099, 15);
            SarahShop.ShopMenu.TryAddStock(1100, 15);
            AllShops = new List<IShop>()
            {
                ToolShop,
                DobbinShop,
                JulianShop,
                ElixirShop,
                KayaShop,
                BuisnessSnailShop,
                SarahShop
            };
        }
        private void LoadCharacters()
        {

            Vector2 elixirPosition = Character.GetWorldPosition(new Vector2(23, 10));
            Elixir = new Character("Elixir", new Vector2(23, 10), graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, AllSchedules[1], Stages.ElixirHouse, false, ElixirQuests, AllTextures.ElixirPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, 48, 0, 16, 48, 6, .15f, elixirPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet,  144, 0, 28, 48, 6, .15f, elixirPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet,  240, 0, 16, 48, 6, .15f, elixirPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet,336, 0, 16, 48, 6, .15f, elixirPosition)
                },

                NPCRectangleXOffSet = 8,
                NPCRectangleYOffSet = 34,
                NPCRectangleHeightOffSet = 8,
                NPCRectangleWidthOffSet = 8,
                SpeakerID = 1,


            DebugColor = Color.HotPink,
            };
            Elixir.LoadLaterStuff(graphics.GraphicsDevice);






            Vector2 dobbinPosition = Character.GetWorldPosition(new Vector2(18, 8));
            Dobbin = new Character("Dobbin", new Vector2(18, 8), graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, AllSchedules[0], Stages.DobbinHouse, false, DobbinQuests, AllTextures.DobbinPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet,0, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet,  167, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, 335, 0, 28, 48, 6, .15f, dobbinPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet,503, 0, 28, 48, 6, .15f, dobbinPosition)
                },

                NPCRectangleXOffSet = 15,
            NPCRectangleYOffSet = 30,
            NPCRectangleHeightOffSet = 2,
           NPCRectangleWidthOffSet = 2,
            SpeakerID = 2,

            DebugColor = Color.HotPink,
            };
           Dobbin.LoadLaterStuff(graphics.GraphicsDevice);



            Vector2 kayaPosition = Character.GetWorldPosition(new Vector2(20, 19));
            Kaya = new Character("Kaya", new Vector2(20, 19), graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, AllSchedules[2], Stages.KayaHouse, false, KayaQuests, AllTextures.KayaPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, 0, 0, 16, 34, 6, .15f, kayaPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, 112, 0, 16, 34, 7, .15f, kayaPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, 224, 0, 16, 34, 7, .15f, kayaPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, 336, 0, 16, 34, 6, .15f, kayaPosition)
                },

                NPCRectangleXOffSet = 5,
                NPCRectangleYOffSet = 15,
                NPCRectangleHeightOffSet = 12,
                NPCRectangleWidthOffSet = 8,
                SpeakerID = 4,

                DebugColor = Color.HotPink,
            };
            Kaya.LoadLaterStuff(graphics.GraphicsDevice);

            Snaw = new Character("Snaw", new Vector2(121, 67), graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                3, AllTextures.SnawPortrait, SnawQuests)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                0, 0, 72, 96, 3, .3f, new Vector2(1280, 500)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 3,
                CurrentStageLocation = Stages.Town,
                FrameToSet = 3,
                IsBasicNPC = true
            };

            Vector2 julianPosition = Character.GetWorldPosition(new Vector2(16, 9));
            Julian = new Character("Julian", new Vector2(16, 9), graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, AllSchedules[3], Stages.JulianHouse, false, JulianQuests, AllTextures.JulianPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, 0, 0, 16, 34, 6, .15f, julianPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, 96, 0, 16, 34, 7, .15f, julianPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, 208, 0, 16, 34, 7, .15f, julianPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, 320, 0, 16, 34, 6, .15f, julianPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 5,

                DebugColor = Color.HotPink,
            };
            Julian.LoadLaterStuff(graphics.GraphicsDevice);


            Vector2 sarahPosition = Character.GetWorldPosition(new Vector2(40, 21));
            Sarah = new Character("Sarah", new Vector2(40, 21), graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, AllSchedules[4], Stages.OverWorld, false, SarahQuests, AllTextures.SarahPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, 0, 0, 16, 32, 6, .15f, sarahPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, 96, 0, 16, 32, 7, .15f, sarahPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, 96, 0, 16, 32, 7, .15f, sarahPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SarahSpriteSheet, 208, 0, 16, 32, 6, .15f, sarahPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 6,

                DebugColor = Color.HotPink,
            };
            Sarah.LoadLaterStuff(graphics.GraphicsDevice);


            BusinessSnail = new Character("Business Snail", new Vector2(34, 80), graphics.GraphicsDevice, Game1.AllTextures.BusinessSnail,
                1, AllTextures.BusinessSnailPortrait, BusinessSnailQuests)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.BusinessSnail,
                0, 0, 32, 32, 1, 1f, new Vector2(1280, 600)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 7,
                CurrentStageLocation = Stages.Town,
                FrameToSet = 0,
                IsBasicNPC = true
            };





            Vector2 mippinPosition = Character.GetWorldPosition(new Vector2(40, 21));
            Mippin = new Character("Mippin", new Vector2(40, 21), graphics.GraphicsDevice, Game1.AllTextures.Mippin, AllSchedules[5], Stages.OverWorld, false, MippinQuests, AllTextures.MippinPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Mippin, 0, 0, 16, 32, 6, .15f, mippinPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Mippin, 96, 0, 16, 32, 7, .15f, mippinPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Mippin, 96, 0, 16, 32, 7, .15f, mippinPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Mippin, 208, 0, 16, 32, 6, .15f, mippinPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 8,

                DebugColor = Color.HotPink,
            };
            Mippin.LoadLaterStuff(graphics.GraphicsDevice);


            Vector2 nedPosition = Character.GetWorldPosition(new Vector2(110, 11));
            Ned = new Character("Ned", new Vector2(110, 11), graphics.GraphicsDevice, Game1.AllTextures.Ned, AllSchedules[6], Stages.OverWorld, false, NedQuests, AllTextures.NedPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Ned, 0, 0, 16, 32, 6, .15f, nedPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Ned, 96, 0, 16, 32, 7, .15f, nedPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Ned, 96, 0, 16, 32, 7, .15f, nedPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Ned, 208, 0, 16, 32, 6, .15f, nedPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 9,

                DebugColor = Color.HotPink,
            };
            Ned.LoadLaterStuff(graphics.GraphicsDevice);



            Vector2 tealPosition = Character.GetWorldPosition(new Vector2(45, 80));
            Teal = new Character("Teal", new Vector2(45, 80), graphics.GraphicsDevice, Game1.AllTextures.Teal, AllSchedules[7], Stages.OverWorld, false, TealQuests, AllTextures.TealPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Teal, 0, 0, 16, 32, 6, .15f, tealPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Teal, 96, 0, 16, 32, 7, .15f, tealPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Teal, 96, 0, 16, 32, 7, .15f, tealPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Teal, 208, 0, 16, 32, 6, .15f, tealPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 10,

                DebugColor = Color.HotPink,
            };
            Teal.LoadLaterStuff(graphics.GraphicsDevice);


            Vector2 marcusPosition = Character.GetWorldPosition(new Vector2(11, 30));
            Marcus = new Character("Marcus", new Vector2(11, 30), graphics.GraphicsDevice, Game1.AllTextures.Marcus, AllSchedules[8], Stages.MarcusHouse, false, MarcusQuests, AllTextures.MarcusPotrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Marcus, 0, 0, 16, 32, 6, .15f, marcusPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Marcus, 96, 0, 16, 32, 7, .15f, marcusPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Marcus, 96, 0, 16, 32, 7, .15f, marcusPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Marcus, 208, 0, 16, 32, 6, .15f, marcusPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 11,
                // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);

                DebugColor = Color.HotPink,
            };
            Marcus.LoadLaterStuff(graphics.GraphicsDevice);


            Vector2 casparPosition = Character.GetWorldPosition(new Vector2(11, 30));
            Caspar = new Character("Caspar", new Vector2(11, 30), graphics.GraphicsDevice, Game1.AllTextures.Caspar, AllSchedules[9], Stages.MarcusHouse,false, CasparQuests, AllTextures.CasparPortrait)
            {
                FrameToSet = 0,
                NPCAnimatedSprite = new Sprite[]
                {
             new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Caspar, 0, 0, 16, 32, 6, .15f, casparPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Caspar, 96, 0, 16, 32, 7, .15f, casparPosition),
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Caspar, 96, 0, 16, 32, 7, .15f, casparPosition) { Flip = true },
            new Sprite(graphics.GraphicsDevice, Game1.AllTextures.Caspar, 208, 0, 16, 32, 6, .15f, casparPosition)
                },

                NPCRectangleXOffSet = 7,
                NPCRectangleYOffSet = 30,
                NPCRectangleHeightOffSet = 2,
                NPCRectangleWidthOffSet = 2,
                SpeakerID = 12,
                // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
                
                DebugColor = Color.HotPink,
            };
            Caspar.LoadLaterStuff(graphics.GraphicsDevice);

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
                Ned,
                Teal,
                Marcus,
                Caspar
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


                default:
                    return ForestTracker;
            }


        }

        //check portal from previous and current stage and set the player to the new position specified. Must be called after loading content.



        public static void SwitchStage(Stages currentStage, Stages stageToSwitchTo, Portal portal = null)
        {
            Player.UserInterface.BeginBlackTransition(.05f);

            Game1.Player.UserInterface.TransitionSpeed = .05f;
            Game1.Player.UserInterface.TransitionTimer.TargetTime = 2f;
            ILocation location = GetStageFromInt(currentStage);
            location.UnloadContent();
            ILocation newLocation = GetStageFromInt(stageToSwitchTo);
            gameStages = (Stages)stageToSwitchTo;
            if (newLocation.LocationType == LocationType.Interior || gameStages == Stages.OverWorld || gameStages == Stages.UnderWorld)
            {
                Game1.Player.LockBounds = false;

            }
            else
            {
                Game1.Player.LockBounds = true;
            }
            ILocation newStage = GetStageFromInt(stageToSwitchTo);
            if (!newStage.IsLoaded)
            {

                newStage.LoadContent(cam, AllSchedules);

            }
            newStage.ReloadContent();
            // List<Portal> testPortal = GetCurrentStage().AllPortals;
            if (portal != null)
            {
                //  ILocation location = GetCurrentStage();
                // List<Portal> newStageTestPortals = GetCurrentStage().AllPortals;
                List<Portal> portalTest = GetStageFromInt(stageToSwitchTo).AllPortals;
                Portal tempPortal = GetStageFromInt(stageToSwitchTo).AllPortals.Find(z => z.From == portal.To && z.To == portal.From);
                if (tempPortal != null)
                {
                    float x = tempPortal.PortalStart.X;
                    float width = tempPortal.PortalStart.Width / 2;
                    float y = tempPortal.PortalStart.Y;
                    float safteyX = tempPortal.SafteyOffSetX;
                    float safteyY = tempPortal.SafteyOffSetY;
                    Player.position = new Vector2(x + width + safteyX, y + safteyY);
                }
                //else if (Game1.GetCurrentStage() == Town)
                //{
                //    Player.Position = new Vector2(1232, 304);
                //}


                //Player.UpdateMovementAnimationsOnce(gameTime);

            }

            //Player.PlayerWardrobe.UpdateMovementAnimations(Player.Position, true);
            if (GetStageFromInt(stageToSwitchTo) == OverWorld)
            {
                Game1.OverWorld.AllTiles.LoadInitialChunks(Game1.Player.Position);
            }

            CurrentStage = newLocation;
        }


        public static void FullScreenToggle()
        {

            ToggleFullScreen = true;
        }

        #region UPDATE
        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.Update();
            // IsEventActive = false;

            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            MouseManager.Update(gameTime);
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

                        mainMenu.Update(gameTime, MouseManager, this);
                        break;

                    case Stages.OverWorld:

                        OverWorld.Update(gameTime, MouseManager, Player);
                        break;


                    case Stages.Town:

                        Town.Update(gameTime, MouseManager, Player);
                        break;


                    case Stages.ElixirHouse:
                        ElixirHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.JulianHouse:
                        JulianHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.DobbinHouse:
                        DobbinHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.PlayerHouse:
                        PlayerHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.GeneralStore:
                        GeneralStore.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.KayaHouse:
                        KayaHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.Cafe:
                        Cafe.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.DobbinHouseUpper:
                        DobbinHouseUpper.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.MarcusHouse:
                        MarcusHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.LightHouse:
                        LightHouse.Update(gameTime, MouseManager, Player);
                        break;
                    case Stages.UnderWorld:
                        UnderWorld.Update(gameTime, MouseManager, Player);
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



            if (!MouseManager.ToggleGeneralInteraction)
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
                    mainMenu.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, MouseManager);
                    break;

                case Stages.OverWorld:
                    this.GraphicsDevice.Clear(Color.Black);
                    OverWorld.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;

                case Stages.Town:
                    this.GraphicsDevice.Clear(Color.Black);
                    Town.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;

                case Stages.ElixirHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    ElixirHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.JulianHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    JulianHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.DobbinHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    DobbinHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;

                case Stages.PlayerHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    PlayerHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.GeneralStore:
                    this.GraphicsDevice.Clear(Color.Black);
                    GeneralStore.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.KayaHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    KayaHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.Cafe:
                    this.GraphicsDevice.Clear(Color.Black);
                    Cafe.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.DobbinHouseUpper:
                    this.GraphicsDevice.Clear(Color.Black);
                    DobbinHouseUpper.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.MarcusHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    MarcusHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.LightHouse:
                    this.GraphicsDevice.Clear(Color.Black);
                    LightHouse.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
                    break;
                case Stages.UnderWorld:
                    this.GraphicsDevice.Clear(Color.Black);
                    UnderWorld.Draw(graphics.GraphicsDevice, MainTarget, NightLightsTarget, DayLightsTarget, gameTime, spriteBatch, MouseManager, Player);
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
            Player = new Player("NAME", new Vector2(600, 600), AllTextures.PlayerBase, 5, this.Content, graphics.GraphicsDevice, MouseManager) { Activate = true, IsDrawn = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);



            //Player.PlayerMovementAnimations = new Sprite[5];
            //for (int i = 0; i < Player.PlayerWardrobe.BasicMovementAnimations.GetLength(1); i++)
            //{
            //    Player.PlayerMovementAnimations[i] = Player.PlayerWardrobe.BasicMovementAnimations[0, i];
            //}

            //for (int i = 0; i < Player.PlayerMovementAnimations.GetLength(0); i++)
            //{
            //    Player.PlayerMovementAnimations[i].SourceRectangle = new Rectangle((int)(Player.PlayerMovementAnimations[i].FirstFrameX + Player.PlayerMovementAnimations[i].FrameWidth * Player.PlayerMovementAnimations[i].CurrentFrame),
            //        (int)Player.PlayerMovementAnimations[i].FirstFrameY, (int)Player.PlayerMovementAnimations[i].FrameWidth, (int)Player.PlayerMovementAnimations[i].FrameHeight);
            //    Player.PlayerMovementAnimations[i].DestinationRectangle = new Rectangle((int)Player.PlayerMovementAnimations[i].Position.X + Player.PlayerMovementAnimations[i].OffSetX,
            //        (int)Player.PlayerMovementAnimations[i].Position.Y + Player.PlayerMovementAnimations[i].OffSetY, Player.PlayerMovementAnimations[i].FrameWidth, Player.PlayerMovementAnimations[i].FrameHeight);

            //}
        }

    }

}