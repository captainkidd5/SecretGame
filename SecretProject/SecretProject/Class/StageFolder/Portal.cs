using Microsoft.Xna.Framework;
using SecretProject.Class.SavingStuff;
using System.IO;

namespace SecretProject.Class.StageFolder
{
    public class Portal : ISaveable
    {
        public int From { get; set; }
        public int To { get; set; }
        public Rectangle PortalStart { get; set; }
        public int SafteyOffSetX { get; set; }
        public int SafteyOffSetY { get; set; }
        public bool MustBeClicked { get; set; }
        //public Rectangle PortalEnd { get; set; }

        public Portal(int from, int to, int safteyX, int safteyY, bool mustBeClicked)
        {
            this.From = from;
            this.To = to;
            this.SafteyOffSetX = safteyX;
            this.SafteyOffSetY = safteyY;
            this.MustBeClicked = mustBeClicked;

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
            GameSerializer.WriteRectangle(PortalStart, writer);
            writer.Write(SafteyOffSetX);
            writer.Write(SafteyOffSetY);
            writer.Write(MustBeClicked);

        }

        public void Load(BinaryReader reader)
        {
            this.From = reader.ReadInt32();
            this.To = reader.ReadInt32();
            this.PortalStart = GameSerializer.ReadRectangle(reader);
            this.SafteyOffSetX = reader.ReadInt32();
            this.SafteyOffSetY = reader.ReadInt32();
            this.MustBeClicked = reader.ReadBoolean();
        }
    }
}
