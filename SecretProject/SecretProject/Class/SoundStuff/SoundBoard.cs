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

        //Sound Effects are WAV
        //Songs are MP3
        public SoundEffect PickUpItem { get; set; }
        public SoundEffectInstance PickUpItemInstance { get; set; }
        public SoundEffect StoneStep { get; set; }
        public SoundEffectInstance StoneStepInstance { get; set; }

        public SoundEffect GrassBreak;
        public SoundEffectInstance GrassBreakInstance;
        public SoundEffect PlaceBarrel;
        public SoundEffectInstance PlaceBarrelInstance;
        public SoundEffect DoorOpen;
        public SoundEffectInstance DoorOpenInstance;
        public SoundEffect DigDirt;
        public SoundEffectInstance DigDirtInstance;
        public SoundEffect StoneSmash;
        public SoundEffectInstance StoneSmashInstance;


        public SoundBoard(Game1 game, ContentManager content)
        {
            PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");
            PickUpItemInstance = PickUpItem.CreateInstance();

            StoneStep = content.Load<SoundEffect>("SoundEffects/bubble");
            StoneStepInstance = StoneStep.CreateInstance();

            GrassBreak = content.Load<SoundEffect>("SoundEffects/grassBreakWav");
            GrassBreakInstance = GrassBreak.CreateInstance();

            PlaceBarrel = content.Load<SoundEffect>("SoundEffects/placeBarrel");
            PlaceBarrelInstance = PlaceBarrel.CreateInstance();

            DoorOpen = content.Load<SoundEffect>("SoundEffects/doorOpen");
            DoorOpenInstance = DoorOpen.CreateInstance();

            DigDirt = content.Load<SoundEffect>("SoundEffects/diggingDirt");
            DigDirtInstance = DigDirt.CreateInstance();

            StoneSmash = content.Load<SoundEffect>("SoundEffects/stoneSmash");
            StoneSmashInstance = StoneSmash.CreateInstance();

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


            
          //  instance.Play();

        }
    }
}
