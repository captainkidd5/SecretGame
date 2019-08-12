using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ObjectFolder;
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

        public Dobbin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule) : base(name, position, graphics, spriteSheet, routeSchedule)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 167, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 335, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 503, 0, 28, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 2;
            this.NPCRectangleWidthOffSet = 2;
            this.SpeakerID = 2;
           // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            //DebugTexture = SetRectangleTexture(graphics, )
            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);
        }

        public void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            this.CollideOccured = Collider.DidCollide(objects, Position);

            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].UpdateAnimations(gameTime, Position);
                    break;
                case 1:
                    NPCAnimatedSprite[1].UpdateAnimations(gameTime, Position);
                    break;
                case 2:
                    NPCAnimatedSprite[2].UpdateAnimations(gameTime, Position);
                    break;
                case 3:
                    NPCAnimatedSprite[3].UpdateAnimations(gameTime, Position);
                    break;
            }
            if (IsMoving)
            {


                UpdateDirection();
                this.PrimaryVelocity = Collider.Velocity;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            }
            if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
            {
                mouse.ChangeMouseTexture(200);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;
                if (mouse.IsRightClicked)
                {


                    Game1.Player.UserInterface.ActivateShop(UI.OpenShop.DobbinShop);
                    UpdateDirectionVector(Game1.Player.position);
                    this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);


                }

            }

            //this.Speed = PrimaryVelocity
            FollowSchedule(gameTime, this.RouteSchedule);
            if (mouse.IsRightClicked)
            {
                Console.WriteLine(this.NPCHitBoxRectangle);

            }


        }
    }
}
