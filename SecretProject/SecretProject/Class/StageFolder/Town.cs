
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{

    public class Town : TmxStageBase, ILocation
    {

        #region FIELDS



        #endregion

        #region CONSTRUCTOR


        Dog Nelja;


        public Town(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, int tileSetNumber, Texture2D tileSet, string tmxMapPath, int dialogueToRetrieve, int backDropNumber) : base(name, locationType, stageType, graphics, content, tileSetNumber, tileSet, tmxMapPath, dialogueToRetrieve, backDropNumber)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;

            if (this.BackDropNumber == 1)
            {
                BackDropPosition = new Vector2(0, 200);
            }

        }

        public override void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {

            var pp = this.Graphics.PresentationParameters;

            List<Texture2D> particleTextures = new List<Texture2D>();
            particleTextures.Add(Game1.AllTextures.RockParticle);
            this.ParticleEngine = new ParticleEngine(particleTextures, Game1.Utility.centerScreen);





            this.Cam = camera;
            this.Cam.pos.X = Game1.Player.position.X;
            this.Cam.pos.Y = Game1.Player.position.Y;



            this.TextBuilder = new TextBuilder("", .1f, 5f);
            SceneChanged += Game1.Player.UserInterface.HandleSceneChanged;
            this.AllTextToWrite = new List<StringWrapper>();
            this.QuadTree = new QuadTree(5, this.Cam.ViewPortRectangle);
            Nelja = new Dog("Nelja", new Vector2(1200, 1300), this.Graphics, Game1.AllTextures.Nelja, (IInformationContainer)this.AllTiles, CurrentBehaviour.Wander) { IsWorldNPC = false };
            this.IsLoaded = true;
            for(int b = 0; b < 5; b++)
            {
                this.Enemies.Add(new Butterfly("butterfly", new Vector2(1200, 1300), this.Graphics, Game1.AllTextures.EnemySpriteSheet, (IInformationContainer)this.AllTiles, CurrentBehaviour.Wander) { IsWorldNPC = false });
            }
          
        }

        public override void UnloadContent()
        {
            //Content.Unload();
            //AllObjects = null;
            // AllLayers = null;
            //AllTiles = null;
            //AllSprites = null;
            //AllDepths = null;
            //AllItems = null;
            //Background = null;
            // MidGround = null;
            // foreGround = null;


            // this.Cam = null;
            //this.SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
        }


        #endregion
        #region UPDATE
        public override void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            player.CollideOccured = false;
            this.QuadTree = new QuadTree(0, this.Cam.ViewPortRectangle);
            for (int i = 0; i < this.Enemies.Count; i++)
            {
                this.Enemies[i].Update(gameTime, mouse);
            }
            foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.Objects)
            {
                for (int z = 0; z < obj.Value.Count; z++)
                {
                    this.QuadTree.Insert(obj.Value[z]);
                }
            }
            for (int i = 0; i < this.AllItems.Count; i++)
            {
                this.QuadTree.Insert(this.AllItems[i].ItemSprite);
            }

            this.QuadTree.Insert(player.BigCollider);

            this.QuadTree.Insert(player.MainCollider);

            foreach (KeyValuePair<string, List<GrassTuft>> grass in this.AllTiles.Tufts)
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

            foreach (Character character in this.CharactersPresent)
            {
                this.QuadTree.Insert(character.Collider);
            }
            float playerOldYPosition = player.position.Y;
            this.IsDark = Game1.GlobalClock.IsNight;
            Game1.myMouseManager.ToggleGeneralInteraction = false;
            Game1.isMyMouseVisible = true;
            //Game1.Player.UserInterface.TextBuilder.PositionToWriteTo = ElixerNPC.Position;
            //keyboard
            Nelja.Update(gameTime, Game1.myMouseManager);
            for (int p = 0; p < this.AllPortals.Count; p++)
            {
                if (player.ClickRangeRectangle.Intersects(this.AllPortals[p].PortalStart) && this.AllPortals[p].MustBeClicked)
                {
                    if (mouse.WorldMouseRectangle.Intersects(this.AllPortals[p].PortalStart))
                    {
                        Game1.isMyMouseVisible = false;
                        Game1.myMouseManager.ToggleGeneralInteraction = true;
                        mouse.ChangeMouseTexture(CursorType.Door);
                        if (mouse.IsClicked)
                        {
                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DoorOpen);
                            Game1.SwitchStage((Stages)this.AllPortals[p].From, (Stages)this.AllPortals[p].To, this.AllPortals[p]);
                            OnSceneChanged();
                            SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                            return;
                        }
                    }


                }
                else if (player.Rectangle.Intersects(this.AllPortals[p].PortalStart) && !this.AllPortals[p].MustBeClicked)
                {
                    Game1.SwitchStage((Stages)this.AllPortals[p].From, (Stages)this.AllPortals[p].To, this.AllPortals[p]);
                    OnSceneChanged();
                    SceneChanged -= Game1.Player.UserInterface.HandleSceneChanged;
                    return;
                }
            }




            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F1)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F1)))
            {
                this.ShowBorders = !this.ShowBorders;

            }
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.F2)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.F2)))
            {

                player.Position = new Vector2(400, 400);
                Game1.GlobalClock.TotalHours = 22;
            }


            this.TextBuilder.PositionToWriteTo = Game1.Elixir.Position;
            this.TextBuilder.Update(gameTime);

            //ParticleEngine.EmitterLocation = mouse.WorldMousePosition;
            Game1.Player.UserInterface.Update(gameTime, Game1.NewKeyBoardState, Game1.OldKeyBoardState, player.Inventory, mouse);
            if (Game1.CurrentWeather != WeatherType.None)
            {
                Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, this.LocationType);
            }
            this.ParticleEngine.Update(gameTime);

            if (!Game1.freeze)
            {
                Game1.GlobalClock.Update(gameTime);
                //--------------------------------------
                //Update players

                player.Update(gameTime, this.AllItems, mouse);
                this.Cam.Follow(new Vector2(player.Position.X + 8, player.Position.Y + 16), this.MapRectangle);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Update(gameTime, this.AllRisingText);
                }
                //--------------------------------------
                //Update sprites
                foreach (Sprite spr in this.AllSprites)
                {
                    if (spr.IsBeingDragged == true)
                    {
                        spr.Update(gameTime, mouse.WorldMousePosition);
                    }
                }

                this.AllTiles.Update(gameTime, mouse);
                
                for (int s = 0; s < this.AllTextToWrite.Count; s++)
                {
                    this.AllTextToWrite[s].Update(gameTime, this.AllTextToWrite);
                }

                for (int i = 0; i < this.AllItems.Count; i++)
                {
                    this.AllItems[i].Update(gameTime);
                }

                foreach (Character character in Game1.AllCharacters)
                {
                    character.Update(gameTime, mouse);
                }
                //Game1.Kaya.Update(gameTime, AllObjects, mouse);
                //Boar.MoveTowardsPosition(Game1.Player.Position, Game1.Player.Rectangle);
                // ElixerNPC.MoveToTile(gameTime, new Point(40, 40));
                // Dobbin.MoveToTile(gameTime, new Point(23, 55));


            }
            Game1.Player.controls.UpdateKeys();

        }
        #endregion

        #region DRAW
        public override void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, RenderTarget2D dayLightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {

            if (player.Health > 0)
            {
                if (this.IsDark)
                {


                    graphics.SetRenderTarget(lightsTarget);
                    //graphics.Clear(Color.White);
                    graphics.Clear(new Color(50, 50, 50, 256));
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: this.Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    for (int l = 0; l < this.AllNightLights.Count; l++)
                    {
                        this.AllNightLights[l].Draw(spriteBatch);
                    }
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() == 4)
                    {
                        spriteBatch.Draw(Game1.AllTextures.lightMask, new Vector2(mouse.WorldMousePosition.X - Game1.AllTextures.lightMask.Width / 2, mouse.WorldMousePosition.Y - Game1.AllTextures.lightMask.Height / 2), Color.White);
                    }
                    spriteBatch.End();
                }
                else
                {
                    graphics.SetRenderTarget(dayLightsTarget);
                    graphics.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: this.Cam.getTransformation(graphics));
                    graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                    

                    for (int dl = 0; dl < this.AllDayTimeLights.Count; dl++)
                    {
                        this.AllDayTimeLights[dl].Draw(spriteBatch);
                    }

                    spriteBatch.End();
                }
               

                graphics.SetRenderTarget(mainTarget);
                graphics.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: this.Cam.getTransformation(graphics));

                graphics.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                if (Game1.CurrentWeather != WeatherType.None)
                {
                    Game1.AllWeather[Game1.CurrentWeather].Draw(spriteBatch, this.LocationType);
                }

                this.ParticleEngine.Draw(spriteBatch);
                for (int i = 0; i < this.Enemies.Count; i++)
                {
                    this.Enemies[i].Draw(spriteBatch, this.Graphics);
                }
                Nelja.Draw(spriteBatch, this.Graphics);
                player.Draw(spriteBatch, .5f + (player.Rectangle.Y + player.Rectangle.Height) * Game1.Utility.ForeGroundMultiplier);
                for (int i = 0; i < this.AllRisingText.Count; i++)
                {
                    this.AllRisingText[i].Draw(spriteBatch);
                }


                foreach (Character character in Game1.AllCharacters)
                {
                    character.Draw(spriteBatch);
                }


                this.TextBuilder.Draw(spriteBatch, .71f);

                if (this.ShowBorders)
                {
                    player.DrawDebug(spriteBatch, .4f);
                    //ElixerNPC.DrawDebug(spriteBatch, .4f);
                    Nelja.DrawDebug(spriteBatch, 1f);

                    foreach (Character character in this.CharactersPresent)
                    {
                        character.DrawDebug(spriteBatch, .4f);
                    }
                }

                this.AllTiles.DrawTiles(spriteBatch);


                if (Game1.Player.UserInterface.DrawTileSelector)
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(Game1.Player.UserInterface.TileSelector.WorldX, Game1.Player.UserInterface.TileSelector.WorldY, 16, 16),
                        new Rectangle(48, 0, 16, 16), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .15f);
                }



                foreach (var sprite in this.AllSprites)
                {
                    sprite.Draw(spriteBatch, .7f);
                }

                for (int i = 0; i < this.AllItems.Count; i++)
                {
                    this.AllItems[i].Draw(spriteBatch);
                }

                foreach (KeyValuePair<string, List<ICollidable>> obj in this.AllTiles.Objects)
                {
                    if (this.ShowBorders)
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
                else
                {
                    Game1.AllTextures.whirlPoolGlow.Parameters["lightMask"].SetValue(dayLightsTarget);
                    Game1.AllTextures.whirlPoolGlow.CurrentTechnique.Passes[0].Apply();

                }



                spriteBatch.Draw(mainTarget, Game1.ScreenRectangle, Color.White);
                //spriteBatch.Draw(lightsTarget, Game1.ScreenRectangle, Color.White);

                spriteBatch.End();
            }
            Game1.Player.DrawUserInterface(spriteBatch);

            //  Graphics.SetRenderTarget(null);
        }
        #endregion
    }
}