using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;

namespace SecretProject.Class.Physics
{
    public class CircleDebugger : IDebuggableShape
    {
        public List<IDebuggableShape> Shapes { get; set; }
    private Body CircleShape { get; set; }
        private Circle DebugCircle { get; set; }

        public CircleDebugger(Body body, List<IDebuggableShape> shapes)
        {
            this.Shapes = shapes;
            this.CircleShape = body;

            this.DebugCircle = new Circle(body.Position,body.FixtureList[0].Shape.Radius * 2, true);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.CircleShape == null)
            {
                Shapes.Remove(this);
                return;
            }
            Vector2 drawPosition = new Vector2(this.CircleShape.Position.X - DebugCircle.Radius / 2, this.CircleShape.Position.Y - DebugCircle.Radius / 2);
            spriteBatch.Draw(this.DebugCircle.DebugTexture, drawPosition, color: Color.White * .5f, layerDepth: 1f);

        }
    }
}
