using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.StageFolder
{
    public class SanctuaryBase : TmxStageBase
    {
        public SanctuaryBase(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, int tileSetNumber, Texture2D tileSet, string tmxMapPath, int dialogueToRetrieve, int backDropNumber) : base(name, locationType, stageType, graphics, content, tileSetNumber, tileSet, tmxMapPath, dialogueToRetrieve, backDropNumber)
        {


        }

        public override void LoadPreliminaryContent()
        {
            this.AllNightLights = new List<LightSource>()
            {

            };


            this.AllDayTimeLights = new List<LightSource>();

            this.AllSprites = new List<Sprite>()
            {

            };

            this.AllObjects = new Dictionary<string, ICollidable>()
            {

            };

            this.AllItems = new List<Item>()
            {

            };

            //AllItems.Add(Game1.ItemVault.GenerateNewItem(147, new Vector2(Game1.Player.Position.X + 50, Game1.Player.Position.Y + 100), true));




            this.AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
            };

            this.Map = new TmxMap(this.TmxMapPath);
            this.Background = this.Map.Layers["background"];
            this.MidGround = this.Map.Layers["midGround"];
            this.Buildings = this.Map.Layers["buildings"];
            this.foreGround = this.Map.Layers["foreGround"];
            this.AllLayers = new List<TmxLayer>()
            {
                this.Background,
                this.MidGround,
                this.Buildings,
                this.foreGround

            };
            this.AllPortals = new List<Portal>();
            this.AllTiles = new SanctuaryTileManager(this.TileSet, this.Map, this.AllLayers, this.Graphics, this.Content, this.TileSetNumber, this.AllDepths, this);
            this.AllTiles.LoadInitialTileObjects(this);
            this.TileWidth = this.Map.Tilesets[this.TileSetNumber].TileWidth;
            this.TileHeight = this.Map.Tilesets[this.TileSetNumber].TileHeight;

            this.TilesetTilesWide = this.TileSet.Width / this.TileWidth;
            this.TilesetTilesHigh = this.TileSet.Height / this.TileHeight;


            this.AllActions = new List<ActionTimer>();

            this.MapRectangle = new Rectangle(0, 0, this.TileWidth * this.Map.Width, this.TileHeight * this.Map.Height);
            this.Map = null;
            this.AllCrops = new Dictionary<string, Crop>();


            //Sprite KayaSprite = new Sprite(graphics, Kaya, new Rectangle(0, 0, 16, 32), new Vector2(400, 400), 16, 32);
            this.QuadTree = new QuadTree(0, this.MapRectangle);
        }
    }
}
