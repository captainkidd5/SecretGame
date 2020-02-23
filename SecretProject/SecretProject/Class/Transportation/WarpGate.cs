using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.Transportation
{
    public class WarpGate : IExclusiveInterfaceComponent
    {
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Button Yes { get; set; }
        public Button No { get; set; }
        public TextBox TextBox { get; set; }
        public Stages To { get; set; }

        public Vector2 CenterOfPortal { get; set; }

        public WarpGate(GraphicsDevice graphics)
        {
            this.Yes = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX - 200, Game1.Utility.CenterScreenY), CursorType.Normal, Game1.Utility.GlobalButtonScale);
            this.No = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY), CursorType.Normal, Game1.Utility.GlobalButtonScale);
            this.TextBox = new TextBox(Game1.AllTextures.MenuText, new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY - 64), "Travel?", Game1.AllTextures.UserInterfaceTileSet);
        }

        public void Update(GameTime gameTime)
        {
            this.Yes.Update(Game1.MouseManager);
            this.No.Update(Game1.MouseManager);
            if (this.Yes.isClicked)
            {
               
                Game1.Player.DoPlayerAnimation(AnimationType.PortalJump);
                Vector2 moveToPosition = CenterOfPortal;
                Game1.Player.MoveToPoint(new Vector2(moveToPosition.X, moveToPosition.Y - 24));
                Game1.Player.TransportAfterMove = true;
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;

            }
            else if (this.No.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.TextBox.Draw(spriteBatch, true);
            this.Yes.Draw(spriteBatch, Game1.AllTextures.MenuText, "Yes", this.Yes.Position, Color.White, Utility.StandardButtonDepth, Utility.StandardButtonDepth + .01f);
            this.No.Draw(spriteBatch, Game1.AllTextures.MenuText, "No", this.No.Position, Color.White, Utility.StandardButtonDepth, Utility.StandardButtonDepth + .01f);
        }


        public void Transport(Stages to)
        {
            if (Game1.GetCurrentStageInt() == Stages.OverWorld)
            {
                for (int i = 0; i < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(0); i++)
                {
                    for (int j = 0; j < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(1); j++)
                    {
                        Game1.GetCurrentStage().AllTiles.ActiveChunks[i, j].Save();
                    }
                }
                Game1.SwitchStage(Game1.GetCurrentStageInt(), to);
                Game1.Player.position = new Vector2(1170, 690);
                Game1.Player.controls.Direction = Dir.Down;
                Game1.Player.PlayAnimation(AnimationType.PortalJump);
                Game1.Player.MoveToPoint(new Vector2(1170, 730));

            }
            else
            {
                Game1.SwitchStage(Game1.GetCurrentStageInt(), to);
                Game1.Player.position = new Vector2(85, 100);
                Game1.Player.controls.Direction = Dir.Down;
                Game1.Player.PlayAnimation(AnimationType.PortalJump);
                Game1.Player.MoveToPoint(new Vector2(85, 150));

            }

            Game1.Player.UserInterface.CurrentOpenInterfaceItem = UI.ExclusiveInterfaceItem.None;
        }
    }
}
