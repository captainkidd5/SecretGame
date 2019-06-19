using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class ContentWrapper
    {
        public ContentManager BasicContent { get; set; }
        public ContentManager OrchardContent { get; set; }
        public ContentManager DockContent { get; set; }


        public ContentWrapper(ContentManager content)
        {
           // this.BasicContent = content;
            //SceneAssets = new List<string>();
        }

        public void Load(ContentManager content)
        {
            
            
        }

        public void Unload()
        {
            this.BasicContent.Unload();
        }

    }
}
