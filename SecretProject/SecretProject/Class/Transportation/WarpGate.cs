using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public WarpGate(GraphicsDevice graphics)
        {
            Yes = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX - 200, Game1.Utility.CenterScreenY), CursorType.Normal,Game1.Utility.GlobalButtonScale);
            No = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX , Game1.Utility.CenterScreenY), CursorType.Normal, Game1.Utility.GlobalButtonScale);
            TextBox = new TextBox(Game1.AllTextures.MenuText, new Vector2(Game1.Utility.CenterScreenX, Game1.Utility.CenterScreenY - 64), "Travel?", Game1.AllTextures.UserInterfaceTileSet);
        }

        public void Update(GameTime gameTime)
        {
            Yes.Update(Game1.myMouseManager);
            No.Update(Game1.myMouseManager);
            if(Yes.isClicked)
            {
                Transport(this.To);
            }
            else if(No.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TextBox.Draw(spriteBatch,true);
            Yes.Draw(spriteBatch, Game1.AllTextures.MenuText, "Yes", Yes.Position, Color.White, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardButtonDepth + .01f);
            No.Draw(spriteBatch, Game1.AllTextures.MenuText, "No", No.Position, Color.White, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardButtonDepth + .01f);
        }
       

        public void Transport(Stages to)
        {
            if (Game1.GetCurrentStageInt() == Stages.World)
            {
                for (int i = 0; i < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(0); i++)
                {
                    for (int j = 0; j < Game1.GetCurrentStage().AllTiles.ActiveChunks.GetLength(1); j++)
                    {
                        Game1.GetCurrentStage().AllTiles.ActiveChunks[i, j].Save();
                    }
                }
                Game1.SwitchStage(Game1.GetCurrentStageInt(), to);
                Game1.Player.Position = new Vector2(800, 800);
            }
            else
            {
                Game1.SwitchStage(Game1.GetCurrentStageInt(), to);
                Game1.Player.Position = new Vector2(80, 80);
                Game1.World.AllTiles.LoadInitialChunks();
            }
            
            Game1.Player.UserInterface.CurrentOpenInterfaceItem = UI.ExclusiveInterfaceItem.None;
        }
    }
}
