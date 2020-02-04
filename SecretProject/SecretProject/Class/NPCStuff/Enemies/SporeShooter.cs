using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
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
    public class SporeShooter : Enemy
    {
        public SporeShooter(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, spriteSheet, container, primaryPlayerInteractionBehavior)
        {
            this.NPCAnimatedSprite = new Sprite[3];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 384, 80, 16, 16, 2, .2f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 400, 80, 16, 16, 2, .2f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 416, 80, 16, 16, 2, .2f, this.Position);


            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 16;
            this.NPCRectangleWidthOffSet = 16;
            this.Speed = .02f;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.IdleSoundEffect = Game1.SoundManager.ToadCroak;
            this.SoundLowerBound = 20f;
            this.SoundUpperBound = 50;
            this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
            this.HitPoints = 2;
            this.DamageColor = Color.GreenYellow;
            this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };
        }

        public void AttackPlayer(GameTime gameTime)
        {

            

        }

        public override void Update(GameTime gameTime, MouseManager mouse, List<Enemy> enemies = null)
        {
            this.IsImmuneToDamage = true;

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





                        break;


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
    }
}
