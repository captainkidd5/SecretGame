using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.ItemStuff.Items
{
    class Food : IItem
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int iD = 1;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int Count { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int InvMaximum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int WorldMaximum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // Food Specific



        public Food(string name, ContentManager content)
        {
            switch(name)
            {
                case "pie":
                    this.texture = content.Load<Texture2D>("");
                    break;

                case "shrimp":
                    this.texture = content.Load<Texture2D>("Item/puzzleFish");
                    break;

                default:
                    throw new NotImplementedException();
                    

            }
        }

    }
}
