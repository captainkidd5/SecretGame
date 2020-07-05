
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
using XMLData.DialogueStuff;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{


    public class TmxStageBase : ILocation
    {

        #region FIELDS
        public LocationType LocationType { get; set; }
        public StageType StageType { get; set; }
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



        public Camera2D Cam { get; set; }



        public Dictionary<string, ICollidable> AllObjects { get; set; }
        public Dictionary<string, Crop> AllCrops { get; set; }

        public List<Sprite> AllSprites { get; set; }


        public List<ActionTimer> AllActions { get; set; }

        public UserInterface MainUserInterface { get; set; }






        public bool TilesLoaded { get; set; } = false;

        public Rectangle MapRectangle { get; set; }

        public ContentManager StageContentManager { get; set; }
        public GraphicsDevice Graphics { get; set; }

        public ParticleEngine ParticleEngine { get; set; }

        public DialogueHolder AllDockDialogue { get; set; }

        public TextBuilder TextBuilder { get; set; }
        public List<Portal> AllPortals { get; set; }

        public int DialogueToRetrieve { get; set; }
        public bool IsDark { get; set; }
        public List<LightSource> AllNightLights { get; set; }
        public List<LightSource> AllDayTimeLights { get; set; }
        public string StageName { get; set; }
        public event EventHandler SceneChanged;

        public bool IsLoaded { get; set; }
        public ITileManager AllTiles { get; set; }

        public List<Character> CharactersPresent { get; set; }


        public List<StringWrapper> AllTextToWrite { get; set; }

        public List<INPC> OnScreenNPCS { get; set; }
        public QuadTree QuadTree { get; set; }
        public List<RisingText> AllRisingText { get; set; }

        public SanctuaryTracker SanctuaryTracker { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Projectile> AllProjectiles { get; set; }
        public List<ParticleEngine> ParticleEngines { get; set; }

        public TileSetType TileSetNumber { get; set; }
        //SAVE STUFF
        public string SavePath { get; set; }

        public NPCGenerator NPCGenerator { get; set; }
        public FunBox FunBox { get; set; }
        #endregion

        #region CONSTRUCTOR



        public TmxStageBase(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber)
        {
            this.StageName = name;
            this.LocationType = locationType;
            this.StageType = stageType;
            this.Graphics = graphics;

            this.StageContentManager = new ContentManager(content.ServiceProvider);
            this.StageContentManager.RootDirectory = "Content";

            this.DialogueToRetrieve = dialogueToRetrieve;
            this.IsLoaded = false;

            this.CharactersPresent = new List<Character>();


            this.OnScreenNPCS = new List<INPC>();
            this.TileSet = tileSet;
            this.AllRisingText = new List<RisingText>();

            this.Enemies = new List<Enemy>();
            this.AllProjectiles = new List<Projectile>();
            this.Map = tmxMap;
            if (locationType == LocationType.Interior)
            {
                this.TileSetNumber = TileSetType.Exterior;
            }
            else if (locationType == LocationType.Exterior)
            {
                this.TileSetNumber = TileSetType.Exterior;
            }
            LoadPreliminaryContent();


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

            this.AllObjects = new Dictionary<string, ICollidable>()
            {

            };

            this.AllPortals = new List<Portal>();
            this.AllTiles = new TileManager(this.TileSet, this.Map, this.Graphics, this.StageContentManager, (int)this.TileSetNumber, this);
            if (!Game1.AreGeneratableTilesLoaded)
            {
                this.AllTiles.LoadGeneratableTileLists(); //just here so it only happens once!
                Game1.AreGeneratableTilesLoaded = true;
            }
            this.TileWidth = this.Map.Tilesets[(int)this.TileSetNumber].TileWidth;
            this.TileHeight = this.Map.Tilesets[(int)this.TileSetNumber].TileHeight;

            this.TilesetTilesWide = this.TileSet.Width / this.TileWidth;
            this.TilesetTilesHigh = this.TileSet.Height / this.TileHeight;


            this.AllActions = new List<ActionTimer>();

            this.MapRectangle = new Rectangle(0, 0, this.TileWidth * this.Map.Width, this.TileHeight * this.Map.Height);
            this.Map = null;
            this.AllCrops = new Dictionary<string, Crop>();
            this.QuadTree = new QuadTree(0, this.MapRectangle);
            this.FunBox = new FunBox(this.Graphics);
        }

        public virtual void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            this.ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);

            this.Cam = camera;
            this.Cam.pos.X = Game1.Player.position.X;
            this.Cam.pos.Y = Game1.Player.position.Y;


            // Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue("", Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours).TextToWrite;

            this.TextBuilder = new TextBuilder("", .1f, 5f);
            SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;

            this.AllTextToWrite = new List<StringWrapper>();
            this.NPCGenerator = new NPCGenerator((IInformationContainer)this.AllTiles, this.Graphics);
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
            if (this != Game1.Town)
            {
                AllTiles.Unload();
            }


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
                AllTiles.StartNew();
            }
        }


        #endregion

        #region UPDATE

        public virtual void PerformQuadTreeInsertions(GameTime gameTime, Player player)
        {
            this.QuadTree = new QuadTree(0, this.Cam.ViewPortRectangle);

            foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.Objects)
            {
                for (int z = 0; z < obj.Value.Count; z++)
                {
                    if (obj.Value[z].ColliderType == ColliderType.TransperencyDetector)
                    {
                        obj.Value[z].Entity.Reset();
                    }
                    this.QuadTree.Insert(obj.Value[z]);
                }
            }

            foreach (KeyValuePair<string, List<GrassTuft>> grass in this.AllTiles.Tufts)
            {
                for (int g = 0; g < grass.Value.Count; g++)
                {
                    if (grass.Value[g].IsUpdating)
                    {
                        grass.Value[g].Update(gameTime);
                    }

                    this.QuadTree.Insert(grass.Value[g].RectangleCollider);

                }
            }

            for (int e = 0; e < this.Enemies.Count; e++)
            {
                if (this.Enemies[e] != null)
                {
                    this.QuadTree.Insert(this.Enemies[e].Collider);

                }

            }
            List<Item> items = this.AllTiles.AllItems;
            for (int i = 0; i < items.Count; i++)
            {
                this.QuadTree.Insert(items[i].RectangleCollider);
                if (items[i].RectangleCollider.Rectangle.Intersects(Cam.CameraScreenRectangle))
                {
                    items[i].Update(gameTime);
                }
            }

            // QuadTree.Insert(player.MainCollider);
            this.QuadTree.Insert(player.BigCollider);

            foreach (Character character in this.CharactersPresent)
            {
                this.QuadTree.Insert(character.Collider);
            }
        }

        public virtual void UpdatePortals(Player player, MouseManager mouse)
        {
            for (int p = 0; p < this.AllPortals.Count; p++)
            {
                if (player.ClickRangeRectangle.Intersects(this.AllPortals[p].PortalStart))
                {
                    if (this.AllPortals[p].MustBeClicked)
                    {
                        if (mouse.WorldMouseRectangle.Intersects(this.AllPortals[p].PortalStart) && mouse.IsClicked)
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpen);
                            Game1.SwitchStage((Stages)this.AllPortals[p].From, (Stages)this.AllPortals[p].To, this.AllPortals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }
                    }
                    else
                    {
                        if (player.Rectangle.Intersects(this.AllPortals[p].PortalStart))
                        {
                            Game1.SwitchStage((Stages)this.AllPortals[p].From, (Stages)this.AllPortals[p].To, this.AllPortals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }

                    }
                }

            }
        }
        public virtual void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            player.CollideOccured = false;
            for (int i = 0; i < this.Enemies.Count; i++)
            {
                this.Enemies[i].Update(gameTime, mouse, Cam.CameraScreenRectangle, this.Enemies);
            }
            PerformQuadTreeInsertions(gameTime, player);
            this.IsDark = Game1.GlobalClock.IsNight;

            UpdatePortals(player, mouse);

            Game1.MouseManager.ToggleGeneralInteraction = false;

            if (Game1.CurrentWeather != WeatherType.None)
            {
                Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, this.LocationType);
            }


            Game1.Player.UserInterface.Update(gameTime, player.Inventory);
            if (Game1.CurrentStage != this)
            {
                return;
            }
            if (Game1.KeyboardManager.WasKeyPressed(Keys.F1))
            {
                this.ShowBorders = !this.ShowBorders;
            }
            if (Game1.KeyboardManager.WasKeyPressed(Keys.F2))
            {

                player.Position = new Vector2(400, 400);
                Game1.GlobalClock.TotalHours = 22;

            }
            this.FunBox.Update(gameTime);
            this.TextBuilder.Update(gameTime);
            this.ParticleEngine.Update(gameTime);

            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);

                this.Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), this.MapRectangle);
                player.Update(gameTime, this.AllTiles.AllItems, mouse);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Update(gameTime, this.AllRisingText);
                }

                foreach (Sprite spr in this.AllSprites)
                {

                    spr.Update(gameTime, mouse.WorldMousePosition);

                }

                this.AllTiles.Update(gameTime, mouse);
                for (int s = 0; s < this.AllTextToWrite.Count; s++)
                {
                    this.AllTextToWrite[s].Update(gameTime, this.AllTextToWrite);
                }

                if (Game1.UpdateCharacters)
                {
                    foreach (Character character in Game1.AllCharacters)
                    {
                        character.Update(gameTime, mouse);
                    }
                }



            }
        }
        #endregion

        #region DRAW
        public virtual void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D nightLightsTarget, RenderTarget2D dayLightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            if (player.Health > 0)
            {
                if (this.IsDark)
                {


                    graphics.SetRenderTarget(nightLightsTarget);
                    //graphics.Clear(Color.White);
                    graphics.Clear(new Color(50, 50, 50, 220));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: this.Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int l = 0; l < this.AllNightLights.Count; l++)
                    {
                        spriteBatch.Draw(this.AllNightLights[l].LightTexture, this.AllNightLights[l].Position, Color.White);
                    }
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() == 4)
                    {
                        spriteBatch.Draw(Game1.AllTextures.lightMask, new Vector2(mouse.WorldMousePosition.X - Game1.AllTextures.lightMask.Width / 2, mouse.WorldMousePosition.Y - Game1.AllTextures.lightMask.Height / 2), Color.White);
                    }
                    spriteBatch.End();
                }


                graphics.SetRenderTarget(mainTarget);
                graphics.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: this.Cam.getTransformation(graphics));
                
                graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                if (Game1.CurrentWeather != WeatherType.None)
                {
                    Game1.AllWeather[Game1.CurrentWeather].Draw(spriteBatch, this.LocationType); //1471
                }

                this.ParticleEngine.Draw(spriteBatch);
                for (int i = 0; i < this.Enemies.Count; i++)
                {
                    this.Enemies[i].Draw(spriteBatch, this.Graphics);
                    if(ShowBorders)
                    {
                        this.Enemies[i].DrawDebug(spriteBatch, .95f);
                    }
                }
                player.Draw(spriteBatch, .5f + (player.Rectangle.Y + player.Rectangle.Height) * Utility.ForeGroundMultiplier);
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
                    foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.Objects)
                    {

                        for (int j = 0; j < obj.Value.Count; j++)
                        {
                            obj.Value[j].DrawDebug(spriteBatch);
                        }

                    }
                    Game1.Player.MainCollider.DrawDebug(spriteBatch);
                }

                this.AllTiles.DrawTiles(spriteBatch);
                for (int s = 0; s < this.AllTextToWrite.Count; s++)
                {
                    this.AllTextToWrite[s].Draw(spriteBatch);
                }
                foreach (Character character in this.CharactersPresent)
                {
                    character.Draw(spriteBatch);
                }

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

                spriteBatch.End();

                graphics.SetRenderTarget(null);



                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (this.IsDark)
                {
                    Game1.AllTextures.practiceLightMaskEffect.Parameters["lightMask"].SetValue(nightLightsTarget);
                    Game1.AllTextures.practiceLightMaskEffect.CurrentTechnique.Passes[0].Apply();
                }

                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);
                if (Game1.KeyboardManager.WasKeyPressed(Keys.PageDown))
                {
                    Game1.Player.UserInterface.CommandConsole.TakeScreenShot(mainTarget);
                }
                spriteBatch.End();
            }
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