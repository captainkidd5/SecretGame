
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
using SecretProject.Class.Camera;
using SecretProject.Class.TileStuff;

namespace SecretProject.Class.Stage
{
    class Iliad : Component
    {
        
        #region FIELDS

        private bool showBorders = false;



        //--------------------------------------
        //Declare Map


        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;

        TmxMap map;
        Texture2D tileSet;

        Texture2D joeSprite;
        Texture2D raftDown;

       // public TiledMap IliadMap;

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

        TileManager backGroundTiles;
        TileManager buildingsTiles;
        TileManager midGroundTiles;

        //--------------------------------------
        //Declare Textures
        
 

        //AnimatedSprite TestSprite;

        //--------------------------------------
        //Declare Lists of stuff

        private List<Object> allObjects;

        private List<Sprite> allSprites;

        

        TmxLayer buildings;
        TmxLayer background;
        TmxLayer midGround;

        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {

            //playerOneInv = new PlayerInventory("player1", graphicsDevice, content);

            joeSprite = content.Load<Texture2D>("Player/Joe/joe");
            player = new Player("joe", new Vector2(800, 800), joeSprite) { Activate = true, Right = Keys.D };
            Player basicRaft = new Player("basicRaft", new Vector2(480, 820), joeSprite) { Activate = false };

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

            //var treee = map.ObjectGroups["buildings"].Objects["Tree"];

            

            
            var buildingLayer = map.ObjectGroups["collision"];

            foreach (var obj in buildingLayer.Objects)
            {
                //if(obj.Name == "Tree")
                //{
                    allObjects.Add(new Object(new Vector2((float)obj.X, (float)obj.Y), (int)obj.Height, (int)obj.Width));

                //}
            }
            

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            
            //joe animation textures
            var joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            var joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            var joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            var joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");



            //declare animations
            player.anim = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            
            //joe animation loads
            player.animations[0] = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            player.animations[1] = new AnimatedSprite(graphicsDevice, joeUp, 1, 4);
            player.animations[2] = new AnimatedSprite(graphicsDevice, joeLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(graphicsDevice, joeRight, 1, 4);


            //load camera stuff
            cam = new Camera2D();
           // cam.Zoom = 2.0f;

            cam.Move(new Vector2(player.Position.X, player.Position.Y));

            backGroundTiles = new TileManager(tileSet, map, background);
            buildingsTiles = new TileManager(tileSet, map, buildings);
            midGroundTiles = new TileManager(tileSet, map, midGround);

            
           // backGroundTiles.LoadTiles();

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

            
            cam.Follow(new Vector2(player.Position.X, player.Position.Y));


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
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: cam.getTransformation(graphicsDevice));
                player.anim.ShowRectangle = showBorders;
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).3);

                backGroundTiles.DrawTiles(spriteBatch, (float).1);
                buildingsTiles.DrawTiles(spriteBatch, (float).2);
                midGroundTiles.DrawTiles(spriteBatch, (float).4);

               


                foreach (var sprite in allSprites)
                {
                    sprite.ShowRectangle = showBorders;
                    sprite.Draw(spriteBatch);
                }


                spriteBatch.End();
            }



            toolBar.Draw(gameTime, spriteBatch);
        }
        #endregion

    }
}
