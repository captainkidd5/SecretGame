using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SecretProject.Class.Controls
{
    class MouseManager
    {
        private MouseState oldState;
        private MouseState newState;
        private int x;
        private int y;
        Rectangle mouseRectangle;

        public MouseManager()
        {
            newState = Mouse.GetState();
            oldState = Mouse.GetState();
            mouseRectangle = new Rectangle(newState.X, newState.Y, 1, 1);
        }


        public void GetPosition()
        {
            x = newState.X;
            y = newState.Y;

        }


        public bool IsClicked()
        {
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                oldState = newState;
                return true;
                
            }
            else
            {
                return false;
            }
        }
    }
}
