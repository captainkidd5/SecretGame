using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff.SanctuaryStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.TileStuff
{
    public class SanctuaryTileManager : TileManager
    {
        public SanctuaryTileManager(Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths, ILocation currentStage) : base( tileSet,  mapName,allLayers,  graphicsDevice,  content,tileSetNumber, allDepths, currentStage)
        {
            AllPlots = new List<SPlot>();

            for (int i = 0; i < mapName.ObjectGroups["SanctuaryPlots"].Objects.Count; i++)
            {

                string plantedItemType = string.Empty;
                string plantID = string.Empty;
                mapName.ObjectGroups["SanctuaryPlots"].Objects[i].Properties.TryGetValue("PlantedItemType", out plantedItemType);
                mapName.ObjectGroups["SanctuaryPlots"].Objects[i].Properties.TryGetValue("plantID", out plantID);
                AllPlots.Add(new SPlot((PlantedItemType)Enum.Parse(typeof(PlantedItemType), plantedItemType), int.Parse(plantID), (int)mapName.ObjectGroups["SanctuaryPlots"].Objects[i].X, (int)mapName.ObjectGroups["SanctuaryPlots"].Objects[i].Y,
                    (int)mapName.ObjectGroups["SanctuaryPlots"].Objects[i].Width,(int)mapName.ObjectGroups["SanctuaryPlots"].Objects[i].Height));

            }
        }
    }
}
