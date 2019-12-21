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
    public class Dog : Enemy
    {
        public Dog(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 288, 0, 48, 32, 3, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position) { Flip = true };
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 144, 0, 48, 32, 3, .15f, this.Position);

            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = -8;
            this.NPCRectangleHeightOffSet = 4;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.SoundID = 14;
            this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 2;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };
        }
    }
}
