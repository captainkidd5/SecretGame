
using System.Collections.Generic;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

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
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Universal;

namespace SecretProject.Class.StageFolder
{

    public class Sea : IStage
    {

        #region FIELDS

        public bool ShowBorders { get; set; }

        [XmlIgnore]
        public Vector2 TileSize = new Vector2(16, 16); // what?
        [XmlIgnore]

        public TmxMap Map { get; set; }

        [XmlIgnore]
        public Player Player { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        [XmlIgnore]
        public Texture2D TileSet { get; set; }

        public List<TileManager> AllStageTiles { get; set; }

        public TmxLayer Buildings { get; set; }

        public TmxLayer Background { get; set; }

        public TmxLayer Background1 { get; set; }

        public TmxLayer MidGround { get; set; }

        public TmxLayer foreGround { get; set; }

        public TmxLayer Placement { get; set; }


        [XmlIgnore]
        public Song MainTheme { get; set; }

        [XmlIgnore]
        public Camera2D Cam { get; set; }

        public int TileSetNumber { get; set; }

        //--------------------------------------
        //Declare Lists
        [XmlArray("AllObjects")]
        public List<ObjectBody> AllObjects { get; set; }

        public List<Sprite> AllSprites { get; set; }

        public List<Item> AllItems { get; set; }

        [XmlArray("AllActions")]
        public List<ActionTimer> AllActions { get; set; }

        //public List<object> ThingsToDraw;

        [XmlIgnore]
        public UserInterface MainUserInterface { get; set; }

        public List<TmxLayer> AllLayers { get; set; }

        public TileManager AllTiles { get; set; }

        //SAVE STUFF

        public bool TilesLoaded { get; set; } = false;

        public List<float> AllDepths;

        [XmlIgnore]
        public Elixir ElixerNPC;

        #endregion

        #region CONSTRUCTOR



        public Sea()
        {
        

            
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics, Camera2D camera, int tileSetNumber)
        {

            AllSprites = new List<Sprite>()
            {

            };

            AllObjects = new List<ObjectBody>()
            {

            };

            AllItems = new List<Item>()
            {

            };
            this.TileSetNumber = tileSetNumber;


            this.TileSet = content.Load<Texture2D>("Map/MasterSpriteSheet");



            //map specifications
            

            AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
                .6f
            };


           
            this.Map = new TmxMap("Content/Map/sea.tmx");
            Background = Map.Layers["background"];
            Buildings = Map.Layers["buildings"];
            MidGround = Map.Layers["midGround"];
            foreGround = Map.Layers["foreGround"];
            Placement = Map.Layers["placement"];
            AllLayers = new List<TmxLayer>()
            {
                Background,
                Buildings,
                MidGround,
                foreGround,
                Placement
            };
            AllTiles = new TileManager(TileSet, Map, AllLayers, graphics, content, TileSetNumber, AllDepths);
            TileWidth = Map.Tilesets[0].TileWidth;
            TileHeight = Map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            ElixerNPC = new Elixir("Elixer", new Vector2(800, 600), graphics);

            AllActions = new List<ActionTimer>();

            this.Cam = camera;
            Game1.cam.Zoom = 3f;
            Cam.Move(new Vector2(Game1.Player.Position.X, Game1.Player.Position.Y));
        }

        public void UnloadContent(ContentManager content)
        {

        }

        #endregion

        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {
            //keyboard
            Game1.myMouseManager.ToggleGeneralInteraction = false;

            Game1.userInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, Player.Inventory, mouse, game);

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                ShowBorders = !ShowBorders;
            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Y)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Y)))
            {
                ElixerNPC.IsUpdating = !ElixerNPC.IsUpdating;
            }

            if (!Game1.freeze)
            {
                // Game1.GlobalClock.Update(gameTime);
                //--------------------------------------
                //Update Players
                Game1.cam.Follow(new Vector2(Player.Position.X, Player.Position.Y));
                Player.Update(gameTime, AllItems, AllObjects);

                //--------------------------------------
                //Update sprites
                foreach (Sprite spr in AllSprites)
                {
                    if (spr.IsBeingDragged == true)
                    {
                        spr.Update(gameTime, mouse.WorldMousePosition);
                    }
                }

                AllTiles.Update(gameTime, mouse);
                //for(int i = 0; i < this.AllActions.Count; i++)
                //{
                //    AllActions[i].Update(gameTime, AllActions);
                //}

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Update(gameTime);
                }
                if (ElixerNPC.IsUpdating)
                {
                    ElixerNPC.Update(gameTime, mouse);
                    ElixerNPC.MoveTowardsPosition(Player.Position);
                }
                ElixerNPC.NPCAnimatedSprite[3].ShowRectangle = ShowBorders;

                if (Player.position.Y < 20 && Player.position.X < 810 && Player.position.X > 730)
                {
                    Player.Position = new Vector2(Player.position.X, 1540);
                    Game1.PreviousStage = 5;
                    this.TilesLoaded = false;
                    Game1.RoyalDock.AllTiles.LoadInitialTileObjects();
                    Game1.gameStages = Stages.Iliad;
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
                Player.PlayerMovementAnimations.ShowRectangle = ShowBorders;


                if (Player.CurrentAction.IsAnimating == false)
                {
                    Player.PlayerMovementAnimations.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y - 3), (float).4);
                }

                //????
                if (Player.CurrentAction.IsAnimating == true)
                {
                    Player.CurrentAction.Draw(spriteBatch, new Vector2(Player.Position.X, Player.Position.Y), (float).4);
                }


                ElixerNPC.Draw(spriteBatch);

                if (ShowBorders)
                {
                    //    spriteBatch.Draw(Game1.Player.BigHitBoxRectangleTexture, Game1.Player.ClickRangeRectangle, Color.White);
                }

                AllTiles.DrawTiles(spriteBatch);

                mouse.Draw(spriteBatch, 1);
                //Game1.userInterface.BottomBar.DrawDraggableItems(spriteBatch, BuildingsTiles, ForeGroundTiles, mouse);

                if (Game1.userInterface.DrawTileSelector)
                {
                    spriteBatch.Draw(Game1.AllTextures.TileSelector, new Vector2(Game1.userInterface.TileSelectorX, Game1.userInterface.TileSelectorY), color: Color.White, layerDepth: .15f);
                }

                //--------------------------------------
                //Draw sprite list

                foreach (var sprite in AllSprites)
                {
                    sprite.ShowRectangle = ShowBorders;
                    sprite.Draw(spriteBatch);
                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Draw(spriteBatch);
                }

                foreach (var obj in AllObjects)
                {
                    if (ShowBorders)
                    {
                        obj.Draw(spriteBatch, .4f);
                    }
                }

                ElixerNPC.Draw(spriteBatch);

                Game1.userInterface.BottomBar.DrawToStageMatrix(spriteBatch);
                spriteBatch.End();
            }
            Game1.userInterface.Draw(spriteBatch);
            //Game1.GlobalClock.Draw(spriteBatch);
        }
        public Camera2D GetCamera()
        {
            return this.Cam;
        }
        #endregion
    }
}