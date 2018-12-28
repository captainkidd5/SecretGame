
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
using System.Runtime.Serialization;
using SecretProject.Class.ItemStuff.Items;

namespace SecretProject.Class.Stage
{
    public class Iliad : Component
    {
        
        #region FIELDS

        private bool showBorders = false;
        public Vector2 TileSize = new Vector2(16, 16); // what?
        public static TmxMap map;
        public Player Player { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        public Texture2D TileSet { get; set; }
        internal TileManager BackGroundTiles { get; set; }
        internal TileManager BuildingsTiles { get => BuildingsTiles1; set => BuildingsTiles1 = value; }
        internal TileManager BuildingsTiles1 { get; set; }
        internal TileManager MidGroundTiles { get => MidGroundTiles1; set => MidGroundTiles1 = value; }
        internal TileManager MidGroundTiles1 { get; set; }
        internal TileManager TestTiles { get; set; }
        public TmxLayer Buildings { get => Buildings1; set => Buildings1 = value; }
        public TmxLayer Buildings1 { get => Buildings2; set => Buildings2 = value; }
        public TmxLayer Buildings2 { get => Buildings3; set => Buildings3 = value; }
        public TmxLayer Buildings3 { get; set; }
        public TmxLayer Background { get => Background1; set => Background1 = value; }
        public TmxLayer Background1 { get; set; }
        public TmxLayer MidGround { get; set; }
        public TmxLayer TestLayer { get; set; }
        public Texture2D JoeSprite { get; set; }
        public Texture2D RaftDown { get; set; }
        public Texture2D PuzzleFish { get; set; }
        public Texture2D HouseKey { get; set; }
        public Song MainTheme { get; set; }
        public KeyboardState KState { get; set; }
        internal ToolBar ToolBar { get; set; }
        public Player Mastodon { get; set; }
        public Camera2D Cam { get; set; }

        //--------------------------------------
        //Declare Lists

        public static List<ObjectBody> allObjects;

        public static List<Sprite> allSprites;

        //SAVE STUFF




        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, Camera2D camera, UserInterface userInterface, Player player) : base(game, graphicsDevice, content, mouse, userInterface)
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
            TileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");

            //layers
            Background = map.Layers["background"];
            Buildings = map.Layers["buildings"];
            MidGround = map.Layers["midGround"];
            TestLayer = map.Layers["test"];

            //E   var treee = map.ObjectGroups["buildings"].Objects["Tree"];

            //object layer
            var buildingLayer = map.ObjectGroups["collision"];


            //map specifications
            TileWidth = map.Tilesets[0].TileWidth;
            TileHeight = map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;

            BackGroundTiles = new TileManager(game,TileSet, map, Background, mouse, graphicsDevice, false);
            BuildingsTiles = new TileManager(game, TileSet, map, Buildings, mouse, graphicsDevice, true);
            MidGroundTiles = new TileManager(game, TileSet, map, MidGround, mouse, graphicsDevice, false);
            TestTiles = new TileManager(game, TileSet, map, TestLayer, mouse, graphicsDevice, false);



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

            this.Player = player;

            //mastodon
      //      var mUp = content.Load<Texture2D>("NPC/Mastodon/MastodonBack");
        //    var mDown = content.Load<Texture2D>("NPC/Mastodon/MastodonFront");
        //    var mLeft = content.Load<Texture2D>("NPC/Mastodon/MastodonLeftSide");
         //   var mRight = content.Load<Texture2D>("NPC/Mastodon/MastodonRightSide");

            var mIdle = content.Load<Texture2D>("NPC/Mastodon/MastodonIdle");

            //load players
            
          //  Mastodon = new Player("basicRaft", new Vector2(850, 850), JoeSprite, 4, content, graphicsDevice, customMouse) { Activate = false };
            //declare animations

          //  Player.Texture = JoeSprite;

            //mastodon animation
          //  Mastodon.Anim = new AnimatedSprite(graphicsDevice, mIdle, 1, 2);
          
           // Mastodon.animations[0] = new AnimatedSprite(graphicsDevice, mDown, 1, 2);
          //  Mastodon.animations[1] = new AnimatedSprite(graphicsDevice, mUp, 1, 4);
          //  Mastodon.animations[2] = new AnimatedSprite(graphicsDevice, mLeft, 1, 4);
          //  Mastodon.animations[3] = new AnimatedSprite(graphicsDevice, mRight, 1, 4);

            //--------------------------------------
            //UI Textures
            //toolBar = new ToolBar(game, graphicsDevice, content, mouse);

            //sprite textures
            PuzzleFish = content.Load<Texture2D>("Item/puzzleFish");
            HouseKey = content.Load<Texture2D>("Item/houseKey");

            //--------------------------------------
            //camera
            this.Cam = camera;
            Cam.Zoom = 2.5f;
            //cam.Move(new Vector2(player.Position.X, player.Position.Y));

            //--------------------------------------
            //Songs
            MainTheme = content.Load<Song>("Music/IntheForest"); 
            //MediaPlayer.Play(mainTheme);

            // midGroundTiles.isActive = true;
            BuildingsTiles.isActive = true;
            BuildingsTiles.isBuilding = true;

           // buildingsTiles.ReplaceTileGid = 3235;

            allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(845, 680), true));
            allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(900, 680), true));
            allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(1200, 680), true));

            //UserInterface
            mainUserInterface = new UserInterface(game, graphicsDevice, content, mouse, Player.Inventory);


        }

        #endregion

        #region UPDATE
        public override void Update(GameTime gameTime)
        {
            //--------------------------------------
            //input

            //keyboard
            KeyboardState oldKeyboardState = KState;
            KState = Keyboard.GetState();

            //--------------------------------------
            //Update Toolbar

            mainUserInterface.Update(gameTime, KState, oldKeyboardState);


            //mouse

            customMouse.Update();
            


            if ((oldKeyboardState.IsKeyDown(Keys.F1)) && (KState.IsKeyUp(Keys.F1)))
            {
                showBorders = !showBorders;
            }

            if (!Game1.freeze)
            {

                if (customMouse.IsClicked)
                {
                    allSprites.Add(new Sprite(graphicsDevice, content, PuzzleFish, customMouse.WorldMousePosition, true) { IsItem = true });
                }

                if(customMouse.IsRightClicked)
                {
                    //Player.Inventory.DropItemFromInventory(new Food("shrimp", content));
                }


                //--------------------------------------
                //Update Players

                Player.Update(gameTime, allSprites, allObjects);

                // mastodon.Update(gameTime, allSprites, allObjects);

                //--------------------------------------
                //Update sprites
                foreach (Sprite spr in allSprites)
                {
                    spr.Update(gameTime, Player);

                }




                //--------------------------------------
                //update camera

                Cam.Follow(new Vector2(Player.Position.X, Player.Position.Y));




            }
        }

        #endregion

        #region DRAW
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            graphicsDevice.Clear(Color.Black);
            if (Player.Health > 0)
            {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphicsDevice));
                Player.Anim.ShowRectangle = showBorders;
                Player.Anim.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y), (float).3);
               // Mastodon.Anim.Draw(spriteBatch, new Vector2(Mastodon.Position.X, Mastodon.Position.Y), (float).3);

                BackGroundTiles.DrawTiles(spriteBatch, (float).1);
                BuildingsTiles.DrawTiles(spriteBatch, (float).2);
                MidGroundTiles.DrawTiles(spriteBatch, (float).4);


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

            mainUserInterface.Draw(spriteBatch);
        }

        
        #endregion

    }
}
