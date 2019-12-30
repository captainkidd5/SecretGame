using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Butterfly : Enemy
    {
        public float Rotation { get; set; }
        public float Angle { get; set; }
        public Vector2 FlutterOffset { get; set; }

        public SimpleTimer FlutterTimer { get; set; }

        public int FlutterDirection { get; set; }

        public Butterfly(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            NPCAnimatedSprite = new Sprite[1];
            int butterflyColor = Game1.Utility.RGenerator.Next(0, 4);
            switch (butterflyColor)
            {
                case 0:
                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 288, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 1:
                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 320, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 2:
                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 352, 48, 16, 16, 2, .15f, this.Position);
                    break;
                case 3:
                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 384, 48, 16, 16, 2, .15f, this.Position);
                    break;
            }

            


            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 8;
            this.NPCRectangleHeightOffSet = 4;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.SoundID = 0;
            this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
            this.HitPoints = 2;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };

            this.Rotation = 0f;
            this.FlutterOffset = new Vector2(0, 0);
            FlutterTimer = new SimpleTimer(.5f);
            FlutterDirection = 1;
        }

        public void Flutter(GameTime gameTime)
        {
            SwitchDirections(gameTime);
            Angle += (float)gameTime.ElapsedGameTime.TotalSeconds  * 4 * FlutterDirection;
            Rotation += Angle * .1f;
            if (Rotation > .5f)
            {
                Rotation = .5f;
            }
            if (Rotation < -.5f)
            {
                Rotation = -.5f;
            }

        }

        public void SwitchDirections(GameTime gameTime)
        {
            if(FlutterTimer.Run(gameTime))
            {
                FlutterDirection = FlutterDirection * -1;
                FlutterTimer.TargetTime = Game1.Utility.RFloat(.5f, 1f);
            }
        }

        public override void Update(GameTime gameTime, MouseManager mouse, List<Enemy> enemies = null)
        {
            base.Update(gameTime, mouse, enemies);
            Flutter(gameTime);


        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        {
            FlutterOffset = new Vector2((float)(10 * Math.Sin(Angle)), (float)(10 * Math.Cos(Angle)));
            NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(Position.X - NPCRectangleXOffSet - 8 + FlutterOffset.X, Position.Y - NPCRectangleYOffSet - 8 + FlutterOffset.Y), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y)), Rotation);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            FlutterOffset = new Vector2((float)(10 * Math.Sin(Angle)), (float)(10 * Math.Cos(Angle)));
            NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(Position.X - NPCRectangleXOffSet - 8 + FlutterOffset.X, Position.Y - NPCRectangleYOffSet - 8 + FlutterOffset.Y), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y)), Rotation);
        }
    }
}
