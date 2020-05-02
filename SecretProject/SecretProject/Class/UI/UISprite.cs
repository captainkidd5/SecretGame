using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public enum UISpriteType
    {
        Coin = 1,
    }
    public class UISprite
    {
        public Sprite Sprite { get; set; }
        public bool HasArrivedAtDestination { get; set; }
        public Vector2 Destination { get; set; }
        public float Speed { get; set; }
        public List<UISprite> AllUISprites { get; set; }

        public UISprite(UISpriteType uiSpriteType, GraphicsDevice graphics, Vector2 position, Vector2 destination, List<UISprite> allUISprites, int speedMin = 10, int speedMax = 30)
        {
            switch(uiSpriteType)
            {
                case UISpriteType.Coin:
                    this.Sprite = new Sprite(graphics, Game1.AllTextures.UserInterfaceTileSet, new Rectangle(16, 320, 32, 32), position, 32, 32);
                    this.Destination = destination;
                    this.Speed = Game1.Utility.RGenerator.Next(speedMin, speedMax);
                    break;
            }
            this.AllUISprites = allUISprites;
        }

        public void Update(GameTime gameTime)
        {
            if(MoveTowardsPoint(this.Destination, gameTime))
            {
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.CoinGet, false, 1f);
               // Game1.Player.Inventory.Money++;
                this.AllUISprites.Remove(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Sprite.Draw(spriteBatch, 1f);
        }

        private bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {

            if (this.Sprite.Position == goal) return true;


            Vector2 direction = Vector2.Normalize(goal - this.Sprite.Position);

            this.Sprite.Position += direction * this.Speed;


            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Sprite.Position)) + 1) < 0.1f)
                this.Sprite.Position = goal;


            return this.Sprite.Position == goal;
        }
    }
}
