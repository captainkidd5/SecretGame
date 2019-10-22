
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
using SecretProject.Class.LightStuff;
using XMLData.RouteStuff;
using XMLData.ItemStuff;
using SecretProject.Class.CollisionDetection;

namespace SecretProject.Class.StageFolder
{

    public class TmxStageBase : ILocation
    {

        #region FIELDS
        public int StageIdentifier { get; set; }
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


        public Camera2D Cam { get; set; }

        public int TileSetNumber { get; set; }

        public Dictionary<string, ICollidable> AllObjects { get; set; }
        public Dictionary<string, Crop> AllCrops { get; set; }

        public List<Sprite> AllSprites { get; set; }

        public List<Item> AllItems { get; set; }

        public List<ActionTimer> AllActions { get; set; }

        public UserInterface MainUserInterface { get; set; }

        public List<TmxLayer> AllLayers { get; set; }




        public bool TilesLoaded { get; set; } = false;

        public List<float> AllDepths { get; set; }

        public Rectangle MapRectangle { get; set; }

        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public ParticleEngine ParticleEngine { get; set; }

        public DialogueHolder AllDockDialogue { get; set; }

        public TextBuilder TextBuilder { get; set; }
        public List<Portal> AllPortals { get; set; }

        public string MapTexturePath { get; set; }
        public string TmxMapPath { get; set; }
        public int DialogueToRetrieve { get; set; }
        public bool IsDark { get; set; }
        public List<LightSource> AllLights { get; set; }
        public string StageName { get; set; }
        public event EventHandler SceneChanged;

        public bool IsLoaded { get; set; }
        public ITileManager AllTiles { get; set; }

        public List<Character> CharactersPresent { get; set; }
        public int BackDropNumber { get; set; }
        public Vector2 BackDropPosition;

        public List<StringWrapper> AllTextToWrite { get; set; }

        public List<INPC> OnScreenNPCS { get; set; }
        public QuadTree QuadTree { get; set; }
        public List<float> MyProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        



        #endregion

        #region CONSTRUCTOR



        public TmxStageBase(string name, GraphicsDevice graphics, ContentManager content, int tileSetNumber, string mapTexturePath, string tmxMapPath, int dialogueToRetrieve, int backDropNumber)
        {
            this.StageName = name;
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;
            this.MapTexturePath = mapTexturePath;
            this.TmxMapPath = tmxMapPath;
            this.DialogueToRetrieve = dialogueToRetrieve;
            this.IsLoaded = false;
            this.BackDropNumber = backDropNumber;
            CharactersPresent = new List<Character>();
            if (this.BackDropNumber == 1)
            {
                this.BackDropPosition = new Vector2(0, 50);
            }

            this.OnScreenNPCS = new List<INPC>();
        }

        public virtual void LoadPreliminaryContent()
        {
            AllLights = new List<LightSource>()
            {

            };

            AllSprites = new List<Sprite>()
            {

            };

            AllObjects = new Dictionary<string, ICollidable>()
            {

            };

            AllItems = new List<Item>()
            {

            };

            //AllItems.Add(Game1.ItemVault.GenerateNewItem(147, new Vector2(Game1.Player.Position.X + 50, Game1.Player.Position.Y + 100), true));

            this.TileSet = Content.Load<Texture2D>(this.MapTexturePath);


            AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
            };

            this.Map = new TmxMap(this.TmxMapPath);
            Background = Map.Layers["background"];
            Buildings = Map.Layers["buildings"];
            MidGround = Map.Layers["midGround"];
            foreGround = Map.Layers["foreGround"];
            AllLayers = new List<TmxLayer>()
            {
                Background,
                Buildings,
                MidGround,
                foreGround
    
            };
            AllPortals = new List<Portal>();
            AllTiles = new TileManager(TileSet, Map, AllLayers, Graphics, Content, TileSetNumber, AllDepths, this);
            AllTiles.LoadInitialTileObjects(this);
            TileWidth = Map.Tilesets[TileSetNumber].TileWidth;
            TileHeight = Map.Tilesets[TileSetNumber].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            AllActions = new List<ActionTimer>();

            MapRectangle = new Rectangle(0, 0, TileWidth * Map.Width, TileHeight * Map.Height);
            Map = null;
            AllCrops = new Dictionary<string, Crop>();


            //Sprite KayaSprite = new Sprite(graphics, Kaya, new Rectangle(0, 0, 16, 32), new Vector2(400, 400), 16, 32);
            this.QuadTree = new QuadTree(5, MapRectangle);
        }

