

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff.SpawnStuff;
using System;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    /// <summary>
    /// Grid Items are any placeable item. Floorboard, barrels, chests etc. This class makes sure that the proper conditions are met for their placement. Update happens
    /// in tile manager update loop
    /// </summary>
    public class GridItem : IEntity
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool IsDrawn { get; set; }
        public int PlaceID { get; set; }
        public bool CanPlace { get; set; }
        public Color DrawColor { get; set; }

        public int[] RectangleCoordinates { get; set; }
        public Rectangle SourceRectangle { get; set; }

        //only to see if intersects player collider.
        public Rectangle DestinationRectangle { get; set; }



        public int NegativeXTest { get; set; }
        public int NegativeYTest { get; set; }
        public int PositiveXTest { get; set; }
        public int PositiveYTest { get; set; }

        public int NegativeXDraw { get; set; }
        public int NegativeYDraw { get; set; }
        public int PositiveXDraw { get; set; }
        public int PositiveYDraw { get; set; }

        public GridItem(TileManager tileManager, int placeID)
        {
            this.GraphicsDevice = tileManager.GraphicsDevice;
            this.IsDrawn = false;
            this.CanPlace = true;

            this.PlaceID = placeID;
            this.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(this.PlaceID, 100);
            LoadNewItem(tileManager);

        }
        //negative ID means indoors only
        public void LoadNewItem(TileManager tileManager)
        {
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(this.PlaceID))
            {
                if (this.PlaceID == 4118)
                {
                    Console.WriteLine("hi");
                }
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties.ContainsKey("newSource"))
                {
                    this.RectangleCoordinates = TileUtility.GetRectangeFromString(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties["newSource"]);


                }
                else
                {
                    this.RectangleCoordinates = new int[4]
                   {0,0,16,16};
                }

                //we use this so only the hitbox section of the tile to place is checking to see if tiles underneath are empty. 
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties.ContainsKey("checkTile"))
                {
                    int[] newRectangleCoordinates = TileUtility.GetRectangeFromString(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties["checkTile"]);

                    this.NegativeXTest = newRectangleCoordinates[0] / 16;
                    this.NegativeYTest = newRectangleCoordinates[1] / 16;
                    this.PositiveXTest = newRectangleCoordinates[2] / 16;
                    this.PositiveYTest = newRectangleCoordinates[3] / 16;
                }
                else
                {
                    this.NegativeXTest = this.RectangleCoordinates[0] / 16;
                    this.NegativeYTest = this.RectangleCoordinates[1] / 16;
                    this.PositiveXTest = this.RectangleCoordinates[2] / 16;
                    this.PositiveYTest = this.RectangleCoordinates[3] / 16;
                }

                this.NegativeXDraw = this.RectangleCoordinates[0] / 16;
                this.NegativeYDraw = this.RectangleCoordinates[1] / 16;
                this.PositiveXDraw = this.RectangleCoordinates[2] / 16;
                this.PositiveYDraw = this.RectangleCoordinates[3] / 16;

                if (this.NegativeYDraw < 0)
                {
                    this.PositiveYDraw = PositiveYDraw + NegativeYDraw;
                    this.PositiveYTest = PositiveYTest + NegativeYTest;
                }
                if (NegativeXDraw < 0)
                {
                    this.PositiveXDraw = PositiveXDraw + NegativeXDraw;
                    this.PositiveXTest = PositiveXTest + NegativeXTest;
                }
            }
            else
            {
                this.RectangleCoordinates = new int[4]
                    {0,0,16,16};

            }


            this.SourceRectangle = new Rectangle(this.SourceRectangle.X + this.RectangleCoordinates[0], this.SourceRectangle.Y + this.RectangleCoordinates[1],
                                   this.SourceRectangle.Width + this.RectangleCoordinates[2], this.SourceRectangle.Height + this.RectangleCoordinates[3]);

        }

        public bool CanPlaceTotal { get; set; }





        public void NormalUpdate(GameTime gameTime, TileManager tileManager, TileManager TileManager)
        {
            this.CanPlace = false;
            this.DestinationRectangle = new Rectangle((int)(Game1.MouseManager.WorldMousePosition.X + NegativeXDraw * 16),
                (int)(Game1.MouseManager.WorldMousePosition.Y + NegativeYDraw * 16),
                PositiveXDraw * 16, PositiveYDraw * 16);


            if (Game1.Player.UserInterface.DrawTileSelector)
            {
                ItemData item = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                if (item != null)
                {
                    TileManager allTiles = StageManager.CurrentStage.AllTiles;
                    this.IsDrawn = true;


                    int subX = 0;
                    int subY = 0;

                    CanPlaceTotal = true;

                    for (int i = this.NegativeXTest; i < this.PositiveXTest; i++)
                    {
                        for (int j = this.NegativeYTest; j < this.PositiveYTest; j++)
                        {



                            //subX = (int)Game1.MouseManager.WorldMousePosition.X + i * 16;
                            //subY = (int)Game1.MouseManager.WorldMousePosition.Y + j * 16;

                            //int subResultX = (int)Math.Floor((float)((float)subX / 16f / 16f));
                            //int subResultY = (int)Math.Floor((float)((float)subY / 16f / 16f));


                            subX = (int)Game1.MouseManager.SquarePosition.X + i;
                            subY = (int)Game1.MouseManager.SquarePosition.Y + j;
                            if (Game1.Player.ColliderRectangle.Intersects(this.DestinationRectangle))
                            {
                                CanPlaceTotal = false;
                            }
                            if (item.TilingLayer == 1)
                            {
                                if (allTiles.AllTiles[1][subX, subY].GID == -1) //Floor tiles check to make sure there's not already a floor there. Grass etc.
                                {

                                }
                                else
                                {
                                    CanPlaceTotal = false;
                                }
                            }
                            else if (allTiles.PathGrid.Weight[subX, subY] == (int)GridStatus.Clear)
                            {

                            }
                            else
                            {
                                CanPlaceTotal = false;
                            }
                        }

                    }

                    if (CanPlaceTotal)
                    {
                        CanPlace = true;
                        if (Game1.MouseManager.IsClicked)
                        {
                            if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                            {



                                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PlaceItem1, true, .075f);



                                if (this.PlaceID == 2157)
                                {
                                    //Portal tempPortal = new Portal(3, 5, -56, 5, true);
                                    //tempPortal.PortalStart = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][ChunkUtility.GetLocalChunkCoord(subX), ChunkUtility.GetLocalChunkCoord(subY)].DestinationRectangle;
                                    //StageManager.CurrentStage.AllPortals.Add(tempPortal);

                                    //if (!Game1.PortalGraph.HasEdge(tempPortal.From, tempPortal.To))
                                    //{
                                    //    Game1.PortalGraph.AddEdge(tempPortal.From, tempPortal.To);
                                    //}
                                }
                                TileUtility.ReplaceTile(item.TilingLayer, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID, TileManager);
                                string tilingSet = item.TilingSet;
                                if (tilingSet != null)
                                {


                                    GenerationType generationType = (GenerationType)Enum.Parse(typeof(GenerationType), item.TilingSet);
                                    if (generationType != 0)
                                    {


                                        TilingTileManager tilingTileManager = Game1.Procedural.GetTilingTileManagerFromGenerationType(generationType);

                                        int worldWidth = StageManager.CurrentStage.AllTiles.MapWidth;
                                        WangManager.ReassignForTiling(PlaceID, tilingTileManager.GeneratableTiles, tilingTileManager.TilingDictionary,
                                            item.TilingLayer, (int)Game1.MouseManager.SquarePosition.X, (int)Game1.MouseManager.SquarePosition.Y, worldWidth, worldWidth, (TileManager)allTiles);
                                    }
                                }

                                Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool());
                                Game1.Player.UserInterface.BackPack.CheckGridItem();


                                return;
                            }

                        }

                    }


                }

            }


        }

        public void PlayPlacementSound()
        {

            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.PlaceItem1, true, .075f);



        }

        public void NormalDraw(SpriteBatch spriteBatch, TileManager tileManager, TileManager TileManager)
        {


            if (this.IsDrawn)
            {

                int subX = 0;
                int subY = 0;

                for (int i = this.NegativeXDraw; i < this.PositiveXDraw; i++)
                {
                    for (int j = this.NegativeYDraw; j < 1; j++)
                    {

                        subX = (int)Game1.MouseManager.SquarePosition.X + i;
                        subY = (int)Game1.MouseManager.SquarePosition.Y + j;



                        int newGID = this.PlaceID + i + (j * 100);
                        Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);


                        if (CanPlace)
                        {
                            spriteBatch.Draw(tileManager.TileSet, StageManager.CurrentStage.AllTiles.AllTiles[3][subX, subY].GetPosition((TileManager)StageManager.CurrentStage.AllTiles),
                                newSourceRectangle, Color.White * .25f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, StageManager.CurrentStage.AllTiles.AllTiles[3][subX, subY].GetPosition((TileManager)StageManager.CurrentStage.AllTiles),
                              newSourceRectangle, Color.Red,
                                      0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                    }

                }


            }

        }
        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }
    }



}



