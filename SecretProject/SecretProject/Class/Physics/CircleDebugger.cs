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

    public class RectangleDebugger : IDebuggableShape
    {
        public List<IDebuggableShape> Shapes { get; set; }
        private Body RectangleShape { get; set; }
        private Rectangle DebugRectangle { get; set; }
        public Texture2D Texture { get; set; }

        public RectangleDebugger(Body body, List<IDebuggableShape> shapes)
        {
            this.Shapes = shapes;
            this.RectangleShape = body;

            this.DebugRectangle = new Rectangle((int)body.Position.X - 8, (int)body.Position.Y - 8, 16, 16);
            this.Texture = RetrieveDebugTexture(16, 16);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.RectangleShape == null)
            {
                Shapes.Remove(this);
                return;
            }
            Vector2 drawPosition = new Vector2(this.RectangleShape.Position.X , this.RectangleShape.Position.Y );
            spriteBatch.Draw(this.Texture, drawPosition, color: Color.White * .5f, layerDepth: 1f);

        }


        public static Dictionary<int, Texture2D> RectangleSizeTextures = new Dictionary<int, Texture2D>();

        /// <summary>
        /// If dictionary already has a texture of desired radius, just use that one. Otherwise create a new one and 
        /// add it to the dictionary.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static Texture2D RetrieveDebugTexture(int width, int height)
        {
            int key = width + height; //really bad, but good enough for now.
            Texture2D debugTexture;
            if (!RectangleSizeTextures.ContainsKey(key))
            {
                debugTexture = Game1.Utility.GetColoredRectangle(width,height, Color.Red);
                RectangleSizeTextures.Add(key, debugTexture);
            }
            else
            {
                debugTexture = RectangleSizeTextures[key];
            }
            return debugTexture;
        }
    }
}
