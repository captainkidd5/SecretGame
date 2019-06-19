using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    interface IContent
    {
        ContentManager Content { get; set; }
        List<string> SceneAssets { get; set; }


        void Load<T>(string asset);


        void Unload();

    }
}
