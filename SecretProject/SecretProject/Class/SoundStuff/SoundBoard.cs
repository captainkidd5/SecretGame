using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.SoundStuff
{
    public class SoundBoard
    {
        public SoundEffect PickUpItem { get; set; }
        public SoundEffectInstance PickUpItemInstance { get; set; }
        public SoundEffect StoneStep { get; set; }
        public SoundEffectInstance StoneStepInstance { get; set; }
        public SoundBoard(Game1 game, ContentManager content)
        {
            PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");
            PickUpItemInstance = PickUpItem.CreateInstance();

            StoneStep = content.Load<SoundEffect>("SoundEffects/bubble");
            StoneStepInstance = StoneStep.CreateInstance();

        }

        public void PlaySoundEffect(SoundEffectInstance instance, bool isLooping, int numberOfLoops)
        {

            if(isLooping == true)
            {
                for(int i = 0; i < numberOfLoops; i++)
                {
                    instance.Play();
                }
            }
            else
            {
                instance.Play();
            }


            
            instance.Play();

        }
    }
}
