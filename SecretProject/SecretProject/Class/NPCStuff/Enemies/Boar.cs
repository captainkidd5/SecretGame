using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Boar : Enemy
    {
        public Boar( List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container) : base(pack, position, graphics, container)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 48, 32, 3, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 288, 0, 48, 32, 3, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 432, 0, 48, 32, 3, .15f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 8;
            this.NPCRectangleHeightOffSet =24;
            this.NPCRectangleWidthOffSet = 24;
            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.PigGrunt;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 30f;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 5;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100), new Loot(254, 50), new Loot(214, 25) };
            this.MakesPeriodicSound = true;
        }
    }
}
