using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.Stage;
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


//TODO: Make enum for player actions, items, world items etc so that strings aren't used
// fix player clipping around when performing action
// diagonal movement
// inside of house
// change screen edge stuff from hardcode
//make screen width/height stuff better
//placeable objects needs two new layers so stuff underneat is preserved.
//Tile random generation
//Work on NPC collision detection

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
        Right
    }

    public enum Stages
    {
        MainMenu = 0,
        LodgeInteior = 1,
        Iliad = 2,
        Exit = 3,
    }


    public class Game1 : Game
    {
        #region FIELDS

        //STAGES
        public static MainMenu mainMenu;
        public static Stage Iliad;
        public static Stage LodgeInterior;
        public static int CurrentStage;
        public static bool freeze = false;

        //SOUND
        public static SoundBoard SoundManager;

        //INPUT

        public static MouseManager myMouseManager;
        public static bool isMyMouseVisible = true;

        //Camera
        public static Camera2D cam;
        public static bool ToggleFullScreen = false;

        //Initialize Starting Stage
        public static Stages gameStages = Stages.Iliad;

        //screen stuff
        public static int ScreenHeight;
        public static int ScreenWidth;

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
        public static float FrameRate = 0f;
        public static List<ActionTimer> AllActions;

        //CLOCK
        public static Clock GlobalClock;

     //   public Item testItem;
        


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

            //SCREEN
            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            AllActions = new List<ActionTimer>();

            base.Initialize();
        }
        #endregion

        public static Stage GetCurrentStage()
        {
            switch(gameStages)
            {



                case Stages.LodgeInteior:
                    return LodgeInterior;

                case Stages.Iliad:
                    return Iliad;

                default:
                    return null;
                   
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

                default:
                    return 0;

            }

        }

        #region LOADCONTENT
        protected override void LoadContent()
        {
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
            Player = new Player("joe", new Vector2(600, 600), MainCharacterTexture, 28, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            Player.PlayerMovementAnimations = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);
            Player.animations[0] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 0, 1, 6);
            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            Player.animations[1] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 18, 1, 25);
            Player.animations[2] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 6, 1, 12);
            Player.animations[3] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 12, 1, 18);

            //UI
            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content, cam) { graphics = graphics.GraphicsDevice };
            DebugWindow = new DebugWindow(AllTextures.MenuText, new Vector2(25, 400), "Debug Window \n \n FrameRate: \n \n PlayerLocation: \n \n PlayerWorldPosition: ", AllTextures.TransparentTextBox) ;

            //STAGES
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, userInterface);
            Iliad = new Stage(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player, AllTextures.Iliad, AllTextures.MasterTileSet, 0);
            Iliad.BuildingsTiles.LoadInitialTileObjects(Iliad);
            LodgeInterior = new Stage(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player, AllTextures.LodgeInterior, AllTextures.LodgeInteriorTileSet, 0);
            LodgeInterior.BuildingsTiles.LoadInitialTileObjects(LodgeInterior);
            //homeStead = new HomeStead(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);

            GlobalClock = new Clock();


        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion


        //public 

        #region UPDATE
        protected override void Update(GameTime gameTime)
        {
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
                    Iliad.Update(gameTime, myMouseManager, this);
                    break;
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