using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using TiledSharp;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public class World : ILocation
    {
        public LocationType LocationType { get; set; }
        public StageType StageType { get; set; }
        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        public List<Enemy> Enemies { get; set; }


        public int WorldSize { get; set; }
        public int StageIdentifier { get; set; }
        public string StageName { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        public Texture2D TileSet { get; set; }
        public ITileManager AllTiles { get; set; }
        public Camera2D Cam { get; set; }
        public int TileSetNumber { get; set; }
        public List<Sprite> AllSprites { get; set; }

        public List<ActionTimer> AllActions { get; set; }
        public List<Portal> AllPortals { get; set; }
        public UserInterface MainUserInterface { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Rectangle MapRectangle { get; set; }
        public Dictionary<string, Crop> AllCrops { get; set; }
        public bool IsDark { get; set; }
        public bool ShowBorders { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public TextBuilder TextBuilder { get; set; }
        public bool IsLoaded { get; set; }
        public List<Character> CharactersPresent { get; set; }
        public List<StringWrapper> AllTextToWrite { get; set; }
        public List<INPC> OnScreenNPCS { get; set; }
        public List<LightSource> AllNightLights { get; set; }
        public List<LightSource> AllDayTimeLights { get; set; }
        public List<float> MyProperty { get; set; }
        public List<float> AllDepths { get; set; }
        public TmxLayer Buildings { get; set; }
        public TmxLayer Background { get; set; }
        public TmxLayer Background1 { get; set; }
        public TmxLayer MidGround { get; set; }
        public TmxLayer foreGround { get; set; }
        public List<TmxLayer> AllLayers { get; set; }
        public string TmxMapPath { get; set; }
        public TmxMap Map { get; set; }

        public QuadTree QuadTree { get; set; }
        public List<RisingText> AllRisingText { get; set; }
        public List<ParticleEngine> ParticleEngines { get; set; }

        public Effect CurrentEffect;

        public float StaminaSafeDistance { get; set; }

        public World(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, int tileSetNumber, Texture2D tileSet, string tmxMapPath, int dialogueToRetrieve, int backDropNumber)
        {
            this.TileWidth = 16;
            this.TileHeight = 16;
            lightsTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);

            this.StageName = name;
            this.LocationType = locationType;
            this.StageType = stageType;
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;
            this.TileSet = tileSet;
            this.TmxMapPath = tmxMapPath;
            this.IsLoaded = false;
            this.CharactersPresent = new List<Character>();

            this.OnScreenNPCS = new List<INPC>();

            this.ParticleEngines = new List<ParticleEngine>();
            this.StaminaSafeDistance = 16 * 8; 

        }

        public bool CheckIfWithinStaminaSafeZone(Vector2 position)
        {
            if(Math.Abs(position.X) > StaminaSafeDistance || Math.Abs(position.Y) > StaminaSafeDistance)
            {
                return false;
            }
            return true;
        }

        public void LoadPreliminaryContent(int worldSize)
        {
            this.WorldSize = worldSize;
            this.AllNightLights = new List<LightSource>()
            {

            };

            this.AllSprites = new List<Sprite>()
            {

            };



            this.AllDepths = new List<float>()
            {
                0f,
                .2f,
                .3f,
                .5f,
            };

            this.Map = new TmxMap(this.TmxMapPath);

            this.Background = this.Map.Layers["background"];
            this.MidGround = this.Map.Layers["midGround"];
            this.Buildings = this.Map.Layers["buildings"];
            this.foreGround = this.Map.Layers["foreGround"];
            this.AllLayers = new List<TmxLayer>()
            {
                this.Background,
                this.MidGround,
                this.Buildings,
                this.foreGround

            };
            this.AllPortals = new List<Portal>();


            // AllTiles = new TileManager(this, TileSet, AllLayers, Map, 5, WorldWidth, WorldHeight, Graphics, Content, TileSetNumber, AllDepths);
            this.AllTiles = new WorldTileManager(this, this.TileSet, this.AllLayers, this.Map, 5, 100, 100, this.Graphics, this.Content, this.TileSetNumber, this.AllDepths);
            this.AllTiles.LoadGeneratableTileLists();
            //AllTiles.LoadInitialTileObjects(this);
            this.TileWidth = this.Map.Tilesets[this.TileSetNumber].TileWidth;
            this.TileHeight = this.Map.Tilesets[this.TileSetNumber].TileHeight;

            this.TilesetTilesWide = this.TileSet.Width / this.TileWidth;
            this.TilesetTilesHigh = this.TileSet.Height / this.TileHeight;


            this.AllActions = new List<ActionTimer>();

            this.MapRectangle = new Rectangle(0, 0, this.TileWidth * 100, this.TileHeight * 100);
            this.Map = null;
            this.AllCrops = new Dictionary<string, Crop>();

            this.Enemies = new List<Enemy>();
            this.AllRisingText = new List<RisingText>();
            // AllTiles.LoadInitialChunks();

            // Game1.SoundManager.DustStormInstance.Play();
        }
        public void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            RenderTarget2D lightsTarget;
            RenderTarget2D mainTarget;
            var pp = this.Graphics.PresentationParameters;
            lightsTarget = new RenderTarget2D(this.Graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(this.Graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            this.ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);



            this.Cam = camera;
            this.Cam.pos.X = Game1.Player.position.X;
            this.Cam.pos.Y = Game1.Player.position.Y;



            this.TextBuilder = new TextBuilder("", .1f, 5f);
            SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;
            this.IsLoaded = true;

            Game1.Player.Position = new Vector2(0, 0);


            this.AllTiles.LoadInitialChunks(Vector2.Zero);
            Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue(1f);
            Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();
            this.QuadTree = new QuadTree(0, this.Cam.CameraScreenRectangle);

        }

        public void UnloadContent()
        {
            //throw new NotImplementedException();
        }


        public int portalIndex;

        public event EventHandler SceneChanged;
        public void PerformQuadTreeInsertions(GameTime gameTime)
        {
            for (int i = 0; i < WorldTileManager.RenderDistance; i++)
            {
                for (int j = 0; j < WorldTileManager.RenderDistance; j++)
                {
                    if (this.AllTiles.ActiveChunks[i, j].IsLoaded)
                    {

                        if (this.AllTiles.ActiveChunks[i, j].GetChunkRectangle().Intersects(this.Cam.CameraScreenRectangle))
                            {



                            foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.ActiveChunks[i, j].Objects)
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

                            foreach (KeyValuePair<string, List<GrassTuft>> grass in this.AllTiles.ActiveChunks[i, j].Tufts)
                            {
                                for (int g = 0; g < grass.Value.Count; g++)
                                {
                                    if (grass.Value[g].IsUpdating)
                                    {
                                        grass.Value[g].Update(gameTime);
                                    }

                                    this.QuadTree.Insert(grass.Value[g]);
                                   
                                }
                            }

                            for (int e = 0; e < this.Enemies.Count; e++)
                            {
                                if (this.Enemies[e] != null)
                                {
                                    this.QuadTree.Insert(this.Enemies[e].Collider);
                                    
                                }

                            }

                            for (int item = 0; item < this.AllTiles.ActiveChunks[i, j].AllItems.Count; item++)
                            {
                                this.QuadTree.Insert(AllTiles.ActiveChunks[i, j].AllItems[item].ItemSprite);
                                
                            }
                        }
                    }


                }
            }


            this.QuadTree.Insert(Game1.Player.MainCollider);
           
            this.QuadTree.Insert(Game1.Player.BigCollider);
            
        }

        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            player.CollideOccured = false;
            this.QuadTree = new QuadTree(0, this.Cam.CameraScreenRectangle);

            PerformQuadTreeInsertions(gameTime);
            



            this.IsDark = Game1.GlobalClock.IsNight;


            //for(int p = 0; p < ParticleEngines.Count; p++)
            //{
            //    ParticleEngines[p].Update(gameTime);
            //}

            Game1.myMouseManager.ToggleGeneralInteraction = false;
            Game1.isMyMouseVisible = true;


            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                this.ShowBorders = !this.ShowBorders;

            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F2)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F2)))
            {

                player.Position = new Vector2(0, 0);
            }


            this.TextBuilder.Update(gameTime);
            if (Game1.CurrentWeather != WeatherType.None)
            {
                Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, this.LocationType);
            }
            this.ParticleEngine.Update(gameTime);
            foreach (Character character in Game1.AllCharacters)
            {
                character.Update(gameTime, mouse);
            }

            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);

                this.Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), this.MapRectangle);


                foreach (Sprite spr in this.AllSprites)
                {
                    if (spr.IsBeingDragged == true)
                    {
                        spr.Update(gameTime, mouse.WorldMousePosition);
                    }
                }

                this.AllTiles.Update(gameTime, mouse);
                player.Update(gameTime, this.AllTiles.ChunkUnderPlayer.AllItems, mouse);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Update(gameTime, this.AllRisingText);
                }



                for (int i = 0; i < this.Enemies.Count; i++)
                {

                    if (this.Cam.CameraScreenRectangle.Intersects(this.Enemies[i].NPCHitBoxRectangle))
                    {
                        this.Enemies[i].TimeInUnloadedChunk = 0f;
                    }

                    else
                    {
                        this.Enemies[i].TimeInUnloadedChunk += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    this.Enemies[i].Update(gameTime, mouse, this.Enemies);
                }

            }
            Game1.Player.controls.UpdateKeys();
            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);


        }

        public void OnSceneChanged()
        {
            if (SceneChanged != null)
            {
                SceneChanged(this, EventArgs.Empty);
            }
        }
        public void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, RenderTarget2D dayLightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            Effect currentEffect = null;
            if (player.Health > 0)
            {
                if (this.IsDark)
                {
                    graphics.SetRenderTarget(lightsTarget);

                    graphics.Clear(new Color(50, 50, 50, 220));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: this.Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int i = WorldTileManager.RenderDistance / 2 - 1; i < WorldTileManager.RenderDistance / 2 + 1; i++)
                    {
                        for (int j = WorldTileManager.RenderDistance / 2 - 1; j < WorldTileManager.RenderDistance / 2 + 1; j++)
                        {
                            if (this.AllTiles.ActiveChunks[i, j].IsLoaded)
                            {
                                for (int l = 0; l < this.AllTiles.ActiveChunks[i, j].NightTimeLights.Count; l++)
                                {
                                    spriteBatch.Draw(this.AllTiles.ActiveChunks[i, j].NightTimeLights[l].LightTexture,
                                        this.AllTiles.ActiveChunks[i, j].NightTimeLights[l].Position, Color.White);
                                }
                            }
                        }
                    }

                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() == 4)
                    {
                        spriteBatch.Draw(Game1.AllTextures.lightMask, new Vector2(mouse.WorldMousePosition.X - Game1.AllTextures.lightMask.Width / 2, mouse.WorldMousePosition.Y - Game1.AllTextures.lightMask.Height / 2), Color.White);
                    }
                    spriteBatch.End();
                }


                graphics.SetRenderTarget(mainTarget);
                graphics.Clear(Color.Transparent);
                //graphics.DepthStencilState = new DepthStencilState() { DepthBufferFunction = CompareFunction.Less, DepthBufferEnable = true };
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: this.Cam.getTransformation(graphics), effect: currentEffect);
                if (Game1.CurrentWeather != WeatherType.None)
                {
                    Game1.AllWeather[Game1.CurrentWeather].Draw(spriteBatch, this.LocationType);
                }
                this.ParticleEngine.Draw(spriteBatch);
                for (int p = 0; p < this.ParticleEngines.Count; p++)
                {
                    this.ParticleEngines[p].Draw(spriteBatch);
                }
                foreach (Character character in this.CharactersPresent)
                {
                    character.Draw(spriteBatch);
                }
                player.Draw(spriteBatch, .5f + (player.Rectangle.Y + player.Rectangle.Height) * Game1.Utility.ForeGroundMultiplier);

                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Draw(spriteBatch);
                }


                this.TextBuilder.Draw(spriteBatch, .71f);

                if (this.ShowBorders)
                {
                    player.DrawDebug(spriteBatch, .4f);

                }

                this.AllTiles.DrawTiles(spriteBatch);

                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    Game1.Player.UserInterface.TileSelector.Draw(spriteBatch);
                }



                foreach (var sprite in this.AllSprites)
                {
                    sprite.Draw(spriteBatch, .7f);
                }
                for (int i = 0; i < this.Enemies.Count; i++)
                {
                    this.Enemies[i].Draw(spriteBatch, this.Graphics, ref CurrentEffect);
                    if (this.ShowBorders)
                    {
                        this.Enemies[i].DrawDebug(spriteBatch, 1f);
                    }
                }


                if (this.ShowBorders)
                {
                    foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.ChunkUnderPlayer.Objects)
                    {

                        for (int j = 0; j < obj.Value.Count; j++)
                        {
                            obj.Value[j].Draw(spriteBatch, .4f);
                        }

                    }
                }

                Game1.Player.UserInterface.BackPack.DrawToStageMatrix(spriteBatch);

                spriteBatch.End();

                graphics.SetRenderTarget(null);


                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (this.IsDark)
                {
                    Game1.AllTextures.practiceLightMaskEffect.Parameters["lightMask"].SetValue(lightsTarget);
                    Game1.AllTextures.practiceLightMaskEffect.CurrentTechnique.Passes[0].Apply();
                }

                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);

                spriteBatch.End();
            }
            Game1.Player.DrawUserInterface(spriteBatch);


        }

        public void LoadPreliminaryContent()
        {
            throw new NotImplementedException();
        }

        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            throw new NotImplementedException();
        }

        public void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade, float scale)
        {
            this.AllRisingText.Add(new RisingText(Game1.Player.Position, yEnd, stringToWrite, speed, color, fade, scale));
        }
    }
}
