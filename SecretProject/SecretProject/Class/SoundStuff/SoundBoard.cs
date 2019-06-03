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

        public SoundEffect WalkGrass;
        public SoundEffectInstance WalkGrassInstance;

        public SoundEffect WalkSand;
        public SoundEffectInstance WalkSandInstance;

        public SoundEffect WalkWood;
        public SoundEffectInstance WalkWoodInstance;

        public SoundEffect WalkStone;
        public SoundEffectInstance WalkStoneInstance;


        public SoundBoard(Game1 game, ContentManager content)
        {
            PickUpItem = content.Load<SoundEffect>("SoundEffects/bubble");
            PickUpItemInstance = PickUpItem.CreateInstance();

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

            WalkGrass = content.Load<SoundEffect>("SoundEffects/walkGrass");
            WalkGrassInstance = WalkGrass.CreateInstance();
            WalkGrassInstance.Volume = .75f;

            WalkSand = content.Load<SoundEffect>("SoundEffects/sandStep");
            WalkSandInstance = WalkSand.CreateInstance();
            WalkSandInstance.Volume = .75f;

            WalkWood = content.Load<SoundEffect>("SoundEffects/woodStep");
            WalkWoodInstance = WalkWood.CreateInstance();

            WalkStone = content.Load<SoundEffect>("SoundEffects/stoneStep");
            WalkStoneInstance = WalkStone.CreateInstance();
            WalkStoneInstance.Volume = .75f;
            

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

        public void PlaySoundEffectFromInt(bool isLooping, int numberOfLoops, int soundKey, float volume)
        {
            

            if (isLooping == true)
            {
                for (int i = 0; i < numberOfLoops; i++)
                {
                    switch (soundKey)
                    {
                        case 1:
                            WalkGrassInstance.Play();
                            break;
                        case 2:
                            WalkStoneInstance.Play();
                            break;
                        case 3:
                            WalkWoodInstance.Play();
                            break;
                        case 4:
                            WalkSandInstance.Play();
                            break;

                    }
                }
            }
            else
            {
                switch (soundKey)
                {
                    case 1:
                        WalkGrassInstance.Volume = volume;
                        WalkGrassInstance.Play();
                        break;
                    case 2:
                        WalkStoneInstance.Volume = volume;
                        WalkStoneInstance.Play();
                        break;
                    case 3:
                        WalkWoodInstance.Volume = volume;
                        WalkWoodInstance.Play();
                        break;
                    case 4:
                        WalkSandInstance.Volume = volume;
                        WalkSandInstance.Play();
                        break;
                    case 5:
                        PickUpItemInstance.Volume = volume;
                        PickUpItemInstance.Play();
                        break;
                    case 6:
                        GrassBreakInstance.Volume = volume;
                        GrassBreakInstance.Play();
                        break;
                    case 7:
                        DigDirtInstance.Volume = volume;
                        DigDirtInstance.Play();
                        break;
                    case 8:
                        StoneSmashInstance.Volume = volume;
                        StoneSmashInstance.Play();
                        break;
                    

                }
            }
        }
    }
}
