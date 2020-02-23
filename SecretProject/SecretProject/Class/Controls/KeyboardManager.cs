using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Controls
{
    public class KeyboardManager
    {
        public KeyboardState OldKeyBoardState { get; set; }
        public KeyboardState NewKeyBoardState { get; set; }

        public KeyboardManager()
        {
            NewKeyBoardState = Keyboard.GetState();
            OldKeyBoardState = NewKeyBoardState;
        }

        public void Update()
        {
            OldKeyBoardState = NewKeyBoardState;
            NewKeyBoardState = Keyboard.GetState();
        }

        public bool WasKeyPressed(Keys key)
        {
            if (this.OldKeyBoardState.IsKeyDown(key) && this.NewKeyBoardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}
