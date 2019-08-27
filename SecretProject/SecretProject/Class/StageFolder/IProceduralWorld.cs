using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;

namespace SecretProject.Class.StageFolder
{
    public interface IProceduralWorld
    {
        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int TilesetTilesWide { get; set; }
        int TilesetTilesHigh { get; set; }
        Texture2D TileSet { get; set; }

        Camera2D Cam { get; set; }

        int TileSetNumber { get; set; }

        TileManager AllTiles { get; set; }

        Dictionary<int, ObjectBody> AllObjects { get; set; }


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set; }


        List<ActionTimer> AllActions { get; set; }

        List<Portal> AllPortals { get; set; }

        List<LightSource> AllLights { get; set; }
        UserInterface MainUserInterface { get; set; }

        ContentManager Content { get; set; }
        GraphicsDevice Graphics { get; set; }
        Rectangle MapRectangle { get; set; }
        Dictionary<int, Crop> AllCrops { get; set; }

        bool IsDark { get; set; }


        ParticleEngine ParticleEngine { get; set; }
        //SAVE STUFF

        bool TilesLoaded { get; set; }
        bool IsLoaded { get; set; }

        TextBuilder TextBuilder { get; set; }
    }
}
