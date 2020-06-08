﻿using Microsoft.Xna.Framework;
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
using System.IO;
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
        public List<Projectile> AllProjectiles { get; set; }
        public List<Enemy> Enemies { get; set; }


        public int StageIdentifier { get; set; }
        public string StageName { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        public Texture2D TileSet { get; set; }
        public ITileManager AllTiles { get; set; }
        public Camera2D Cam { get; set; }

        public List<Sprite> AllSprites { get; set; }

        public List<ActionTimer> AllActions { get; set; }
        public List<Portal> AllPortals { get; set; }
        public UserInterface MainUserInterface { get; set; }
        public ContentManager StageContentManager { get; set; }
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
        public TmxMap Map { get; set; }

        public QuadTree QuadTree { get; set; }
        public List<RisingText> AllRisingText { get; set; }
        public List<ParticleEngine> ParticleEngines { get; set; }

        public Effect CurrentEffect;

        public float StaminaSafeDistance { get; set; }

        public string SavePath { get; set; }

        public TileSetType TileSetNumber { get; set; }
        public World(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber)
        {
            this.TileWidth = 16;
            this.TileHeight = 16;
            lightsTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);

            this.StageName = name;
            this.LocationType = locationType;
            this.StageType = stageType;
            this.Graphics = graphics;
            this.StageContentManager = content;
            this.Map = tmxMap;
            this.TileSet = tileSet;
   
            this.IsLoaded = false;
            this.CharactersPresent = new List<Character>();

            this.OnScreenNPCS = new List<INPC>();

            this.ParticleEngines = new List<ParticleEngine>();
            this.StaminaSafeDistance = 16 * 8;
            this.AllProjectiles = new List<Projectile>();
            if (locationType == LocationType.Interior)
            {
                TileSetNumber = TileSetType.Interior;
            }
            else if (locationType == LocationType.Exterior)
            {
                this.TileSetNumber = TileSetType.Exterior;
            }
            LoadPreliminaryContent();

        }

        public bool CheckIfWithinStaminaSafeZone(Vector2 position)
        {
            if (Math.Abs(position.X) > StaminaSafeDistance || Math.Abs(position.Y) > StaminaSafeDistance)
            {
                return false;
            }
            return true;
        }

        public void AssignPath(string startPath)
        {
            this.SavePath = startPath + this.StageName;
        }

        public void LoadPreliminaryContent()
        {

            this.AllNightLights = new List<LightSource>()
            {

            };

            this.AllSprites = new List<Sprite>()
            {

            };





            this.AllPortals = new List<Portal>();


            // AllTiles = new TileManager(this, TileSet, AllLayers, Map, 5, WorldWidth, WorldHeight, Graphics, Content, TileSetNumber, AllDepths);
            this.AllTiles = new WorldTileManager(this, this.TileSet, this.Map, 100, 100, this.Graphics, this.StageContentManager, (int)this.TileSetNumber);
            this.AllTiles.LoadGeneratableTileLists();
            //AllTiles.LoadInitialTileObjects(this);
            this.TileWidth = this.Map.Tilesets[(int)TileSetNumber].TileWidth;
            this.TileHeight = this.Map.Tilesets[(int)TileSetNumber].TileHeight;

            this.TilesetTilesWide = this.TileSet.Width / this.TileWidth;
            this.TilesetTilesHigh = this.TileSet.Height / this.TileHeight;


            this.AllActions = new List<ActionTimer>();

            this.MapRectangle = new Rectangle(0, 0, this.TileWidth * 100, this.TileHeight * 100);
            this.Map = null;
            this.AllCrops = new Dictionary<string, Crop>();

            this.Enemies = new List<Enemy>();
            this.AllRisingText = new List<RisingText>();


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


            //Game1.Player.Position = new Vector2(0, 0);


            //this.AllTiles.LoadInitialChunks(Game1.Player.position);
            Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue(1f);
            Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();
            this.QuadTree = new QuadTree(0, this.Cam.CameraScreenRectangle);
            this.IsLoaded = true;
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
                        Chunk chunk = this.AllTiles.ActiveChunks[i, j];
                        if (chunk.GetChunkRectangle().Intersects(this.Cam.CameraScreenRectangle))
                        {
                            foreach (KeyValuePair<string, List<ICollidable>> obj in chunk.Objects)
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

                            foreach (KeyValuePair<string, List<GrassTuft>> grass in chunk.Tufts)
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

                            for (int p = 0; p < AllProjectiles.Count; p++)
                            {
                                this.QuadTree.Insert(AllProjectiles[p].Collider);

                            }

                            for (int item = 0; item < chunk.AllItems.Count; item++)
                            {
                                this.QuadTree.Insert(chunk.AllItems[item].Collider);

                            }

                            foreach (KeyValuePair<string, Sprite> sprite in this.AllTiles.ActiveChunks[i, j].QuestIcons)
                            {
                                sprite.Value.BobInPlace(gameTime, sprite.Value.AnchorPosition.Y - 20, sprite.Value.AnchorPosition.Y - 25, 5f);
                            }
                        }
                    }


                }
            }


            this.QuadTree.Insert(Game1.Player.MainCollider);

            this.QuadTree.Insert(Game1.Player.BigCollider);
            this.QuadTree.Insert(Game1.MouseManager.MouseCollider);

        }

        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            Game1.Player.UserInterface.Update(gameTime, player.Inventory);
            player.CollideOccured = false;
            this.QuadTree = new QuadTree(0, this.Cam.CameraScreenRectangle);

            PerformQuadTreeInsertions(gameTime);




            this.IsDark = Game1.GlobalClock.IsNight;


            //for(int p = 0; p < ParticleEngines.Count; p++)
            //{
            //    ParticleEngines[p].Update(gameTime);
            //}

            Game1.MouseManager.ToggleGeneralInteraction = false;
            Game1.isMyMouseVisible = true;


            if ((Game1.KeyboardManager.WasKeyPressed(Keys.F1)))
            {
                this.ShowBorders = !this.ShowBorders;

            }

            for (int p = 0; p < AllProjectiles.Count; p++)
            {
                AllProjectiles[p].Update(gameTime);
            }

            this.TextBuilder.Update(gameTime);
            if (Game1.CurrentWeather != WeatherType.None)
            {
                Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, this.LocationType);
            }
            this.ParticleEngine.Update(gameTime);
            

            if (!Game1.freeze)
            {
                //foreach (Character character in Game1.AllCharacters)
                //{
                //    character.Update(gameTime, mouse);
                //}
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
                    if (this.Enemies[i] != null)
                    {


                        if (this.Cam.CameraScreenRectangle.Intersects(this.Enemies[i].NPCHitBoxRectangle))
                        {
                            this.Enemies[i].TimeInUnloadedChunk = Game1.GlobalClock.SecondsPassedToday;
                        }
                        if (Game1.GlobalClock.SecondsPassedToday - this.Enemies[i].TimeInUnloadedChunk > 100)
                        {
                            Enemies.RemoveAt(i);
                        }
                        else
                        {
                            this.Enemies[i].Update(gameTime, mouse, Cam.CameraScreenRectangle, this.Enemies);
                        }


                    }
                }

            }
            //Game1.Player.controls.UpdateKeys();



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

                    for (int i = 0; i < WorldTileManager.RenderDistance; i++)
                    {
                        for (int j = 0; j < WorldTileManager.RenderDistance; j++)
                        {
                            if (this.AllTiles.ActiveChunks[i, j].IsLoaded)
                            {
                                Chunk chunk = this.AllTiles.ActiveChunks[i, j];
                                if (chunk.GetChunkRectangle().Intersects(this.Cam.CameraScreenRectangle))
                                {
                                    for (int l = 0; l < this.AllTiles.ActiveChunks[i, j].NightTimeLights.Count; l++)
                                    {
                                        spriteBatch.Draw(this.AllTiles.ActiveChunks[i, j].NightTimeLights[l].LightTexture,
                                            this.AllTiles.ActiveChunks[i, j].NightTimeLights[l].Position, Color.White);
                                    }
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
                    if (this.Enemies[i] != null)
                    {

                            this.Enemies[i].Draw(spriteBatch, this.Graphics, ref CurrentEffect);
                            if (this.ShowBorders)
                            {
                                this.Enemies[i].DrawDebug(spriteBatch, 1f);
                            }
                        
                        
                    }
                }
                for (int p = 0; p < this.AllProjectiles.Count; p++)
                {
                    AllProjectiles[p].Draw(spriteBatch);
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


        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            throw new NotImplementedException();
        }

        public void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade, float scale)
        {
            this.AllRisingText.Add(new RisingText(Game1.Player.Position, yEnd, stringToWrite, speed, color, fade, scale));
        }

        public void SaveLocation()
        {
            this.AllTiles.SaveTiles();
        }

        public void Save(BinaryWriter writer)
        {
            // this.AllTiles.Save(writer);
        }

        public void Load(BinaryReader reader)
        {
            //this.AllTiles.Load(reader);
        }

        public void UnloadContent(BinaryWriter writer)
        {

        }

        public void TryLoadExistingStage()
        {

        }
    }
}
