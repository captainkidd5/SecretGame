﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.ObjectFolder;
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
    public class World : TmxStageBase, ILocation
    {

        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }
        public List<Boar> Boars;



        public World(string name, GraphicsDevice graphics, ContentManager content, int tileSetNumber, string mapTexturePath, string tmxMapPath, int dialogueToRetrieve) : base(name, graphics, content, tileSetNumber, mapTexturePath, tmxMapPath, dialogueToRetrieve)
        {
            this.TileWidth = 16;
            this.TileHeight = 16;
            lightsTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight);
        }

        

        public void LoadPreliminaryContent(int worldSize)
        {
            AllLights = new List<LightSource>()
            {

            };

            AllSprites = new List<Sprite>()
            {

            };

            AllObjects = new Dictionary<float, ObjectBody>()
            {

            };

            AllItems = new List<Item>()
            {

            };



            this.TileSet = Content.Load<Texture2D>(this.MapTexturePath);


            AllDepths = new List<float>()
            {
                0f,
                .2f,
                .3f,
                .5f,
                .6f
            };

            this.Map = new TmxMap(this.TmxMapPath);

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
            switch (worldSize)
            {
                case 1:
                    this.WorldWidth = 400;
                    this.WorldHeight = 400;
                    break;
                case 2:
                    this.WorldWidth = 700;
                    this.WorldHeight = 700;
                    break;
                case 3:
                    this.WorldWidth = 1000;
                    this.WorldHeight = 1000;
                    break;

            }
            AllTiles = new TileManager(this, TileSet, AllLayers, Map, 5, WorldWidth, WorldHeight, Graphics, Content, TileSetNumber, AllDepths, this);

            AllTiles.LoadInitialTileObjects(this);
            TileWidth = Map.Tilesets[TileSetNumber].TileWidth;
            TileHeight = Map.Tilesets[TileSetNumber].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;


            AllActions = new List<ActionTimer>();

            MapRectangle = new Rectangle(0, 0, TileWidth * WorldWidth, TileHeight * WorldHeight);
            Map = null;
            AllCrops = new Dictionary<float, Crop>();
            Boars = new List<Boar>() { };
            for (int i = 1; i < 15; i++)
            {
                Boars.Add(new Boar("Boar", new Vector2(Game1.Utility.RFloat(300, 1200), Game1.Utility.RFloat(300, 1200)), Graphics, Game1.AllTextures.EnemySpriteSheet));
            }
        }

        public override void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            this.IsDark = Game1.GlobalClock.IsNight;
            for (int p = 0; p < AllPortals.Count; p++)
            {
                if (player.ClickRangeRectangle.Intersects(AllPortals[p].PortalStart) && AllPortals[p].MustBeClicked)
                {
                    if (mouse.WorldMouseRectangle.Intersects(AllPortals[p].PortalStart) && mouse.IsClicked)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                        Game1.SwitchStage(AllPortals[p].From, AllPortals[p].To, gameTime,AllPortals[p]);
                        OnSceneChanged();
                        this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                        return;
                    }

                }
                else if (player.Rectangle.Intersects(AllPortals[p].PortalStart) && !AllPortals[p].MustBeClicked)
                {
                    Game1.SwitchStage(AllPortals[p].From, AllPortals[p].To, gameTime,AllPortals[p]);
                    OnSceneChanged();
                    this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                    return;
                }
            }

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


            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.M)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.M)))
            {
                Game1.SwitchStage(5, 4, gameTime);
                return;
            }

            TextBuilder.PositionToWriteTo = Game1.Elixer.Position;
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

                for (int e = 0; e < Boars.Count; e++)
                {
                    Boars[e].Update(gameTime, AllObjects, mouse);
                    //Console.WriteLine(Boars[1].cu);
                    //Boars[e].MoveTowardsPosition(Game1.Player.Position,Game1.Player.Rectangle);
                }
                // foreach (Character character in Game1.AllCharacters)
                // {
                //     character.Update(gameTime, AllObjects, mouse);
                // }

            }
            Game1.Player.controls.UpdateKeys();
            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);

        }
        public override void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
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
                ParticleEngine.Draw(spriteBatch, 1f);

                player.Draw(spriteBatch, .4f + (.0001f * ((float)player.Rectangle.Y + player.Rectangle.Height - 10)));
                //Console.WriteLine("Player Position" + player.position);


                Game1.Elixer.Draw(spriteBatch);
                Game1.Dobbin.Draw(spriteBatch);


                TextBuilder.Draw(spriteBatch, .71f);

                if (ShowBorders)
                {
                    player.DrawDebug(spriteBatch, .4f);
                    //ElixerNPC.DrawDebug(spriteBatch, .4f);
                    Game1.Dobbin.DrawDebug(spriteBatch, .4f);
                    Game1.Elixer.DrawDebug(spriteBatch, .4f);
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
                for (int e = 0; e < Boars.Count; e++)
                {
                    Boars[e].Draw(spriteBatch);

                }

                for (int i = 0; i < AllItems.Count; i++)
                {
                    AllItems[i].Draw(spriteBatch);
                }

                foreach (var obj in AllObjects.Values)
                {
                    if (ShowBorders)
                    {
                        obj.Draw(spriteBatch, .4f);
                    }
                }


                //Game1.Elixer.Draw(spriteBatch);

                Game1.Player.UserInterface.BottomBar.DrawToStageMatrix(spriteBatch);

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
            Game1.GlobalClock.Draw(spriteBatch);
            //  Graphics.SetRenderTarget(null);

        }

        public override void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
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
            Cam.Zoom = 3f;
            Cam.pos.X = Game1.Player.position.X;
            Cam.pos.Y = Game1.Player.position.Y;


            //AllItems.Add(Game1.ItemVault.GenerateNewItem(129, new Vector2(500, 500), true));
            //AllDockDialogue = Content.Load<DialogueHolder>("Dialogue/AllDialogue");
            //Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(1, 1);

            TextBuilder = new TextBuilder(Game1.DialogueLibrary.RetrieveDialogue(1, Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours), .1f, 5f);
            this.SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;
            this.IsLoaded = true;
        }
    }
}
