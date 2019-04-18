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
    public class PlayerControls
    {
        public ControlType controls { get; set; } = ControlType.Keyboard;
        public Dir Direction { get; set; }
        public SecondaryDir SecondaryDirection { get; set; }
        public Keys Up { get; set; }
        public Keys Right { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }

        public KeyboardState oldKeys = Keyboard.GetState();


        public Keys MovementKey { get; set; } = Keys.None;
        public Keys SecondMovementKey { get; set; } = Keys.None;

        public Keys[] pressedKeys;

        public bool IsMoving { get; set; } = false;
        public bool IsSprinting { get; set; } = false;

        List<Keys> MovementKeys = new List<Keys>() { Keys.None };

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

                    pressedKeys = currentKeys.GetPressedKeys();


                    if (currentKeys.IsKeyDown(Keys.D) && !MovementKeys.Contains(Keys.D))
                    {

                        MovementKeys.Add(Keys.D);
                        
                    }


                    if (currentKeys.IsKeyDown(Keys.A) && !MovementKeys.Contains(Keys.A))
                    {
                        MovementKeys.Add(Keys.A);
                        
                    }

                    if (currentKeys.IsKeyDown(Keys.W) && !MovementKeys.Contains(Keys.W))
                    {
                        MovementKeys.Add(Keys.W);
                        
                    }

                    if (currentKeys.IsKeyDown(Keys.S) && !MovementKeys.Contains(Keys.S))
                    {
                        MovementKeys.Add(Keys.S);
                        
                    }

                    //Now for the removal

                    if(!currentKeys.IsKeyDown(Keys.D) && oldKeys.IsKeyDown(Keys.D))
                    {
                        MovementKeys.Remove(Keys.D);
                    }

                    if (!currentKeys.IsKeyDown(Keys.A) && oldKeys.IsKeyDown(Keys.A))
                    {
                        MovementKeys.Remove(Keys.A);
                    }

                    if (!currentKeys.IsKeyDown(Keys.W) && oldKeys.IsKeyDown(Keys.W))
                    {
                        MovementKeys.Remove(Keys.W);
                    }
                    if (!currentKeys.IsKeyDown(Keys.S) && oldKeys.IsKeyDown(Keys.S))
                    {
                        MovementKeys.Remove(Keys.S);
                    }

                    //active movement key is the one at the front of the list

                    MovementKey = MovementKeys[MovementKeys.Count - 1];
                    if((MovementKeys.Count -2) >= 0)
                    {
                        SecondMovementKey = MovementKeys[MovementKeys.Count - 2];
                    }
                    else
                    {
                        SecondMovementKey = Keys.None;
                    }
                    

                    oldKeys = currentKeys;


                    ////////
                    ///
                    

                    switch (MovementKey)
                    {
                        case Keys.D:
                            Direction = Dir.Right;
                            IsMoving = true;
                            break;

                        case Keys.A:
                            Direction = Dir.Left;
                            IsMoving = true;
                            break;

                        case Keys.W:
                            Direction = Dir.Up;
                            IsMoving = true;
                            break;

                        case Keys.S:
                            Direction = Dir.Down;
                            IsMoving = true;
                            break;

                        case Keys.None:
                            IsMoving = false;
                            break;
                    }

                    switch(SecondMovementKey)
                    {
                        case Keys.D:
                            SecondaryDirection = SecondaryDir.Right;
                            
                            break;

                        case Keys.A:
                            SecondaryDirection = SecondaryDir.Left;
                            
                            break;

                        case Keys.W:
                            SecondaryDirection = SecondaryDir.Up;
                            
                            break;

                        case Keys.S:
                            SecondaryDirection = SecondaryDir.Down;
                          
                            break;

                        case Keys.None:
                            SecondaryDirection = SecondaryDir.None;
                            
                            break;

                    }
                    if(oldKeys.IsKeyDown(Keys.LeftShift))
                    {
                        this.IsSprinting = true;
                    }
                    else
                    {
                        this.IsSprinting = false;
                    }


                    break;
            }
        }
        
 
    }
}

