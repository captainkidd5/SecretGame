using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretProject.Class.Playable;

namespace SecretProject.Class.Universal
{
    public static class GameSerializer
    {



        public static void SavePlayer(Player player, BinaryWriter writer, float version)
        {
            writer.Write(Game1.Player.Position.X);
            writer.Write(Game1.Player.Position.Y);
            writer.Write(Game1.Player.Name);
            //writer.Write(Game1.)
        }




    }
}
