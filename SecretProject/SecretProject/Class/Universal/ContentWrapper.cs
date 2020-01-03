using Microsoft.Xna.Framework.Content;

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
