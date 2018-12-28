using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Stage;

namespace SecretProject.Class.ItemStuff.Items
{
    class Food : IItem
    {
        public string Name { get; set; }

        public int ID { get; set; } = 1;

        public Texture2D Texture { get; set; }

        public int Count { get; set; }
        public int InvMaximum { get; set; }
        public int WorldMaximum { get; set; }
        public bool IsDropped { get ; set ; }

        // Food Specific



        public Food(string name, ContentManager content)
        {
            IsDropped = false;
            switch(name)
            {
                case "pie":
                    this.Texture = content.Load<Texture2D>("Item/pie");
                    break;

                case "shrimp":
                    this.Texture = content.Load<Texture2D>("Item/puzzleFish");
                    break;

                default:
                    throw new NotImplementedException();
                    

            }
        }

        public void PickUp()
        {
            throw new NotImplementedException();
        }

        public void Drop(GraphicsDevice graphics, ContentManager content, Vector2 position)
        {
            Iliad.allSprites.Add(new Sprite(graphics, content, Texture, position, true));
        }
    }
}
