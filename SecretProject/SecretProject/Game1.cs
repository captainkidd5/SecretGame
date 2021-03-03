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
using SecretProject.Class.UI.ShopStuff;
using SecretProject.Class.NetworkStuff;
using System;
using TiledSharp;
using SecretProject.Class.StageFolder.DungeonStuff;
using System.Linq;
using XMLData.ItemStuff.MessageStuff;
using SecretProject.Class.StageFolder.DungeonStuff.Desert;
using Penumbra;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;
using VelcroPhysics.Extensions;
using VelcroPhysics.Extensions.DebugView;
using VelcroPhysics.Factories;
using SecretProject.Class.LightStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.MovieStuff;
using SecretProject.Class.Misc;
using VelcroPhysics.DebugViews.MonoGame;




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



    

    public enum WeatherType
    {
        None = 0,
        Sunny = 1,
        Rainy = 2
    }


    public class Game1 : Game
    {
        #region FIELDS
        public static Flags Flags;

        public static int NPCSpawnCountLimit = 50;

        public static StageManager StageHandler { get; set; }

        public static bool IsFirstTimeStartup;
        public static bool AreGeneratableTilesLoaded;

        public static bool EnableMultiplayer = false;

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager MainMenuContentManager;

        //STAGES
        public static MainMenu mainMenu;
        
        
        public static int PreviousStage = 0;
        public static bool freeze = false;

        //SOUND
        public static SoundBoard SoundManager;

        public static bool IsFadingOut;
        public static bool IsFadingIn;

        //INPUT

        public static MouseManager MouseManager;
        public static bool isMyMouseVisible = true;

        public static KeyboardManager KeyboardManager;


        //Camera
        public static Camera2D cam;
        public static bool ToggleFullScreen = false;

        //Initialize Starting Stage


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

        public static DebugView VelcroDebugger;

        //CLOCK
        public static Clock GlobalClock;

        //ITEMS
        public static ItemBank ItemVault;

        //World Quests
        public static WorldQuestHolder WorldQuestHolder;


        public PlayerManager PlayerManager { get; set; }
        //XMLDATA

        public static ItemHolder AllItems{ get; private set; }
        public static LootBank LootBank{ get; private set; }
        public static CropHolder AllCrops{ get; private set; }


        public static CookingGuide AllCookingRecipes{ get; private set; }

        public static SpawnHolder OverWorldSpawnHolder { get; private set; }

        public static MessageHolder MessageHolder { get; private set; }

        //SHOPS AND MENUS
        public static List<IShop> AllShops { get; set; }


        //LIGHTING
        public static PenumbraComponent Penumbra;


        public static PresentationParameters PresentationParameters;

        //event handlers
        //Events
        public static List<IEvent> AllEvents;
        public static IEvent CurrentEvent;

        

        public CharacterManager CharacterManager { get; set; }

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

        //NETWORK
        private NetworkConnection netWorkConnection;

        //VELCRO PHYSICS

        public static World VelcroWorld;

        //Movies
        protected static MoviePlayer MoviePlayer;

        //Props
        public static Train Train;

        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Flags = new Flags();
            
            HomeContentManager = new ContentManager(this.Content.ServiceProvider);
            MainMenuContentManager = new ContentManager(this.Content.ServiceProvider);
            this.Content.RootDirectory = "Content";
            HomeContentManager.RootDirectory = "Content";
            MainMenuContentManager.RootDirectory = "Content";
            PlayerManager = new PlayerManager( GraphicsDevice, HomeContentManager);
            CharacterManager = new CharacterManager(GraphicsDevice, HomeContentManager);
            StageHandler = new StageManager(this, GraphicsDevice,HomeContentManager, PlayerManager, CharacterManager);
            //set window dimensions


            this.IsFixedTimeStep = false;
            netWorkConnection = new NetworkConnection();

            VelcroWorld = new World(new Vector2(0, 9.8f));
            //CreatePinBoard();

        }
        #endregion

        #region INITIALIZE
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            ScreenRectangle.Width = graphics.PreferredBackBufferWidth;
            ScreenRectangle.Height = graphics.PreferredBackBufferHeight;
            //seed parameter
            Utility = new Utility(this.GraphicsDevice, 1);
            //CAMERA
            cam = new Camera2D(this.GraphicsDevice.Viewport);
            //MOUSE

            this.IsMouseVisible = isMyMouseVisible;
            MouseManager = new MouseManager(cam, graphics.GraphicsDevice);



            //NETWORK
            if (EnableMultiplayer)
            {
                netWorkConnection.Start();
            }



            //SCREEN


            AllActions = new List<ActionTimer>();
            Penumbra = new PenumbraComponent(this,Content)
            {
                AmbientColor = Color.White,
            };
            Services.AddService(Penumbra);
            Components.Add(Penumbra);
            Penumbra.Enabled = true;
            base.Initialize();
        }
        #endregion

        /// <summary>
        /// primarily used by portals.
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public static Stage GetStageFromEnum(StagesEnum stage)
        {
            return StageHandler.GetStageFromEnum(stage);
        }

        private void LoadPhysics()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(16f); //one world unit is 16 pixels.
        }

        #region LOADCONTENT
        protected override void LoadContent()
        {
            IsFirstTimeStartup = true;
            SaveLoadManager = new SaveLoadManager();
            PresentationParameters = this.GraphicsDevice.PresentationParameters;
            
            //ORDER MATTERS!!!



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


            StageHandler.Load();


            DebugWindow = new DebugWindow(this.GraphicsDevice, AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet, this.GraphicsDevice);

            LoadItems();
            WorldQuestHolder = new WorldQuestHolder();


            Procedural = new Procedural();
            PlayerManager = new PlayerManager(graphics.GraphicsDevice, Content);
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
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, MainMenuContentManager, Content.ServiceProvider);
            // Game1.SaveLoadManager.Load(graphics.GraphicsDevice, Game1.SaveLoadManager.MainMenuData, false);
            PortalGraph = new Graph(Enum.GetNames(typeof(Stages)).Length);


            LoadStages();
            LoadShops();

            LineTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            LineTexture.SetData<Color>(new Color[] { Color.White });


            AllEvents = new List<IEvent>()
            {

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


            Penumbra.Initialize();

            LoadPhysics();

            MoviePlayer = new MoviePlayer(Content.ServiceProvider);
            Train = new Train();

        }


        private void LoadItems()
        {
            ItemVault = new ItemBank();
            AllItems = this.Content.Load<ItemHolder>("Item/ItemHolder");
            AllItems.AllItems.Sort((x, y) => x.Name.CompareTo(y.Name));// = AllItems.AllItems.OrderBy(x => x.Name).ToList(); 
            ItemVault.Load();
            LootBank = new LootBank(Content.Load<LootHolder>("Item/Loot/LootHolder"));
            MessageHolder = Content.Load<MessageHolder>("Item/Messages/Messages");
        }


        /// <summary>
        /// Map must be physically placed in bin/content/bin/map
        /// </summary>
        private void LoadStages()
        {
            




            
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

            KayaShop.ShopMenu.TryAddStock(320, 5);


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


       

        
        public static void SwitchStage(Stage stageToSwitchTo, Portal portal = null)
        {
            Player.UserInterface.LoadingScreen.BeginBlackTransition(.05f, 2f);

            IsFadingOut = true;

            VelcroWorld.Clear();


            //VelcroWorld.ProcessChanges();

            StageHandler.CurrentStage.UnloadContent();
            UnloadPenumbraEntities();

            if (CurrentStage == PlayerHouse)
                stageToSwitchTo = TurnDial.CurrentLocation;

            Stage newLocation = stageToSwitchTo;


            Game1.Player.LockBounds = true;

            newLocation.TryLoadExistingStage();
            if (CurrentStage.DebuggableShapes != null)
            {
                CurrentStage.DebuggableShapes.Clear();
            }
            if (portal != null)
            {
                List<Portal> portalTest = stageToSwitchTo.AllPortals;
                Portal tempPortal = stageToSwitchTo.AllPortals.Find(z => z.From == portal.To && z.To == portal.From);
                if (tempPortal != null)
                {
                    float x = tempPortal.PortalStart.X;
                    float width = tempPortal.PortalStart.Width / 2;
                    float y = tempPortal.PortalStart.Y;
                    float safteyX = tempPortal.SafteyOffSetX;
                    float safteyY = tempPortal.SafteyOffSetY;
                    Player.CreateBody();
                    Player.SetPosition(new Vector2(x + width + safteyX, y + safteyY));
                    cam.pos = Player.Position;

                }
                else
                {
                    Player.CreateBody();
                }
            }
            else
            {
                Player.CreateBody();
            }

            Player.Wardrobe.UpdateForCreationMenu();



            MouseManager.AttachMouseBody();
            Train.SwitchStage(CurrentStage.StageIdentifier, newLocation.StageIdentifier);
            CurrentStage = newLocation;
            CurrentStage.DebuggableShapes.Add(new RectangleDebugger(Player.LargeProximitySensor, CurrentStage.DebuggableShapes));
            CurrentStage.AllTiles.UpdateCropTile();
           
            Player.LoadPenumbra(CurrentStage);

        
            VelcroWorld.ProcessChanges();
            Game1.GlobalClock.ProcessNewDayChanges();

        }

        public static void PlayMovie(MovieName movieName)
        {
            MoviePlayer.ChangeMovie(movieName);
            MoviePlayer.IsActive = true;
            
        }
        public static void FullScreenToggle()
        {

            ToggleFullScreen = true;
        }

        #region UPDATE
        protected override void Update(GameTime gameTime)
        {
            VelcroWorld.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * .001f);

            KeyboardManager.Update();
            // IsEventActive = false;

            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            MouseManager.Update(gameTime);
            DebugWindow.Update(gameTime);


            //SOUND
            MediaPlayer.IsRepeating = true;
            if (Game1.Flags.EnableMusic)
            {
                SoundManager.PlaySong();
                if (IsFadingOut)
                {
                    IsFadingOut = SoundManager.FadeSongOut(gameTime);

                }
                else if (IsFadingIn)
                {
                    IsFadingIn = SoundManager.FadeSongIn(gameTime);
                }
                // SoundManager.CurrentSongInstance.Volume = SoundManager.GameVolume;
            }

            //KEYBOARD

            if (ToggleFullScreen)
            {
                graphics.ToggleFullScreen();
                ToggleFullScreen = false;
            }

            if(MoviePlayer.IsActive)
            {
                MoviePlayer.Update(gameTime);
            }
            else
            {
                Game1.Player.UserInterface.CinematicMode = false;
                StageHandler.Update(gameTime);
            }
                


            if (!MouseManager.ToggleGeneralInteraction)
            {
                this.IsMouseVisible = true;
            }

            base.Update(gameTime);
        }
        #endregion

        #region DRAW
        public static bool DrawPenumbra;
        protected override void Draw(GameTime gameTime)
        {



            this.GraphicsDevice.Clear(Color.Black);


            if(MoviePlayer.IsActive)
            {
                MoviePlayer.Draw(spriteBatch);
            }
            else
            {
                if (Globals.EnableVelcroDraw)
                    DebugVelcro();
                StageHandler.Draw(spriteBatch);

            }
           

            Game1.DebugWindow.Draw(spriteBatch);
            DrawPenumbra = false;
            base.Draw(gameTime);
        }
        #endregion

        private void InitializeVelcro()
        {
            VelcroWorld = new World(new Vector2(0, 9.8f));
            //TODO: Re-enable DEBUG VIEWS
            if (VelcroDebugger == null)
            {
                VelcroDebugger = new DebugView(VelcroWorld);

                VelcroDebugger.AppendFlags(DebugViewFlags.Shape);
                VelcroDebugger.DefaultShapeColor = Color.LightPink;
                VelcroDebugger.SleepingShapeColor = Color.MediumPurple;
            }
        }

        /// <summary>
        /// Turn on to draw velcro physics shapes.
        /// </summary>
        private void DebugVelcro()
        {
            Matrix proj = Matrix.CreateOrthographicOffCenter(new Rectangle(-GraphicsDevice.Viewport.Width / 2 * (int)cam.Zoom,
                -GraphicsDevice.Viewport.Height / 2 * (int)cam.Zoom, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 1f, -1f);
            Matrix view = cam.GetViewMatrix(Vector2.One);
            VelcroDebugger.RenderDebugData(proj, view);
        }
    }

}