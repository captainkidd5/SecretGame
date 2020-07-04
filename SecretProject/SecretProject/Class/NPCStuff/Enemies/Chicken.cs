using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Chicken : Enemy
    {
        public Chicken( List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container ) : base( pack, position, graphics, container)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 416, 48, 16, 16, 3, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 464, 48, 16, 16, 3, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 512, 48, 16, 16, 3, .15f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 512, 48, 16, 16, 3, .15f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;

            this.Speed = 1f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.ChickenCluck1;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 30f;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.HitPoints = 2;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };
            this.MakesPeriodicSound = true;
        }
    }
}
