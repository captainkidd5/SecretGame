
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




using TiledSharp;

using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using Object = SecretProject.Class.ObjectFolder.Object;
using System;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.TileStuff;
using Microsoft.Xna.Framework.Media;

namespace SecretProject.Class.Stage
{
    public class Iliad : Component
    {
        
        #region FIELDS

        private bool showBorders = false;



        //--------------------------------------
        //Declare Map


        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;

       public static TmxMap map;
        Texture2D tileSet;

        Texture2D joeSprite;
        Texture2D raftDown;

       // public TiledMap IliadMap;

        public Vector2 TileSize = new Vector2(32, 32);

        //--------------------------------------
        //Instantiate playables
        Player player;
        Player mastodon;

        //--------------------------------------
        //Declare input stuff
        Camera2D cam;

        Camera camera;


        KeyboardState kState;

        ToolBar toolBar;

        TileManager backGroundTiles;
        TileManager buildingsTiles;
        TileManager midGroundTiles;
        TileManager testTiles;

        //--------------------------------------
        //Declare Textures



        //AnimatedSprite TestSprite;

        //--------------------------------------
        //Declare Lists of stuff

        public List<Object> allObjects;

        private List<Sprite> allSprites;

        

        TmxLayer buildings;
        TmxLayer background;
        TmxLayer midGround;
        TmxLayer testLayer;


        //Declare Music
        Song mainTheme;

        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {

            //playerOneInv = new PlayerInventory("player1", graphicsDevice, content);

            joeSprite = content.Load<Texture2D>("Player/Joe/joe");
            player = new Player("joe", new Vector2(1000, 650), joeSprite, 4) { Activate = true, Right = Keys.D };
            mastodon = new Player("basicRaft", new Vector2(850, 850), joeSprite, 4) { Activate = false };

            allSprites = new List<Sprite>()
            {

            };

            allObjects = new List<Object>()
            {

            };
            

            //UI Textures
            toolBar = new ToolBar(game, graphicsDevice, content, mouse);



            //load tile stuff
            map = new TmxMap("Content/Map/worldMap.tmx");

            tileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");

            background= map.Layers["background"];
            buildings = map.Layers["buildings"];
            midGround = map.Layers["midGround"];

            testLayer = map.Layers["test"];

            //var treee = map.ObjectGroups["buildings"].Objects["Tree"];

            

            
            var buildingLayer = map.ObjectGroups["collision"];



            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            
            //joe animation textures
            var joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            var joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            var joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            var joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");

            var mUp = content.Load<Texture2D>("NPC/Mastodon/MastodonBack");
            var mDown = content.Load<Texture2D>("NPC/Mastodon/MastodonFront");
            var mLeft = content.Load<Texture2D>("NPC/Mastodon/MastodonLeftSide");
            var mRight = content.Load<Texture2D>("NPC/Mastodon/MastodonRightSide");

            var mIdle = content.Load<Texture2D>("NPC/Mastodon/MastodonIdle");



            //declare animations
            player.anim = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            
            //joe animation loads
            player.animations[0] = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            player.animations[1] = new AnimatedSprite(graphicsDevice, joeUp, 1, 4);
            player.animations[2] = new AnimatedSprite(graphicsDevice, joeLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(graphicsDevice, joeRight, 1, 4);

            mastodon.anim = new AnimatedSprite(graphicsDevice, mIdle, 1, 2);

            mastodon.animations[0] = new AnimatedSprite(graphicsDevice, mDown, 1, 2);
            mastodon.animations[1] = new AnimatedSprite(graphicsDevice, mUp, 1, 4);
            mastodon.animations[2] = new AnimatedSprite(graphicsDevice, mLeft, 1, 4);
            mastodon.animations[3] = new AnimatedSprite(graphicsDevice, mRight, 1, 4);


            //load camera stuff
            //camera = new Camera();
            // camera.Zoom = 2;

            cam = new Camera2D();
           cam.Zoom = 2.5f;

           cam.Move(new Vector2(player.Position.X, player.Position.Y));

            backGroundTiles = new TileManager(tileSet, map, background);
            buildingsTiles = new TileManager(tileSet, map, buildings);
            midGroundTiles = new TileManager(tileSet, map, midGround);
            testTiles = new TileManager(tileSet, map, testLayer);



            foreach (Tile someTile in buildingsTiles.Tiles)
            {
                if(someTile.GID != 0)
                {
                    allObjects.Add(new Object(graphicsDevice, someTile.DestinationRectangle));
                }
               
                
            }


            // backGroundTiles.LoadTiles();

            //Songs
            mainTheme = content.Load<Song>("Music/IntheForest");
            //MediaPlayer.Play(mainTheme);

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

           // mastodon.Update(gameTime, allSprites, allObjects);

            /*

            foreach (Sprite sprite in allSprites)
            {
                sprite.Update(gameTime);
            }

            foreach(Object obj in allObjects)
            {
                obj.Update(gameTime);
            }
            */


            toolBar.Update(gameTime, mouse);


            cam.Follow(new Vector2(player.Position.X, player.Position.Y));

            //camera.follow(player.Position, player.Rectangle);


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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            graphicsDevice.Clear(Color.Black);
            if (player.Health > 0)
            {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: cam.getTransformation(graphicsDevice));
                player.anim.ShowRectangle = showBorders;
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).3);
                mastodon.anim.Draw(spriteBatch, new Vector2(mastodon.Position.X, mastodon.Position.Y), (float).3);

                backGroundTiles.DrawTiles(spriteBatch, (float).1);
                buildingsTiles.DrawTiles(spriteBatch, (float).2);
                midGroundTiles.DrawTiles(spriteBatch, (float).4);

               


                foreach (var sprite in allSprites)
                {
                    sprite.ShowRectangle = showBorders;
                    sprite.Draw(spriteBatch);
                }

                foreach(var obj in allObjects)
                {
                    obj.ShowRectangle = showBorders;
                    obj.Draw(spriteBatch);
                }


                spriteBatch.End();
            }



            toolBar.Draw(gameTime, spriteBatch);
        }
        #endregion

    }
}
