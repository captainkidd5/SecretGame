using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.SanctuaryStuff;

namespace SecretProject.Class.UI.SanctuaryStuff
{
    public class CompletionHub :IExclusiveInterfaceComponent
    {
        public GraphicsDevice Graphics { get; set; }
        public List<CompletionGuide> AllGuides { get; set; }
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Button> HubButtons { get; set; }
        CompletionGuide ActiveGuide;


        public CompletionHub(GraphicsDevice graphicsDevice, ContentManager content)
        {
            SanctuaryHolder ForestHolder = content.Load<SanctuaryHolder>("SanctuarySTuff/ForestSanctuary");
            this.Graphics = graphicsDevice;
            AllGuides = new List<CompletionGuide>()
            {
               new CompletionGuide(Graphics, ForestHolder),
            };
            ActiveGuide = AllGuides[0];

        }

        public void Update(GameTime gameTime)
        {
            ActiveGuide.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ActiveGuide.Draw(spriteBatch);
        }
    }
}
