using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SecretProject.Class.Controls
{
    public class MouseManager
    {
        public MouseState myMouse;
        public bool IsClicked { get; set; }
        Vector2 position;
        //public Vector2 Position { get { return position; } set { position = value; } }


        public MouseManager(MouseState myMouse)
        {
            this.myMouse = myMouse;
            IsClicked = false;

        }

        public void Update()
        {
            IsClicked = false;

            MouseState oldMouse = myMouse;
            myMouse = Mouse.GetState();

            position.X = myMouse.Position.X;
            position.Y = myMouse.Position.Y;

            if ((myMouse.LeftButton == ButtonState.Released) && (oldMouse.LeftButton == ButtonState.Pressed)) IsClicked = true;



        }

        public bool IsHovering(Rectangle rectangle)
        {
            Rectangle mouseRectangle = new Rectangle(myMouse.X, myMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
        
}
