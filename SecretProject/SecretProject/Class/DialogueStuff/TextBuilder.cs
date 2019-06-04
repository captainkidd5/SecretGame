using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.DialogueStuff
{
    public class TextBuilder
    {
        public string StringToWrite { get; set; }
        public float WriteSpeed { get; set; }
        private string outputString = "";
        int currentTextIndex = 0;

        public TextBuilder(string stringToWrite, float writeSpeed)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
        }

        public void Update(GameTime gameTime)
        {
            WriteSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(WriteSpeed < 0)
            {
                outputString += StringToWrite[currentTextIndex];
                currentTextIndex++;
            }
        }
    }
}
