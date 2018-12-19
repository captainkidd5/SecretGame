
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TiledSharp;

using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using ObjectBody = SecretProject.Class.ObjectFolder.ObjectBody;
using System;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.TileStuff;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;

namespace SecretProject.Class.Stage
{
    public class Iliad : Component
    {
        
        #region FIELDS

        private bool showBorders = false;


        //--------------------------------------
        //tile information
        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;
        public Vector2 TileSize = new Vector2(16, 16); // what?
        public static TmxMap map;
        Texture2D tileSet;

        TileManager backGroundTiles;
        TileManager buildingsTiles;
        TileManager midGroundTiles;
        TileManager testTiles;

        TmxLayer buildings;
        TmxLayer background;
        TmxLayer midGround;
        TmxLayer testLayer;

        //--------------------------------------
        //character sprites
        Texture2D joeSprite;
        Texture2D raftDown;


        //--------------------------------------
        //item sprites
        Texture2D puzzleFish;
        Texture2D houseKey;

        //--------------------------------------
        //players
        Player player;
        Player mastodon;

        //--------------------------------------
        //camera
        Camera2D cam;

        //--------------------------------------
        //keyboard
        KeyboardState kState;

        //--------------------------------------
        //menu
        ToolBar toolBar;

        //--------------------------------------
        //Declare Lists

        public static List<ObjectBody> allObjects;

        private List<Sprite> allSprites;

        //--------------------------------------
        //Declare Music
        Song mainTheme;

        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, Camera2D camera) : base(game, graphicsDevice, content, mouse)
        {
            //ORDER MATTERS!
            //Lists
            //--------------------------------------
            allSprites = new List<Sprite>()
            {

            };

            allObjects = new List<ObjectBody>()
            {

            };


            //--------------------------------------
            //Tile/map
            map = new TmxMap("Content/Map/worldMap.tmx");

            //tileset
            tileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");

            //layers
            background = map.Layers["background"];
            buildings = map.Layers["buildings"];
            midGround = map.Layers["midGround"];
            testLayer = map.Layers["test"];

            //E   var treee = map.ObjectGroups["buildings"].Objects["Tree"];

            //object layer
            var buildingLayer = map.ObjectGroups["collision"];


            //map specifications
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            backGroundTiles = new TileManager(game,tileSet, map, background, mouse, graphicsDevice, false);
            buildingsTiles = new TileManager(game, tileSet, map, buildings, mouse, graphicsDevice, true);
            midGroundTiles = new TileManager(game, tileSet, map, midGround, mouse, graphicsDevice, false);
            testTiles = new TileManager(game, tileSet, map, testLayer, mouse, graphicsDevice, false);

            

            //buildingsTiles.isBuilding = true;

            //add objects to object layer
            /*
            foreach (Tile someTile in buildingsTiles.Tiles)
            {
                if (someTile.GID != 0)
                {
                    allObjects.Add(new ObjectBody(graphicsDevice, someTile.DestinationRectangle));
                }
            }
            */

            //--------------------------------------
            //Player Stuff

            //joe
            joeSprite = content.Load<Texture2D>("Player/Joe/joe");

            var joeDown = content.Load<Texture2D>("Player/Joe/JoeWalkForwardNew");
            var joeUp = content.Load<Texture2D>("Player/Joe/JoeWalkBackNew");
            var joeRight = content.Load<Texture2D>("Player/Joe/JoeWalkRightNew");
            var joeLeft = content.Load<Texture2D>("Player/Joe/JoeWalkLefttNew");

            //mastodon
            var mUp = content.Load<Texture2D>("NPC/Mastodon/MastodonBack");
            var mDown = content.Load<Texture2D>("NPC/Mastodon/MastodonFront");
            var mLeft = content.Load<Texture2D>("NPC/Mastodon/MastodonLeftSide");
            var mRight = content.Load<Texture2D>("NPC/Mastodon/MastodonRightSide");

            var mIdle = content.Load<Texture2D>("NPC/Mastodon/MastodonIdle");

            //load players
            player = new Player("joe", new Vector2(1000, 650), joeSprite, 4) { Activate = true, Right = Keys.D };
            mastodon = new Player("basicRaft", new Vector2(850, 850), joeSprite, 4) { Activate = false };
            //declare animations
            player.anim = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);

