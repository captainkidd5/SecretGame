using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using ObjectBody = SecretProject.Class.ObjectFolder.ObjectBody;
using System;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.TileStuff;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;
using System.Runtime.Serialization;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.Universal;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.LightStuff;
using XMLData.RouteStuff;
using XMLData.ItemStuff;
/*
this.TileSet = Content.Load<Texture2D>("Map/MasterSpriteSheet");



//map specifications


AllDepths = new List<float>()
{
.1f,
.2f,
.3f,
.5f,
.6f
};



this.Map = new TmxMap("Content/Map/worldMap.tmx");
*/
namespace SecretProject.Class.StageFolder
{
    public interface ITmxStage
    {
        bool ShowBorders { get; set; }
        TmxMap Map { get; set; }
        string StageName { get; set; }

        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int TilesetTilesWide { get; set; }
        int TilesetTilesHigh { get; set; }

        Texture2D TileSet { get; set; }



        Camera2D Cam { get; set; }

        int TileSetNumber { get; set; }

        List<TmxLayer> AllLayers { get; set; }

        TileManager AllTiles { get; set; }



        Dictionary<int, ObjectBody> AllObjects { get; set; }


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set;}


List<ActionTimer> AllActions { get; set; }

        List<Portal> AllPortals { get; set; }

        List<LightSource> AllLights { get; set; }





        UserInterface MainUserInterface { get; set; }

        ContentManager Content { get; set; }
        GraphicsDevice Graphics { get; set; }
        Rectangle MapRectangle { get; set; }
        Dictionary<int,Crop> AllCrops { get; set; }

        bool IsDark { get; set; }


        ParticleEngine ParticleEngine { get; set; }
        //SAVE STUFF

        bool TilesLoaded { get; set; }
        bool IsLoaded { get; set; }

        TextBuilder TextBuilder { get; set; }
        void Update(GameTime gameTime, MouseManager mouse, Player player);
        void LoadPreliminaryContent();
        void LoadContent( Camera2D camera, List<RouteSchedule> routeSchedules);
        void UnloadContent();
        void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player);


    }
}
