using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;

using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Dobbin  : Character
    {

        public Dobbin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule,0, false, characterPortraitTexture)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 167, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 335, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 503, 0, 28, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 34;
            this.NPCRectangleHeightOffSet = 8;
            this.NPCRectangleWidthOffSet = 8;
            this.SpeakerID = 2;
           // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            //DebugTexture = SetRectangleTexture(graphics, )
            Collider = new Collider(graphics,this.PrimaryVelocity, this.NPCHitBoxRectangle,this, ColliderType.NPC);
            this.DebugColor = Color.HotPink;
        }

        public override void Update(GameTime gameTime, MouseManager mouse)
        {
            if (Game1.GetCurrentStageInt() == this.CurrentStageLocation)
            {
                this.DisableInteractions = false;
            }
            else
            {
                this.DisableInteractions = true;
            }
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetStageFromInt(CurrentStageLocation).QuadTree.Retrieve(returnObjects, Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {
                //if obj collided with item in list stop it from moving boom badda bing
                if (returnObjects[i].ColliderType != ColliderType.NPC && Collider.DidCollide(returnObjects[i], Position))
                {
                    CollideOccured = true;
                }

            }

            for (int i = 0; i < 4; i++)
            {
                NPCAnimatedSprite[i].UpdateAnimations(gameTime, Position);
            }
            if (IsMoving)
            {


                UpdateDirection();
                if(CollideOccured)
                {
                    this.PrimaryVelocity = Collider.Velocity;
                }
                
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            }
            if (!DisableInteractions)
            {


                if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
                {
                    mouse.ChangeMouseTexture(CursorType.Chat);
                    mouse.ToggleGeneralInteraction = true;
                    Game1.isMyMouseVisible = false;
                    if (mouse.IsRightClicked)
                    {


                        Game1.Player.UserInterface.ActivateShop(UI.OpenShop.DobbinShop);
                        UpdateDirectionVector(Game1.Player.position);
                        this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);


                    }

                }
                if (mouse.IsRightClicked)
                {
                   // Console.WriteLine(this.NPCHitBoxRectangle);

                }
                CheckSpeechInteraction(mouse, FrameToSet);
            }

            //this.Speed = PrimaryVelocity
            FollowSchedule(gameTime, this.RouteSchedule);
            


        }
    }
}
