
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

            cam = new Camera2D();
           // cam.Zoom = 1f;

            cam.Move(new Vector2(player.Position.X, player.Position.Y));

            backGroundTiles = new TileManager(tileSet, map, background);
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

            




           //cam.LookAt(new Vector2(player.Position.X, player.Position.Y));
           //cam.ZoomIn(2);
           //cam.MaximumZoom = 2;

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
           // var tileSet = map.Tilesets["MasterSpriteSheet"];


            graphicsDevice.Clear(Color.Black);
            if (player.Health > 0)
            {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: cam.getTransformation(graphicsDevice));
                player.anim.ShowRectangle = showBorders;
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).3);

                backGroundTiles.DrawTiles(spriteBatch, (float).1);

               // backGroundTiles.DrawTiles3(spriteBatch, (float).1);

               // backGroundTiles.DrawTiles2(spriteBatch, (float).1);

                /*
                DrawTiles(spriteBatch, tileSet, map, midGround, (float).4, tilesetTilesWide, tileWidth, tileHeight);

                DrawTiles(spriteBatch, tileSet, map, buildings, (float).2, tilesetTilesWide, tileWidth, tileHeight);

              

                DrawTiles(spriteBatch, tileSet, map, background, (float).1, tilesetTilesWide, tileWidth, tileHeight);

    */

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

        public static void DrawTiles(SpriteBatch spriteBatch, Texture2D tileSet, TmxMap map, TmxLayer layerName, float depth, int tilesetTilesWide, int tileWidth, int tileHeight)
        {
            for (var i = 0; i < layerName.Tiles.Count; i++)
            {
                int gid = layerName.Tiles[i].Gid;


                if (gid == 0)
                {

                }

                else
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    Rectangle tileSetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(tileSet, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tileSetRec, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                }
            }

        }
        

    }
}
