﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.Transitions
{
    public class LoadingScreen
    {
        public GraphicsDevice Graphics { get; private set; }
        public DayTransitioner DayTransitioner { get; private set; }


        public bool IsTransitioning { get; set; }
        public string Text { get; set; }
        public SimpleTimer TransitionTimer { get; set; }

        public Texture2D Texture { get; set; }

        public float ColorMultiplier { get; set; }
        public float Speed { get; set; }

        public LoadingScreen(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.Texture = Game1.Utility.GetColoredRectangle(this.Graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight, Color.Black);
            this.ColorMultiplier = 1f;
            this.Speed = .05f;
            this.TransitionTimer = new SimpleTimer(2f);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void BeginBlackTransition(float transitionSpeed = .005f, float targetTime = 2f, bool startOfNewDay = false)
        {
            this.TransitionTimer.ResetToZero();
            this.ColorMultiplier = 1f;
            this.IsTransitioning = true;
            this.Speed = transitionSpeed;
            this.TransitionTimer.TargetTime = targetTime;
            this.DayTransitioner = new DayTransitioner();
            if (startOfNewDay)
            {
                this.DayTransitioner.UpdateText();
                this.DayTransitioner.IsActive = true;
            }

        }

        public void BlackTransition(GameTime gameTime)
        {
            if (this.TransitionTimer.Time <= this.TransitionTimer.TargetTime)
            {
                this.ColorMultiplier -= this.Speed;
            }
            else
            {
                this.ColorMultiplier += this.Speed;
            }
            if (this.TransitionTimer.Run(gameTime))
            {
                this.ColorMultiplier = 1f;
                this.IsTransitioning = false;
                this.DayTransitioner.IsActive = false;
            }

        }

        public void DrawTransitionTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, Game1.Utility.Origin, null, Color.White * this.ColorMultiplier, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, .99f);
            this.DayTransitioner.Draw(spriteBatch, Color.White * this.ColorMultiplier);
        }
    }
}
