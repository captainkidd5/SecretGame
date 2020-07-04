using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Butterfly : Enemy
    {
        public float Rotation { get; set; }
        public float Angle { get; set; }
        public Vector2 FlutterOffset { get; set; }

        public SimpleTimer FlutterTimer { get; set; }

        public int FlutterDirection { get; set; }

        public Butterfly( List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container) : base( pack, position, graphics, container)
        {
            this.NPCAnimatedSprite = new Sprite[1];
            int butterflyColor = Game1.Utility.RGenerator.Next(0, 4);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            switch (butterflyColor)
            {
                case 0:
                    this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 288, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 1:
                    this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 320, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 2:
                    this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 352, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 3:
                    this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 384, 48, 16, 16, 2, .15f, this.Position);
                    break;
            }

            this.IdleSoundEffect = Game1.SoundManager.SlimeHit;


            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
            this.HitPoints = 2;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(295, 100) };
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 30f;
            this.Rotation = 0f;
            this.FlutterOffset = new Vector2(0, 0);
            this.FlutterTimer = new SimpleTimer(.5f);
            this.FlutterDirection = 1;
        }

        public void Flutter(GameTime gameTime)
        {
            SwitchDirections(gameTime);
            this.Angle += (float)gameTime.ElapsedGameTime.TotalSeconds * 4 * this.FlutterDirection;
            this.Rotation += this.Angle * .1f;
            if (this.Rotation > .5f)
            {
                this.Rotation = .5f;
            }
            if (this.Rotation < -.5f)
            {
                this.Rotation = -.5f;
            }

        }

        public override void QuadTreeInsertion()
        {
            //no insertion at all bc butterflies are too light to move anything lul
        }

        public void SwitchDirections(GameTime gameTime)
        {
            if (this.FlutterTimer.Run(gameTime))
            {
                this.FlutterDirection = this.FlutterDirection * -1;
                this.FlutterTimer.TargetTime = Game1.Utility.RFloat(.5f, 1f);
            }
        }

        public override void Update(GameTime gameTime, MouseManager mouse, Rectangle cameraRectangle, List<Enemy> enemies = null)
        {
            base.Update(gameTime, mouse,cameraRectangle, enemies);
            Flutter(gameTime);


        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        {
            this.FlutterOffset = new Vector2((float)(10 * Math.Sin(this.Angle)), (float)(10 * Math.Cos(this.Angle)));
            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X + this.FlutterOffset.X, this.Position.Y + this.FlutterOffset.Y), .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)), this.Rotation);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            this.FlutterOffset = new Vector2((float)(10 * Math.Sin(this.Angle)), (float)(10 * Math.Cos(this.Angle)));
            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X + this.FlutterOffset.X, this.Position.Y  + this.FlutterOffset.Y), .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)), this.Rotation);
        }
    }
}
