
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

    public class StageBase : IStage
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

        public TmxLayer Buildings { get; set; }

        public TmxLayer Background { get; set; }

        public TmxLayer Background1 { get; set; }

        public TmxLayer MidGround { get; set; }

        public TmxLayer foreGround { get; set; }

        public TmxLayer Placement { get; set; }

        public Camera2D Cam { get; set; }

        public int TileSetNumber { get; set; }

        public List<ObjectBody> AllObjects { get; set; }

        public List<Sprite> AllSprites { get; set; }

        public List<Item> AllItems { get; set; }

        public List<ActionTimer> AllActions { get; set; }

        public UserInterface MainUserInterface { get; set; }

        public List<TmxLayer> AllLayers { get; set; }

        public TileManager AllTiles { get; set; }


        public bool TilesLoaded { get; set; } = false;

        public List<float> AllDepths;

        public Rectangle MapRectangle { get; set; }

        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public ParticleEngine ParticleEngine { get; set; }

        public DialogueHolder AllDockDialogue { get; set; }

        public TextBuilder TextBuilder { get; set; }
        public List<Portal> AllPortals { get; set; }

        #endregion

        #region CONSTRUCTOR



        public StageBase(GraphicsDevice graphics, ContentManager content, int tileSetNumber)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;


        }

        public void LoadContent(Camera2D camera, string mapTexturePath, string tmxMapPath, int dialogueToRetrieve)
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

            this.TileSet = Content.Load<Texture2D>(mapTexturePath);


            AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
                .6f
            };

            this.Map = new TmxMap(tmxMapPath);
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
            AllPortals = new List<Portal>();
            AllTiles = new TileManager(TileSet, Map, AllLayers, Graphics, Content, TileSetNumber, AllDepths);
            AllTiles.LoadInitialTileObjects();
            TileWidth = Map.Tilesets[TileSetNumber].TileWidth;
            TileHeight = Map.Tilesets[TileSetNumber].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            AllActions = new List<ActionTimer>();

            this.Cam = camera;
            Cam.Zoom = 2f;
            MapRectangle = new Rectangle(0, 0, TileWidth * Map.Width, TileHeight * Map.Height);
            Map = null;

            Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(dialogueToRetrieve);

            TextBuilder = new TextBuilder(Game1.DialogueLibrary.RetrieveDialogue(dialogueToRetrieve), .1f, 5f);


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
            for (int p = 0; p < AllPortals.Count; p++)
            {
                if (player.Rectangle.Intersects(AllPortals[p].PortalStart))
                {
                    Game1.SwitchStage(AllPortals[p].From, AllPortals[p].To, AllPortals[p]);
                    return;
                }
            }

            Game1.myMouseManager.ToggleGeneralInteraction = false;

            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                ShowBorders = !ShowBorders;
            }


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

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Update(gameTime);
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
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY, 16, 16),
                        new Rectangle(48, 0, 16, 16), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);
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