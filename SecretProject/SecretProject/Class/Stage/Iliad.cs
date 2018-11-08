using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

using TiledSharp;

using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using Object = SecretProject.Class.ObjectFolder.Object;

namespace SecretProject.Class.Stage
{
    class Iliad : Component
    {
        #region FIELDS

        private bool showBorders = false;



        //--------------------------------------
        //Declare Map
        TiledMapRenderer mapRenderer;

        
        

        TiledMapLayer backGround;
        TiledMapLayer buildings;
        TiledMapLayer midGround;
        TiledMapLayer foreGround;
        TiledMapLayer alwaysFront;

        Texture2D joeSprite;
        Texture2D raftDown;

        public TiledMap IliadMap;

        public Vector2 TileSize = new Vector2(32, 32);

        //--------------------------------------
        //Instantiate playables
        Player player;
        Player basicRaft;

        //--------------------------------------
        //Declare input stuff
        Camera2D cam;
        KeyboardState kState;
        ToolBar toolBar;

        //--------------------------------------
        //Declare Textures
        Texture2D woodenPost;
        Texture2D basicHouse;
        Texture2D greatPine;

        //PlayerInventory playerOneInv;



        //AnimatedSprite TestSprite;

        //--------------------------------------
        //Declare Lists of stuff

        private List<Object> allObjects;

        private List<Sprite> allSprites;

        private List<TiledMapTile> allTiles;
        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {

            //playerOneInv = new PlayerInventory("player1", graphicsDevice, content);

            joeSprite = content.Load<Texture2D>("Player/Joe/joe");
            player = new Player("joe", new Vector2(800, 800), joeSprite) { Activate = true, Right = Keys.D };
            Player basicRaft = new Player("basicRaft", new Vector2(480, 820), joeSprite) { Activate = false };

            IliadMap = content.Load<TiledMap>("Map/Iliad");
            alwaysFront = IliadMap.GetLayer<TiledMapTileLayer>("FrontAlways");
            foreGround = IliadMap.GetLayer<TiledMapLayer>("ForeGround");
            midGround = IliadMap.GetLayer<TiledMapLayer>("MidGround");
            buildings = IliadMap.GetLayer<TiledMapLayer>("Buildings");
            backGround = IliadMap.GetLayer<TiledMapLayer>("BackGround");

            //UI Textures
            toolBar = new ToolBar(game, graphicsDevice, content, mouse);


            //static textures
            //woodenPost = content.Load<Texture2D>("Objects/woodenPost");

            //greatPine = content.Load<Texture2D>("Objects/GreatPineFixed");

            //Texture Lists

            allSprites = new List<Sprite>()
            {

            };

            allObjects = new List<Object>()
            {

            };

            allTiles = new List<TiledMapTile>();

            //TestSprite = new AnimatedSprite(content.Load<Texture2D>("character/joe"), 1, 4);


            //joe animation textures
            var joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            var joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            var joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            var joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");

            //raft animation textures
           /* var raftDown = content.Load<Texture2D>("character/Boats/Rafts/raftDown");
            var raftUp = content.Load<Texture2D>("character/Boats/Rafts/raftUp");
            var raftRight = content.Load<Texture2D>("character/Boats/Rafts/raftRight");
            var raftLeft = content.Load<Texture2D>("character/Boats/Rafts/raftLeft");
            */


            //basicHouse = content.Load<Texture2D>("Objects/basichouse");



            //declare animations
            player.anim = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            
            //joe animation loads
            player.animations[0] = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            player.animations[1] = new AnimatedSprite(graphicsDevice, joeUp, 1, 4);
            player.animations[2] = new AnimatedSprite(graphicsDevice, joeLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(graphicsDevice, joeRight, 1, 4);

            cam = new Camera2D(graphicsDevice);

            /*Song song = content.Load<Song>("Music/TheCarnival");
            MediaPlayer.Volume = 0.7F;
            MediaPlayer.Play(song);
            */

            //Increasing TextureAdjustment.X moves the top left corner of the sprite rectangle to the right
            //Increasing TextureAdjustment.y moves the top left corner of the sprite rectangle down
            //Decreasing WidthSubtractor moves the right of the rectangle left
            //Decreasing HeightSubtractor moves the bottom of the rectangle up


            TiledMapObject[] objectLayer = IliadMap.GetLayer<TiledMapObjectLayer>("Objects").Objects;
            foreach (var objl in objectLayer)
            {
                string type;
                objl.Properties.TryGetValue("type", out type);

                if (type == "building")
                {
                    allObjects.Add(new Object(graphicsDevice, objl.Position, (int)objl.Size.Height, (int)objl.Size.Width));
                }

            



            }
            

        }

        #endregion

        #region UPDATE
        public override void Update(GameTime gameTime, MouseState mouse)
        {

            KeyboardState oldKeyboardState = kState;
            kState = Keyboard.GetState();


            if ((oldKeyboardState.IsKeyDown(Keys.F1)) && (kState.IsKeyUp(Keys.F1)))
            {
                showBorders = !showBorders;
            }


            player.Update(gameTime, allSprites, allObjects);

            foreach (Sprite sprite in allSprites)
            {
                sprite.Update(gameTime);
            }


            toolBar.Update(gameTime, mouse);




            cam.LookAt(new Vector2(player.Position.X, player.Position.Y));
            cam.ZoomIn(2);
            cam.MaximumZoom = 2;

           // if ((oldKeyboardState.IsKeyDown(Keys.R)) && (kState.IsKeyUp(Keys.R)))
              //  game.gameStages = Stages.WorldMap;

            if ((oldKeyboardState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            {
                game.gameStages = Stages.MainMenu;

            }

            if ((oldKeyboardState.IsKeyDown(Keys.E)) && (kState.IsKeyUp(Keys.E)))
            {
               // playerOneInv.IsOpen = !playerOneInv.IsOpen;
                //playerOneInv.UpdateInventory();
            }

        }


        #endregion

        #region DRAW
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, TiledMapRenderer renderer)
        {



            if (player.Health > 0)
            {


                spriteBatch.Begin();

                renderer.Draw(backGround, cam.GetViewMatrix(), depth: (float).1);
                renderer.Draw(buildings, cam.GetViewMatrix(), depth: (float).2);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: cam.GetViewMatrix());

                player.anim.ShowRectangle = showBorders;
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).3);

                foreach (var obj in allObjects)
                {
                    obj.ShowRectangle = showBorders;
                    obj.Draw(spriteBatch);
                }


                foreach (var sprite in allSprites)
                {
                    sprite.ShowRectangle = showBorders;
                    sprite.Draw(spriteBatch);
                }
                //player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).1);
                //basicRaft.anim.Draw(spriteBatch, new Vector2(basicRaft.Position.X, basicRaft.Position.Y), basicRaft.playerDepth);

                spriteBatch.End();
            }




            spriteBatch.End();
            spriteBatch.Begin();
            renderer.Draw(midGround, cam.GetViewMatrix(), depth: (float).4);
            renderer.Draw(foreGround, cam.GetViewMatrix(), depth: (float).5);
            renderer.Draw(alwaysFront, cam.GetViewMatrix(), depth: (float).6);
           // playerOneInv.Draw(spriteBatch);
            spriteBatch.End();





            toolBar.Draw(gameTime, spriteBatch, renderer);
        }
        #endregion

    }
}
