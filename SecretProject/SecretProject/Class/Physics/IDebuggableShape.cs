using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Physics
{
    public interface IDebuggableShape
    {
        List<IDebuggableShape> Shapes { get; set; }
        void Draw(SpriteBatch spriteBatch);


    }
}
