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
        LodgeInteior = 1,
        Iliad = 2,
        Exit = 3,
        Sea = 4,
        RoyalDock = 5,
        GreatLibrary = 6,
        WestBeach = 7,
        DobbinsOrchard = 8
    }


    public class Game1 : Game
    {
        #region FIELDS

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager SeaContentManager;
        public ContentManager OrchardContentManager;

        //STAGES
        public static MainMenu mainMenu;
        public static NormalStage Iliad;
        public static NormalStage LodgeInterior;
        public static RoyalDock RoyalDock;
        public static Sea Sea;
        public static NormalStage GreatLibrary;
        public static NormalStage WestBeach;
        public static NormalStage DobbinsOrchard;
        public static List<NormalStage> AllStages;
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
        
        

        //TOOLS
        
        public static Utility Utility;
        public static float FrameRate = 0f;
        public static List<ActionTimer> AllActions;

        //CLOCK
        public static Clock GlobalClock;

        //ITEMS
        public static ItemBank ItemVault;

        //XMLDATA
        public DialogueSkeleton ElixirDialogue;
        public DialogueSkeleton DobbinDialogue;
        public DialogueHolder AllElixirDialogue;
        public DialogueHolder AllDobbinDialogue;

        //DIALOGUE
        public static DialogueLibrary DialogueLibrary;

        public static List<IShop> AllShops { get; set; }

        //RENDERTARGETS
        public RenderTarget2D MainTarget;
        public RenderTarget2D LightsTarget;
        

        public static PresentationParameters PresentationParameters;

        //event handlers
        
        

        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            HomeContentManager = new ContentManager(Content.ServiceProvider);
            SeaContentManager = new ContentManager(Content.ServiceProvider);
            OrchardContentManager = new ContentManager(Content.ServiceProvider);
            Content.RootDirectory = "Content";
            SeaContentManager.RootDirectory = "Content";
            HomeContentManager.RootDirectory = "Content";
            OrchardContentManager.RootDirectory = "Content";

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
        

        public static IStage GetCurrentStage()
        {
            switch(gameStages)
            {

                case Stages.LodgeInteior:
                    return LodgeInterior;

                case Stages.Iliad:
                    return Iliad;
                case Stages.Sea:
                    return Sea;

                case Stages.RoyalDock:
                    return RoyalDock;

                case Stages.GreatLibrary:
                    return GreatLibrary;
                case Stages.WestBeach:
                    return WestBeach;
                case Stages.DobbinsOrchard:
                    return DobbinsOrchard;

                default:
                    return null;
                   
            }
        }

        public static IStage GetStageFromInt(int stageNumber)
        {
            switch (stageNumber)
            {
                case 1:
                    return LodgeInterior;
                case 2:
                    return Iliad;
                case 4:
                    return Sea;
                case 5:
                    return RoyalDock;
                case 6:
                    return GreatLibrary;
                case 7:
                    return WestBeach;
                case 8:
                    return DobbinsOrchard;
                default:
                    return Iliad;
            }

        }

        public static int GetCurrentStageInt()
        {
            switch (gameStages)
            {

                case Stages.LodgeInteior:
                    return 1;

                case Stages.Iliad:
                    return 2;

                case Stages.Sea:
                    return 4;
                case Stages.RoyalDock:
                    return 5;

                case Stages.GreatLibrary:
                    return 6;
                case Stages.WestBeach:
                    return 7;
                case Stages.DobbinsOrchard:
                    return 8;

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
            AllElixirDialogue = Content.Load<DialogueHolder>("Dialogue/ElixirDialogue");
            AllDobbinDialogue = Content.Load<DialogueHolder>("Dialogue/DobbinDialogue");

            List<DialogueHolder> tempListHolder = new List<DialogueHolder>() { AllElixirDialogue, AllDobbinDialogue };
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
            Player = new Player("joe", new Vector2(250, 250), AllTextures.PlayerSpriteSheet, 26, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            // = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);
            Player.animations[0] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerSpriteSheet, 64, 0, 16, 48, 6, .15f, Game1.Player.position);
            Player.PlayerMovementAnimations = Player.animations[0];
            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            Player.animations[2] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerSpriteSheet, 160, 0, 16, 48, 6, .15f, Game1.Player.position);
            Player.animations[3] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerSpriteSheet, 256, 0, 16, 48, 6, .15f, Game1.Player.position);
            Player.animations[1] = new Sprite(GraphicsDevice, Game1.AllTextures.PlayerSpriteSheet, 352, 0, 16, 48, 6, .15f, Game1.Player.position);

            //UI

            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.UserInterfaceTileSet) ;

            //ITEMS
            ItemVault = new ItemBank();
            //Item item = new Item { Name = "pie", ID = 0, id = "0", InvMaximum = 3, TextureString = Game1.AllTextures.pie.ToString(), IsPlaceable = false };
            //ItemVault.Items.Add(item.id, item);
            //ItemVault.Items.Save(@"Content/StartUpData/itemData.xml");
            ItemVault.RawItems.Load(@"Content/StartUpData/itemData.xml");
            ItemVault.LoadItems(GraphicsDevice, Content);

            

           
            Player.UserInterface = new UserInterface(Player, graphics.GraphicsDevice, Content, cam) { graphics = graphics.GraphicsDevice };

            //Sea = new Sea(graphics.GraphicsDevice, myMouseManager, cam, userInterface, Player, AllTextures.Sea, AllTextures.MasterTileSet, 0);
            Sea = new Sea(graphics.GraphicsDevice,SeaContentManager, 0);
            

            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, Player.UserInterface);
            WestBeach = new NormalStage("WestBeach",graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/westBeach.tmx", 1);
            Iliad = new NormalStage("Wilderness",graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/worldMap.tmx", 1);
            
            RoyalDock = new RoyalDock("Dock",graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/royalDocks.tmx", 1);

            GreatLibrary = new NormalStage("Library",graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/greatLibrary.tmx", 1);


            ElixirDialogue = Content.Load<DialogueSkeleton>("Dialogue/CharacterDialogue");
            DobbinsOrchard = new NormalStage("Dobbin's Orchard",graphics.GraphicsDevice, HomeContentManager, 0, "Map/MasterSpriteSheet", "Content/Map/dobbinsOrchard.tmx", 1);


            LodgeInterior = new NormalStage("Lodge",graphics.GraphicsDevice, HomeContentManager, 0, "Map/InteriorSpriteSheet1", "Content/Map/lodgeInterior.tmx",1);
            //homeStead = new HomeStead(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);

            GlobalClock = new Clock();

            

            AllStages = new List<NormalStage>() { Iliad, LodgeInterior };

            

            Shop ToolShop = new Shop(graphics.GraphicsDevice, 1, "ToolShop", new ShopMenu("ToolShopInventory", graphics.GraphicsDevice));
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
            ToolShop.ShopMenu.TryAddStock(128, 10);
            ToolShop.ShopMenu.TryAddStock(141, 1);
            ToolShop.ShopMenu.TryAddStock(143, 3);
            ToolShop.ShopMenu.TryAddStock(161, 5);
            ToolShop.ShopMenu.TryAddStock(145, 1);
            ToolShop.ShopMenu.TryAddStock(165, 1);
            ToolShop.ShopMenu.TryAddStock(167, 1);
            ToolShop.ShopMenu.TryAddStock(171, 1);
            ToolShop.ShopMenu.TryAddStock(181, 1);
            ToolShop.ShopMenu.TryAddStock(191, 1);

            AllShops = new List<IShop>()
            {
                ToolShop
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
            GetStageFromInt(stageToSwitchTo).LoadContent(cam);
            
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

                    case Stages.LodgeInteior:

                        LodgeInterior.Update(gameTime, myMouseManager, Player);
                        break;

                    case Stages.Iliad:


                        Iliad.Update(gameTime, myMouseManager, Player);
                        break;

                    case Stages.RoyalDock:

                        RoyalDock.Update(gameTime, myMouseManager, Player);
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

                case Stages.Sea:
                        Sea.Update(gameTime, myMouseManager, Player);
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
                    GraphicsDevice.Clear(Color.Black);
                    mainMenu.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;

                case Stages.LodgeInteior:
                    GraphicsDevice.Clear(Color.Black);
                    LodgeInterior.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                   // spriteBatch.Begin();
                   // spriteBatch.Draw(AllTextures.LodgeInteriorTileSet, new Vector2(0, 0), Color.White);
                   // spriteBatch.End();
                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    Iliad.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
                    break;

                case Stages.RoyalDock:
                    GraphicsDevice.Clear(Color.Black);
                    RoyalDock.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
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



                case Stages.Sea:
                        Sea.Draw(graphics.GraphicsDevice, MainTarget, LightsTarget, gameTime, spriteBatch, myMouseManager, Player);
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