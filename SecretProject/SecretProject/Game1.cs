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
        RoyalDock = 5
    }


    public class Game1 : Game
    {
        #region FIELDS

        //ContentManagers
        public ContentManager HomeContentManager;
        public ContentManager SeaContentManager;

        //STAGES
        public static MainMenu mainMenu;
        public static Home Iliad;
        public static Home LodgeInterior;
        public static RoyalDock RoyalDock;
        public static Sea Sea;
        public static List<Home> AllStages;
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
        public static Stages gameStages = Stages.RoyalDock;

        //screen stuff
        public static Rectangle ScreenRectangle = new Rectangle(0, 0, 0, 0);
        public static int ScreenHeight { get { return ScreenRectangle.Height; } }
        public static int ScreenWidth { get { return ScreenRectangle.Width; } }
        

        //UI
        public static UserInterface userInterface;

        public static DebugWindow DebugWindow;

        //TEXTURES
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D JoeSprite { get; set; }
        private Texture2D joeDown;
        private Texture2D joeUp;
        private Texture2D joeRight;
        private Texture2D joeLeft;
        public static Player Player { get; set; }
        public Texture2D MainCharacterTexture { get; set; }
        public static Texture2D ItemAtlas;
        public static TextureBook AllTextures;
        
        

        //TOOLS
        public static Random RGenerator = new Random();
        public static Utility Utility;
        public static float FrameRate = 0f;
        public static List<ActionTimer> AllActions;

        //CLOCK
        public static Clock GlobalClock;

        //ITEMS
        public static ItemBank ItemVault;
        


        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set window dimensions
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            HomeContentManager = new ContentManager(Content.ServiceProvider);
            SeaContentManager = new ContentManager(Content.ServiceProvider);

            AllActions = new List<ActionTimer>();

            base.Initialize();
        }
        #endregion
        

        //use this sparingly for now. Seems to cause a buildup of memory.
        public static void ReloadHome(GraphicsDevice graphics, ContentManager content)
        {
            Home newHome = new Home(graphics, content, myMouseManager, cam, userInterface, Player, AllTextures.Iliad, AllTextures.MasterTileSet, 0);
            Iliad = newHome;
            Iliad.AllTiles.LoadInitialTileObjects();
        }

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

                default:
                    return null;
                   
            }
        }

        public static IStage GetPreviousStage(int stageNumber)
        {
            switch (stageNumber)
            {
                case 1:
                    return LodgeInterior;
                case 2:
                    return Iliad;
                case 5:
                    return RoyalDock;
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

                default:
                    return 0;

            }

        }

        #region LOADCONTENT
        protected override void LoadContent()
        {
            //ORDER MATTERS!!!

            //TEXTURES
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AllTextures = new TextureBook(Content, spriteBatch);
            MainCharacterTexture = AllTextures.MainCharacterSpriteStrip;
            JoeSprite = AllTextures.JoeSprite;
            joeDown = AllTextures.joeDown;
            joeUp = AllTextures.joeUp;
            joeRight = AllTextures.joeRight;
            joeLeft = AllTextures.joeLeft;

          //  testItem = Content.Load&lt;XMLDataLib.Item&gt;("Level1");

            //SOUND
            SoundManager = new SoundBoard(this, Content);
            

            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");

            //PLAYERS
            Player = new Player("joe", new Vector2(400, 700), MainCharacterTexture, 26, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            Player.PlayerMovementAnimations = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);
            Player.animations[0] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 0, 1, 6);
            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            Player.animations[1] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 18, 1, 25);
            Player.animations[2] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 6, 1, 12);
            Player.animations[3] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 12, 1, 18);

            //UI
            
            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.TransparentTextBox) ;

            //ITEMS
            ItemVault = new ItemBank();
            //Item item = new Item { Name = "pie", ID = 0, id = "0", InvMaximum = 3, TextureString = Game1.AllTextures.pie.ToString(), IsPlaceable = false };
            //ItemVault.Items.Add(item.id, item);
            //ItemVault.Items.Save(@"Content/StartUpData/itemData.xml");
            ItemVault.RawItems.Load(@"Content/StartUpData/itemData.xml");
            ItemVault.LoadItems(GraphicsDevice, Content);

            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content, cam) { graphics = graphics.GraphicsDevice };

            //Sea = new Sea(graphics.GraphicsDevice, myMouseManager, cam, userInterface, Player, AllTextures.Sea, AllTextures.MasterTileSet, 0);
            Sea = new Sea();
            Sea.LoadContent(SeaContentManager, graphics.GraphicsDevice, cam, 0);

            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, userInterface);
            Iliad = new Home();
            
            RoyalDock = new RoyalDock();
            RoyalDock.LoadContent(HomeContentManager, graphics.GraphicsDevice, cam, 0);

            
            LodgeInterior = new Home();
            //homeStead = new HomeStead(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);

            GlobalClock = new Clock();

            

            AllStages = new List<Home>() { Iliad, LodgeInterior };

            

        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion

        protected void UnloadStageObects(IStage stage)
        {
            List<ObjectBody> newEmptyObjectList = new List<ObjectBody>();
            stage.AllObjects = newEmptyObjectList;
        }

        public void FullScreenToggle()
        {
            graphics.ToggleFullScreen();
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
                    GraphicsDevice.Clear(Color.Black);
                    LodgeInterior.Update(gameTime, myMouseManager, this);
                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    if(!Iliad.TilesLoaded)
                    {
                        Iliad.AllTiles.LoadInitialTileObjects();
                    }
                    Iliad.TilesLoaded = true;
                    if (PreviousStage != 0)
                    {
                        UnloadStageObects(GetPreviousStage(PreviousStage));
                    }
                    Iliad.Update(gameTime, myMouseManager, this);
                    break;

                case Stages.RoyalDock:
                    GraphicsDevice.Clear(Color.Black);
                    if(!RoyalDock.TilesLoaded)
                    {
                        RoyalDock.AllTiles.LoadInitialTileObjects();
                    }
                    RoyalDock.TilesLoaded = true;
                    
                    if (PreviousStage != 0)
                    {
                        UnloadStageObects(GetPreviousStage(PreviousStage));
                    }
                    RoyalDock.Update(gameTime, myMouseManager, this);
                    break;

                    //case Stages.Sea:
                    //    GraphicsDevice.Clear(Color.Black);
                    //    Sea.Update(gameTime, myMouseManager, this);
                    //    break;
            }

            base.Update(gameTime);
        }
        #endregion

        #region DRAW
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            switch (gameStages)
            {
                case Stages.MainMenu:
                    GraphicsDevice.Clear(Color.Black);
                    mainMenu.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;

                case Stages.LodgeInteior:
                    LodgeInterior.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                   // spriteBatch.Begin();
                   // spriteBatch.Draw(AllTextures.LodgeInteriorTileSet, new Vector2(0, 0), Color.White);
                   // spriteBatch.End();
                    break;

                case Stages.Iliad:
                    Iliad.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;

                case Stages.RoyalDock:
                    RoyalDock.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;



                    //case Stages.Sea:
                    //    Sea.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);


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