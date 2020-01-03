using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

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
