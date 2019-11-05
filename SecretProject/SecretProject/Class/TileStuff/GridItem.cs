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

        int SourceRectangleOffSetX { get; set; }
        int SourceRectangleOffSetY { get; set; }

        public GridItem(ITileManager tileManager, int placeID)
        {
            this.IsDrawn = false;
            this.CanPlace = false;

            this.PlaceID = placeID;
            this.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(PlaceID, 100);

            SourceRectangleOffSetX = 0;
            SourceRectangleOffSetY = 0;
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(PlaceID))
            {


                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("newSource"))
                {
                    int[] rectangleCoords = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);
                    SourceRectangle = new Rectangle(SourceRectangle.X + rectangleCoords[0], SourceRectangle.Y + rectangleCoords[1],
                                            SourceRectangle.Width + rectangleCoords[2], SourceRectangle.Height + rectangleCoords[3]);
                    SourceRectangleOffSetX = rectangleCoords[0];
                    SourceRectangleOffSetY = rectangleCoords[1];

                }
            }
        }

        public void Update(GameTime gameTime, ITileManager tileManager, IInformationContainer container)
        {
            this.CanPlace = true;
            this.IsDrawn = true;
            if (tileManager.AbleToDrawTileSelector)
            {
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                {


                    bool ableToPlace = true;
                    for (int z = 1; z < container.AllTiles.Count; z++)
                    {



                        //tile under mouse is occupied
                        if (container.AllTiles[z][Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY].GID != -1)
                        {


                            this.DrawColor = Color.Red;

                            ableToPlace = false;

                        }
                    }
                    if (ableToPlace)
                    {
                        this.DrawColor = Color.Green;


                        if (Game1.myMouseManager.IsClicked)
                        {
                            if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                            {



                                int soundRandom = Game1.Utility.RGenerator.Next(0, 2);
                                switch (soundRandom)
                                {
                                    case 0:
                                        Game1.SoundManager.PlaceItem1.Play();
                                        break;
                                    case 1:
                                        Game1.SoundManager.PlaceItem2.Play();
                                        break;
                                }
                                TileUtility.ReplaceTilePermanent(3, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1, Game1.GetCurrentStage(), container);
                                Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool());
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

        public void Draw(SpriteBatch spriteBatch, ITileManager tileManager, IInformationContainer container)
        {
            if (this.IsDrawn)
            {
                int[] rectangleCoords = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);
                int negativeX = rectangleCoords[0] / 16;
                int negativeY = rectangleCoords[1] / 16;
                int positiveX = rectangleCoords[2] / 16;
                int positiveY = rectangleCoords[3] / 16;

                int activeChunkX = container.ArrayI;
                int activeChunkY = container.ArrayJ;

                int x = 0;
                int y = 0;

                for (int z = 1; z < container.AllTiles.Count; z++)
                {
                    
                    for (int i = negativeX; i < positiveX ; i++)
                    {

                        x = Game1.Player.UserInterface.TileSelector.IndexX + i;
                        activeChunkX = container.ArrayI;
                        for (int j = negativeY; j < positiveY; j++)
                        {
                            
                            y = Game1.Player.UserInterface.TileSelector.IndexY + j;
                            
                            activeChunkY = container.ArrayJ + 1;


                            //check if index is out of bounds of current chunk
                            if (x > 15)
                            {
                                x = x - 15;
                                activeChunkX ++;
                            }
                            else if (x < 0)
                            {
                                x = 15 + x;
                                activeChunkX --;
                            }

                            if(y > 15)
                            {
                                y = y - 15;
                                activeChunkY ++;
                            }
                            else if (y < 0)
                            {
                                y = y + 15;
                                activeChunkY --;
                            }

                            int gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][x, y].GID;
                            int newGID = PlaceID + i + (j * 100);
                            Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);
                            if (gid == -1)
                            {
                                

                                spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][x, y].DestinationRectangle.X,
                                    tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][x, y].DestinationRectangle.Y),
                                    newSourceRectangle, Color.White,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[z]);
                            }
                            else
                            {
                                spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][x, y].DestinationRectangle.X,
                                    tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][x,y].DestinationRectangle.Y),
                                    newSourceRectangle, Color.Red,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[z]);
                            }
                        }

                    }
                }

            }
        }


        //spriteBatch.Draw(tileManager.TileSet, new Vector2(Game1.Player.UserInterface.TileSelector.WorldX + SourceRectangleOffSetX, Game1.Player.UserInterface.TileSelector.WorldY + SourceRectangleOffSetY), this.SourceRectangle, this.DrawColor,
        //                    0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
    }



}

    

