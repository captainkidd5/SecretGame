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
    public class Goat : Enemy
    {
        public Goat(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 96, 48, 32, 5, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 240, 96, 48, 32, 5, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 480, 96, 48, 32, 5, .15f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 480, 96, 48, 32, 5, .15f, this.Position)  ;

            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 8;
            this.NPCRectangleHeightOffSet = 24;
            this.NPCRectangleWidthOffSet = 24;
            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.GoatBleat;
            this.SoundLowerBound = 40f;
            this.SoundUpperBound = 100f;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 5;
            this.DamageColor = Color.White;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100), new Loot(254, 50), new Loot(214, 25) };
            this.MakesPeriodicSound = true;
        }
    }
}
