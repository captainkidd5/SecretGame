using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using TiledSharp;
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    class Interior : IStage
    {
        public bool ShowBorders { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TmxMap Map { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TilesetTilesWide { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TilesetTilesHigh { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Texture2D TileSet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Song MainTheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Camera2D Cam { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileSetNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<TmxLayer> AllLayers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TileManager AllTiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ObjectBody> AllObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Sprite> AllSprites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Item> AllItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ActionTimer> AllActions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UserInterface MainUserInterface { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContentManager Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDevice Graphics { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle MapRectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParticleEngine ParticleEngine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TilesLoaded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TextBuilder TextBuilder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Portal> AllPortals { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDark { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<LightSource> AllLights { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Crop> AllCrops { get; set; }
        Dictionary<int, ObjectBody> IStage.AllObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget,GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Camera2D camera)
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
