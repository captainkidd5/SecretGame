using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SoundStuff
{
    public class SongChooser
    {
        public List<SoundEffect> Songs { get; set; }
        public List<SoundEffect> LastPlayedSongs { get; set; }

        public SongChooser(List<SoundEffect> songs)
        {
            this.Songs = songs;
            this.LastPlayedSongs = new List<SoundEffect>();
        }

        public void PlaySong()
        {
            int songIndex = Game1.Utility.RNumber(0, Songs.Count);
            if(!LastPlayedSongs.Contains(Songs[songIndex]))
            {

            }
        }
    }
}
