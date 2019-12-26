using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;

using SecretProject.Class.ParticileStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public enum LocationType
    {
        Exterior = 1,
        Interior = 2
    }
    public enum StageType
    {
        Standard = 1,
        Procedural = 2,
        Sanctuary = 3
    }
    public interface ILocation
    {
        LocationType LocationType { get; set; }
        StageType StageType { get; set; }
        int StageIdentifier { get; set; }
        string StageName { get; set; }
        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int TilesetTilesWide { get; set; }
        int TilesetTilesHigh { get; set; }
        Texture2D TileSet { get; set; }
        ITileManager AllTiles { get; set; }

        Camera2D Cam { get; set; }

        int TileSetNumber { get; set; }
        


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set; }
        List<LightSource> AllLights { get; set; }


        List<ActionTimer> AllActions { get; set; }

        List<Portal> AllPortals { get; set; }

        
        UserInterface MainUserInterface { get; set; }

        ContentManager Content { get; set; }
        GraphicsDevice Graphics { get; set; }
        Rectangle MapRectangle { get; set; }

        bool IsDark { get; set; }
        bool ShowBorders { get; set; }
        ParticleEngine ParticleEngine { get; set; }
        TextBuilder TextBuilder { get; set; }
        bool IsLoaded { get; set; }
        List<Character> CharactersPresent { get; set; }
        List<StringWrapper> AllTextToWrite { get; set; }
        List<INPC> OnScreenNPCS { get; set; }

        List<float> AllDepths { get; set; }

         TmxLayer Buildings { get; set; }

         TmxLayer Background { get; set; }

         TmxLayer Background1 { get; set; }

         TmxLayer MidGround { get; set; }

         TmxLayer foreGround { get; set; }

        List<TmxLayer> AllLayers { get; set; }

         TmxMap Map { get; set; }
        event EventHandler SceneChanged;
         string TmxMapPath { get; set; }

        QuadTree QuadTree { get; set; }

        List<RisingText> AllRisingText { get; set; }

        void Update(GameTime gameTime, MouseManager mouse, Player player);
        void LoadPreliminaryContent();
        void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules);
        void UnloadContent();
        void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player);
        void AddTextToAllStrings(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration);
        void ActivateNewRisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade, float scale);



    }
}
