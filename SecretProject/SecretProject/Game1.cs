using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Stage;
using System;
using TiledSharp;

namespace SecretProject
{

    //TODO: need to be able to draw new tiles such as a tree to the screen and simulanteously create a hitbox around it. 
    // also need this for mouseover events. Possible use of a 2D array?

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

        //Camera
        Camera2D cam;

        

        //Initialize Starting Stage
        public Stages gameStages = Stages.MainMenu;

        //screen stuff
        public static int ScreenHeight;
        public static int ScreenWidth;



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

        }
        #endregion

        #region INITIALIZE
        protected override void Initialize()
        {
            this.IsMouseVisible = true;


            cam = new Camera2D();

            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            base.Initialize();
        }
        #endregion

        #region LOADCONTENT
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;

            //Load Stages
            _mainMenu = new MainMenu(this, graphics.GraphicsDevice, Content, mouse);
           _iliad = new Iliad(this, graphics.GraphicsDevice, Content, mouse);

            
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

            MediaPlayer.IsRepeating = true;
            MouseState myMouse = Mouse.GetState();
            KeyboardState oldKeyboardState = kState;
            kState = Keyboard.GetState();



            switch (gameStages)
            {
                case Stages.MainMenu:
                    _mainMenu.Update(gameTime, mouse);

                    break;

                case Stages.Iliad:
                    GraphicsDevice.Clear(Color.Black);
                    _iliad.Update(gameTime, mouse);
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
}
