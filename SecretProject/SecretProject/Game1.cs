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
        MainMenu = 0,
        World = 1,
        Wilderness = 2,
        Exit = 3,
        Pass = 4,
        Town = 5,
        GreatLibrary = 6,
        WestBeach = 7,
        DobbinsOrchard = 8,
        ElixirShop = 9
            
    }


    public class Game1 : Game
    {
        #region FIELDS

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager SeaContentManager;
        public ContentManager OrchardContentManager;
        public ContentManager MainMenuContentManager;

        //STAGES
        public static MainMenu mainMenu;
        //public static NormalStage Iliad;
        public static TmxStageBase LodgeInterior;
        public static Town Town;
        public static TmxStageBase GreatLibrary;
        public static TmxStageBase WestBeach;
        public static TmxStageBase DobbinsOrchard;
        public static TmxStageBase ElixirShop;
        public static TmxStageBase Pass;
        public static Wilderness Wilderness;
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

        //CLOCK
        public static Clock GlobalClock;

        //ITEMS
        public static ItemBank ItemVault;

        //XMLDATA

        public DialogueHolder ElixirDialogue;
        public DialogueHolder DobbinDialogue;
        public DialogueHolder SnawDialogue;

        public RouteSchedule DobbinRouteSchedule;
        public RouteSchedule ElixirRouteSchedule;
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

        //NPCS
        public static Elixir Elixer;
        public static Dobbin Dobbin;

        public static Character Snaw;
        public static List<Character> AllCharacters;


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

            //CAMERA
            cam = new Camera2D(GraphicsDevice.Viewport);
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager = new MouseManager(cam, graphics.GraphicsDevice);

            ScreenRectangle.Width = graphics.PreferredBackBufferWidth;
            ScreenRectangle.Height = graphics.PreferredBackBufferHeight;

            Utility = new Utility();
            

            //SCREEN
            

            AllActions = new List<ActionTimer>();

            base.Initialize();
        }
        #endregion
        

        public static ILocation GetCurrentStage()
        {
            switch(gameStages)
            {

                case Stages.World:
                    return World;

                case Stages.Wilderness:
                    return Wilderness;

                case Stages.Town:
                    return Town;
                case Stages.Pass:
                    return Pass;


                case Stages.GreatLibrary:
                    return GreatLibrary;
                case Stages.WestBeach:
                    return WestBeach;
                case Stages.DobbinsOrchard:
                    return DobbinsOrchard;
                case Stages.ElixirShop:
                    return ElixirShop;

                default:
                    return null;
                   
            }
        }

        public static ILocation GetStageFromInt(int stageNumber)
        {
            switch (stageNumber)
            {
                case 1:
                    return World;
                case 2:
                    return Wilderness;
                case 3:
                    return Pass;
                case 5:
                    return Town;
                case 6:
                    return GreatLibrary;
                case 7:
                    return WestBeach;
                case 8:
                    return DobbinsOrchard;
                case 9:
                    return ElixirShop;
                default:
                    return Town;
            }

        }

        public static int GetCurrentStageInt()
        {
            switch (gameStages)
            {

                case Stages.World:
                    return 1;

                case Stages.Wilderness:
                    return 2;
                case Stages.Pass:
                    return 3;
                case Stages.Town:
                    return 5;

                case Stages.GreatLibrary:
                    return 6;
                case Stages.WestBeach:
                    return 7;
                case Stages.DobbinsOrchard:
                    return 8;
                case Stages.ElixirShop:
                    return 9;

                default:
                    return 0;

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
            
            DobbinRouteSchedule = Content.Load<RouteSchedule>("Route/DobbinRouteSchedule");
            ElixirRouteSchedule = Content.Load<RouteSchedule>("Route/ElixerRouteSchedule");
            AllSchedules = new List<RouteSchedule>() { DobbinRouteSchedule, ElixirRouteSchedule };
            AllCrops = Content.Load<CropHolder>("Crop/CropStuff");

            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue };
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
            Player = new Player("joe", new Vector2(1200, 600), AllTextures.PlayerParts, 24,10, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);

            //meaning hair of direction forward:
            Player.animations[0,0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 16, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000011f };
            //head
            Player.animations[0, 1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 64, 16, 48, 6, .1f, Game1.Player.position){ LayerDepth = .0000010f };
            //right arm
            Player.animations[0, 2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 112, 16, 48, 6, .1f, Game1.Player.position) { LayerDepth = .0000009f };
//right hand
            Player.animations[0, 3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 48, 160, 16, 48, 6, .1f, Game1.Player.position){ LayerDepth = .0000008f };
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
            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            // Player.animations[2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 160, 0, 16, 48, 6, .15f, Game1.Player.position);
            //Player.animations[3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 256, 0, 16, 48, 6, .15f, Game1.Player.position);
            //Player.animations[1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerParts, 352, 0, 16, 48, 6, .15f, Game1.Player.position);

            //UI

            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet);

            //ITEMS
            ItemVault = new ItemBank();
            //Item item = new Item { Name = "pie", ID = 0, id = "0", InvMaximum = 3, TextureString = Game1.AllTextures.pie.ToString(), IsPlaceable = false };
            //ItemVault.Items.Add(item.id, item);
            //ItemVault.Items.Save(@"Content/StartUpData/itemData.xml");
            // ItemVault.RawItems.Load(@"Content/StartUpData/itemData.xml");
            // ItemVault.LoadItems(GraphicsDevice, Content);

            AllItems = Content.Load<ItemHolder>("Item/ItemHolder");






            Player.UserInterface = new UserInterface(Player, graphics.GraphicsDevice, Content, cam) { graphics = graphics.GraphicsDevice };

            //Sea = new Sea(graphics.GraphicsDevice, myMouseManager, cam, userInterface, Player, AllTextures.Sea, AllTextures.MasterTileSet, 0);



            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, MainMenuContentManager, myMouseManager, Player.UserInterface);
            WestBeach = new TmxStageBase("WestBeach", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/elixirShop.tmx", 1);
            Wilderness = new Wilderness("Wilderness", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Wilderness.tmx", 1);
            World = new World("World", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Town.tmx", 1);
            Town = new Town("Town", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Town.tmx", 1);
            Pass = new TmxStageBase("Pass", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/Pass.tmx", 1);

            GreatLibrary = new TmxStageBase("Library", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/elixirShop.tmx", 1);


            //ElixirDialogue = Content.Load<DialogueSkeleton>("Dialogue/CharacterDialogue");
            DobbinsOrchard = new TmxStageBase("Dobbin's Orchard", graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/elixirShop.tmx", 1);


            LodgeInterior = new TmxStageBase("Lodge", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/elixirShop.tmx", 1);
            //homeStead = new HomeStead(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);
            ElixirShop = new TmxStageBase("ElixirShop", graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/elixirShop.tmx", 1);

            GlobalClock = new Clock();



            AllStages = new List<ILocation>() { Wilderness, Town, ElixirShop, Pass, World };



            Shop ToolShop = new Shop(graphics.GraphicsDevice, 1, "ToolShop", new ShopMenu("ToolShopInventory", graphics.GraphicsDevice, 25));
            ToolShop.ShopMenu.TryAddStock(3, 1);
            ToolShop.ShopMenu.TryAddStock(0, 1);
            ToolShop.ShopMenu.TryAddStock(1, 1);
            ToolShop.ShopMenu.TryAddStock(4, 1);
            ToolShop.ShopMenu.TryAddStock(147, 1);
            ToolShop.ShopMenu.TryAddStock(2, 1);
            ToolShop.ShopMenu.TryAddStock(122, 1);
            ToolShop.ShopMenu.TryAddStock(124, 1);
            ToolShop.ShopMenu.TryAddStock(125, 1);
            ToolShop.ShopMenu.TryAddStock(127, 1);
            ToolShop.ShopMenu.TryAddStock(128, 100);
            ToolShop.ShopMenu.TryAddStock(141, 1);
            ToolShop.ShopMenu.TryAddStock(143, 3);
            ToolShop.ShopMenu.TryAddStock(161, 5);
            ToolShop.ShopMenu.TryAddStock(145, 1);
            ToolShop.ShopMenu.TryAddStock(165, 1);
            ToolShop.ShopMenu.TryAddStock(167, 100); //bloodcorn seeds
            ToolShop.ShopMenu.TryAddStock(171, 1);
            ToolShop.ShopMenu.TryAddStock(181, 1);
            ToolShop.ShopMenu.TryAddStock(191, 1);

            Shop DobbinShop = new Shop(graphics.GraphicsDevice, 2, "DobbinShop", new ShopMenu("DobbinShopInventory", graphics.GraphicsDevice, 5));
            DobbinShop.ShopMenu.TryAddStock(128, 10);
            DobbinShop.ShopMenu.TryAddStock(167, 10);

            AllShops = new List<IShop>()
            {
                ToolShop,
                DobbinShop
            };




            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });

            Elixer = new Elixir("Elixer", new Vector2(840, 300), graphics.GraphicsDevice, Game1.AllTextures.ElixirSpriteSheet, AllSchedules[1]) { FrameToSet = 0 };
            Dobbin = new Dobbin("Dobbin", new Vector2(930, 300), graphics.GraphicsDevice, Game1.AllTextures.DobbinSpriteSheet, AllSchedules[0]) { FrameToSet = 0 } ;
            Snaw = new Character("Snaw", new Vector2(1280, 500), graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                3) { NPCAnimatedSprite = new Sprite[1] { new Sprite(graphics.GraphicsDevice, Game1.AllTextures.SnawSpriteSheet,
                0, 0, 72, 96, 3, .3f, new Vector2(1400, 600)) { IsAnimated = true,  } }, CurrentDirection = 0, SpeakerID = 3, FrameToSet = 3, IsBasicNPC = true};
            AllCharacters = new List<Character>()
            {
                Elixer,
                Dobbin,
                Snaw
            };
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
            if (!GetStageFromInt(stageToSwitchTo).IsLoaded)
            {
               
                GetStageFromInt(stageToSwitchTo).LoadContent(cam, AllSchedules);
            }


            if (portal != null)
            {
                Portal tempPortal = GetCurrentStage().AllPortals.Find(z => z.From == portal.To && z.To == portal.From);
                float x = tempPortal.PortalStart.X;
                float width = tempPortal.PortalStart.Width / 2;
                float y = tempPortal.PortalStart.Y;
                float safteyX = tempPortal.SafteyOffSetX;
                float safteyY = tempPortal.SafteyOffSetY;
                Player.position = new Vector2(x + width + safteyX, y + safteyY);
            }


        }

        
        public static void FullScreenToggle()
        {

            ToggleFullScreen = true;
        }
        //public 

        #region UPDATE
        protected override void Update(GameTime gameTime)
        {
            OldKeyBoardState = NewKeyBoardState;
            NewKeyBoardState = Keyboard.GetState();
            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager.Update();
            DebugWindow.Update(gameTime);

            //SOUND
            MediaPlayer.IsRepeating = true;

            //KEYBOARD

            if (ToggleFullScreen)
            {
                graphics.ToggleFullScreen();
                ToggleFullScreen = false;
            }



                //switch between stages for updating
                switch (gameStages)
                {
                    case Stages.MainMenu:

                        mainMenu.Update(gameTime, myMouseManager, this);
                        break;

                    case Stages.World:

                        World.Update(gameTime, myMouseManager, Player);
                        break;

                case Stages.Pass:
                    Pass.Update(gameTime, myMouseManager, Player);
                    break;

                    case Stages.Wilderness:


                        Wilderness.Update(gameTime, myMouseManager, Player);
                        break;

                    case Stages.Town:

                        Town.Update(gameTime, myMouseManager, Player);
                        break;

                    case Stages.GreatLibrary:
                        GreatLibrary.Update(gameTime, myMouseManager, Player);
                        break;
                    case Stages.WestBeach:
                        WestBeach.Update(gameTime, myMouseManager, Player);
                        break;
                case Stages.DobbinsOrchard:
                    DobbinsOrchard.Update(gameTime, myMouseManager, Player);
                    break;
                case Stages.ElixirShop:
                    ElixirShop.Update(gameTime, myMouseManager, Player);
                    break;

                        //case Stages.GreatLibrary:

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
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            

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
                   // spriteBatch.Begin();
                   // spriteBatch.Draw(AllTextures.LodgeInteriorTileSet, new Vector2(0, 0), Color.White);
                   // spriteBatch.End();
                    break;

                case Stages.Pass:
                    GraphicsDevice.Clear(Color.Black);
                    Pass.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;


                case Stages.Wilderness:
                    GraphicsDevice.Clear(Color.Black);
                    Wilderness.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.Town:
                    GraphicsDevice.Clear(Color.Black);
                    Town.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.GreatLibrary:
                    GraphicsDevice.Clear(Color.Black);
                    GreatLibrary.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.WestBeach:
                    GraphicsDevice.Clear(Color.Black);
                    WestBeach.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.DobbinsOrchard:
                    GraphicsDevice.Clear(Color.Black);
                    DobbinsOrchard.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;
                case Stages.ElixirShop:
                    GraphicsDevice.Clear(Color.Black);
                    ElixirShop.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                    //break;
            }

            Game1.DebugWindow.Draw(spriteBatch);

            base.Draw(gameTime);
        }
        #endregion
    }
    //IDEAS
    //Minigame where your characters is running and 'exploding' objects pop up around them and you have to dodge
    //
}