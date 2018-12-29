using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class WorldItem
    {


        public string Name { get; set; }
        public int ID { get; set; }
        public int Count { get; set; }
        public int WorldMaximum { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
        public Sprite ItemSprite { get; set; }
        public bool IsFull { get; set; }

        public GraphicsDevice Graphics { get; set; }
        public ContentManager Content { get; set; }


        public WorldItem(string Name, GraphicsDevice graphics, ContentManager content, Vector2 WorldPosition)
        {
            this.Content = content;
            this.Graphics = graphics;
            IsDropped = true;
            this.Name = Name;

            switch (Name)
            {
                case "pie":
                    this.Texture = content.Load<Texture2D>("Item/pie");
                    this.WorldMaximum = 5;
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, WorldPosition, true) { IsWorldItem = true } ;
                    break;

                case "shrimp":
                    this.Texture = content.Load<Texture2D>("Item/puzzleFish");
                    this.ItemSprite = new Sprite(graphics, content, this.Texture, WorldPosition, true) { IsWorldItem = true};
                    this.WorldMaximum = 10;

                    break;

                default:
                    throw new NotImplementedException();


            }


        }

        public void Update(GameTime gameTime)
        {
            this.ItemSprite.Update(gameTime);
            if(ItemSprite.PickedUp == true && IsDropped == true)
            {
               // InventoryItem invItem = new InventoryItem()
                Game1.Player.Inventory.AddItemToInventory(new InventoryItem(this.Name, this.Graphics, this.Content));
                IsDropped = false;
            }

          //  if(Game1.Player.Inventory.)


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDropped == true)
            {
                this.ItemSprite.Draw(spriteBatch, .4f);
            }
            
        }
    }
}
