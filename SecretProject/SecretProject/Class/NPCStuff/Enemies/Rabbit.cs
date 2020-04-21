using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Rabbit : Enemy
    {
        public Rabbit(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 144, 48, 16, 16, 3, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 192, 48, 16, 16, 3, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 240, 48, 16, 16, 3, .15f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 240, 48, 16, 16, 3, .15f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 8;
            this.NPCRectangleHeightOffSet = 4;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.RabbitWeet;
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
