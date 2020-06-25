using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using System;
using System.Collections.Generic;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionHub : IExclusiveInterfaceComponent
    {
        public GraphicsDevice Graphics { get; set; }
        public List<CompletionGuide> AllGuides { get; set; }
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private Button redEsc;
        public List<Button> HubButtons { get; set; }
        CompletionGuide ActiveGuide;


        public CompletionHub(GraphicsDevice graphicsDevice, ContentManager content)
        {
            SanctuaryHolder ForestHolder = content.Load<SanctuaryHolder>("SanctuarySTuff/ForestSanctuary");
            this.Graphics = graphicsDevice;
            this.AllGuides = new List<CompletionGuide>()
            {
               new CompletionGuide(this.Graphics, ForestHolder),
            };
            ActiveGuide = this.AllGuides[0];
            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), this.Graphics,
                new Vector2(this.AllGuides[0].BackGroundPosition.X + this.AllGuides[0].BackGroundSourceRectangle.Width - 32, this.AllGuides[0].BackGroundPosition.Y - this.AllGuides[0].BackGroundSourceRectangle.Height), CursorType.Normal);

        }

        public void Update(GameTime gameTime)
        {
            redEsc.Update(Game1.MouseManager);
            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
            ActiveGuide.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ActiveGuide.Draw(spriteBatch);
            redEsc.Draw(spriteBatch);
        }
    }
}
