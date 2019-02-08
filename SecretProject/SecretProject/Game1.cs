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



        //STAGES
        public static MainMenu mainMenu;
        public static Iliad iliad;
        public static int CurrentStage;
        public static bool freeze = false;

        //SOUND
        public static SoundBoard SoundManager;

        //INPUT
        private MouseState mouse;
        private KeyboardState kState;
        public static MouseManager myMouseManager;
        public static bool isMyMouseVisible = true;

        //Camera
        public static Camera2D cam;
        public static bool ToggleFullScreen = false;

        //Initialize Starting Stage
        public Stages gameStages = Stages.MainMenu;

        //screen stuff
        public static int ScreenHeight;
        public static int ScreenWidth;

        //UI
        public static UserInterface userInterface;

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
            myMouseManager = new MouseManager(mouse, cam, graphics.GraphicsDevice);

            //SCREEN
            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            base.Initialize();
        }
        #endregion

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

            //SOUND
            SoundManager = new SoundBoard(this, Content);
            

            //ItemAtlas = Content.Load<Texture2D>("Item/ItemAnimationSheet");

            //PLAYERS
            Player = new Player("joe", new Vector2(900, 250), MainCharacterTexture, 28, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };
            Player.Anim = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 6, 25);
            Player.animations[0] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 0, 1, 6);
            //gotta fix up animation to sit properly on correct frame, it currently has one extra for smooth movement
            Player.animations[1] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 18, 1, 25);
            Player.animations[2] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 6, 1, 12);
            Player.animations[3] = new AnimatedSprite(GraphicsDevice, MainCharacterTexture, 1, 25, 25, 12, 1, 18);

            //UI
            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content);

            //STAGES
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
            //MOUSE
            this.IsMouseVisible = isMyMouseVisible;
            myMouseManager.Update();

            //SOUND
            MediaPlayer.IsRepeating = true;

            //KEYBOARD
            KeyboardState oldKeyboardState = kState;

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

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    iliad.Update(gameTime, myMouseManager, this);
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

                case Stages.Iliad:
                    iliad.Draw(graphics.GraphicsDevice, gameTime, spriteBatch, myMouseManager);
                    break;
            }

            base.Draw(gameTime);
        }
        #endregion
    }
    //IDEAS
    //Minigame where your characters is running and 'exploding' objects pop up around them and you have to dodge
    //
}