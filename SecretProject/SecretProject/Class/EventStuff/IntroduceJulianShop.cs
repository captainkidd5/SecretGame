using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.EventStuff
{
    public class IntroduceJulianShop : IEvent
    {
        public GraphicsDevice Graphics { get; set; }
        public List<Character> CharactersInvolved { get; set; }
        public bool FreezePlayerControls { get; set; }
        public int DayToTrigger { get; set; }
        public int StageToTrigger { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStep { get; set; }
        public int TotalSteps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IntroduceJulianShop(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.CharactersInvolved = new List<Character>()
            {
                Game1.Julian
            };
            this.FreezePlayerControls = false;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.JulianHouse;
            this.IsCompleted = false;
            this.CurrentStep = 0;

        }
        public void Start()
        {
            if (!Game1.GetCurrentStage().CharactersPresent.Contains(Game1.Julian))
            {
                Game1.GetCurrentStage().CharactersPresent.Add(Game1.Julian);
            }

            Game1.Julian.CurrentStageLocation = Stages.JulianHouse;
            Game1.Julian.IsInEvent = true;
            Game1.Julian.Position = new Vector2(340, 580);
            //  Game1.Julian.ResetPathFinding();
            this.FreezePlayerControls = true;
            this.IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Game1.freeze)
            {
                Game1.cam.Follow(new Vector2(Game1.Player.Position.X + 8, Game1.Player.Position.Y + 16), Game1.GetCurrentStage().MapRectangle);
                Game1.Player.Update(gameTime, Game1.GetCurrentStage().AllItems, Game1.myMouseManager);
                Game1.Julian.EventUpdate(gameTime);
            }

            if (Game1.Player.UserInterface.IsTransitioning)
            {
                Game1.Player.UserInterface.BeginTransitionCycle(gameTime);
            }

            Game1.Player.UserInterface.TextBuilder.Update(gameTime);
            switch (this.CurrentStep)
            {
                case 0:
                    Game1.Julian.EventMoveToTile(gameTime, new Point(26, 36));
                    if (Game1.Julian.CurrentPath.Count <= 0)
                    {
                        Game1.freeze = true;
                        Game1.Julian.UpdateDirectionVector(Game1.Player.position);
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " + "This is my shop. I sell all sorts of tools and can even repair your gadgets! ", 2f, null, null);
                        this.CurrentStep = 1;
                    }
                    break;
                case 1:
                    if (!Game1.Player.UserInterface.TextBuilder.IsActive)
                    {


                        Game1.Julian.EventMoveToTile(gameTime, new Point(41, 35));
                        if (Game1.Julian.CurrentPath.Count <= 0)
                        {
                            Game1.Julian.UpdateDirectionVector(new Vector2(Game1.Julian.Position.X, Game1.Julian.Position.Y - 10));
                            Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                                "This bookshelf contains all sorts of useful information. If you find any of the pages let me know and we might be able to learn a thing or two.", 2f, null, null);
                            this.CurrentStep = 2;
                        }
                    }
                    break;
                case 2:
                    Game1.Julian.EventMoveToTile(gameTime, new Point(21, 36));
                    if (Game1.Julian.CurrentPath.Count <= 0)
                    {
                        Game1.Julian.UpdateDirectionVector(new Vector2(Game1.Player.Position.X, Game1.Player.Position.Y));
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                            "Anyway, it was nice to meet you!", 2f, null, null);
                        this.CurrentStep = 3;
                    }

                    break;
                case 3:
                    Console.WriteLine("Event has ended");
                    Game1.IsEventActive = false;
                    this.IsActive = false;
                    this.IsCompleted = true;
                    Game1.Julian.IsInEvent = false;
                    break;

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
