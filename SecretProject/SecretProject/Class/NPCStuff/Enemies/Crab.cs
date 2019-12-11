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
    public class Crab : Enemy
    {
        public Crab(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container) : base(name, position, graphics, spriteSheet, container)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 32, 48, 32, 1, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);

            this.NPCRectangleXOffSet = 15;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 4;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.SoundID = 14;
            this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
            this.HitPoints = 1;
            this.DamageColor = Color.Red;
            this.PossibleLoot = new List<Loot>() { new Loot(14, 75) };
        }
    }
}
