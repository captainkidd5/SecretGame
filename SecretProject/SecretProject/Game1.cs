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

    }


    public class Game1 : Game
    {
        #region FIELDS

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Declare Stages
        public static MainMenu mainMenu;
        public static Iliad iliad;

        //Renderers

        //sound
        public static SoundBoard SoundManager;

        public static int CurrentStage;

        


        //Input Fields
        MouseState mouse;
        KeyboardState kState;

        MouseManager myMouseManager;

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

        public static bool freeze;

        //UserInterface
        public UserInterface userInterface;

        //player
        public Texture2D JoeSprite { get; set; }

        Texture2D joeDown;
        Texture2D joeUp;
        Texture2D joeRight;
        Texture2D joeLeft;
        public static Player Player { get; set; }

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

            freeze = false;

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

            joeDown = AllTextures.joeDown;
            joeUp = AllTextures.joeUp;
            joeRight = AllTextures.joeRight;
            joeLeft = AllTextures.joeLeft;

            Player = new Player("joe", new Vector2(900, 250), JoeSprite, 4, Content, graphics.GraphicsDevice, myMouseManager) { Activate = true };

            Player.Anim = new AnimatedSprite(GraphicsDevice, joeDown, 1, 4, 4);

            //joe animation 
            Player.animations[0] = new AnimatedSprite(GraphicsDevice, joeDown, 1, 4, 4);
            Player.animations[1] = new AnimatedSprite(GraphicsDevice, joeUp, 1, 4, 4);
            Player.animations[2] = new AnimatedSprite(GraphicsDevice, joeLeft, 1, 4, 4);
            Player.animations[3] = new AnimatedSprite(GraphicsDevice, joeRight, 1, 4, 4);

            //Load Stages
            mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, userInterface);
           iliad = new Iliad(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface, Player);

            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content, myMouseManager);


        }
        #endregion

        #region UNLOADCONTENT
        protected override void UnloadContent()
        {

        }
        #endregion

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


            //switch between stages for updating
            switch (gameStages)
            {
                case Stages.MainMenu:
                   // if(Game1.freeze == false)
                   // {
                        mainMenu.Update(gameTime);

                    //}
                    

                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    //if (Game1.freeze == false)
                    //{
                        iliad.Update(gameTime);
                    //}
                    break;

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
                    iliad.Draw(gameTime, spriteBatch);
                    
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
