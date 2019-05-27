
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

        public Vector2 TileSize = new Vector2(16, 16); // what?


        public TmxMap Map { get; set; }


        public Player player { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }

        public Texture2D TileSet { get; set; }

        public List<TileManager> AllStageTiles { get; set; }

        public TmxLayer underWater { get; set; }
        public TmxLayer Buildings { get; set; }

        public TmxLayer Background { get; set; }

        

        public TmxLayer foreGround { get; set; }



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

        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public Elixir ElixerNPC;

        #endregion

        #region CONSTRUCTOR



        public Sea(GraphicsDevice graphics,ContentManager content, int tileSetNumber)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;
        }

        public void LoadContent(Camera2D camera)
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



            this.TileSet = Content.Load<Texture2D>("Map/MasterSpriteSheet");

            AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .5f,
                .6f
            };

            this.Map = new TmxMap("Content/Map/sea.tmx");
            Background = Map.Layers["background"];
            Buildings = Map.Layers["buildings"];
            foreGround = Map.Layers["foreGround"];
            underWater = Map.Layers["underWater"];
            AllLayers = new List<TmxLayer>()
            {
                Background,
                Buildings,

                foreGround,
                underWater
            };
            AllTiles = new TileManager(TileSet, Map, AllLayers, Graphics, Content, TileSetNumber, AllDepths);
            TileWidth = Map.Tilesets[0].TileWidth;
            TileHeight = Map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;
            AllTiles.LoadInitialTileObjects();

            ElixerNPC = new Elixir("Elixer", new Vector2(800, 600), Graphics);

            AllActions = new List<ActionTimer>();

            this.Cam = camera;
            Cam.Zoom = 3f;

            this.Map = null;
        }

        public void UnloadContent()
        {
            Content.Unload();
            AllObjects = null; 
            AllLayers = null;
            AllTiles = null;
            AllSprites = null;
            AllDepths = null;
            AllItems = null;
            Background = null;
            foreGround = null;
            underWater = null;
            this.Cam = null;
            ElixerNPC = null;
        }

        #endregion

        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            //keyboard
            Game1.myMouseManager.ToggleGeneralInteraction = false;

            Game1.userInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);

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
                //Update players
                Cam.Follow(new Vector2(player.Position.X, player.Position.Y));
                player.Update(gameTime, AllItems, AllObjects);

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
                    ElixerNPC.MoveTowardsPosition(player.Position);
                }
                ElixerNPC.NPCAnimatedSprite[3].ShowRectangle = ShowBorders;

                if (player.position.Y < 20 && player.position.X < 810 && player.position.X > 730)
                {
                    player.Position = new Vector2(player.position.X, 1540);
                    Game1.PreviousStage = 5;
                    this.TilesLoaded = false;
                    Game1.RoyalDock.AllTiles.LoadInitialTileObjects();
                    Game1.gameStages = Stages.Iliad;
                }
            }
        }
        #endregion

        #region DRAW
        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            graphics.Clear(Color.Black);
            if (player.Health > 0)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics));
                player.PlayerMovementAnimations.ShowRectangle = ShowBorders;


                if (player.CurrentAction.IsAnimating == false)
                {
                    player.PlayerMovementAnimations.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y - 3), (float).4);
                }

                //????
                if (player.CurrentAction.IsAnimating == true)
                {
                    player.CurrentAction.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y), (float).4);
                }


                ElixerNPC.Draw(spriteBatch);

                if (ShowBorders)
                {
                    //    spriteBatch.Draw(Game1.player.BigHitBoxRectangleTexture, Game1.player.ClickRangeRectangle, Color.White);
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