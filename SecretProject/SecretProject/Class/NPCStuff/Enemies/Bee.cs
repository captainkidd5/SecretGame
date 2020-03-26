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
    public class Bee : Enemy
    {
        public Bee(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 144, 32, 16, 16, 2, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 176, 32, 16, 16, 2, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 208, 32, 16, 16, 2, .15f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 208, 32, 16, 16, 2, .15f, this.Position);

            this.NPCRectangleXOffSet = 15;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 8;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.ChickenCluck1;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 35f;
            this.SoundTimer = Game1.Utility.RFloat(45f, 100f);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 2;
            this.DamageColor = Color.Yellow;
            this.PossibleLoot = new List<ItemStuff.Loot>() { new Loot(296, 100) };
        }
    }
}
