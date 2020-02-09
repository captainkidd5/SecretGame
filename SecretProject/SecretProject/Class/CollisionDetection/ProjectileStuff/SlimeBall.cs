﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{
    class SlimeBall : Projectile
    {
        public SlimeBall(GraphicsDevice graphics, Collider colliderFiredFrom, Dir directionFiredFrom, Vector2 startPosition, float rotation, float speed, Vector2 positionToMoveToward, List<Projectile> allProjectiles, bool damagesPlayer) : base(graphics, colliderFiredFrom, directionFiredFrom, startPosition, rotation, speed, positionToMoveToward, allProjectiles, damagesPlayer)
        {
            this.SourceRectangle = Game1.ItemVault.GenerateNewItem(255, null).SourceTextureRectangle;
            this.MissSound = Game1.SoundManager.SlimeHit;
        }

        public override void Miss()
        {
            Game1.SoundManager.PlaySoundEffectInstance(this.MissSound, true, .15f);
            Game1.GetCurrentStage().ParticleEngine.ActivationTime = .05f;
            Game1.GetCurrentStage().ParticleEngine.EmitterLocation = this.CurrentPosition;
            Game1.GetCurrentStage().ParticleEngine.Color = Color.Green;
        }
    }
}