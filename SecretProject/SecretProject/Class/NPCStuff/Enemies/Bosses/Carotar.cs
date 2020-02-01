using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
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

namespace SecretProject.Class.NPCStuff.Enemies.Bosses
{
    public class Carotar : Enemy
    {
        public SimpleTimer TimeInAirTimer { get; set; }
        public SimpleTimer TimeBetweenAttacks { get; set; }

        public Carotar(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[1];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 32, 32, 5, .15f, this.Position);

            this.NPCRectangleXOffSet = 15;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 4;
            this.NPCRectangleWidthOffSet = 4;
            this.Speed = .05f;
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.PigGrunt;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 30f;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.HitPoints = 5;
            this.DamageColor = Color.Black;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };

            this.TimeInAirTimer = new SimpleTimer(6f);
            this.TimeBetweenAttacks = new SimpleTimer(3f);
        }


        public void AttackPlayer(GameTime gameTime)
        {
            
            if(!this.TimeInAirTimer.Run(gameTime))
            {
                if (this.TimeInAirTimer.Time < this.TimeInAirTimer.TargetTime / 2)
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y - 1f);
                    this.NPCAnimatedSprite[0].UpdateAnimations(gameTime, this.Position);
                    if(this.NPCAnimatedSprite[0].CurrentFrame == 0)
                    {
                        this.NPCAnimatedSprite[0].CurrentFrame = 4;
                    }
                }
                else
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y + 1f);
                    if (this.NPCAnimatedSprite[0].CurrentFrame == 1)
                    {
                        this.NPCAnimatedSprite[0].CurrentFrame = 0;
                    }
                }
            }
            else
            {
                this.CurrentBehaviour = CurrentBehaviour.Flee;
            }
           
        }

        public override void Update(GameTime gameTime, MouseManager mouse, List<Enemy> enemies = null)
        {

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
            //this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.IsMoving = true;
            TestImmunity(gameTime);
            this.PrimaryVelocity = new Vector2(.5f, .5f);
            this.Collider.Rectangle = new Rectangle((int)(this.Position.X + this.NPCRectangleXOffSet / 2), (int)(this.Position.Y + this.NPCRectangleYOffSet / 2), (int)(this.NPCRectangleWidthOffSet * 2), (int)(this.NPCRectangleHeightOffSet * 2));
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {

                this.CollideOccured = true;
                if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
                {
                    if (this.Collider.IsIntersecting(Game1.Player.MainCollider))
                    {
                        if (!Game1.Player.IsImmuneToDamage)
                        {
                            Game1.Player.PlayerCollisionInteraction(1, 200, this.CurrentDirection);
                        }

                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.grass)
                {
                    if (this.Collider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].IsUpdating = true;
                        returnObjects[i].InitialShuffDirection = this.CurrentDirection;
                    }
                }
                else
                {
                    if (this.IsMoving)
                    {


                        //if (returnObjects[i].ColliderType == ColliderType.inert)
                        //{
                        //    Collider.HandleMove(Position, ref primaryVelocity, returnObjects[i]);
                        //}
                    }

                }


                // IsMoving = false;



            }

            UpdateDirection();

            if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
            {
                mouse.ChangeMouseTexture(CursorType.Normal);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;

            }

            if (this.IsMoving)
            {
                switch (this.CurrentBehaviour)
                {
                    case CurrentBehaviour.Wander:
                        //Wander(gameTime);
                        break;
                    case CurrentBehaviour.Chase: //this is fight 
                       // MoveTowardsPoint(new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y), gameTime);
                        AttackPlayer(gameTime);
                        break;


                    case CurrentBehaviour.Hurt:
                        this.CurrentBehaviour = CurrentBehaviour.Chase;


                        break;

                    case CurrentBehaviour.Flee:
                        //if has waited for a bit carotar will start hopping again
                        if(TimeBetweenAttacks.Run(gameTime))
                        {
                            this.CurrentBehaviour = CurrentBehaviour.Chase;
                        }
                        
                        break;


                }


            }


            if (this.IsMoving)
            {

                for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].UpdateAnimations(gameTime, this.Position);
                }



            }
            else
            {
                for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].SetFrame(0);
                }


            }
            if (this.IdleSoundEffect != null)
            {
                this.SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.SoundTimer <= 0)
                {
                    Game1.SoundManager.PlaySoundEffectInstance(this.IdleSoundEffect, true, 1f);
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


            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X - this.NPCRectangleXOffSet - 8, this.Position.Y - this.NPCRectangleYOffSet - 8), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)));


        }
    }
}
