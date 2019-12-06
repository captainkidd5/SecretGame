using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class GridItem : IEntity
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool IsDrawn { get; set; }
        public int PlaceID { get; set; }
        public bool CanPlace { get; set; }
        public Color DrawColor { get; set; }

        public int[] RectangleCoordinates { get; set; }
        public Rectangle SourceRectangle { get; set; }



        public int NegativeX { get; set; }
        public int NegativeY { get; set; }
        public int PositiveX { get; set; }
        public int PositiveY { get; set; }

        public GridItem(ITileManager tileManager, int placeID)
        {
            this.GraphicsDevice = tileManager.GraphicsDevice;
            this.IsDrawn = false;
            this.CanPlace = true;

            this.PlaceID = placeID;
            this.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(PlaceID, 100);
            LoadNewItem(tileManager);

        }
        //negative ID means indoors only
        public void LoadNewItem(ITileManager tileManager)
        {
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(PlaceID))
            {
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("newSource"))
                {
                    this.RectangleCoordinates = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);


                }
                else
                {
                    this.RectangleCoordinates = new int[4]
                   {0,0,16,16};
                }

                //we use this so only the hitbox section of the tile to place is checking to see if tiles underneath are empty. 
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("checkTile"))
                {
                    int[] newRectangleCoordinates = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["checkTile"]);

                    this.NegativeX = newRectangleCoordinates[0] / 16;
                    this.NegativeY = newRectangleCoordinates[1] / 16;
                    this.PositiveX = newRectangleCoordinates[2] / 16;
                    this.PositiveY = newRectangleCoordinates[3] / 16;
                }
                else
                {
                    this.NegativeX = RectangleCoordinates[0] / 16;
                    this.NegativeY = RectangleCoordinates[1] / 16;
                    this.PositiveX = RectangleCoordinates[2] / 16;
                    this.PositiveY = RectangleCoordinates[3] / 16;
                }
            }
            else
            {
                this.RectangleCoordinates = new int[4]
                    {0,0,16,16};

            }


            SourceRectangle = new Rectangle(SourceRectangle.X + RectangleCoordinates[0], SourceRectangle.Y + RectangleCoordinates[1],
                                   SourceRectangle.Width + RectangleCoordinates[2], SourceRectangle.Height + RectangleCoordinates[3]);

        }

        public void NormalUpdate(GameTime gameTime, ITileManager tileManager, IInformationContainer container)
        {
            if (Game1.Player.UserInterface.DrawTileSelector)
            {
                if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() != -50)
                {
                    this.IsDrawn = true;



                    int subX = 0;
                    int subY = 0;

                    bool canPlaceTotal = true;

                    for (int i = this.NegativeX; i < this.PositiveX; i++)
                    {



                        for (int j = this.NegativeY; j < 1; j++)
                        {
                            this.CanPlace = true;


                            subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                            subY = Game1.Player.UserInterface.TileSelector.IndexY + j;


                            //check if index is out of bounds of current chunk

                            for (int z = 1; z < container.AllTiles.Count; z++)
                            {
                                int gid = 0;
                                if (tileManager.AllTiles[z][subX, subY] != null)
                                {
                                    gid = tileManager.AllTiles[z][subX, subY].GID;
                                }
                                else
                                {
                                    return;
                                }

                                if (gid == -1)
                                {
                                }
                                else
                                {
                                    this.CanPlace = false;
                                    canPlaceTotal = false;

                                }
                            }


                        }

                    }

                    if (canPlaceTotal)
                    {
                        if (Game1.myMouseManager.IsClicked)
                        {
                            if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                            {



                                int soundRandom = Game1.Utility.RGenerator.Next(0, 2);
                                switch (soundRandom)
                                {
                                    case 0:
                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem1);

                                        break;
                                    case 1:
                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem2);
                                        break;
                                }

                                TileUtility.ReplaceTile(3, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1,  container);
                                Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool());

                                return;
                            }

                        }

                    }
                }
                else
                {
                    this.IsDrawn = false;
                }
            }
            else
            {
                this.IsDrawn = false;
            }

        }

        public void NormalDraw(SpriteBatch spriteBatch, ITileManager tileManager, IInformationContainer container)
        {
            if (this.IsDrawn)
            {


                int subX = 0;
                int subY = 0;



                for (int i = this.NegativeX; i < this.PositiveX; i++)
                {

                    //assumes newsource is always at bottom tiles, left or right doesn't matter though

                    for (int j = this.NegativeY; j < 1; j++)
                    {
                        bool canPlace = true;

                        subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                        subY = Game1.Player.UserInterface.TileSelector.IndexY + j;



                        //check if index is out of bounds of current chunk


                        int newGID = PlaceID + i + (j * 100);
                        Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);

                        for (int z = 1; z < container.AllTiles.Count; z++)
                        {
                            int gid = 0;
                            if (tileManager.AllTiles[z][subX, subY] != null)
                            {
                                gid = tileManager.AllTiles[z][subX, subY].GID;
                            }
                            else
                            {
                                return;
                            }

                            if (gid == -1)
                            {
                            }
                            else
                            {
                                canPlace = false;

                            }
                        }

                        if (canPlace)
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(tileManager.AllTiles[3][subX, subY]).X,
                                TileUtility.GetDestinationRectangle(tileManager.AllTiles[3][subX, subY]).Y),
                                newSourceRectangle, Color.White * .25f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(tileManager.AllTiles[3][subX, subY]).X,
                                TileUtility.GetDestinationRectangle(tileManager.AllTiles[3][subX, subY]).Y),
                                newSourceRectangle, Color.Red * .25f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                    }

                }



            }
        }

        public void ChunkUpdate(GameTime gameTime, ITileManager tileManager, IInformationContainer container)
        {
            if (Game1.Player.UserInterface.DrawTileSelector)
            {
                if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() != -50)
                {
                    this.IsDrawn = true;


                    int activeChunkX = container.ArrayI;
                    int activeChunkY = container.ArrayJ;

                    int subX = 0;
                    int subY = 0;

                    bool canPlaceTotal = true;

                    for (int i = this.NegativeX; i < this.PositiveX; i++)
                    {



                        for (int j = this.NegativeY; j < 1; j++)
                        {
                            this.CanPlace = true;


                            subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                            subY = Game1.Player.UserInterface.TileSelector.IndexY + j;
                            activeChunkX = container.ArrayI;
                            activeChunkY = container.ArrayJ;


                            //check if index is out of bounds of current chunk
                            if (subX > 15)
                            {
                                subX = subX - 16;


                                if (activeChunkX < 2)
                                {
                                    activeChunkX++;
                                }

                            }
                            else if (subX < 0)
                            {
                                subX = 16 + subX;
                                if (activeChunkX > 0)
                                {
                                    activeChunkX--;
                                }

                            }

                            if (subY > 15)
                            {
                                subY = subY - 16;
                                if (activeChunkY < 2)
                                {
                                    activeChunkY++;
                                }

                            }
                            else if (subY < 0)
                            {
                                subY = subY + 16;
                                if (activeChunkY > 0)
                                {
                                    activeChunkY--;
                                }

                            }
                            for (int z = 1; z < container.AllTiles.Count; z++)
                            {
                                int gid = 0;
                                if (tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY] != null)
                                {
                                    gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY].GID;
                                }
                                else
                                {
                                    return;
                                }

                                if (gid == -1)
                                {
                                }
                                else
                                {
                                    this.CanPlace = false;
                                    canPlaceTotal = false;

                                }
                            }


                        }

                    }

                    if (canPlaceTotal)
                    {
                        if (Game1.myMouseManager.IsClicked)
                        {
                            if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                            {



                                int soundRandom = Game1.Utility.RGenerator.Next(0, 2);
                                switch (soundRandom)
                                {
                                    case 0:
                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem1);

                                        break;
                                    case 1:
                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem2);
                                        break;
                                }
                                if (this.PlaceID == 2157)
                                {
                                    Portal tempPortal = new Portal(3, 5, 0, 50, true);
                                    tempPortal.PortalStart = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle;
                                    // tempPortal.
                                    Game1.World.AllPortals.Add(tempPortal);

                                    if (!Game1.PortalGraph.HasEdge(tempPortal.From, tempPortal.To))
                                    {
                                        Game1.PortalGraph.AddEdge(tempPortal.From, tempPortal.To);
                                    }
                                }
                                Item item = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                                TileUtility.ReplaceTile(3, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1, container);
                                if (item.TilingDictionary != null)
                                {

                                    ReassignTileForTiling(this.PlaceID, Game1.Procedural.FenceGeneratableTiles,
                                        item.TilingDictionary, 3,
                                        Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                        TileUtility.ChunkX, TileUtility.ChunkY, container);
                                    int i = Game1.Player.UserInterface.TileSelector.IndexX;
                                    int j = Game1.Player.UserInterface.TileSelector.IndexY;
                                    TileUtility.ReassignGroupOfTiles(3, i, j, this.PlaceID, Game1.Procedural.FenceGeneratableTiles, item.TilingDictionary, container);
                                    //for (int t = -1; t < 2; t++)
                                    //{
                                    //    for (int q = -1; q < 2; q++)
                                    //    {
                                    //        if (i > 0 && j > 0 && i < TileUtility.ChunkX - 1 && j < TileUtility.ChunkY - 1)
                                    //        {
                                    //            ReassignTileForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i + t, j + q, container.MapWidth, container.MapHeight, container);
                                    //        }
                                    //        else if (i > 0 && j <= 0 && i < TileUtility.ChunkX - 1 && j < TileUtility.ChunkY - 1)
                                    //        {
                                    //            ReassignTileForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i + t, j, container.MapWidth, container.MapHeight, container);
                                    //        }
                                    //        else if (i <= 0 && j > 0 && i < TileUtility.ChunkX - 1 && j < TileUtility.ChunkY - 1)
                                    //        {
                                    //            ReassignTileForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i, j + q, container.MapWidth, container.MapHeight, container);
                                    //        }
                                    //        else if (i >= TileUtility.ChunkX && j < TileUtility.ChunkY - 1)
                                    //        {
                                    //            ReassignTileForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i, j + q, container.MapWidth, container.MapHeight, container);
                                    //        }
                                    //        else if (i < TileUtility.ChunkX - 1 && j >= TileUtility.ChunkY)
                                    //        {
                                    //            ReassignTileForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i + t, j, container.MapWidth, container.MapHeight, container);
                                    //        }
                                    //        else if (i == TileUtility.ChunkX && j == TileUtility.ChunkY)
                                    //        {
                                    //            TileUtility.GenerationReassignForTiling(this.PlaceID , Game1.Utility.FenceGeneratableTiles, item.TilingDictionary, 3, i, j, container.MapWidth, container.MapHeight, container);
                                    //        }

                                    //    }
                                    //}

                                }
                                Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool());


                                return;
                            }

                        }

                    }
                }
                else
                {
                    this.IsDrawn = false;
                }
            }
            else
            {
                this.IsDrawn = false;
            }

        }

        public void ChunkDraw(SpriteBatch spriteBatch, ITileManager tileManager, IInformationContainer container)
        {
            if (this.IsDrawn)
            {


                int activeChunkX = container.ArrayI;
                int activeChunkY = container.ArrayJ;

                int subX = 0;
                int subY = 0;



                for (int i = this.NegativeX; i < this.PositiveX; i++)
                {

                    //assumes newsource is always at bottom tiles, left or right doesn't matter though

                    for (int j = this.NegativeY; j < 1; j++)
                    {
                        bool canPlace = true;

                        subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                        subY = Game1.Player.UserInterface.TileSelector.IndexY + j;
                        activeChunkX = container.ArrayI;
                        activeChunkY = container.ArrayJ;


                        //check if index is out of bounds of current chunk
                        if (subX > 15)
                        {
                            subX = subX - 16;


                            if (activeChunkX < 2)
                            {
                                activeChunkX++;
                            }

                        }
                        else if (subX < 0)
                        {
                            subX = 16 + subX;
                            if (activeChunkX > 0)
                            {
                                activeChunkX--;
                            }

                        }

                        if (subY > 15)
                        {
                            subY = subY - 16;
                            if (activeChunkY < 2)
                            {
                                activeChunkY++;
                            }

                        }
                        else if (subY < 0)
                        {
                            subY = subY + 16;
                            if (activeChunkY > 0)
                            {
                                activeChunkY--;
                            }

                        }

                        int newGID = PlaceID + i + (j * 100);
                        Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);

                        for (int z = 1; z < container.AllTiles.Count; z++)
                        {
                            int gid = 0;
                            if (tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY] != null)
                            {
                                gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY].GID;
                            }
                            else
                            {
                                return;
                            }

                            if (gid == -1)
                            {
                            }
                            else
                            {
                                canPlace = false;

                            }
                        }

                        if (canPlace)
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY]).X,
                                TileUtility.GetDestinationRectangle(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY]).Y),
                                newSourceRectangle, Color.White * .25f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY]).X,
                                TileUtility.GetDestinationRectangle(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY]).Y),
                                newSourceRectangle, Color.Red * .25f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                    }

                }



            }
        }

        public static void ReassignTileForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
            int x, int y, int worldWidth, int worldHeight, IInformationContainer container)
        {

            if (!generatableTiles.Contains(container.AllTiles[layer][x, y].GID))
            {
                return;
            }
            int keyToCheck = 0;
            if (y > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y - 1].GID))
                {
                    keyToCheck += 1;
                }
            }
            //if top tile is 0 we look at the chunk above it


            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1].AllTiles[layer][x, 15].GID))
            {
                keyToCheck += 1;
            }


            if (y < worldHeight - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y + 1].GID))
                {
                    keyToCheck += 8;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1].AllTiles[layer][x, 0].GID))
            {
                keyToCheck += 8;
            }


            //looking at rightmost tile
            if (x < worldWidth - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x + 1, y].GID))
                {
                    keyToCheck += 4;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ].AllTiles[layer][0, y].GID) )
            {
                keyToCheck += 4;
            }




            if (x > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x - 1, y].GID))
                {
                    keyToCheck += 2;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ].AllTiles[layer][15, y].GID) )
            {
                keyToCheck += 2;
            }


            TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] + 1, container);
            


        }

        public void PlayerCollisionInteraction()
        {
            throw new NotImplementedException();
        }
    }



}



