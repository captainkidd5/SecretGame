﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public enum SporeShooterState
    {
        Vulnerable = 0,
        Shooting = 1,
        Hiding = 2, 
    }
    public class SporeShooter : Enemy
    {
        SimpleTimer ShootTimer;
        SimpleTimer AttackCooldown;
        SimpleTimer HideTimer;
        int ShotsFiredDuringInterval;
        public SporeShooterState ShooterState { get; set; }

        public SporeShooter( List<Enemy> pack, Vector2 position, GraphicsDevice graphics, TileManager TileManager ) : base(pack, position, graphics, TileManager)
        {
            this.NPCAnimatedSprite = new Sprite[3];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 384, 80, 16, 16, 1, .2f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 400, 80, 16, 16, 1, .2f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 416, 80, 16, 16, 1, .2f, this.Position);

            this.CurrentDirection = Dir.Down;
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            this.Speed = .02f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.BushCut;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 50;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.HitPoints = 2;
            this.DamageColor = Color.DarkSeaGreen;
            this.PossibleLoot = new List<Loot>() { new Loot(255, 100) };

            this.ShootTimer = new SimpleTimer(3f);
            this.AttackCooldown = new SimpleTimer(.75f);
            this.ShotsFiredDuringInterval = 0;
            this.ShooterState = SporeShooterState.Hiding;
            this.HideTimer = new SimpleTimer(2f);
            this.IsImmuneToDamage = true;
        }

        public void AttackPlayer(GameTime gameTime)
        {
            this.CurrentDirection = Dir.Down;
            if(this.ShotsFiredDuringInterval < 3)
            {
                if (AttackCooldown.Run(gameTime))
                {


                 //   float angleFromTarget = Game1.Utility.GetAngleBetweenTwoVectors(this.Position, new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y));
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SporeShoot, true, .15f);
                  //  StageManager.CurrentStage.AllProjectiles.Add(new SlimeBall(this.Graphics,this.Collider, this.CurrentDirection, new Vector2(this.Position.X + 8, this.Position.Y + 8), MathHelper.ToRadians(angleFromTarget - 90), 160f, Vector2.Zero, StageManager.CurrentStage.AllProjectiles,true, 2));
                    this.ShotsFiredDuringInterval++;
                    
                }
            }
            else
            {
                this.CurrentBehaviour = CurrentBehaviour.Hurt;
            }
                     
        }

        public override void Update(GameTime gameTime)
        {

            this.IsMoving = true;
            //if (this.TimeInUnloadedChunk > 100)
            //{
            //    enemies.Remove(this);
            //    return;
            //}
            if (this.HitPoints <= 0)
            {
                RollPeriodicDrop(this.Position);
                Pack.Remove(this);
                return;
            }


            //this.Collider.Rectangle = this.NPCHitBoxRectangle;
            //List<ICollidable> returnObjects = new List<ICollidable>();
         
            //for (int i = 0; i < returnObjects.Count; i++)
            //{

            //    this.CollideOccured = true;
            //    if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
            //    {
            //       if(this.Collider.Rectangle.Intersects(returnObjects[i].Rectangle))
            //        {
            //            this.CurrentBehaviour = CurrentBehaviour.Chase;
            //        }
            //    }

            //}
            this.NPCAnimatedSprite[0].UpdateAnimations(gameTime, this.Position);
          //  UpdateDirection();

            



                switch (this.CurrentBehaviour)
                {
                    case CurrentBehaviour.Wander:
                        break;
                    case CurrentBehaviour.Chase:
                    this.IsImmuneToDamage = true;
                    this.ShooterState = SporeShooterState.Shooting;
                        AttackPlayer(gameTime);
                        break;


                    case CurrentBehaviour.Hurt: //vulnerable in this case
                    this.ShooterState = SporeShooterState.Vulnerable;
                    this.IsImmuneToDamage = false;
                    if(this.HideTimer.Run(gameTime))
                    {
                        this.CurrentBehaviour = CurrentBehaviour.Flee;
                    }
                        


                        break;

                    case CurrentBehaviour.Flee:

                    this.IsImmuneToDamage = true;
                    this.ShooterState = SporeShooterState.Hiding;
                    
                    if (this.ShootTimer.Run(gameTime))
                        {
                        this.ShotsFiredDuringInterval = 0;
                            this.CurrentBehaviour = CurrentBehaviour.Chase;
                        }


                        break;


                }


            



            if (this.IdleSoundEffect != null)
            {
                this.SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.SoundTimer <= 0)
                {
                    Game1.SoundManager.PlaySoundEffect(this.IdleSoundEffect, true, 1f);
                    this.SoundTimer = Game1.Utility.RFloat(this.SoundLowerBound, this.SoundUpperBound);

                    RollPeriodicDrop(this.Position);
                }
            }



        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, this.Position, .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)));
        }

        public override void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            if (!this.IsImmuneToDamage)
            {



                Stage.ParticleEngine.ActivationTime = .25f;
                Stage.ParticleEngine.EmitterLocation = this.Position;
                Stage.ParticleEngine.Color = this.DamageColor;


                this.CurrentBehaviour = CurrentBehaviour.Flee;
               

                TakeDamage(dmgAmount);
                this.IsImmuneToDamage = true;
            }
            else
            {

            }
        }
    }
}