            //joe animation 
            player.animations[0] = new AnimatedSprite(graphicsDevice, joeDown, 1, 4);
            player.animations[1] = new AnimatedSprite(graphicsDevice, joeUp, 1, 4);
            player.animations[2] = new AnimatedSprite(graphicsDevice, joeLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(graphicsDevice, joeRight, 1, 4);


            //mastodon animation
            mastodon.anim = new AnimatedSprite(graphicsDevice, mIdle, 1, 2);

            mastodon.animations[0] = new AnimatedSprite(graphicsDevice, mDown, 1, 2);
            mastodon.animations[1] = new AnimatedSprite(graphicsDevice, mUp, 1, 4);
            mastodon.animations[2] = new AnimatedSprite(graphicsDevice, mLeft, 1, 4);
            mastodon.animations[3] = new AnimatedSprite(graphicsDevice, mRight, 1, 4);

            //--------------------------------------
            //UI Textures
            toolBar = new ToolBar(game, graphicsDevice, content, mouse);

            //sprite textures
            puzzleFish = content.Load<Texture2D>("Item/puzzleFish");
            houseKey = content.Load<Texture2D>("Item/houseKey");

            //--------------------------------------
            //camera
            this.cam = camera;
            cam.Zoom = 2.5f;
            //cam.Move(new Vector2(player.Position.X, player.Position.Y));

            //--------------------------------------
            //Songs
            mainTheme = content.Load<Song>("Music/IntheForest"); 
            //MediaPlayer.Play(mainTheme);

            // midGroundTiles.isActive = true;
            buildingsTiles.isActive = true;
            buildingsTiles.isBuilding = true;

           // buildingsTiles.ReplaceTileGid = 3235;

            allSprites.Add(new Sprite(graphicsDevice, content, houseKey, new Vector2(845, 680), true));
            allSprites.Add(new Sprite(graphicsDevice, content, houseKey, new Vector2(900, 680), true));
            allSprites.Add(new Sprite(graphicsDevice, content, houseKey, new Vector2(1200, 680), true));


        }

        #endregion

        #region UPDATE
        public override void Update(GameTime gameTime)
        {
            //--------------------------------------
            //input

            //keyboard
            KeyboardState oldKeyboardState = kState;
            kState = Keyboard.GetState();


            if ((oldKeyboardState.IsKeyDown(Keys.F1)) && (kState.IsKeyUp(Keys.F1)))
            {
                showBorders = !showBorders;
            }

            //mouse

            customMouse.Update();
            if(customMouse.IsClicked)
            {
                allSprites.Add(new Sprite(graphicsDevice, content, puzzleFish, customMouse.WorldMousePosition, true));
            }
            

            //--------------------------------------
            //Update Players

            player.Update(gameTime, allSprites, allObjects);

            // mastodon.Update(gameTime, allSprites, allObjects);

            //--------------------------------------
            //Update sprites
            foreach(Sprite spr in allSprites)
            {
                spr.Update(gameTime);
                
            }


            //--------------------------------------
            //Update Toolbar

            toolBar.Update(gameTime);

            //--------------------------------------
            //update camera

            cam.Follow(new Vector2(player.Position.X, player.Position.Y));

            // E camera.follow(player.Position, player.Rectangle);


            //--------------------------------------
            //Change Game Stages

            // E if ((oldKeyboardState.IsKeyDown(Keys.R)) && (kState.IsKeyUp(Keys.R)))
            //  game.gameStages = Stages.WorldMap;

            if ((oldKeyboardState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            {
                game.gameStages = Stages.MainMenu;

            }

            //--------------------------------------
            //Open Menus

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


                //--------------------------------------
                //Draw sprite list

                foreach (var sprite in allSprites)
                {
                    sprite.ShowRectangle = showBorders;
                    sprite.Draw(spriteBatch, .4f);
                }

                foreach (var obj in allObjects)
                {
                    if(showBorders)
                    {
                      obj.Draw(spriteBatch, .4f);
                    }
                    
                }
                //--------------------------------------
                //Draw object list




                spriteBatch.End();
            }

            //--------------------------------------
            //Draw Toolbar

            toolBar.Draw(gameTime, spriteBatch);
        }
        #endregion

    }
}
