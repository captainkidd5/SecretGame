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

namespace SecretProject
{

    //TODO:
    //figure out what I want to do with component
    //drag and drop items
    //main menu
    //SAVE GAME
    

        

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
        private MainMenu _mainMenu;
        private Iliad _iliad;
        
        //Renderers


        //Input Fields
        MouseState mouse;
        KeyboardState kState;

        MouseManager myMouseManager;

        public static bool isMyMouseVisible = true;
        //public bool IsMyMouseVisible { get { return isMyMouseVisible; } set { isMyMouseVisible = value; } }

        //Camera
        Camera2D cam;

        

        //Initialize Starting Stage
        public Stages gameStages = Stages.MainMenu;

        //screen stuff
        public static int ScreenHeight;
        public static int ScreenWidth;

        //game freeze

        public static bool freeze;

        //UserInterface
        public UserInterface userInterface;



        #endregion

        #region CONSTRUCTOR
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set window dimensions
             graphics.PreferredBackBufferWidth = 1280;
             graphics.PreferredBackBufferHeight = 720;

            //graphics.IsFullScreen = true;

            freeze = false;

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

            userInterface = new UserInterface(this, graphics.GraphicsDevice, Content, myMouseManager);

            base.Initialize();
        }
        #endregion

        #region LOADCONTENT
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Stages
            _mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, myMouseManager, userInterface);
           _iliad = new Iliad(this, graphics.GraphicsDevice, Content, myMouseManager, cam, userInterface);

            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            this.IsMouseVisible = isMyMouseVisible;

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
                    if(Game1.freeze == false)
                    {
                        _mainMenu.Update(gameTime);

                    }
                    

                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    if (Game1.freeze == false)
                    {
                        _iliad.Update(gameTime);
                    }
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
                    _mainMenu.Draw(gameTime, spriteBatch);
                    break;

                case Stages.Iliad:
                    _iliad.Draw(gameTime, spriteBatch);
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
