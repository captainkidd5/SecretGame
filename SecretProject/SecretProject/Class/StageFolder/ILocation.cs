﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.ObjectFolder;
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
using XMLData.ItemStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.StageFolder
{
    public interface ILocation
    {
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
        Dictionary<float, ObjectBody> AllObjects { get; set; }


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set; }


        List<ActionTimer> AllActions { get; set; }

        List<Portal> AllPortals { get; set; }

        List<LightSource> AllLights { get; set; }
        UserInterface MainUserInterface { get; set; }

        ContentManager Content { get; set; }
        GraphicsDevice Graphics { get; set; }
        Rectangle MapRectangle { get; set; }
        Dictionary<float, Crop> AllCrops { get; set; }

        bool IsDark { get; set; }
        bool ShowBorders { get; set; }
        ParticleEngine ParticleEngine { get; set; }
        TextBuilder TextBuilder { get; set; }
        bool IsLoaded { get; set; }
        List<Character> CharactersPresent { get; set; }
        void Update(GameTime gameTime, MouseManager mouse, Player player);
        void LoadPreliminaryContent();
        void LoadContent(Camera2D camera, List<RouteSchedule> routeSchedules);
        void UnloadContent();
        void Draw(GraphicsDevice graphics, RenderTarget2D mainTarget, RenderTarget2D lightsTarget, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player);
    }
}