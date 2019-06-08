
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
using SecretProject.Class.ParticileStuff;
using XMLData.DialogueStuff;
using SecretProject.Class.DialogueStuff;

namespace SecretProject.Class.StageFolder
{

    public class RoyalDock : IStage
    {

        #region FIELDS

        public bool ShowBorders { get; set; }

        [XmlIgnore]
        public Vector2 TileSize = new Vector2(16, 16); // what?
        [XmlIgnore]

        public TmxMap Map { get; set; }

        [XmlIgnore]
        public Player player { get; set; }

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

        public Rectangle MapRectangle { get; set; }

        [XmlIgnore]
        public Elixir ElixerNPC;
        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public ParticleEngine ParticleEngine { get; set; }

        public DialogueHolder AllDockDialogue { get; set; }

        public TextBuilder TextBuilder { get; set; }

        #endregion

        #region CONSTRUCTOR



        public RoyalDock(GraphicsDevice graphics,ContentManager content, int tileSetNumber)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;


        }

        public void LoadContent( Camera2D camera )
        {
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);

            AllSprites = new List<Sprite>()
            {

            };

            AllObjects = new List<ObjectBody>()
            {

            };

            AllItems = new List<Item>()
            {

            };

            AllItems.Add(Game1.ItemVault.GenerateNewItem(147, new Vector2(Game1.Player.Position.X + 50, Game1.Player.Position.Y + 100), true));

            this.TileSet = Content.Load<Texture2D>("Map/MasterSpriteSheet");



            //map specifications


            AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
                .6f
            };



            this.Map = new TmxMap("Content/Map/royalDocks.tmx");
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
            AllTiles = new TileManager(TileSet, Map, AllLayers, Graphics, Content, TileSetNumber, AllDepths);
            AllTiles.LoadInitialTileObjects();
            TileWidth = Map.Tilesets[0].TileWidth;
            TileHeight = Map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            ElixerNPC = new Elixir("Elixer", new Vector2(800, 600), Graphics);

            AllActions = new List<ActionTimer>();

            this.Cam = camera;
            Cam.Zoom = 2f;
            MapRectangle = new Rectangle(0, 0, TileWidth * 100, TileHeight * 100);
            Map = null;

            AllItems.Add(Game1.ItemVault.GenerateNewItem(129, new Vector2(500, 500), true));
            //AllDockDialogue = Content.Load<DialogueHolder>("Dialogue/AllDialogue");
            Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(2);

            TextBuilder = new TextBuilder(Game1.DialogueLibrary.RetrieveDialogue(2), .25f, 5f);
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
            MidGround = null;
            foreGround = null;
            Placement = null;

            this.Cam = null;

        }


        #endregion

        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            Game1.Player.UserInterface.TextBuilder.PositionToWriteTo = ElixerNPC.Position;
            //keyboard
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.O)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.O)))
            {
                Game1.SwitchStage(5, 4);
                return;
            }
            Game1.myMouseManager.ToggleGeneralInteraction = false;

            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                ShowBorders = !ShowBorders;
            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Y)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Y)))
            {
                ElixerNPC.IsUpdating = !ElixerNPC.IsUpdating;
                //TextBuilder.IsActive = !TextBuilder.IsActive;
                //ParticleEngine.ActivationTime = 5f;
                //ParticleEngine.InvokeParticleEngine(gameTime, 20, mouse.WorldMousePosition);
            }

            TextBuilder.PositionToWriteTo = ElixerNPC.Position;
            TextBuilder.Update(gameTime);

            //ParticleEngine.EmitterLocation = mouse.WorldMousePosition;
            ParticleEngine.Update(gameTime);

            if (!Game1.freeze)
            {
                 Game1.GlobalClock.Update(gameTime);
                //--------------------------------------
                //Update players
                Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), MapRectangle);
                player.Update(gameTime, AllItems, AllObjects, mouse);

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
                    ElixerNPC.Update(gameTime,AllObjects, mouse);
                    ElixerNPC.MoveTowardsPosition(player.Position, player.Rectangle);
                }
                //ElixerNPC.NPCAnimatedSprite[3].ShowRectangle = ShowBorders;

                if (player.position.Y < 20 && player.position.X < 810 && player.position.X > 730)
                {
                    player.Position = new Vector2(player.position.X, 1550);
                    Game1.SwitchStage(5, 2);
                    return;
                }
            }
        }
        #endregion

        #region DRAW
        public void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
           // graphics.Clear(Color.Black);
            if (player.Health > 0)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics));
                //player.PlayerMovementAnimations.ShowRectangle = ShowBorders;
                ParticleEngine.Draw(spriteBatch, 1f);

                player.Draw(spriteBatch, .4f);


                ElixerNPC.Draw(spriteBatch);
                TextBuilder.Draw(spriteBatch, .71f);

                if (ShowBorders)
                {
                    //    spriteBatch.Draw(Game1.player.BigHitBoxRectangleTexture, Game1.player.ClickRangeRectangle, Color.White);
                }

                AllTiles.DrawTiles(spriteBatch);

                mouse.Draw(spriteBatch, 1);
                //Game1.userInterface.BottomBar.DrawDraggableItems(spriteBatch, BuildingsTiles, ForeGroundTiles, mouse);

                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    spriteBatch.Draw(Game1.AllTextures.TileSelector, new Vector2(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY), color: Color.White, layerDepth: .15f);
                }

                //--------------------------------------
                //Draw sprite list

                foreach (var sprite in AllSprites)
                {
                    //sprite.ShowRectangle = ShowBorders;
                    sprite.Draw(spriteBatch, .7f);
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

                Game1.Player.UserInterface.BottomBar.DrawToStageMatrix(spriteBatch);
                spriteBatch.End();
            }
            Game1.Player.DrawUserInterface(spriteBatch);
            Game1.GlobalClock.Draw(spriteBatch);
        }
        public Camera2D GetCamera()
        {
            return this.Cam;
        }
        #endregion
    }
}