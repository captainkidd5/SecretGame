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
using SecretProject.Class.ObjectFolder;
using XMLData.DialogueStuff;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ShopStuff;
using XMLData.RouteStuff;
using XMLData.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding;
using static SecretProject.Class.UI.CheckList;
using SecretProject.Class.EventStuff;


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
        Pass = 1,
        Center = 2,
        World = 3,
        Sanctuary = 4,
        ElixirHouse = 5,
        JulianHouse = 6,
        DobbinHouse = 7,
        MainMenu = 50,
        Exit = 55

    }


    public class Game1 : Game
    {
        #region FIELDS

        public static bool IsFirstTimeStartup;

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager SeaContentManager;
        public ContentManager OrchardContentManager;
        public ContentManager MainMenuContentManager;

        //STAGES
        public static MainMenu mainMenu;
        //public static NormalStage Iliad;
        public static Town Town;
        public static TmxStageBase ElixirHouse;
        public static TmxStageBase Pass;
        public static TmxStageBase Sanctuary;
        public static TmxStageBase Center;
        public static TmxStageBase JulianHouse;
        public static TmxStageBase DobbinHouse;
        public static World World;
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

        public RouteSchedule DobbinRouteSchedule;
        public RouteSchedule ElixirRouteSchedule;
        public RouteSchedule KayaRouteSchedule;
        public RouteSchedule JulianRouteSchedule;
        public static List<RouteSchedule> AllSchedules;
        public static ItemHolder AllItems;

        public static CropHolder AllCrops;

        //DIALOGUE
        public static DialogueLibrary DialogueLibrary;

        public static List<IShop> AllShops { get; set; }

        //RENDERTARGETS
        public RenderTarget2D MainTarget;
        public RenderTarget2D LightsTarget;


        public static PresentationParameters PresentationParameters;

        //event handlers
        //Events
        public static List<IEvent> AllEvents;

        //NPCS
        public static Elixir Elixer;
        public static Dobbin Dobbin;
        public static Kaya Kaya;

        public static Character Snaw;
        public static Julian Julian;
        public static List<Character> AllCharacters;

        //PORTALS
        public static Graph PortalGraph;

        public static bool IsEventActive;


        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            HomeContentManager = new ContentManager(Content.ServiceProvider);
            SeaContentManager = new ContentManager(Content.ServiceProvider);
            OrchardContentManager = new ContentManager(Content.ServiceProvider);
            MainMenuContentManager = new ContentManager(Content.ServiceProvider);
            Content.RootDirectory = "Content";
            SeaContentManager.RootDirectory = "Content";
            HomeContentManager.RootDirectory = "Content";
            OrchardContentManager.RootDirectory = "Content";
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
            Utility = new Utility();
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

                case Stages.World:
                    return World;

                case Stages.Center:
                    return Center;
                case Stages.Town:
                    return Town;
                case Stages.Pass:
                    return Pass;
                case Stages.Sanctuary:
                    return Sanctuary;

                case Stages.ElixirHouse:
                    return ElixirHouse;
                case Stages.JulianHouse:
                    return JulianHouse;
                case Stages.DobbinHouse:
                    return DobbinHouse;

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
        public static ILocation GetStageFromInt(int stageNumber)
        {
            switch (stageNumber)
            {
                case 0:
                    return Town;
                case 1:
                    return Pass;
                case 2:
                    return Center;
                case 3:
                    return World;
                case 4:
                    return Sanctuary;
                case 5:
                    return ElixirHouse;
                case 6:
                    return JulianHouse;
                case 7:
                    return DobbinHouse;
                default:
                    return null;

            }

        }

        public static int GetCurrentStageInt()
        {
            switch (gameStages)
            {
                case Stages.Town:
                    return 0;
                case Stages.Pass:
                    return 1;
                case Stages.Center:
                    return 2;

                case Stages.World:
                    return 3;

                case Stages.Sanctuary:
                    return 4;

                case Stages.ElixirHouse:
                    return 5;
                case Stages.JulianHouse:
                    return 6;
                case Stages.DobbinHouse:
                    return 7;

                default:
                    return 50;

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

            DobbinRouteSchedule = Content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = Content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            KayaRouteSchedule = Content.Load<RouteSchedule>("Route/KayaRouteSchedule");
            JulianRouteSchedule = Content.Load<RouteSchedule>("Route/JulianRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule, KayaRouteSchedule, JulianRouteSchedule };
            AllCrops = Content.Load<CropHolder>("Crop/CropStuff");

            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue, KayaDialogue, JulianDialogue };
            DialogueLibrary = new DialogueLibrary(tempListHolder);
            //TEXTURES
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AllTextures = new TextureBook(Content, spriteBatch);
            MainCharacterTexture = AllTextures.MainCharacterSpriteStrip;


            //  testItem = Content.Load&lt;XMLDataLib.Item&gt;("Level1");

            //SOUND
            SoundManager = new SoundBoard(this, Content);


            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");
            //PLAYERS
            Player = new Player("joe", new Vector2(1600, 700), AllTextures.PlayerParts, 4, 10, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);

            //meaning hair of direction forward:
            Player.animations[0, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 16, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000011f };
            //head
            Player.animations[0, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 64, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000010f };
            //right arm
            Player.animations[0, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 112, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000009f };
            //right hand
            Player.animations[0, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 160, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000008f };
            //left arm
            Player.animations[0, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 208, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000007f };
            //left hand
            Player.animations[0, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 256, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000006f };
            Player.animations[0, 6] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 304, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000005f };
            Player.animations[0, 7] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 352, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000004f };
            Player.animations[0, 8] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 400, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000003f };
            Player.animations[0, 9] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 448, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000002f };
            //left shoe
            //right shoe
            //legs

            //up
            Player.animations[1, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 16, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000011f };
            Player.animations[1, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 64, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000010f };
            Player.animations[1, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 112, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000009f };
            Player.animations[1, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 160, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000008f };
            Player.animations[1, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 208, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000007f };
            Player.animations[1, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 256, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000006f };
            Player.animations[1, 6] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 304, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000005f };
            Player.animations[1, 7] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 352, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000004f };
            Player.animations[1, 8] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 400, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000003f };
            Player.animations[1, 9] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 336, 448, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000002f };

            //Left
            Player.animations[2, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 16, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000011f };
            Player.animations[2, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 64, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000010f };
            Player.animations[2, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 112, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000009f };
            Player.animations[2, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 160, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000008f };
            Player.animations[2, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 208, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000007f };
            Player.animations[2, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 256, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000006f };
            Player.animations[2, 6] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 304, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000005f };
            Player.animations[2, 7] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 352, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000004f };
            Player.animations[2, 8] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 400, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000003f };
            Player.animations[2, 9] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 240, 448, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000002f };

            //Right
            Player.animations[3, 0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 16, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000011f };
            Player.animations[3, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 64, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000010f };
            Player.animations[3, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 112, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000009f };
            Player.animations[3, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 160, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000008f };
            Player.animations[3, 4] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 208, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000007f };
            Player.animations[3, 5] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 256, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000006f };
            Player.animations[3, 6] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 304, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000005f };
            Player.animations[3, 7] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 352, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000004f };
            Player.animations[3, 8] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 400, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000003f };
            Player.animations[3, 9] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 144, 448, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000002f };




            //Player.PlayerMovementAnimations = Player.animations[0];
            Player.PlayerMovementAnimations = new Sprite[10];
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

            //UI

            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet, GraphicsDevice);

            //ITEMS
            ItemVault = new ItemBank();


            AllItems = Content.Load<ItemHolder>("Item/ItemHolder");


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
            Town = new Town("Town", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Town.tmx", 1, 1) { StageIdentifier = 0 };
            Pass = new TmxStageBase("Pass", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Pass.tmx", 1, 1) { StageIdentifier = 1 };
            Center = new TmxStageBase("Center", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/Center.tmx", 1, 0) { StageIdentifier = 2 };
            World = new World("World", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Town.tmx", 1, 0) { StageIdentifier = 3 };


            Sanctuary = new TmxStageBase("Sanctuary", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Sanctuary.tmx", 1, 1) { StageIdentifier = 4, BackDropPosition = new Vector2(900, 50) };

            ElixirHouse = new TmxStageBase("ElixirHouse", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/elixirShop.tmx", 1, 0) { StageIdentifier = 5 };
            JulianHouse = new TmxStageBase("JulianHouse", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/JulianShop.tmx", 1, 0) { StageIdentifier = 6 };
            DobbinHouse = new TmxStageBase("DobbinHouse", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/DobbinHouse.tmx", 1, 0) { StageIdentifier = 7 };
            GlobalClock = new Clock();



            AllStages = new List<ILocation>() { Pass, Town, Center, World, Sanctuary, ElixirHouse, JulianHouse,DobbinHouse };
            PortalGraph = new Graph(AllStages.Count);



            Shop ToolShop = new Shop(graphics.GraphicsDevice, 1, "ToolShop", new ShopMenu("ToolShopInventory", graphics.GraphicsDevice, 25));
            ToolShop.ShopMenu.TryAddStock(3, 1);
            ToolShop.ShopMenu.TryAddStock(0, 1);
            ToolShop.ShopMenu.TryAddStock(1, 1);
            ToolShop.ShopMenu.TryAddStock(4, 1);
            ToolShop.ShopMenu.TryAddStock(140, 50);
            ToolShop.ShopMenu.TryAddStock(2, 1);
            ToolShop.ShopMenu.TryAddStock(160, 50);
            ToolShop.ShopMenu.TryAddStock(187, 15);
            ToolShop.ShopMenu.TryAddStock(128, 99);
            ToolShop.ShopMenu.TryAddStock(121, 5);
            ToolShop.ShopMenu.TryAddStock(127, 3);
            ToolShop.ShopMenu.TryAddStock(161, 5);
            ToolShop.ShopMenu.TryAddStock(80, 5);
            ToolShop.ShopMenu.TryAddStock(167, 9); //bloodcorn seeds
            ToolShop.ShopMenu.TryAddStock(231, 5);
            ToolShop.ShopMenu.TryAddStock(221, 5);
            ToolShop.ShopMenu.TryAddStock(191, 1);
            ToolShop.ShopMenu.TryAddStock(123, 10);
            ToolShop.ShopMenu.TryAddStock(212, 10);
            ToolShop.ShopMenu.TryAddStock(126, 10);
            ToolShop.ShopMenu.TryAddStock(170, 10);

            Shop DobbinShop = new Shop(graphics.GraphicsDevice, 2, "DobbinShop", new ShopMenu("DobbinShopInventory", graphics.GraphicsDevice, 5));
            DobbinShop.ShopMenu.TryAddStock(128, 10);
            DobbinShop.ShopMenu.TryAddStock(167, 10);

            Shop JulianShop = new Shop(graphics.GraphicsDevice, 3, "JulianShop", new ShopMenu("JulianShopInventory", graphics.GraphicsDevice, 10));
            JulianShop.ShopMenu.TryAddStock(0, 5);
            JulianShop.ShopMenu.TryAddStock(1, 5);
            JulianShop.ShopMenu.TryAddStock(2, 5);
            JulianShop.ShopMenu.TryAddStock(3, 5);

            Shop ElixirShop = new Shop(graphics.GraphicsDevice, 4, "ElixirShop", new ShopMenu("ElixirShopInventory", graphics.GraphicsDevice, 10));
            ElixirShop.ShopMenu.TryAddStock(80, 1);
            ElixirShop.ShopMenu.TryAddStock(81, 1);
            ElixirShop.ShopMenu.TryAddStock(82, 1);
            ElixirShop.ShopMenu.TryAddStock(83, 1);
            ElixirShop.ShopMenu.TryAddStock(84, 1);
            ElixirShop.ShopMenu.TryAddStock(85, 1);
            AllShops = new List<IShop>()
            {
                ToolShop,
                DobbinShop,
                JulianShop,
                ElixirShop
            };




            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });

            Elixer = new Elixir("Elixer", new Vector2(1450, 800), graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, AllSchedules[1]) { FrameToSet = 0 };
            Dobbin = new Dobbin("Dobbin", new Vector2(160, 128), graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, AllSchedules[0]) { FrameToSet = 0 };
            Kaya = new Kaya("Kaya", new Vector2(512, 240), graphics.GraphicsDevice, Game1.AllTextures.KayaSpriteSheet, AllSchedules[2]) { FrameToSet = 0 };
            Snaw = new Character("Snaw", new Vector2(1280, 500), graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                3)
            {
                NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                0, 0, 72, 96, 3, .3f, new Vector2(1280, 500)) { IsAnimated = true,  } },
                CurrentDirection = 0,
                SpeakerID = 3,
                CurrentStageLocation = 4,
                FrameToSet = 3,
                IsBasicNPC = true
            };
            Julian = new Julian("Julian", new Vector2(192, 128), graphics.GraphicsDevice, Game1.AllTextures.JulianSpriteSheet, AllSchedules[3]) { FrameToSet = 0 };
            AllCharacters = new List<Character>()
            {
                Elixer,
                Dobbin,
                Kaya,
                Snaw,
                Julian
            };

            foreach (ILocation stage in AllStages)
            {
                foreach (Character character in AllCharacters)
                {
                    if (character.CurrentStageLocation == stage.StageIdentifier)
                    {
                        stage.CharactersPresent.Add(character);
                    }
                }



            }

            AllEvents = new List<IEvent>()
            {
                new IntroduceSanctuary(),
                new IntroduceJulianShop()
            };
            IsEventActive = false;

            RectangleOutlineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            RectangleOutlineTexture.SetData(new Color[] { Color.Red });
        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion

        //check portal from previous and current stage and set the player to the new position specified. Must be called after loading content.



        public static void SwitchStage(int currentStage, int stageToSwitchTo, Portal portal = null)
        {


            GetStageFromInt(currentStage).UnloadContent();
            gameStages = (Stages)stageToSwitchTo;
            if(gameStages == Stages.World)
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
            IsEventActive = false;
            OldKeyBoardState = NewKeyBoardState;
            NewKeyBoardState = Keyboard.GetState();
            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager.Update(gameTime);
            DebugWindow.Update(gameTime);

            //SOUND
            MediaPlayer.IsRepeating = true;

            //KEYBOARD

            if (ToggleFullScreen)
            {
                graphics.ToggleFullScreen();
                ToggleFullScreen = false;
            }

            foreach (IEvent e in AllEvents)
            {
                if (e.DayToTrigger == GlobalClock.TotalDays && e.StageToTrigger == GetCurrentStageInt() && !e.IsCompleted)
                {
                    int num = GetCurrentStageInt();
                    if (!e.IsActive)
                    {
                        e.Start();
                    }
                    else
                    {
                        IsEventActive = true;
                        e.Update(gameTime);

                    }

                }
            }

            if (!IsEventActive)
            {

                switch (gameStages)
                {
                    case Stages.MainMenu:

                        mainMenu.Update(gameTime, myMouseManager, this);
                        break;

                    case Stages.World:

                        World.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.Center:
                        Center.Update(gameTime, myMouseManager, Player);
                        break;

                    case Stages.Pass:
                        Pass.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.Sanctuary:
                        Sanctuary.Update(gameTime, myMouseManager, Player);
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

                }


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

                case Stages.World:
                    GraphicsDevice.Clear(Color.Black);
                    World.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.Center:
                    GraphicsDevice.Clear(Color.Black);
                    Center.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.Pass:
                    GraphicsDevice.Clear(Color.Black);
                    Pass.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.Sanctuary:
                    GraphicsDevice.Clear(Color.Black);
                    Sanctuary.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
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

            }

            Game1.DebugWindow.Draw(spriteBatch);

            base.Draw(gameTime);
        }
        #endregion

    }

}