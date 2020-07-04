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
    public class InkWizard : Enemy
    {
        public InkWizard(List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container) : base( pack, position, graphics, container)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 128, 16, 32, 4, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 64, 128, 16, 32, 4, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 128, 128, 16, 32, 4, .15f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 128, 128, 16, 32, 4, .15f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;

            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.ChickenCluck1;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 35f;
            this.SoundTimer = Game1.Utility.RFloat(45f, 100f);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 2;
            this.DamageColor = Color.Brown;
            this.PossibleLoot = new List<ItemStuff.Loot>() { new Loot(215, 100) };
        }
    }
}
