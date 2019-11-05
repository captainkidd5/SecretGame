using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class GridItem
    {
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
            this.IsDrawn = false;
            this.CanPlace = true;

            this.PlaceID = placeID;
            this.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(PlaceID, 100);
            LoadNewItem(tileManager);

            //if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(PlaceID))
            //{


            //    if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("newSource"))
            //    {
            //        int[] rectangleCoords = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);
            //        SourceRectangle = new Rectangle(SourceRectangle.X + rectangleCoords[0], SourceRectangle.Y + rectangleCoords[1],
            //                                SourceRectangle.Width + rectangleCoords[2], SourceRectangle.Height + rectangleCoords[3]);
            //        SourceRectangleOffSetX = rectangleCoords[0];
            //        SourceRectangleOffSetY = rectangleCoords[1];

            //    }
            //}
        }

        public void LoadNewItem(ITileManager tileManager)
        {
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(PlaceID))
            {
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("newSource"))
                {
                    int[] rectangleCoords = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);
                    this.NegativeX = rectangleCoords[0] / 16;
                    this.NegativeY = rectangleCoords[1] / 16;
                    this.PositiveX = rectangleCoords[2] / 16;
                    this.PositiveY = rectangleCoords[3] / 16;
                }
            }
            
        }

        public void Update(GameTime gameTime, ITileManager tileManager, IInformationContainer container)
        {
            if (tileManager.AbleToDrawTileSelector)
            {
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                {
                    this.IsDrawn = true;


                    int activeChunkX = container.ArrayI;
                    int activeChunkY = container.ArrayJ;

                    int subX = 0;
                    int subY = 0;



                    for (int i = this.NegativeX; i < this.PositiveX; i++)
                    {



                        for (int j = this.NegativeY; j < this.PositiveY; j++)
                        {
                            this.CanPlace = true;


                            subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                            subY = Game1.Player.UserInterface.TileSelector.IndexY + j;
                            activeChunkX = container.ArrayI;
                            activeChunkY = container.ArrayJ;


                            //check if index is out of bounds of current chunk
                            if (subX > 15)
                            {
                                subX = subX - 15;


                                if (activeChunkX < 2)
                                {
                                    activeChunkX++;
                                }

                            }
                            else if (subX < 0)
                            {
                                subX = 15 + subX;
                                if (activeChunkX > 0)
                                {
                                    activeChunkX--;
                                }

                            }

                            if (subY > 15)
                            {
                                subY = subY - 15 - 1;
                                if (activeChunkY < 2)
                                {
                                    activeChunkY++;
                                }

                            }
                            else if (subY < 0)
                            {
                                subY = subY + 15;
                                if (activeChunkY > 0)
                                {
                                    activeChunkY--;
                                }

                            }
                            for (int z = 1; z < container.AllTiles.Count; z++)
                            {
                                int gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY].GID;

                                if (gid == -1)
                                {
                                }
                                else
                                {
                                    this.CanPlace = false;

                                }
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

            if(CanPlace)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch, ITileManager tileManager, IInformationContainer container)
        {
            if (this.IsDrawn)
            {


                int activeChunkX = container.ArrayI;
                int activeChunkY = container.ArrayJ;

                int subX = 0;
                int subY = 0;



                for (int i = this.NegativeX; i < this.PositiveX; i++)
                {



                    for (int j = this.NegativeY; j < this.PositiveY; j++)
                    {


                        subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                        subY = Game1.Player.UserInterface.TileSelector.IndexY + j;
                        activeChunkX = container.ArrayI;
                        activeChunkY = container.ArrayJ;


                        //check if index is out of bounds of current chunk
                        if (subX > 15)
                        {
                            subX = subX - 15;


                            if (activeChunkX < 2)
                            {
                                activeChunkX++;
                            }

                        }
                        else if (subX < 0)
                        {
                            subX = 15 + subX;
                            if (activeChunkX > 0)
                            {
                                activeChunkX--;
                            }

                        }

                        if (subY > 15)
                        {
                            subY = subY - 15 - 1;
                            if (activeChunkY < 2)
                            {
                                activeChunkY++;
                            }

                        }
                        else if (subY < 0)
                        {
                            subY = subY + 15;
                            if (activeChunkY > 0)
                            {
                                activeChunkY--;
                            }

                        }

                        int newGID = PlaceID + i + (j * 100);
                        Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);

                        if (this.CanPlace)
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.X,
                                tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.Y),
                                newSourceRectangle, Color.White,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.X,
                                tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.Y),
                                newSourceRectangle, Color.Red,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                    }

                }
                


            }
        }

    }



}