        public virtual void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);



            this.Cam = camera;
            Cam.Zoom = 3f;
            Cam.pos.X = Game1.Player.position.X;
            Cam.pos.Y = Game1.Player.position.Y;


            Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(this.DialogueToRetrieve, Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours).TextToWrite;

            TextBuilder = new TextBuilder(Game1.DialogueLibrary.RetrieveDialogue(this.DialogueToRetrieve, Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours).TextToWrite, .1f, 5f);
            this.SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;

            this.AllTextToWrite = new List<StringWrapper>();
            
            this.IsLoaded = true;

        }
        public void OnSceneChanged()
        {
            if (SceneChanged != null)
            {
                SceneChanged(this, EventArgs.Empty);
            }
        }

        public virtual void UnloadContent()
        {
            //Content.Unload();
            //AllObjects = null;
            //AllLayers = null;
            //AllTiles = null;
            //AllSprites = null;
            //AllDepths = null;
            //AllItems = null;
            //Background = null;
            //MidGround = null;
            //foreGround = null;

            //this.Cam = null;
            // this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;

        }


        #endregion

        #region UPDATE
        public virtual void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            player.CollideOccured = false;
            QuadTree = new QuadTree(5, MapRectangle);

            foreach (var obj in AllTiles.CurrentObjects.Values)
            {
                QuadTree.Insert(obj);
            }

            QuadTree.Insert(player.MyCollider);

            foreach (Character character in CharactersPresent)
            {
                QuadTree.Insert(character.Collider);
            }
            this.IsDark = Game1.GlobalClock.IsNight;
            float playerOldYPosition = player.position.Y;
            for (int p = 0; p < AllPortals.Count; p++)
            {
                if (player.ClickRangeRectangle.Intersects(AllPortals[p].PortalStart) && AllPortals[p].MustBeClicked)
                {
                    if (mouse.WorldMouseRectangle.Intersects(AllPortals[p].PortalStart) && mouse.IsClicked)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                        Game1.SwitchStage(AllPortals[p].From, AllPortals[p].To, AllPortals[p]);
                        OnSceneChanged();
                        this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                        return;
                    }

                }
                else if (player.Rectangle.Intersects(AllPortals[p].PortalStart) && !AllPortals[p].MustBeClicked)
                {
                    Game1.SwitchStage(AllPortals[p].From, AllPortals[p].To, AllPortals[p]);
                    OnSceneChanged();
                    this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                    return;
                }
            }

            Game1.myMouseManager.ToggleGeneralInteraction = false;

            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                ShowBorders = !ShowBorders;
            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F2)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F2)))
            {

                player.Position = new Vector2(400, 400);
                Game1.GlobalClock.TotalHours = 22;

            }

            TextBuilder.Update(gameTime);
            ParticleEngine.Update(gameTime);

            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);

                Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), MapRectangle);
                player.Update(gameTime, AllItems, mouse);

 
                foreach (Sprite spr in AllSprites)
                {

                    spr.Update(gameTime, mouse.WorldMousePosition);

                }

                AllTiles.Update(gameTime, mouse);
                for (int s = 0; s < AllTextToWrite.Count; s++)
                {
                    AllTextToWrite[s].Update(gameTime, AllTextToWrite);
                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Update(gameTime);
                }
                foreach (Character character in Game1.AllCharacters)
                {
                    character.Update(gameTime, mouse);
                }
                if (this.BackDropNumber == 1)
                {
                    if (player.position.Y < 250)
                    {
                        this.BackDropPosition.Y += ((player.position.Y - playerOldYPosition) / 4);
                    }

                }

            }
        }
        #endregion

        #region DRAW
        public virtual void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            if (player.Health > 0)
            {
                if (this.IsDark)
                {


                    graphics.SetRenderTarget(lightsTarget);
                    //graphics.Clear(Color.White);
                    graphics.Clear(new Color(50, 50, 50, 220));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int l = 0; l < this.AllLights.Count; l++)
                    {
                        spriteBatch.Draw(AllLights[l].LightTexture, AllLights[l].Position, Color.White);
                    }
                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 4)
                    {
                        spriteBatch.Draw(Game1.AllTextures.lightMask, new Vector2(mouse.WorldMousePosition.X - Game1.AllTextures.lightMask.Width / 2, mouse.WorldMousePosition.Y - Game1.AllTextures.lightMask.Height / 2), Color.White);
                    }
                    spriteBatch.End();
                }


                graphics.SetRenderTarget(mainTarget);
                graphics.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics));

                graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                if (this.BackDropNumber == 1)
                {
                    spriteBatch.Draw(Game1.AllTextures.WildernessBackdrop, this.BackDropPosition, null, Color.White, 0f, Game1.Utility.Origin, .5f, SpriteEffects.None, .0001f);
                }
                ParticleEngine.Draw(spriteBatch, 1f);

                player.Draw(spriteBatch, .5f + (player.Rectangle.Y + player.Rectangle.Bottom) * .00000001f);
                TextBuilder.Draw(spriteBatch, .71f);

                if (ShowBorders)
                {
                    foreach (Character character in CharactersPresent)
                    {
                        character.DrawDebug(spriteBatch, .4f);
                    }
                }

                AllTiles.DrawTiles(spriteBatch);
                for (int s = 0; s < AllTextToWrite.Count; s++)
                {
                    AllTextToWrite[s].Draw(spriteBatch);
                }
                foreach (Character character in CharactersPresent)
                {
                    character.Draw(spriteBatch);
                }

                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    Game1.Player.UserInterface.TileSelector.Draw(spriteBatch);
                }



                foreach (var sprite in AllSprites)
                {
                    sprite.Draw(spriteBatch, .7f);
                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Draw(spriteBatch);
                }

                foreach (var obj in AllTiles.Objects.Values)
                {
                    if (ShowBorders)
                    {
                        obj.Draw(spriteBatch, .4f);
                    }
                }

                Game1.Player.UserInterface.BottomBar.DrawToStageMatrix(spriteBatch);

                spriteBatch.End();

                graphics.SetRenderTarget(null);



                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (IsDark)
                {
                    Game1.AllTextures.practiceLightMaskEffect.Parameters["lightMask"].SetValue(lightsTarget);
                    Game1.AllTextures.practiceLightMaskEffect.CurrentTechnique.Passes[0].Apply();
                }

                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);

                spriteBatch.End();
            }
            Game1.Player.DrawUserInterface(spriteBatch);
            Game1.GlobalClock.Draw(spriteBatch);

        }

        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            this.AllTextToWrite.Add(new StringWrapper(message, position, endAtX, endAtY, rate, duration));
        }
        public Camera2D GetCamera()
        {
            return this.Cam;
        }
        #endregion
    }
}