using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff.Items
{
    public class InventoryItem
    {


        public string Name { get; set; }
        public int ID { get; set; }
        public int Count { get; set; }
        public int InvMaximum { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
        public Sprite ItemSprite { get; set; }

        public GraphicsDevice Graphics { get; set; }
        public ContentManager Content { get; set; }



        public InventoryItem(string Name, GraphicsDevice graphics, ContentManager content)
        {
            this.Content = content;
            this.Graphics = graphics;

            this.Name = Name;

            switch (Name)
            {
                case "pie":
                    this.Texture = content.Load<Texture2D>("Item/pie");
                    this.InvMaximum = 5;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false);
                    break;

                case "shrimp":
                    this.Texture = content.Load<Texture2D>("Item/puzzleFish");
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false);
                    this.InvMaximum = 10;
                    
                    break;

                default:
                    throw new NotImplementedException();


            }

            
        }

        public void Update(GameTime gameTime)
        {
            //this.ItemSprite.Update(gameTime);
        }
    }
}
