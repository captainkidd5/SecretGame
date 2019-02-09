
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
using SecretProject.Class.ItemStuff;

namespace SecretProject.Class.Stage
{
    public class Iliad : IStage
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
        public TileManager BackGroundTiles { get; set; }
        public TileManager BuildingsTiles { get; set; }
        public TileManager MidGroundTiles { get; set; }
        public TileManager ForeGroundTiles { get; set; }
        public TileManager PlacementTiles { get; set; }
        public TmxLayer Buildings { get; set; }
        public TmxLayer Background { get; set; }
        public TmxLayer Background1 { get; set; }
        public TmxLayer MidGround { get; set; }
        public TmxLayer foreGround { get; set; }
        public TmxLayer Placement { get; set; }
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

        public List<ObjectBody> allObjects;

        public List<Sprite> allSprites;

        public List<WorldItem> allItems;

        public UserInterface MainUserInterface { get; set; }

        //SAVE STUFF

        public bool TilesLoaded { get; set; } = false;
        #endregion

        #region CONSTRUCTOR

        public Iliad(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, Camera2D camera, UserInterface userInterface, Player player)
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

            allItems = new List<WorldItem>()
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
            foreGround = map.Layers["foreGround"];
            Placement = map.Layers["placement"];

            //E   var treee = map.ObjectGroups["buildings"].Objects["Tree"];

            //object layer
            // var buildingLayer = map.ObjectGroups["collision"];


            //map specifications
            TileWidth = map.Tilesets[0].TileWidth;
            TileHeight = map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;

            BackGroundTiles = new TileManager(TileSet, map, Background, graphicsDevice, content, false);
            BuildingsTiles = new TileManager(TileSet, map, Buildings, graphicsDevice, content, true);
            MidGroundTiles = new TileManager(TileSet, map, MidGround, graphicsDevice, content, false);
            ForeGroundTiles = new TileManager(TileSet, map, foreGround, graphicsDevice, content, false);
            PlacementTiles = new TileManager(TileSet, map, Placement, graphicsDevice, content, false) { isPlacement = true };




            //--------------------------------------
            //Player Stuff

            this.Player = player;


            var mIdle = content.Load<Texture2D>("NPC/Mastodon/MastodonIdle");

            //load players

            //sprite textures
            PuzzleFish = content.Load<Texture2D>("Item/puzzleFish");
            HouseKey = content.Load<Texture2D>("Item/houseKey");

            //--------------------------------------
            //camera
            this.Cam = camera;
            Game1.cam.Zoom = 3f;
            //Cam.Move(new Vector2(player.Position.X, player.Position.Y));

            //--------------------------------------
            //Songs
            MainTheme = content.Load<Song>("Music/IntheForest");
            //MediaPlayer.Play(MainTheme);

            // midGroundTiles.isActive = true;
            BuildingsTiles.isActive = true;
            BuildingsTiles.isBuilding = true;

            // buildingsTiles.ReplaceTileGid = 3235;

            // allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(845, 680), true) { IsBeingDragged = true });
            // allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(900, 680), true));
            // allSprites.Add(new Sprite(graphicsDevice, content, HouseKey, new Vector2(1200, 680), true));



            //UserInterface


            allItems.Add(new WorldItem("pie", graphicsDevice, content, new Vector2(200, 450)));
            allItems.Add(new WorldItem("pie", graphicsDevice, content, new Vector2(200, 300)));
            allItems.Add(new WorldItem("pie", graphicsDevice, content, new Vector2(350, 200)));

            allItems.Add(new WorldItem("shrimp", graphicsDevice, content, new Vector2(200, 500)));

            allItems.Add(new WorldItem("barrel", graphicsDevice, content, new Vector2(Game1.Player.position.X + 50, Game1.Player.position.Y + 50)));

            allItems.Add(new WorldItem("barrel", graphicsDevice, content, new Vector2(Game1.Player.position.X + 70, Game1.Player.position.Y + 50)));

            allItems.Add(new WorldItem("barrel", graphicsDevice, content, new Vector2(Game1.Player.position.X + 30, Game1.Player.position.Y + 50)));

            allItems.Add(new WorldItem("barrel", graphicsDevice, content, new Vector2(Game1.Player.position.X + 10, Game1.Player.position.Y + 50)));

            allItems.Add(new WorldItem("barrel", graphicsDevice, content, new Vector2(Game1.Player.position.X + 100, Game1.Player.position.Y + 50)));


        }

        #endregion

        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {
            //--------------------------------------
            //input

            //keyboard
            KeyboardState oldKeyboardState = KState;
            KState = Keyboard.GetState();

            //--------------------------------------
            //Update Toolbar

            Game1.userInterface.Update(gameTime, KState, oldKeyboardState, Player.Inventory, mouse, game);



            if ((oldKeyboardState.IsKeyDown(Keys.F1)) && (KState.IsKeyUp(Keys.F1)))
            {
                showBorders = !showBorders;
            }

            if (!Game1.freeze)
            {

                //--------------------------------------
                //Update Players
                Game1.cam.Follow(new Vector2(Player.Position.X, Player.Position.Y));
                Player.Update(gameTime, allItems, allObjects);

                // mastodon.Update(gameTime, allSprites, allObjects);

                //--------------------------------------
                //Update sprites
                foreach (Sprite spr in allSprites)
                {
                    if (spr.IsBeingDragged == true)
                    {
                        spr.Update(gameTime, mouse.WorldMousePosition);
                    }


                }

                BackGroundTiles.Update(gameTime, mouse);
                BuildingsTiles.Update(gameTime, mouse);
                MidGroundTiles.Update(gameTime, mouse);
                PlacementTiles.Update(gameTime, mouse);

                foreach (WorldItem item in allItems)
                {
                    item.Update(gameTime);
                }

            }
        }

        #endregion

        #region DRAW
        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)
        {


            graphics.Clear(Color.Black);
            if (Player.Health > 0)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics));
                Player.PlayerMovementAnimations.ShowRectangle = showBorders;

                if (Player.CurrentAction.IsAnimating == false)
                {
                    Player.PlayerMovementAnimations.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y), (float).4);
                }
                

                //????
                if(Player.CurrentAction.IsAnimating == true)
                {
                    Player.CurrentAction.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y), (float).4);
                }
                
                // Mastodon.Anim.Draw(spriteBatch, new Vector2(Mastodon.Position.X, Mastodon.Position.Y), (float).3);

                BackGroundTiles.DrawTiles(spriteBatch, (float).1);
                BuildingsTiles.DrawTiles(spriteBatch, (float).2);
                MidGroundTiles.DrawTiles(spriteBatch, (float).3);
                ForeGroundTiles.DrawTiles(spriteBatch, (float).5);
                PlacementTiles.DrawTiles(spriteBatch, (float).6);
                Game1.userInterface.BottomBar.DrawDraggableItems(spriteBatch, BuildingsTiles, ForeGroundTiles, mouse);

                //drawn in wrong spot


                //--------------------------------------
                //Draw sprite list

                foreach (var sprite in allSprites)
                {
                    sprite.ShowRectangle = showBorders;
                    sprite.Draw(spriteBatch);
                }

                foreach (WorldItem item in allItems)
                {
                    item.Draw(spriteBatch);
                }

                foreach (var obj in allObjects)
                {
                    if (showBorders)
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

            Game1.userInterface.Draw(spriteBatch);
        }


        #endregion

    }
}