﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Crab : Enemy
    {
        public Crab( List<Enemy> pack, Vector2 position, GraphicsDevice graphics, TileManager TileManager) : base( pack, position, graphics, TileManager)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 32, 48, 32, 1, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
            this.Texture = Game1.AllTextures.EnemySpriteSheet;

            this.Speed = .05f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.DigDirt;
            this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
            this.HitPoints = 1;
            this.DamageColor = Color.Red;
            this.PossibleLoot = new List<Loot>() { new Loot(14, 75) };
        }
    }
}
