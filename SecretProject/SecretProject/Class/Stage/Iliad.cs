
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
using System;

namespace SecretProject.Class.Stage
{
    class Iliad : Component
    {
        
        #region FIELDS

        private bool showBorders = false;



        //--------------------------------------
        //Declare Map
        TiledMapRenderer mapRenderer;



        /*
                TiledMapLayer backGround;
                TiledMapLayer buildings;
                TiledMapLayer midGround;
                TiledMapLayer foreGround;
                TiledMapLayer alwaysFront;
                */

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

        //--------------------------------------
        //Declare Textures
        
        //Texture2D woodenPost;
       // Texture2D basicHouse;
      //  Texture2D greatPine;

        //PlayerInventory playerOneInv;



        //AnimatedSprite TestSprite;

        //--------------------------------------
        //Declare Lists of stuff

        private List<Object> allObjects;

        private List<Sprite> allSprites;

        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseState mouse) : base(game, graphicsDevice, content, mouse)
        {

            //playerOneInv = new PlayerInventory("player1", graphicsDevice, content);

            joeSprite = content.Load<Texture2D>("Player/Joe/joe");
            player = new Player("joe", new Vector2(800, 800), joeSprite) { Activate = true, Right = Keys.D };
            Player basicRaft = new Player("basicRaft", new Vector2(480, 820), joeSprite) { Activate = false };

            //UI Textures
            toolBar = new ToolBar(game, graphicsDevice, content, mouse);

            map = new TmxMap("Content/Map/worldMap.tmx");

            tileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");


            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            allSprites = new List<Sprite>()
            {

            };

            allObjects = new List<Object>()
            {

            };


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

            cam = new Camera2D(graphicsDevice);

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
           // cam.ZoomIn(1);
        //    cam.MaximumZoom = (float)1.5;

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

            var treeLayer = map.Layers["Tile Layer 2"];

            graphicsDevice.Clear(Color.Black);
            if (player.Health > 0)
            {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: cam.GetViewMatrix());
                player.anim.ShowRectangle = showBorders;
                player.anim.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).2);
                
                for (var i = 0; i < treeLayer.Tiles.Count; i++)
                {
                    int gid = treeLayer.Tiles[i].Gid;


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

                        spriteBatch.Draw(tileSet, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tileSetRec, Color.White, (float)0, new Vector2(0,0), SpriteEffects.None, (float).3);
                    }
                }
                


                
                for (var i = 0; i < map.Layers["Tile Layer 1"].Tiles.Count; i++)
                    {
                        int gid = map.Layers["Tile Layer 1"].Tiles[i].Gid;


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

                        spriteBatch.Draw(tileSet, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tileSetRec, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, (float).1);
                    }
                    }
                    
               




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
