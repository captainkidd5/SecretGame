//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using SecretProject.Class.CameraStuff;
//using SecretProject.Class.Controls;
//using SecretProject.Class.ItemStuff;
//using SecretProject.Class.ObjectFolder;
//using SecretProject.Class.Playable;
//using SecretProject.Class.SpriteFolder;
//using SecretProject.Class.TileStuff;
//using SecretProject.Class.UI;
//using SecretProject.Class.Universal;
//using TiledSharp;

//namespace SecretProject.Class.StageFolder
//{
//    public class Sea : IStage
//    {


//        public bool ShowBorders { get; set; }
//        public Vector2 TileSize = new Vector2(16, 16); // what?
//        public TmxMap Map { get; set; }
//        public Player Player { get; set; }
//        public int TileWidth { get; set; }
//        public int TileHeight { get; set; }
//        public int TilesetTilesWide { get; set; }
//        public int TilesetTilesHigh { get; set; }
//        public Texture2D TileSet { get; set; }
//        public TileManager BackGroundTiles { get; set; }
//        public TileManager BuildingsTiles { get; set; }
//        public TileManager MidGroundTiles { get; set; }
//        public TileManager ForeGroundTiles { get; set; }
//        public TileManager PlacementTiles { get; set; }
//        public List<TileManager> AllStageTiles { get; set; }
//        public TmxLayer Buildings { get; set; }
//        public TmxLayer Background { get; set; }
//        public TmxLayer Background1 { get; set; }
//        public TmxLayer MidGround { get; set; }
//        public TmxLayer foreGround { get; set; }
//        public TmxLayer Placement { get; set; }
//        public Texture2D JoeSprite { get; set; }
//        public Texture2D RaftDown { get; set; }

//        public Texture2D PuzzleFish { get; set; }

//        public Texture2D HouseKey { get; set; }

//        public Song MainTheme { get; set; }

//        public KeyboardState KState { get; set; }

//        public Player Mastodon { get; set; }

//        public Camera2D Cam { get; set; }

//        public int TileSetNumber { get; set; }

//        //--------------------------------------
//        //Declare Lists        
//        public List<ObjectBody> AllObjects { get; set; }
//        public List<Sprite> AllSprites { get; set; }
//        public List<Item> AllItems { get; set; }
//        public List<ActionTimer> AllActions { get; set; }

//        //public List<object> ThingsToDraw;
//        public UserInterface MainUserInterface { get; set; }

//        //SAVE STUFF

//        public bool TilesLoaded { get; set; } = false;


//        public Sea(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse, Camera2D camera, UserInterface userInterface, Player player, TmxMap map, Texture2D TileSet, int TileSetNumber)
//        {
//            //ORDER MATTERS!
//            //Lists
//            //--------------------------------------
//            AllSprites = new List<Sprite>()
//            {

//            };

//            AllObjects = new List<ObjectBody>()
//            {

//            };

//            AllItems = new List<Item>()
//            {

//            };
//            this.TileSetNumber = 1000;

//            //--------------------------------------
//            //Tile/map

//            this.Map = map;
//            // map = new TmxMap("Content/Map/worldMap.tmx");

//            //tileset
//            this.TileSet = TileSet;
//            // TileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");

//            //layers
//            Background = map.Layers["background"];
//            //Buildings = map.Layers["buildings"];
//            //MidGround = map.Layers["midGround"];
//            //foreGround = map.Layers["foreGround"];
//            //Placement = map.Layers["placement"];

//            //map specifications
//            TileWidth = map.Tilesets[0].TileWidth;
//            TileHeight = map.Tilesets[0].TileHeight;

//            TilesetTilesWide = TileSet.Width / TileWidth;
//            TilesetTilesHigh = TileSet.Height / TileHeight;

//            BackGroundTiles = new TileManager(TileSet, map, Background, graphicsDevice, content, false, TileSetNumber, .1f) { isBackground = true };
//            //BuildingsTiles = new TileManager(TileSet, map, Buildings, graphicsDevice, content, true, TileSetNumber, .2f) { IsBuilding = true };
//            //MidGroundTiles = new TileManager(TileSet, map, MidGround, graphicsDevice, content, false, TileSetNumber, .3f);
//            //ForeGroundTiles = new TileManager(TileSet, map, foreGround, graphicsDevice, content, false, TileSetNumber, .5f);
//            //PlacementTiles = new TileManager(TileSet, map, Placement, graphicsDevice, content, false, TileSetNumber, .6f) { isPlacement = true };

//            AllStageTiles = new List<TileManager>() { BackGroundTiles/*, BuildingsTiles, MidGroundTiles, ForeGroundTiles, PlacementTiles */};

