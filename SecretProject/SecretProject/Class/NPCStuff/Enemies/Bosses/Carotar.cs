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
    public class Carotar
    {
        //public GraphicsDevice Graphics { get; set; }
        //public SimpleTimer TimeInAirTimer { get; set; }
        //public SimpleTimer TimeBetweenAttacks { get; set; }

        //public bool DrawShadow { get; set; }
        //public float ShadowScale { get; set; }
        //public Vector2 LandingSpot { get; set; }
        ////public Carotar(string name, List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior) : base(name, pack, position, graphics, container, primaryPlayerInteractionBehavior)
        ////{
        ////    this.NPCAnimatedSprite = new Sprite[1];

        ////    this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 32, 32, 5, .15f, this.Position);
        ////    this.Graphics = graphics;
        ////    this.NPCRectangleXOffSet = 15;
        ////    this.NPCRectangleYOffSet = 15;
        ////    this.NPCRectangleHeightOffSet = 32;
        ////    this.NPCRectangleWidthOffSet = 16;
        ////    this.Speed = .05f;
        ////    this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
        ////    this.IdleSoundEffect = Game1.SoundManager.PigGrunt;
        ////    this.SoundLowerBound = 20f;
        ////    this.SoundUpperBound = 30f;
        ////    this.SoundTimer = Game1.Utility.RFloat(SoundLowerBound, SoundUpperBound);
        ////    this.CurrentBehaviour = CurrentBehaviour.Wander;
        ////    this.HitPoints = 15;
        ////    this.DamageColor = Color.White;
        ////    this.PossibleLoot = new List<Loot>() { new Loot(294, 100) };

        ////    this.TimeInAirTimer = new SimpleTimer(6f);
        ////    this.TimeBetweenAttacks = new SimpleTimer(3f);
        ////    this.ShadowScale = 1f;
        ////    this.LandingSpot = Vector2.Zero;
        ////    this.PrimaryVelocity = new Vector2(4, 4);

        ////}


        //public void AttackPlayer(GameTime gameTime)
        //{
           
        //    if(!this.TimeInAirTimer.Run(gameTime))
        //    {
        //        if (this.TimeInAirTimer.Time < this.TimeInAirTimer.TargetTime / 4)
        //        {
        //            this.Position = new Vector2(this.Position.X, this.Position.Y - 4f);
        //            this.NPCAnimatedSprite[0].UpdateAnimations(gameTime, this.Position);
        //            if(this.NPCAnimatedSprite[0].CurrentFrame == 0)
        //            {
        //                this.NPCAnimatedSprite[0].SetFrame(4);
        //            }
        //            this.ShadowScale += .01f;
                    
        //        }
        //        else if (this.TimeInAirTimer.Time < this.TimeInAirTimer.TargetTime / 2)
        //        {
        //            DrawShadow = true;
        //            this.LandingSpot = new Vector2(Game1.Player.position.X + 10, Game1.Player.position.Y + 28);
        //            this.ShadowScale -= .01f;


        //        }
        //        else
        //        {
        //            DrawShadow = true;
        //            MoveTowardsPoint(new Vector2(LandingSpot.X + 4, LandingSpot.Y), gameTime);

        //            this.NPCAnimatedSprite[0].UpdateAnimations(gameTime, this.Position);
        //            if (this.NPCAnimatedSprite[0].CurrentFrame == 3)
        //            {
        //                this.NPCAnimatedSprite[0].SetFrame(2);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        this.CurrentBehaviour = CurrentBehaviour.Flee;
        //    }
           
        //}

        //public override void Update(GameTime gameTime, MouseManager mouse, Rectangle cameraRectangle, List<Enemy> enemies = null)
        //{
        //    this.IsImmuneToDamage = true;
        //    DrawShadow = false;
        //    if (this.TimeInUnloadedChunk > 100)
        //    {
        //        enemies.Remove(this);
        //        return;
        //    }
        //    if (this.HitPoints <= 0)
        //    {
        //        RollPeriodicDrop(this.Position);
        //        enemies.Remove(this);
        //        return;
        //    }
        //    //this.CurrentBehaviour = CurrentBehaviour.Wander;
        //    this.IsMoving = true;
        //    TestImmunity(gameTime);

        //    this.Collider.Rectangle = new Rectangle((int)(this.Position.X + this.NPCRectangleXOffSet / 2), (int)(this.Position.Y + this.NPCRectangleYOffSet / 2), (int)(this.NPCRectangleWidthOffSet * 2), (int)(this.NPCRectangleHeightOffSet * 2));
        //    List<ICollidable> returnObjects = new List<ICollidable>();
        //    Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.Collider);
        //    for (int i = 0; i < returnObjects.Count; i++)
        //    {

        //        this.CollideOccured = true;
        //        if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
        //        {
        //            if (this.Collider.IsIntersecting(Game1.Player.MainCollider))
        //            {
        //                if (!Game1.Player.IsImmuneToDamage)
        //                {
        //                    Game1.Player.DamageCollisionInteraction(1, 200, this.CurrentDirection);
        //                }

        //            }
        //        }
        //        else if (returnObjects[i].ColliderType == ColliderType.grass)
        //        {
        //            if (this.Collider.IsIntersecting(returnObjects[i]))
        //            {
        //                returnObjects[i].IsUpdating = true;
        //                returnObjects[i].InitialShuffDirection = this.CurrentDirection;
        //            }
        //        }
        //        else
        //        {
        //            if (this.IsMoving)
        //            {


        //                //if (returnObjects[i].ColliderType == ColliderType.inert)
        //                //{
        //                //    Collider.HandleMove(Position, ref primaryVelocity, returnObjects[i]);
        //                //}
        //            }

        //        }


        //        // IsMoving = false;



        //    }

        //    UpdateDirection();

        //    if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
        //    {
        //        mouse.ChangeMouseTexture(CursorType.Normal);
        //        mouse.ToggleGeneralInteraction = true;
        //        Game1.isMyMouseVisible = false;

        //    }

        //    if (this.IsMoving)
        //    {
        //        switch (this.CurrentBehaviour)
        //        {
        //            case CurrentBehaviour.Wander:
        //                //Wander(gameTime);
        //                break;
        //            case CurrentBehaviour.Chase: //this is fight 
        //               // MoveTowardsPoint(new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y), gameTime);
        //                AttackPlayer(gameTime);
        //                break;


        //            case CurrentBehaviour.Hurt:
        //                this.CurrentBehaviour = CurrentBehaviour.Chase;


        //                break;

        //            case CurrentBehaviour.Flee:
        //                if(!MoveTowardsPoint(LandingSpot, gameTime))
        //                {
        //                    this.ShadowScale -= .01f;
        //                    this.DrawShadow = true;
        //                    this.NPCAnimatedSprite[0].UpdateAnimations(gameTime, this.Position);
        //                    if (this.NPCAnimatedSprite[0].CurrentFrame == 3)
        //                    {
        //                        this.NPCAnimatedSprite[0].SetFrame(2);
        //                    }
        //                }
        //                else
        //                {
        //                    this.DrawShadow = false;
        //                   //if has waited for a bit carotar will start hopping again
        //                   this.NPCAnimatedSprite[0].UpdateAnimationsBackwards(gameTime, this.Position);
        //                    if (this.NPCAnimatedSprite[0].CurrentFrame == 4)
        //                    {
        //                        this.NPCAnimatedSprite[0].SetFrame(0);
        //                    }
        //                    this.IsImmuneToDamage = false;
        //                    //if (TimeBetweenAttacks.Run(gameTime))
        //                    //{
        //                    //    Enemy rabbit = Enemy.GetEnemyFromType(EnemyType.Rabbit, null, this.Position, this.Graphics, Game1.GetCurrentStage().AllTiles.ChunkUnderPlayer, true);
        //                    //    rabbit.CurrentBehaviour = CurrentBehaviour.Chase;
        //                    //    Game1.GetCurrentStage().Enemies.Add(rabbit);
        //                    //    this.CurrentBehaviour = CurrentBehaviour.Chase;
        //                    //}
        //                }

                       
                        
                        
        //                break;


        //        }


        //    }



        //    if (this.IdleSoundEffect != null)
        //    {
        //        this.SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //        if (this.SoundTimer <= 0)
        //        {
        //            Game1.SoundManager.PlaySoundEffect(this.IdleSoundEffect, true, 1f);
        //            this.SoundTimer = Game1.Utility.RFloat(this.SoundLowerBound, this.SoundUpperBound);

        //            RollPeriodicDrop(this.Position);
        //        }
        //    }



        //}
        //public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        //{
        //    if (this.CurrentBehaviour == CurrentBehaviour.Hurt)
        //    {

        //        //Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue((float)Math.Sin((float)this.PulseTimer.Time * 2 / this.PulseTimer.TargetTime + (float)Math.PI / 2 * ((float)(Math.PI * 3))));
        //        //Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
        //        //Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();



        //    }

        //    if(DrawShadow)
        //    {
        //        spriteBatch.Draw(Game1.AllTextures.CarotarShadow, this.LandingSpot, null, Color.Black, 0f, new Vector2(16,16), this.ShadowScale, SpriteEffects.None, .3f );
        //    }

        //    this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X - this.NPCRectangleXOffSet - 8, this.Position.Y - this.NPCRectangleYOffSet - 8), .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y)));


        //}
    }
}
