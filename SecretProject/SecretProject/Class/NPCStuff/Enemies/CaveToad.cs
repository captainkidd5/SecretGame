using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class CaveToad : Enemy
    {
        public CaveToad( List<Enemy> pack, Vector2 position, GraphicsDevice graphics , IInformationContainer container) : base(pack, position, graphics, container)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 288, 80, 16, 16, 2, .2f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 320, 80, 16, 16, 2, .2f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 352, 80, 16, 16, 2, .2f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 352, 80, 16, 16, 2, .2f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 16;
            this.NPCRectangleWidthOffSet = 16;
            this.Speed = .02f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.ToadCroak;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 50;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.HitPoints = 2;
            this.DamageColor = Color.GreenYellow;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };
            this.MakesPeriodicSound = true;
        }
    }
}
