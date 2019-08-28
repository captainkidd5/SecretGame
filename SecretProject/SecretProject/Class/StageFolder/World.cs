using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public class World : IProceduralWorld, ILocation
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        public Texture2D TileSet { get; set; }
        public Camera2D Cam { get; set; }
        public int TileSetNumber { get; set; }
        public TileManager AllTiles { get; set; }
        public Dictionary<int, ObjectBody> AllObjects { get; set; }
        public List<Sprite> AllSprites { get; set; }
        public List<Item> AllItems { get; set; }
        public List<ActionTimer> AllActions { get; set; }
        public List<Portal> AllPortals { get; set; }
        public List<LightSource> AllLights { get; set; }
        public UserInterface MainUserInterface { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Rectangle MapRectangle { get; set; }
        public Dictionary<int, Crop> AllCrops { get; set; }
        public bool IsDark { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public bool TilesLoaded { get; set; }
        public bool IsLoaded { get; set; }
        public TextBuilder TextBuilder { get; set; }
        public string StageName { get; set; }
        ITileManager ILocation.AllTiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public World()
        {
            this.TileWidth = 16;
            this.TileHeight = 16;
           // this.TilesetTilesWide = 
        }

        public void Update(GameTime gameTime, MouseManager mouse, Player player)
        {
            throw new NotImplementedException();
        }

        public void LoadPreliminaryContent()
        {
            throw new NotImplementedException();
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player)
        {
            throw new NotImplementedException();
        }

        public void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules)
        {
            throw new NotImplementedException();
        }
    }
}
