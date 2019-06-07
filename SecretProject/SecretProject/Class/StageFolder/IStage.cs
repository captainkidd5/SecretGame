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


        //TmxLayer Buildings { get; set; }

        //TmxLayer Background { get; set; }

        //TmxLayer Background1 { get; set; }

        //TmxLayer MidGround { get; set; }

        //TmxLayer foreGround { get; set; }

        //TmxLayer Placement { get; set; }


        Song MainTheme { get; set; }


        Camera2D Cam { get; set; }

        int TileSetNumber { get; set; }

        List<TmxLayer> AllLayers { get; set; }

        TileManager AllTiles { get; set; }



        List<ObjectBody> AllObjects { get; set; }


        List<Sprite> AllSprites { get; set; }

        List<Item> AllItems { get; set;}


List<ActionTimer> AllActions { get; set; }





        UserInterface MainUserInterface { get; set; }

        ContentManager Content { get; set; }
        GraphicsDevice Graphics { get; set; }
        Rectangle MapRectangle { get; set; }


        ParticleEngine ParticleEngine { get; set; }
        //SAVE STUFF

        bool TilesLoaded { get; set; }

        TextBuilder TextBuilder { get; set; }
        void Update(GameTime gameTime, MouseManager mouse, Player player);
        void LoadContent( Camera2D camera);
        void UnloadContent();
        void Draw(GraphicsDevice graphics, GameTime gameTime, SpriteBatch spriteBatch, MouseManager mouse, Player player);

    }
}
