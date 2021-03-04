
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.Misc;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.Playable;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using TiledSharp;
using VelcroPhysics.Dynamics;
using XMLData.DialogueStuff;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public enum LocationType
    {

        Exterior = 0,
        Interior = 1
    }

    public class Stage : Component
    {

        #region FIELDS


        public string Name { get; private set; }

        public LocationType LocationType { get; set; }
        public StagesEnum StageIdentifier { get; set; }
        public bool ShowBorders { get; set; }

        public TmxMap Map { get; set; }

        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }

        public Texture2D TileSet { get; set; }

        public List<TileManager> AllStageTiles { get; set; }



        public Camera2D Cam { get; set; }



        public Dictionary<string, Body> AllObjects { get; set; }
        public Dictionary<string, Crop> AllCrops { get; set; }

        private EnemyManager EnemyManager { get; set; }

        public List<Sprite> AllSprites { get; set; }


        public List<ActionTimer> AllActions { get; set; }

        public UserInterface MainUserInterface { get; set; }



        public Rectangle MapRectangle { get; set; }

        public ContentManager StageContentManager { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public static ParticleEngine ParticleEngine { get; set; }

        public TextBuilder TextBuilder { get; set; }
        public List<Portal> AllPortals { get; set; }

        public int DialogueToRetrieve { get; set; }


        public bool IsDark { get; set; }
        public List<LightSource> AllNightLights { get; set; }
        public List<LightSource> AllDayTimeLights { get; set; }
        public StageManager StageManager { get; private set; }

        public string StageName { get; set; }
        public event EventHandler SceneChanged;

        public bool IsLoaded { get; set; }
        public TileManager AllTiles { get; set; }

        public List<Character> CharactersPresent { get; set; }


        public List<StringWrapper> AllTextToWrite { get; set; }

        public List<INPC> OnScreenNPCS { get; set; }

        public List<RisingText> AllRisingText { get; set; }

        public SanctuaryTracker SanctuaryTracker { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Projectile> AllProjectiles { get; set; }
        public List<ParticleEngine> ParticleEngines { get; set; }

        //SAVE STUFF
        public string SavePath { get; set; }

        public NPCGenerator NPCGenerator { get; set; }
        public FunBox FunBox { get; set; }

        protected bool IsBasedOnPreloadedMap { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }
        public PlayerManager PlayerManager { get; }
        public CharacterManager CharacterManager { get; }
        public PenumbraComponent Penumbra { get; set; }

        public Dictionary<string, Hull> Hulls { get; set; }
        public Dictionary<string, Light> Lights { get; set; }

        public List<IDebuggableShape> DebuggableShapes { get; set; }

        public List<GrassTuft> UpdatingGrassTufts { get; set; }

        private Player Player { get; set; }
        #endregion

        #region CONSTRUCTOR



        public Stage(StageManager stageManager, string name, LocationType locationType, GraphicsDevice graphicsDevice, ContentManager content, Texture2D tileSet, TmxMap tmxMap,
             IServiceProvider service, PlayerManager playerManager, CharacterManager characterManager, bool isBasedOnPreloadedMap = true) : base(graphicsDevice,  content)
        {
            StageManager = stageManager;
            this.StageName = name;
            this.LocationType = locationType;


            this.StageContentManager = new ContentManager(content.ServiceProvider);
            this.StageContentManager.RootDirectory = "Content";
            this.IsLoaded = false;

            this.CharactersPresent = new List<Character>();


            this.OnScreenNPCS = new List<INPC>();
            this.TileSet = tileSet;
            this.AllRisingText = new List<RisingText>();

            this.Enemies = new List<Enemy>();
            this.AllProjectiles = new List<Projectile>();
            this.Map = tmxMap;

            LoadPreliminaryContent();
            this.IsBasedOnPreloadedMap = isBasedOnPreloadedMap;
            this.ServiceProvider = service;
            PlayerManager = playerManager;
            CharacterManager = characterManager;
            Penumbra = (PenumbraComponent)ServiceProvider.GetService(typeof(PenumbraComponent));

            this.Lights = new Dictionary<string, Light>();
            this.Hulls = new Dictionary<string, Hull>();

            this.DebuggableShapes = new List<IDebuggableShape>();
            this.UpdatingGrassTufts = new List<GrassTuft>();
            this.EnemyManager = new EnemyManager(Graphics, content);

            this.Player = PlayerManager.Player;
        }
        public Stage(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
        }

        public override void Load()
        {
            EnemyManager.Load();
        }

        public override void Unload()
        {
            EnemyManager.Unload();
        }

        public virtual void AssignPath(string startPath)
        {
            this.SavePath = startPath + "/GameLocations/" + this.StageName;

            if (File.Exists(this.SavePath))
            {

            }
            else
            {
                File.WriteAllText(this.SavePath, string.Empty);
            }


        }

        public virtual void LoadPreliminaryContent()
        {
            this.AllNightLights = new List<LightSource>()
            {

            };


            this.AllDayTimeLights = new List<LightSource>();

            this.AllSprites = new List<Sprite>()
            {

            };

            this.AllObjects = new Dictionary<string, Body>()
            {

            };

            this.AllPortals = new List<Portal>();
            LoadTileManager();
            Tile.TileWidth = this.Map.Tilesets[(int)this.LocationType].TileWidth;
            Tile.TileWidth = this.Map.Tilesets[(int)this.LocationType].TileHeight;

            this.TilesetTilesWide = this.TileSet.Width / Tile.TileWidth;
            this.TilesetTilesHigh = this.TileSet.Height / Tile.TileWidth;


            this.AllActions = new List<ActionTimer>();

            this.MapRectangle = new Rectangle(0, 0, Tile.TileWidth * this.Map.Width, Tile.TileWidth * this.Map.Height);
            this.Map = null;
            this.AllCrops = new Dictionary<string, Crop>();

            this.FunBox = new FunBox(this.Graphics);



        }

        protected virtual void LoadTileManager()
        {
            this.AllTiles = new TileManager(this.TileSet, this.Map, this.Graphics, this.StageContentManager, 0, this); //tileset should always be zero!
            if (!Game1.AreGeneratableTilesLoaded)
            {
                this.AllTiles.LoadGeneratableTileLists(); //just here so it only happens once!
                Game1.AreGeneratableTilesLoaded = true;
            }
        }

        public virtual void StartNew()
        {
            this.AllTiles.StartNew(this.IsBasedOnPreloadedMap);
        }

        public virtual void LoadContent()
        {
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);

            this.Cam.pos.X = Game1.Player.position.X;
            this.Cam.pos.Y = Game1.Player.position.Y;


            // Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue("", Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours).TextToWrite;

            this.TextBuilder = new TextBuilder("", .1f, 5f);
            SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;

            this.AllTextToWrite = new List<StringWrapper>();
            this.NPCGenerator = new NPCGenerator((TileManager)this.AllTiles, this.Graphics);
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
            File.WriteAllText(this.SavePath, string.Empty);

            FileStream fileStream = File.OpenWrite(this.SavePath);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);








            Save(binaryWriter);
            binaryWriter.Flush();
            binaryWriter.Close();

            AllTiles.Unload();
            this.Lights.Clear();
            this.Hulls.Clear();
            this.UpdatingGrassTufts.Clear();



            // this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;

        }

        public virtual void TryLoadExistingStage()
        {
            if (new FileInfo(this.SavePath).Length > 0)
            {
                FileStream fileStream = File.OpenRead(this.SavePath);
                BinaryReader binaryReader = new BinaryReader(fileStream);

                Load(binaryReader);

                binaryReader.Close();
            }
            else
            {
                AllTiles.StartNew(true);
            }
        }


        #endregion


        #region UPDATE

        public virtual void UpdatePortals()
        {
            for (int p = 0; p < this.AllPortals.Count; p++)
            {
                if (Player.ClickRangeRectangle.Intersects(this.AllPortals[p].PortalStart))
                {
                    if (this.AllPortals[p].MustBeClicked)
                    {
                        if (Game1.MouseManager.WorldMouseRectangle.Intersects(this.AllPortals[p].PortalStart) && Game1.MouseManager.IsClicked)
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpen);
                            StageManager.SwitchStage(Game1.GetStageFromEnum((StagesEnum)this.AllPortals[p].To), this.AllPortals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }
                    }
                    else
                    {
                        if (Player.Rectangle.Intersects(this.AllPortals[p].PortalStart))
                        {
                            StageManager.SwitchStage(Game1.GetStageFromEnum((StagesEnum)this.AllPortals[p].To), this.AllPortals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }

                    }
                }

            }
        }
        public virtual void Update(GameTime gameTime)
        {
            PlayerManager.Update(gameTime);

            EnemyManager.Update(gameTime);


            this.IsDark = Game1.GlobalClock.IsNight;

            UpdatePortals();

            Game1.MouseManager.ToggleGeneralInteraction = false;

            if (Game1.CurrentWeather != WeatherType.None)
            {
                Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, this.LocationType);
            }


            Game1.Player.UserInterface.Update(gameTime, Player.Inventory);
            if (StageManager.CurrentStage != this)
            {
                return;
            }
            if (Game1.KeyboardManager.WasKeyPressed(Keys.F1))
            {
                this.ShowBorders = !this.ShowBorders;
            }
            if (Game1.KeyboardManager.WasKeyPressed(Keys.F2))
            {

                Player.Position = new Vector2(400, 400);
                Game1.GlobalClock.TotalHours = 22;

            }
            DrawPenumbra(gameTime);
            this.FunBox.Update(gameTime);
            this.TextBuilder.Update(gameTime);
            ParticleEngine.Update(gameTime);
            for(int i =0; i < this.UpdatingGrassTufts.Count;i++)
            {
                this.UpdatingGrassTufts[i].Update(gameTime, UpdatingGrassTufts);
            }
            for (int p = 0; p < AllProjectiles.Count; p++)
            {
                AllProjectiles[p].Update(gameTime);
            }
            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);
                Game1.Train.Update(gameTime);
                // 
                PlayerManager.Update(gameTime);
                this.Cam.Follow(new Vector2(Player.PlayerCamPos.X + 8, Player.PlayerCamPos.Y + 16), this.MapRectangle);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Update(gameTime, this.AllRisingText);
                }

                foreach (Sprite spr in this.AllSprites)
                {

                    spr.Update(gameTime, Game1.MouseManager.WorldMousePosition);

                }

                this.AllTiles.Update(gameTime);
                for (int s = 0; s < this.AllTextToWrite.Count; s++)
                {
                    this.AllTextToWrite[s].Update(gameTime, this.AllTextToWrite);
                }

                CharacterManager.Update(gameTime);



            }
        }
        #endregion

        protected virtual void BeginPenumbra()
        {
            if (IsDark)
            {
                Game1.DrawPenumbra = true;
                Penumbra.AmbientColor = Color.DarkSlateGray;
                Penumbra.BeginDraw();
            }

        }

        protected virtual void DrawPenumbra(GameTime gameTime)
        {
            if (IsDark)
            {
              
                Penumbra.Transform = Game1.cam.getTransformation(this.Graphics);
                Penumbra.Draw(gameTime);
            }

        }

        #region DRAW
        public virtual void Draw(SpriteBatch spriteBatch, RenderTarget2D mainTarget, RenderTarget2D nightLightsTarget, RenderTarget2D dayLightsTarget)
        {
            //Game1.Penumbra.Hulls.Clear();
                if (this.IsDark)
                {


                graphicsDevice.SetRenderTarget(nightLightsTarget);
                //graphics.Clear(Color.White);
                graphicsDevice.Clear(new Color(50, 50, 50, 220));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: this.Cam.getTransformation(graphicsDevice));
                graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int l = 0; l < this.AllNightLights.Count; l++)
                    {
                        spriteBatch.Draw(this.AllNightLights[l].LightTexture, this.AllNightLights[l].Position, Color.White);
                    }
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() == 4)
                    {
                        spriteBatch.Draw(Game1.AllTextures.lightMask, new Vector2(Game1.MouseManager.WorldMousePosition.X - Game1.AllTextures.lightMask.Width / 2, Game1.MouseManager.WorldMousePosition.Y - Game1.AllTextures.lightMask.Height / 2), Color.White);
                    }
                    spriteBatch.End();
                }

            graphicsDevice.SetRenderTarget(mainTarget);


            graphicsDevice.Clear(Color.Transparent);

                BeginPenumbra();




                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: this.Cam.getTransformation(graphicsDevice));

                graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                if (Game1.CurrentWeather != WeatherType.None)
                {
                    Game1.AllWeather[Game1.CurrentWeather].Draw(spriteBatch, this.LocationType); //1471
                }

                ParticleEngine.Draw(spriteBatch);
            EnemyManager.Draw(spriteBatch);

                for (int p = 0; p < this.AllProjectiles.Count; p++)
                {
                    AllProjectiles[p].Draw(spriteBatch);
                }
                Game1.Train.Draw(spriteBatch);
            PlayerManager.Draw(spriteBatch);
              
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Draw(spriteBatch);
                }
                this.TextBuilder.Draw(spriteBatch, .71f);

                if (this.ShowBorders)
                {
                    foreach (Character character in this.CharactersPresent)
                    {
                        character.DrawDebug(spriteBatch, .4f);
                    }

                    for (int i = 0; i < this.DebuggableShapes.Count; i++)
                    {
                        DebuggableShapes[i].Draw(spriteBatch);
                    }
                }
                //for(int v =0; v < Game1.VelcroWorld.BodyList.Count; v++)
                //{
                //    Game1.VelcroWorld.BodyList[v].
                //}
                this.AllTiles.DrawTiles(spriteBatch);
                for (int s = 0; s < this.AllTextToWrite.Count; s++)
                {
                    this.AllTextToWrite[s].Draw(spriteBatch);
                }
            CharacterManager.Draw(spriteBatch);

                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    Game1.Player.UserInterface.TileSelector.Draw(spriteBatch);
                }



                foreach (var sprite in this.AllSprites)
                {
                    sprite.Draw(spriteBatch, .7f);
                }

                this.FunBox.Draw(spriteBatch);



                Game1.Player.UserInterface.BackPack.DrawToStageMatrix(spriteBatch);
                if (Game1.MouseManager.EnableBody)
                {
                    Game1.MouseManager.DrawDebug(spriteBatch);
                }
                spriteBatch.End();


            graphicsDevice.SetRenderTarget(null);



                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                //if (this.IsDark)
                //{
                //    Game1.AllTextures.practiceLightMaskEffect.Parameters["lightMask"].SetValue(nightLightsTarget);
                //    Game1.AllTextures.practiceLightMaskEffect.CurrentTechnique.Passes[0].Apply();
                //}
                
                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);
                if (Game1.KeyboardManager.WasKeyPressed(Keys.PageDown))
                {
                    Game1.Player.UserInterface.CommandConsole.TakeScreenShot(mainTarget);
                }
                
                spriteBatch.End();
            
            Game1.Player.DrawUserInterface(spriteBatch);

        }

        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            this.AllTextToWrite.Add(new StringWrapper(message, position, endAtX, endAtY, rate, duration));
        }


        public void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade, float scale)
        {
            this.AllRisingText.Add(new RisingText(Game1.Player.Position, yEnd, stringToWrite, speed, color, fade, scale));
        }
        #endregion
        public void SaveLocation()
        {

        }

        public virtual void Save(BinaryWriter writer)
        {

            this.AllTiles.Save(writer);
            writer.Write(this.SavePath);
        }

        public virtual void Load(BinaryReader reader)
        {

            this.AllTiles.Load(reader);
            this.SavePath = reader.ReadString();


        }

        public virtual string GetDebugString()
        {
            throw new NotImplementedException();
        }

        
    }

}