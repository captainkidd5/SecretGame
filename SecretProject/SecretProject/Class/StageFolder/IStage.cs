﻿using System.Collections.Generic;
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

namespace SecretProject.Class.StageFolder
{
    public interface IStage
    {
        bool ShowBorders { get; set; }
        TmxMap Map { get; set; }

        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int TilesetTilesWide { get; set; }
        int TilesetTilesHigh { get; set; }

        Texture2D TileSet { get; set; }


        TmxLayer Buildings { get; set; }

        TmxLayer Background { get; set; }

        TmxLayer Background1 { get; set; }

        TmxLayer MidGround { get; set; }

        TmxLayer foreGround { get; set; }

        TmxLayer Placement { get; set; }

        Texture2D JoeSprite { get; set; }

        Texture2D RaftDown { get; set; }

        Texture2D PuzzleFish { get; set; }

        Texture2D HouseKey { get; set; }

        Song MainTheme { get; set; }


        Player Mastodon { get; set; }

        Camera2D Cam { get; set; }

        int TileSetNumber { get; set; }

        List<TmxLayer> AllLayers { get; set; }

        TileManager AllTiles { get; set; }



        List<ObjectBody> AllObjects { get; set; }


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set;}


List<ActionTimer> AllActions { get; set; }





        UserInterface MainUserInterface { get; set; }



        //SAVE STUFF

        bool TilesLoaded { get; set; }

        void Update(GameTime gameTime, MouseManager mouse, Game1 game);
        void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse);

    }
}
