using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SecretProject.Class.ItemStuff.Items
{
    [Serializable()]
    [XmlRoot("WorldItem")]
    public class ItemBank
    {
        XmlSerializer serializer;
        public readonly Dictionary<int, WorldItem> allItems;
        List<WorldItem> intermediateList;
        string path = "Content/WorldItem.xml";
        GraphicsDevice graphics;
        ContentManager content;

        public ItemBank(GraphicsDevice graphics, ContentManager content)
        {
            this.graphics = graphics;
            this.content = content;
            serializer = new XmlSerializer(typeof(List<WorldItem>));
            allItems = new Dictionary<int, WorldItem>();
            StreamReader reader = new StreamReader(path);
            reader.ReadToEnd();
            
            intermediateList = (List<WorldItem>)serializer.Deserialize(reader);
            reader.Close();

            

            foreach(WorldItem item in intermediateList)
            {
                allItems.Add(item.ID, item);
            }
                             
        }

        public WorldItem GetWorldItem(int ID, Vector2 location, bool isTossable)
        {
            if(allItems.ContainsKey(ID))
            return new WorldItem(graphics, content) { Name = allItems[ID].Name, Texture = allItems[ID].Texture, ID = allItems[ID].ID,
                InventoryMaximum = allItems[ID].InventoryMaximum, WorldMaximum = allItems[ID].WorldMaximum, WorldPosition = location, IsTossable = isTossable};
            else
            {
                return null;
            }
        }
    }
}
