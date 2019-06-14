
using System.Collections.Generic;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

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
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Universal;
using SecretProject.Class.ParticileStuff;
using XMLData.DialogueStuff;
using SecretProject.Class.DialogueStuff;

namespace SecretProject.Class.StageFolder
{

    public class NormalStage : StageBase
    {

        #region FIELDS

        

        #endregion

        #region CONSTRUCTOR



        public NormalStage(GraphicsDevice graphics, ContentManager content, int tileSetNumber, string mapTexturePath, string tmxMapPath, int dialogueToRetrieve) :base(graphics, content, tileSetNumber, mapTexturePath, tmxMapPath, dialogueToRetrieve)
        {
            this.Graphics = graphics;
            this.Content = content;
            this.TileSetNumber = tileSetNumber;


        }


        #endregion

    }
}