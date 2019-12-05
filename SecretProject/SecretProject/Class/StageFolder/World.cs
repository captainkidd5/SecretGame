using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
using TiledSharp;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public class World : ILocation
    {

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
        public List<Item> AllItems { get; set; }
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
        public List<LightSource> AllLights { get; set; }
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

        public Effect CurrentEffect;


        public World(string name, GraphicsDevice graphics, ContentManager content, int tileSetNumber, Texture2D tileSet, string tmxMapPath, int dialogueToRetrieve, int backDropNumber)
        {
            this.TileWidth = 16;
            this.TileHeight = 16;
            lightsTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);

            this.StageName = name;
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;
            this.TileSet = tileSet;
            this.TmxMapPath = tmxMapPath;
            this.IsLoaded = false;
            CharactersPresent = new List<Character>();

            this.OnScreenNPCS = new List<INPC>();

        }



        public void LoadPreliminaryContent(int worldSize)
        {
            this.WorldSize = worldSize;
            AllLights = new List<LightSource>()
            {

            };

            AllSprites = new List<Sprite>()
            {

            };


            AllItems = new List<Item>()
            {

            };


            AllDepths = new List<float>()
            {
                0f,
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


            // AllTiles = new TileManager(this, TileSet, AllLayers, Map, 5, WorldWidth, WorldHeight, Graphics, Content, TileSetNumber, AllDepths);
            this.AllTiles = new WorldTileManager(this, TileSet, AllLayers, Map, 5, 100, 100, Graphics, Content, TileSetNumber, AllDepths);
            AllTiles.LoadGeneratableTileLists();
            //AllTiles.LoadInitialTileObjects(this);
            TileWidth = Map.Tilesets[TileSetNumber].TileWidth;
            TileHeight = Map.Tilesets[TileSetNumber].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            AllActions = new List<ActionTimer>();

            MapRectangle = new Rectangle(0, 0, TileWidth * 100, TileHeight * 100);
            Map = null;
            AllCrops = new Dictionary<string, Crop>();

            Enemies = new List<Enemy>();
            AllRisingText = new List<RisingText>();
            // AllTiles.LoadInitialChunks();

            // Game1.SoundManager.DustStormInstance.Play();
        }
        public void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            RenderTarget2D lightsTarget;
            RenderTarget2D mainTarget;
            var pp = Graphics.PresentationParameters;
            lightsTarget = new RenderTarget2D(Graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(Graphics, pp.BackBufferWidth, pp.BackBufferHeight);
            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);



            this.Cam = camera;
            Cam.Zoom =4f;
            Cam.pos.X = Game1.Player.position.X;
            Cam.pos.Y = Game1.Player.position.Y;



            TextBuilder = new TextBuilder("", .1f, 5f);
            this.SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;
            this.IsLoaded = true;

            Game1.Player.Position = new Vector2(0, 0);


            AllTiles.LoadInitialChunks();
            Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue(1f);
            Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();
            this.QuadTree = new QuadTree(0, Cam.CameraScreenRectangle);

        }

        public void UnloadContent()
        {
            //throw new NotImplementedException();
        }


        public int portalIndex;

        public event EventHandler SceneChanged;

        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            player.CollideOccured = false;
            QuadTree = new QuadTree(0, Cam.ViewPortRectangle);

            for (int i = 0; i < AllTiles.ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < AllTiles.ActiveChunks.GetLength(1); j++)
                {
                    if (AllTiles.ActiveChunks[i, j].IsLoaded)
                    {


                        if (AllTiles.ActiveChunks[i, j].GetChunkRectangle().Intersects(Cam.CameraScreenRectangle))
                        {

                            foreach (KeyValuePair<string, List<ICollidable>> obj in AllTiles.ActiveChunks[i, j].Objects)
                            {
                                for (int z = 0; z < obj.Value.Count; z++)
                                {
   
                                    QuadTree.Insert(obj.Value[z]);
                                }
                            }

                            foreach (KeyValuePair<string, List<GrassTuft>> grass in AllTiles.ActiveChunks[i, j].Tufts)
                            {
                                for(int g =0; g < grass.Value.Count; g++)
                                {
                                    if(grass.Value[g].IsUpdating)
                                    {
                                        grass.Value[g].Update(gameTime);
                                    }
                                    
                                    QuadTree.Insert(grass.Value[g]);
                                }
                            }

                                for (int e = 0; e < Enemies.Count; e++)
                            {
                                if (Enemies[e] != null)
                                {
                                    QuadTree.Insert(Enemies[e].Collider);
                                }

                            }
                        }
                    }

                }
            }
            for (int i = 0; i < AllItems.Count; i++)
            {
                QuadTree.Insert(AllItems[i].ItemSprite);
            }

            QuadTree.Insert(player.MainCollider);
            QuadTree.Insert(player.BigCollider);


            this.IsDark = Game1.GlobalClock.IsNight;




            Game1.myMouseManager.ToggleGeneralInteraction = false;
            Game1.isMyMouseVisible = true;


            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                ShowBorders = !ShowBorders;

            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F2)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F2)))
            {

                player.Position = new Vector2(400, 400);
                Game1.GlobalClock.TotalHours = 22;
            }




            // TextBuilder.PositionToWriteTo = Game1.Elixer.Position;
            TextBuilder.Update(gameTime);

            //ParticleEngine.EmitterLocation = mouse.WorldMousePosition;
            ParticleEngine.Update(gameTime);

            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);
                //--------------------------------------
                //Update players
                Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), MapRectangle);


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
                player.Update(gameTime, AllItems, mouse);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    AllRisingText[i].Update(gameTime, AllRisingText);
                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Update(gameTime);
                }

                for (int i = 0; i < Enemies.Count; i++)
                {

                    if (Cam.ViewPortRectangle.Intersects(Enemies[i].NPCHitBoxRectangle))
                    {
                        Enemies[i].TimeInUnloadedChunk = 0f;
                    }

                    else
                    {
                        Enemies[i].TimeInUnloadedChunk += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    Enemies[i].Update(gameTime, mouse, Enemies);
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
        public void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            Effect currentEffect = null;
            if (player.Health > 0)
            {
                if (this.IsDark)
                {


                    graphics.SetRenderTarget(lightsTarget);
                    //graphics.Clear(Color.White);
                    graphics.Clear(new Color(50, 50, 50, 220));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int i = 0; i < AllTiles.ActiveChunks.GetLength(0); i++)
                    {
                        for (int j = 0; j < AllTiles.ActiveChunks.GetLength(1); j++)
                        {
                            if (AllTiles.ActiveChunks[i, j].IsLoaded)
                            {
                                for (int l = 0; l < AllTiles.ActiveChunks[i, j].Lights.Count; l++)
                                {
                                    spriteBatch.Draw(AllTiles.ActiveChunks[i, j].Lights[l].LightTexture,
                                        AllTiles.ActiveChunks[i, j].Lights[l].Position, Color.White);
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
                graphics.DepthStencilState = new DepthStencilState() { DepthBufferFunction = CompareFunction.Less, DepthBufferEnable = true };
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Cam.getTransformation(graphics), effect: currentEffect);


                ParticleEngine.Draw(spriteBatch);

                player.Draw(spriteBatch, .5f + (player.Rectangle.Y + player.Rectangle.Height) * .0000001f);

                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    AllRisingText[i].Draw(spriteBatch);
                }


                TextBuilder.Draw(spriteBatch, .71f);

                if (ShowBorders)
                {
                    player.DrawDebug(spriteBatch, .4f);
                    //ElixerNPC.DrawDebug(spriteBatch, .4f);
                    //Game1.Dobbin.DrawDebug(spriteBatch, .4f);
                    // Game1.Elixer.DrawDebug(spriteBatch, .4f);
                }

                AllTiles.DrawTiles(spriteBatch);
                //Game1.userInterface.BottomBar.DrawDraggableItems(spriteBatch, BuildingsTiles, ForeGroundTiles, mouse);

                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    Game1.Player.UserInterface.TileSelector.Draw(spriteBatch);
                }

                //--------------------------------------
                //Draw sprite list

                foreach (var sprite in AllSprites)
                {
                    //sprite.ShowRectangle = ShowBorders;
                    sprite.Draw(spriteBatch, .7f);
                }
                for (int i = 0; i < Enemies.Count; i++)
                {
                    Enemies[i].Draw(spriteBatch, Graphics, ref CurrentEffect);
                    if (ShowBorders)
                    {
                        Enemies[i].DrawDebug(spriteBatch, 1f);
                    }
                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Draw(spriteBatch);
                }


                foreach (KeyValuePair<string, List<ICollidable>> obj in AllTiles.ChunkUnderPlayer.Objects)
                {
                    if (ShowBorders)
                    {
                        for (int j = 0; j < obj.Value.Count; j++)
                        {
                            obj.Value[j].Draw(spriteBatch, .4f);
                        }

                    }
                }
                //for (int i = 0; i < AllTiles.ChunkUnderPlayer.Objects.Count; i++)
                //{
                //    if (ShowBorders)
                //    {
                //        AllTiles.ChunkUnderPlayer.Objects[i].Draw(spriteBatch, .4f);
                //    }
                //}

                Game1.Player.UserInterface.BackPack.DrawToStageMatrix(spriteBatch);

                spriteBatch.End();

                graphics.SetRenderTarget(null);
                // graphics.Clear(Color.Black);


                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                if (IsDark)
                {
                    Game1.AllTextures.practiceLightMaskEffect.Parameters["lightMask"].SetValue(lightsTarget);
                    Game1.AllTextures.practiceLightMaskEffect.CurrentTechnique.Passes[0].Apply();
                }

                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);
                //spriteBatch.Draw(lightsTarget, Game1.ScreenRectangle, Color.White);

                spriteBatch.End();
            }
            Game1.Player.DrawUserInterface(spriteBatch);

            //  Graphics.SetRenderTarget(null);

        }

        public void LoadPreliminaryContent()
        {
            throw new NotImplementedException();
        }

        public void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            throw new NotImplementedException();
        }

        public void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color)
        {
            AllRisingText.Add(new RisingText(yStart, yEnd, stringToWrite, speed, color));
        }
    }
}