//            //--------------------------------------
//            //Player Stuff

//            this.Player = player;

//            //load players

//            //sprite textures

//            //--------------------------------------
//            //camera
//            this.Cam = camera;
//            Game1.cam.Zoom = 3f;
//            Cam.Move(new Vector2(player.Position.X, player.Position.Y));

//            //--------------------------------------
//            //Songs
//            MainTheme = content.Load<Song>("Music/IntheForest");
//            //MediaPlayer.Play(MainTheme);

//            // midGroundTiles.isActive = true;
//            BuildingsTiles.isActive = true;
//            BuildingsTiles.IsBuilding = true;

//            //UserInterface
//            //allItems.Add(Game1.ItemVault.GenerateNewItem(0, new Vector2(Game1.Player.position.X + 100, Game1.Player.position.Y + 50), true));


//            AllActions = new List<ActionTimer>();
//        }



//        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
//        {
//            KeyboardState oldKeyboardState = KState;
//            KState = Keyboard.GetState();
//            Game1.myMouseManager.ToggleGeneralInteraction = false;

//            Game1.userInterface.Update(gameTime, KState, oldKeyboardState, Player.Inventory, mouse, game);

//            if ((oldKeyboardState.IsKeyDown(Keys.F1)) && (KState.IsKeyUp(Keys.F1)))
//            {
//                ShowBorders = !ShowBorders;
//            }

//            if (!Game1.freeze)
//            {
//                Game1.GlobalClock.Update(gameTime);
//                //--------------------------------------
//                //Update Players
//                Game1.cam.Follow(new Vector2(Player.Position.X, Player.Position.Y));
//                Player.Update(gameTime, AllItems, AllObjects);

//                //--------------------------------------
//                //Update sprites
//                foreach (Sprite spr in AllSprites)
//                {
//                    if (spr.IsBeingDragged == true)
//                    {
//                        spr.Update(gameTime, mouse.WorldMousePosition);
//                    }
//                }

//                for (int i = 0; i < AllStageTiles.Count; i++)
//                {
//                    AllStageTiles[i].Update(gameTime, mouse);
//                }

//                for (int i = 0; i < AllItems.Count; i++)
//                {
//                    AllItems[i].Update(gameTime);
//                }
//            }
//        }
//        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse)
//        {
//            graphics.Clear(Color.Black);
//            if (Player.Health > 0)
//            {
//                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics));
//                Player.PlayerMovementAnimations.ShowRectangle = ShowBorders;


//                if (Player.CurrentAction.IsAnimating == false)
//                {
//                    Player.PlayerMovementAnimations.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y - 3), (float).4);
//                }

//                //????
//                if (Player.CurrentAction.IsAnimating == true)
//                {
//                    Player.CurrentAction.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y), (float).4);
//                }

//                if (ShowBorders)
//                {
//                    //    spriteBatch.Draw(Game1.Player.BigHitBoxRectangleTexture, Game1.Player.ClickRangeRectangle, Color.White);
//                }

//                for (int i = 0; i < AllStageTiles.Count; i++)
//                {
//                    AllStageTiles[i].DrawTiles(spriteBatch);
//                }

//                mouse.Draw(spriteBatch, 1);
//                Game1.userInterface.BottomBar.DrawDraggableItems(spriteBatch, BuildingsTiles, ForeGroundTiles, mouse);

//                if (Game1.userInterface.DrawTileSelector)
//                {
//                    spriteBatch.Draw(Game1.AllTextures.TileSelector, new Vector2(Game1.userInterface.TileSelectorX, Game1.userInterface.TileSelectorY), color: Color.White, layerDepth: .15f);
//                }

//                //--------------------------------------
//                //Draw sprite list

//                foreach (var sprite in AllSprites)
//                {
//                    sprite.ShowRectangle = ShowBorders;
//                    sprite.Draw(spriteBatch);
//                }

//                for (int i = 0; i < AllItems.Count; i++)
//                {
//                    AllItems[i].Draw(spriteBatch);
//                }

//                foreach (var obj in AllObjects)
//                {
//                    if (ShowBorders)
//                    {
//                        obj.Draw(spriteBatch, .4f);
//                    }
//                }

//                Game1.userInterface.BottomBar.DrawToStageMatrix(spriteBatch);
//                spriteBatch.End();
//            }
//            Game1.userInterface.Draw(spriteBatch);
//            Game1.GlobalClock.Draw(spriteBatch);
//        }
//        public Camera2D GetCamera()
//        {
//            return this.Cam;
//        }
//    }
    
//}
