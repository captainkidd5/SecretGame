﻿
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
using SecretProject.Class.LightStuff;
using XMLData.RouteStuff;
using SecretProject.Class.NPCStuff.Enemies;

namespace SecretProject.Class.StageFolder
{
    public class Wilderness : StageBase
    {
        public List<Boar> Boars;
        public Boar Boar;
        public Wilderness(string name, GraphicsDevice graphics, ContentManager content, int tileSetNumber, string mapTexturePath, string tmxMapPath, int dialogueToRetrieve) : base(name, graphics, content, tileSetNumber, mapTexturePath, tmxMapPath, dialogueToRetrieve)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;


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

            AllLights = new List<LightSource>()
            {

            };

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



            this.Map = new TmxMap("Content/Map/worldMap.tmx");
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
            TileWidth = Map.Tilesets[0].TileWidth;
            TileHeight = Map.Tilesets[0].TileHeight;

            TilesetTilesWide = TileSet.Width / TileWidth;
            TilesetTilesHigh = TileSet.Height / TileHeight;

            Boar = new Boar("Boar", new Vector2(900, 200), Graphics, Game1.AllTextures.EnemySpriteSheet);
            Boars = new List<Boar>() { };
            for (int i = 0; i < 10; i++)
            {
                Boars.Add(new Boar("Boar", new Vector2(100 * i, 200), Graphics, Game1.AllTextures.EnemySpriteSheet));
            }

            AllActions = new List<ActionTimer>();

            this.Cam = camera;
            Cam.Zoom = 3f;
            MapRectangle = new Rectangle(0, 0, TileWidth * 100, TileHeight * 100);
            Map = null;

            AllItems.Add(Game1.ItemVault.GenerateNewItem(129, new Vector2(500, 500), true));
            //AllDockDialogue = Content.Load<DialogueHolder>("Dialogue/AllDialogue");
            //Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(1, 1);

            TextBuilder = new TextBuilder(Game1.DialogueLibrary.RetrieveDialogue(1, 1), .1f, 5f);
            this.SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;


        }

        public override void UnloadContent()
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
            //this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
        }


        #region UPDATE
        public override void Update(GameTime gameTime, MouseManager mouse, Player player)
        {

            this.IsDark = Game1.GlobalClock.IsNight;
            //Game1.Player.UserInterface.TextBuilder.PositionToWriteTo = ElixerNPC.Position;
            //keyboard
            for (int p = 0; p < AllPortals.Count; p++)
            {
                if (player.Rectangle.Intersects(AllPortals[p].PortalStart))
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
            //if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Y)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Y)))
            //{
            //    ElixerNPC.IsUpdating = !ElixerNPC.IsUpdating;
            //    //TextBuilder.IsActive = !TextBuilder.IsActive;
            //    //ParticleEngine.ActivationTime = 5f;
            //    //ParticleEngine.InvokeParticleEngine(gameTime, 20, mouse.WorldMousePosition);
            //}


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
                    //Boars[e].MoveTowardsPosition(Game1.Player.Position,Game1.Player.Rectangle);
                }
                Boar.Update(gameTime, AllObjects, mouse);
                //Boar.MoveTowardsPosition(Game1.Player.Position, Game1.Player.Rectangle);
                // ElixerNPC.MoveToTile(gameTime, new Point(40, 40));
                // Dobbin.MoveToTile(gameTime, new Point(23, 55));


            }
        }
        #endregion

        #region DRAW
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
                    if (Game1.Player.UserInterface.DrawTileSelector && Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 4)
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

                player.Draw(spriteBatch, .4f);
                Console.WriteLine("Player Position" + player.position);


                Boar.Draw(spriteBatch);
                for (int e = 0; e < Boars.Count; e++)
                {
                    Boars[e].Draw(spriteBatch);

                }

                TextBuilder.Draw(spriteBatch, .71f);

                if (ShowBorders)
                {

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
    }
}
#endregion