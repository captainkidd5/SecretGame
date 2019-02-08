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

using System.Runtime.Serialization;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using SecretProject.Class.SoundStuff;
using SecretProject.Class.TextureStuff;

namespace SecretProject
{
    //TODO:
    //Mini draw tiles in Toolbar


    public enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public enum Stages
    {
        MainMenu = 0,
        //WorldMap = 1,
        Iliad = 2,
        Exit = 3,
        //HomeStead = 4,

    }


    public class Game1 : Game
    {
        #region FIELDS

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        public static MainMenu mainMenu;
        public static Iliad iliad;

        public static SoundBoard SoundManager;

        public static int CurrentStage;




        //Input Fields
        private MouseState mouse;
        private KeyboardState kState;

        public static MouseManager myMouseManager;

        public static bool isMyMouseVisible = true;
        //public bool IsMyMouseVisible { get { return isMyMouseVisible; } set { isMyMouseVisible = value; } }

        //Camera
        public static Camera2D cam;



        //Initialize Starting Stage
        public Stages gameStages = Stages.MainMenu;

        //screen stuff
        public static int ScreenHeight;
        public static int ScreenWidth;

        //game freeze

        public static bool freeze = false;

        //UserInterface
        public static UserInterface userInterface;

        //player
        public Texture2D JoeSprite { get; set; }

        private Texture2D joeDown;
        private Texture2D joeUp;
        private Texture2D joeRight;
        private Texture2D joeLeft;
        public static Player Player { get; set; }

        public static bool ToggleFullScreen = false;

        public Texture2D MainCharacterTexture { get; set; }

        public static Texture2D ItemAtlas;

        public static Random RGenerator = new Random();

        public static TextureBook AllTextures;


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
            //graphics.IsFullScreen = true;

            //  freeze = false;

            //RGenerator = Utility.RGenerator;


        }
        #endregion

        #region INITIALIZE
        protected override void Initialize()
        {

            //initialize mouse
            this.IsMouseVisible = isMyMouseVisible;
            cam = new Camera2D(GraphicsDevice.Viewport);
            myMouseManager = new MouseManager(mouse, cam, graphics.GraphicsDevice);

            //camera


            //screen dimensions
            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            // graphics.ToggleFullScreen();


            base.Initialize();
        }
        #endregion

        #region LOADCONTENT
        protected override void LoadContent()
        {
            AllTextures = new TextureBook(Content, spriteBatch);
            SoundManager = new SoundBoard(this, Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");

            JoeSprite = AllTextures.JoeSprite;

            MainCharacterTexture = AllTextures.MainCharacterSpriteStrip;

            joeDown = AllTextures.joeDown;
            joeUp = AllTextures.joeUp;
            joeRight = AllTextures.joeRight;
            joeLeft = AllTextures.joeLeft;

            Player = new Player("joe", new Vector2(900, 250), MainCharacterTexture, 28, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };

            Player.Anim = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);

            //joe animation 
            Player.animations[0] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 0, 1, 6);

            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            Player.animations[1] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 18, 1, 25);
            Player.animations[2] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 6, 1, 12);
            Player.animations[3] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 12, 1, 18);

            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content);

            //Load Stages
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, userInterface);
            iliad = new Iliad(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);
            iliad.BuildingsTiles.LoadInitialTileObjects();
            //homeStead = new HomeStead(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);




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
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //   Exit();
            this.IsMouseVisible = isMyMouseVisible;

            myMouseManager.Update();

            //music
            MediaPlayer.IsRepeating = true;

            //input
            // MouseState myMouse = Mouse.GetState();
            KeyboardState oldKeyboardState = kState;
            //kState = Keyboard.GetState();
            if (ToggleFullScreen)
            {
                graphics.ToggleFullScreen();
                ToggleFullScreen = false;
            }



            //switch between stages for updating
            switch (gameStages)
            {
                case Stages.MainMenu:
                    // if(Game1.freeze == false)
                    // {

                    mainMenu.Update(gameTime, myMouseManager, this);

                    //}


                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    //if (Game1.freeze == false)
                    //{
                    iliad.Update(gameTime, myMouseManager, this);
                    //}
                    break;

                    // case Stages.HomeStead:
                    //  GraphicsDevice.Clear(Color.Black);
                    //if (Game1.freeze == false)
                    //{
                    // homeStead.Update(gameTime);
                    //}
                    // break;


            }



            base.Update(gameTime);
        }
        #endregion

        #region DRAW
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            //switch between stages for drawing
            switch (gameStages)
            {
                case Stages.MainMenu:
                    GraphicsDevice.Clear(Color.Black);
                    mainMenu.Draw(gameTime, spriteBatch);

                    break;

                case Stages.Iliad:
                    iliad.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);

                    break;

                    // case Stages.HomeStead:
                    //  homeStead.Draw(gameTime, spriteBatch);

                    //  break;
            }


            base.Draw(gameTime);
        }
        #endregion


    }
    //IDEAS
    //Minigame where your characters is running and 'exploding' objects pop up around them and you have to dodge
    //
}