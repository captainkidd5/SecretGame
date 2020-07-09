using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        public Dir SecondaryDirection { get; set; }
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

        private PlayerControls()
        {

        }

        public PlayerControls(int type)
        {

            switch (type)
            {
                case 0:
                    this.controls = ControlType.Keyboard;
                    break;
                case 1:
                    this.controls = ControlType.JoyStick;
                    break;
                default:
                    this.controls = ControlType.Keyboard;
                    break;
            }
        }


        public void UpdateKeys()
        {
            switch (this.controls)
            {
                case ControlType.Keyboard:
                    KeyboardState currentKeys = Keyboard.GetState();
                    pressedKeys = currentKeys.GetPressedKeys();
                    break;
            }
        }
        public void Update()
        {
            switch (this.controls)
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

                    if (!currentKeys.IsKeyDown(Keys.D) && oldKeys.IsKeyDown(Keys.D))
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

                    this.MovementKey = MovementKeys[MovementKeys.Count - 1];
                    if ((MovementKeys.Count - 2) >= 0)
                    {
                        this.SecondMovementKey = MovementKeys[MovementKeys.Count - 2];
                    }
                    else
                    {
                        this.SecondMovementKey = Keys.None;
                    }


                    oldKeys = currentKeys;


                    ////////
                    ///

                    IsMoving = false; 
                    switch (this.MovementKey)
                    {
                        case Keys.D:
                            this.Direction = Dir.Right;
                            this.IsMoving = true;
                            break;

                        case Keys.A:
                            this.Direction = Dir.Left;
                            this.IsMoving = true;
                            break;

                        case Keys.W:
                            this.Direction = Dir.Up;
                            this.IsMoving = true;
                            break;

                        case Keys.S:
                            this.Direction = Dir.Down;
                            this.IsMoving = true;
                            break;

                        case Keys.None:
                            this.Direction = Dir.None;
                            this.IsMoving = false;
                            break;
                    }

                    switch (this.SecondMovementKey)
                    {
                        case Keys.D:
                            this.SecondaryDirection = Dir.Right;

                            break;

                        case Keys.A:
                            this.SecondaryDirection = Dir.Left;

                            break;

                        case Keys.W:
                            this.SecondaryDirection = Dir.Up;

                            break;

                        case Keys.S:
                            this.SecondaryDirection = Dir.Down;

                            break;

                        case Keys.None:
                            this.SecondaryDirection = Dir.None;

                            break;

                    }
                    if (oldKeys.IsKeyDown(Keys.LeftShift))
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

