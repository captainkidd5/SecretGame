using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;

using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Boar
    {
        //public Boar(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet) : base(name, position, graphics, spriteSheet)
        //{
        //    NPCAnimatedSprite = new Sprite[4];

        //    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position);
        //    NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 48, 32, 3, .15f, this.Position);
        //    NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 288, 0, 48, 32, 3, .15f, this.Position);
        //    NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 432, 0, 48, 32, 3, .15f, this.Position);

        //    this.NPCRectangleXOffSet = 15;
        //    this.NPCRectangleYOffSet = 15;
        //    this.NPCRectangleHeightOffSet = 4;
        //    this.NPCRectangleWidthOffSet = 4;
        //    this.Speed = .05f;
        //    this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
        //    this.SoundID = 14;
        //    this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
        //    this.CurrentBehaviour = CurrentBehaviour.Wander;
        //}

        //public void Update(GameTime gameTime, MouseManager mouse, IInformationContainer container)
        //{
           
        //    this.IsMoving = true;
        //    this.PrimaryVelocity = new Vector2(1, 1);
        //    Collider.Rectangle = this.NPCHitBoxRectangle;
        //    Collider.Velocity = this.PrimaryVelocity;
        //    List<ICollidable> returnObjects = new List<ICollidable>();
        //    Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, Collider);
        //    for (int i = 0; i < returnObjects.Count; i++)
        //    {
        //        //if obj collided with item in list stop it from moving boom badda bing
        //            if (Collider.DidCollide(returnObjects[i], Position))
        //            {
        //                CollideOccured = true;
        //            if(returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
        //            {
        //                this.CurrentBehaviour = CurrentBehaviour.Chase;
        //            }
        //            else if (returnObjects[i].ColliderType == ColliderType.grass)
        //            {
        //                if (Collider.IsIntersecting(returnObjects[i]))
        //                {
        //                    returnObjects[i].IsUpdating = true;
        //                    returnObjects[i].InitialShuffDirection = this.CurrentDirection;
        //                }
        //            }


        //            //  IsMoving = false;
        //        }
                

        //    }
        //    for (int i = 0; i < 4; i++)
        //    {
        //        NPCAnimatedSprite[i].UpdateAnimationPosition(Position);
        //    }
        //    UpdateDirection();

        //    if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
        //    {
        //        mouse.ChangeMouseTexture(CursorType.Normal);
        //        mouse.ToggleGeneralInteraction = true;
        //        Game1.isMyMouseVisible = false;

        //    }
            
        //    if(CollideOccured)
        //    {
        //        this.PrimaryVelocity = Collider.Velocity;
        //    }
        //    if(IsMoving)
        //    {
        //        switch (CurrentBehaviour)
        //        {
        //            case CurrentBehaviour.Wander:
        //                Wander(gameTime, container);
        //                break;
        //            case CurrentBehaviour.Chase:
        //                MoveTowardsPoint(Game1.Player.position, gameTime);
        //                break;
        //            case CurrentBehaviour.Hurt:
        //                for(int i =0; i < NPCAnimatedSprite.Length; i++)
        //                {
        //                    NPCAnimatedSprite[i].Flash(gameTime, .05f, Color.Red);
        //                }
        //                //CurrentBehaviour = CurrentBehaviour.Wander;
        //                break;
        //        }

                
        //    }
            
            
        //    if (IsMoving)
        //    {
                
        //        for (int i = 0; i < 4; i++)
        //        {
        //            NPCAnimatedSprite[i].UpdateAnimations(gameTime, Position);
        //        }


                
        //    }
        //    else
        //    {
        //        this.NPCAnimatedSprite[(int)CurrentDirection].SetFrame(0);

        //    }
        //    SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    if (SoundTimer <= 0)
        //    {
        //        Game1.SoundManager.PlaySoundEffectFromInt(1, SoundID, Game1.SoundManager.GameVolume);
        //        SoundTimer = Game1.Utility.RFloat(5f, 50f);
        //        RollDrop(gameTime, 148, this.Position, 10);
        //    }

        //}
    }
}
