using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Controls
{

    public enum ControlType
    {
        Keyboard = 0,
        JoyStick = 1,
    }
    class PlayerControls
    {
        public ControlType controls { get; set; } = ControlType.Keyboard;
        public Keys Up { get; set; }
        public Keys Right { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }

 

        public PlayerControls(int type)
        {

            switch (type)
            {
                case 0:
                    controls = ControlType.Keyboard;
                    break;
                case 1:
                    controls = ControlType.JoyStick;
                    break;
                default:
                    controls = ControlType.Keyboard;
                    break;
            }
        }

        public void Update()
        {

            switch (controls)
            {
                case ControlType.Keyboard:
                   KeyboardState currentKeys = Keyboard.GetState();


                    break;
            }
        }
        
 
    }
}
