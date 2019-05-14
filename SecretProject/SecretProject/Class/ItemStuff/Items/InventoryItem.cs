using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.BuildingItems;
using SecretProject.Class.SpriteFolder;
using System.Xml.Serialization;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff.Items
{
    [Serializable, XmlRoot("Item")]
    public class InventoryItem
    {


        public string Name { get; set; }
        public int ID { get; set; }
        [XmlIgnore]
        public int Count { get; set; } = 0;
        public int InvMaximum { get; set; }
        [XmlIgnore]
        public Texture2D Texture { get; set; }
        public bool IsDropped { get; set; }
        [XmlIgnore]
        public Sprite ItemSprite { get; set; }
        [XmlIgnore]
        public GraphicsDevice Graphics { get; set; }
        [XmlIgnore]
        public ContentManager Content { get; set; }

        public bool IsPlaceable { get; set; } = false;
        [XmlIgnore]
        public PlaceableBuilding Building { get; set; }

        public string id { get; set; }

        public string TextureString { get; set; }

        public InventoryItem()
        {

        }

        public InventoryItem(int id, GraphicsDevice graphics, ContentManager content)
        {
            string ID = id.ToString();
            this.Name = Game1.ItemVault.Items[ID].Name;
            this.InvMaximum = Game1.ItemVault.Items[ID].InvMaximum;
            this.TextureString = Game1.ItemVault.Items[ID].TextureString;
            this.Texture = content.Load<Texture2D>(TextureString);
            this.IsPlaceable = Game1.ItemVault.Items[ID].IsPlaceable;

            this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
        }



        public InventoryItem(string Name, GraphicsDevice graphics, ContentManager content)
        {
            this.Content = content;
            this.Graphics = graphics;

            this.Name = Name;

            //switch (Name)
            //{
            //    case "pie":
            //        this.Texture = Game1.AllTextures.pie;
            //        this.InvMaximum = 5;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        break;

            //    case "shrimp":
            //        this.Texture = Game1.AllTextures.puzzleFish;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 10;
                    
            //        break;

            //    case "grass":
            //        this.Texture = Game1.AllTextures.grass;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 10;
            //        break;

            //    case "barrel":
            //        this.Texture = Game1.AllTextures.barrel;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 3;
            //        this.IsPlaceable = true;
            //        this.Building = new PlaceableBuilding(Name);
            //        break;

            //    case "secateur":
            //        this.Texture = Game1.AllTextures.Secateurs;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 1;               
            //        break;

            //    case "lodgeKey":
            //        this.Texture = Game1.AllTextures.lodgeKey;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 1;
            //        break;

            //    case "shovel":
            //        this.Texture = Game1.AllTextures.shovel;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 1;
            //        break;

            //    case "stone":
            //        this.Texture = Game1.AllTextures.stone;
            //        this.ItemSprite = new Sprite(graphics, content, this.Texture, new Vector2(500, 635), false, .4f);
            //        this.InvMaximum = 10;
            //        break;




            //    default:
            //        throw new NotImplementedException();


            //}

            
        }



        public void Update(GameTime gameTime)
        {
            //this.ItemSprite.Update(gameTime);
        }
    }
}
