using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public interface IItem
    {
        string Name { get; set; }
        int ID { get; set; }
        int Count { get; set; }
        int InvMaximum { get; set; }
        int WorldMaximum { get; set; }
        Texture2D Texture { get; set; }
        bool IsDropped { get; set; }

        void PickUp();

        void Drop(GraphicsDevice graphics, ContentManager content, Vector2 position);

    }
}
