using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public class ShipModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public ShipModel()
        {
            
        }

        public void AssignModel(int ID)
        {
            switch (ID)
            {
                case 1:
                    this.Name = "Basic Ship";
                    this.SourceRectangle = new Rectangle(48, 16, 16, 32);
                    break;
            }
            
        }
    }
}
