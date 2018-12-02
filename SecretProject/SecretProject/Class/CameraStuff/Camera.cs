using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CameraStuff
{
    
    public class Camera
    {
        public Matrix Transform { get; private set; }

        //--------------------------------------
        //Zoom
        private float zoom = 1;
        public float Zoom { get { return zoom; } set { zoom = value; } }


        public Camera()
        {

        }

        public void follow(Vector2 position, Rectangle rectangle)
        {
            var cameraPosition = Matrix.CreateTranslation(
                (-position.X - (rectangle.Width / 2)),
                (-position.Y - (rectangle.Height / 2)),
                (0));

            var offSet = Matrix.CreateTranslation(
                    Game1.ScreenWidth /2, 
                    Game1.ScreenHeight / 2, 0);

            var zoomAmount = Matrix.CreateScale(Zoom);

            Transform = cameraPosition * offSet * zoomAmount;

        }

        public void Update(GameTime gameTime, Player player)
        {

        }

    }
    
}
