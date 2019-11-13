﻿using Microsoft.Xna.Framework;
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
        public WarpGate(GraphicsDevice graphics)
        {
            Yes = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX - 200, Game1.Utility.CenterScreenY), CursorType.Normal,Game1.Utility.GlobalButtonScale);
            No = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(896, 50, 81, 28), graphics, new Vector2(Game1.Utility.CenterScreenX , Game1.Utility.CenterScreenY), CursorType.Normal, Game1.Utility.GlobalButtonScale);
        }

        public void Update(GameTime gameTime)
        {
            Yes.Update(Game1.myMouseManager);
            No.Update(Game1.myMouseManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Yes.Draw(spriteBatch, Game1.AllTextures.MenuText, "Yes", Yes.Position, Color.White, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardButtonDepth - .1f);
            No.Draw(spriteBatch, Game1.AllTextures.MenuText, "No", No.Position, Color.White, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardButtonDepth - .1f);
        }
       

        public void Transport(Stages from, Stages to)
        {

        }
    }
}
