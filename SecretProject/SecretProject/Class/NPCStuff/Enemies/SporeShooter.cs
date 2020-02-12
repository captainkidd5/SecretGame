using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
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

        public SporeShooter(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[3];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 384, 80, 16, 16, 1, .2f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 400, 80, 16, 16, 1, .2f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 416, 80, 16, 16, 1, .2f, this.Position);

            this.CurrentDirection = Dir.Down;

            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 16;
            this.NPCRectangleWidthOffSet = 16;
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


                    float angleFromTarget = Game1.Utility.GetAngleBetweenTwoVectors(this.Position, new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y));
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SporeShoot, true, .15f);
                    Game1.GetCurrentStage().AllProjectiles.Add(new SlimeBall(this.Graphics,this.Collider, this.CurrentDirection, new Vector2(this.Position.X + 8, this.Position.Y + 8), MathHelper.ToRadians(angleFromTarget - 90), 160f, Vector2.Zero, Game1.GetCurrentStage().AllProjectiles,true, 2));
                    this.ShotsFiredDuringInterval++;
                    
                }
            }
            else
            {
                this.CurrentBehaviour = CurrentBehaviour.Hurt;
            }
                     
        }

        public override void Update(GameTime gameTime, MouseManager mouse, List<Enemy> enemies = null)
        {

            this.IsMoving = true;
            if (this.TimeInUnloadedChunk > 100)
            {
                enemies.Remove(this);
                return;
            }
            if (this.HitPoints <= 0)
            {
                RollPeriodicDrop(this.Position);
                enemies.Remove(this);
                return;
            }


            this.Collider.Rectangle = new Rectangle((int)(this.Position.X + this.NPCRectangleXOffSet / 2), (int)(this.Position.Y + this.NPCRectangleYOffSet / 2), (int)(this.NPCRectangleWidthOffSet * 2), (int)(this.NPCRectangleHeightOffSet * 2));
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {

                this.CollideOccured = true;
                if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
                {
                   if(this.Collider.Rectangle.Intersects(returnObjects[i].Rectangle))
                    {
                        this.CurrentBehaviour = CurrentBehaviour.Chase;
                    }
                }

            }
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

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        {
            if (this.CurrentBehaviour == CurrentBehaviour.Hurt)
            {

                //Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue((float)Math.Sin((float)this.PulseTimer.Time * 2 / this.PulseTimer.TargetTime + (float)Math.PI / 2 * ((float)(Math.PI * 3))));
                //Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
                //Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();



            }


            this.NPCAnimatedSprite[(int)this.ShooterState].DrawAnimation(spriteBatch, new Vector2(this.Position.X , this.Position.Y), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[(int)this.ShooterState].DestinationRectangle.Y)));


        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X - this.NPCRectangleXOffSet - 8, this.Position.Y - this.NPCRectangleYOffSet - 8), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)));
        }

        public override void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            if (!this.IsImmuneToDamage)
            {



                Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = this.Position;
                Game1.GetCurrentStage().ParticleEngine.Color = this.DamageColor;


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
