﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.StageFolder;
using System;

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



        public int NegativeXTest { get; set; }
        public int NegativeYTest { get; set; }
        public int PositiveXTest { get; set; }
        public int PositiveYTest { get; set; }

        public int NegativeXDraw { get; set; }
        public int NegativeYDraw { get; set; }
        public int PositiveXDraw { get; set; }
        public int PositiveYDraw { get; set; }

        public GridItem(ITileManager tileManager, int placeID)
        {
            this.GraphicsDevice = tileManager.GraphicsDevice;
            this.IsDrawn = false;
            this.CanPlace = true;

            this.PlaceID = placeID;
            this.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(this.PlaceID, 100);
            LoadNewItem(tileManager);

        }
        //negative ID means indoors only
        public void LoadNewItem(ITileManager tileManager)
        {
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(this.PlaceID))
            {
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties.ContainsKey("newSource"))
                {
                    this.RectangleCoordinates = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties["newSource"]);


                }
                else
                {
                    this.RectangleCoordinates = new int[4]
                   {0,0,16,16};
                }

                //we use this so only the hitbox section of the tile to place is checking to see if tiles underneath are empty. 
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties.ContainsKey("checkTile"))
                {
                    int[] newRectangleCoordinates = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[this.PlaceID].Properties["checkTile"]);

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

                if(this.NegativeYDraw < 0)
                {
                    this.PositiveYDraw = PositiveYDraw + NegativeYDraw;
                    this.PositiveYTest = PositiveYTest + NegativeYTest;
                }
                if(NegativeXDraw < 0)
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


        public void ChunkUpdate(GameTime gameTime, ITileManager tileManager, IInformationContainer container)
        {
            if (Game1.Player.UserInterface.DrawTileSelector)
            {
                Item item = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                if (item != null)
                {

                    this.IsDrawn = true;


                    int activeChunkX = container.ArrayI;
                    int activeChunkY = container.ArrayJ;

                    int subX = 0;
                    int subY = 0;

                    bool canPlaceTotal = true;

                    for (int i = this.NegativeXTest; i < this.PositiveXTest; i++)
                    {
                        for (int j = this.NegativeYTest; j < this.PositiveYTest; j++)
                        {
                            this.CanPlace = true;


                            subX = (int)Game1.myMouseManager.WorldMousePosition.X + i * 16;
                            subY = (int)Game1.myMouseManager.WorldMousePosition.Y + j * 16;
                            if (Game1.myMouseManager.WorldMousePosition.X < 0)
                            {
                                subX -= 16;
                            }
                            if (Game1.myMouseManager.WorldMousePosition.Y < 0)
                            {
                                subY -= 16;
                            }
                            for (int z = item.TilingLayer; z < 4; z++)
                            {

                                GridStatus gridStatus = GridStatus.Clear;

                             //   int pathGridSpecification = TileUtility.GetLocalChunkCoord()
                                Tile tile = ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), z, tileManager.ActiveChunks);
                                if (tile != null)
                                {
                                    //if()
                                    if (ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), z, tileManager.ActiveChunks).GID == -1)
                                    {

                                    }
                                    else
                                    {
                                        canPlaceTotal = false;
                                    }
                                }

                                else
                                {
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



                                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem1, true, .075f);



                                if (this.PlaceID == 2157)
                                {
                                    Portal tempPortal = new Portal(3, 5, -56, 5, true);
                                    tempPortal.PortalStart = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][ChunkUtility.GetLocalChunkCoord(subX), ChunkUtility.GetLocalChunkCoord(subY)].DestinationRectangle;
                                    Game1.OverWorld.AllPortals.Add(tempPortal);

                                    if (!Game1.PortalGraph.HasEdge(tempPortal.From, tempPortal.To))
                                    {
                                        Game1.PortalGraph.AddEdge(tempPortal.From, tempPortal.To);
                                    }
                                }
                                TileUtility.ReplaceTile(item.TilingLayer, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1, container);
                                if (item.GenerationType != 0)
                                {


                                    TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGenerationType(item.GenerationType);
                                    WangManager.GroupReassignForTiling((int)Game1.myMouseManager.WorldMousePosition.X, (int)Game1.myMouseManager.WorldMousePosition.Y, this.PlaceID, tilingContainer.GeneratableTiles,
                                tilingContainer.TilingDictionary, item.TilingLayer, tileManager);
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

                int subX = 0;
                int subY = 0;
               
                for (int i = this.NegativeXDraw; i < this.PositiveXDraw; i++)
                {
                    for (int j = this.NegativeYDraw; j < 1; j++)
                    {
                        bool canPlace = true;

                        subX = (int)Game1.myMouseManager.WorldMousePosition.X + i * 16;
                        subY = (int)Game1.myMouseManager.WorldMousePosition.Y + j * 16;

                        if (Game1.myMouseManager.WorldMousePosition.X < 0)
                        {
                            subX -= 16;
                        }
                        if (Game1.myMouseManager.WorldMousePosition.Y < 0)
                        {
                            subY -= 16;
                        }
                        Tile tile = ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks);
                        if (tile != null)
                        {
                            if (ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks).GID == -1)
                            {

                            }
                            else
                            {
                                this.CanPlace = false;

                            }
                        }

                        else
                        {
                            this.CanPlace = false;
                            return;
                        }

                        int newGID = this.PlaceID + i + (j * 100);
                        Rectangle newSourceRectangle = TileUtility.GetSourceRectangleWithoutTile(newGID, 100);


                        if (canPlace)
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks)).X,
                                TileUtility.GetDestinationRectangle(ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks)).Y),
                                newSourceRectangle, Color.White * 1f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(TileUtility.GetDestinationRectangle(ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks)).X,
                                TileUtility.GetDestinationRectangle(ChunkUtility.GetChunkTile(TileUtility.GetSquareTileCoord(subX), TileUtility.GetSquareTileCoord(subY), 3, tileManager.ActiveChunks)).Y),
                                newSourceRectangle, Color.Red * .1f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                    }

                }

            }
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

                    for (int i = this.NegativeXTest; i < this.PositiveXTest; i++)
                    {



                        for (int j = this.NegativeYTest; j < 1; j++)
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





                                if (Game1.GetCurrentStage().StageType == StageType.Sanctuary)
                                {
                                    SanctuaryTracker tracker = Game1.GetSanctuaryTrackFromStage(Game1.GetCurrentStageInt());
                                    if (tracker.UpdateCompletionGuide(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool()))
                                    {
                                        TileUtility.ReplaceTile(3, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1, container);

                                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool());
                                        PlayPlacementSound();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    TileUtility.ReplaceTile(3, Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY,
                                    this.PlaceID + 1, container);

                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool());
                                    PlayPlacementSound();
                                    return;
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

        }

        public void PlayPlacementSound()
        {

            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PlaceItem1, true, .075f);



        }

        public void NormalDraw(SpriteBatch spriteBatch, ITileManager tileManager, IInformationContainer container)
        {
            if (this.IsDrawn)
            {


                int subX = 0;
                int subY = 0;



                for (int i = this.NegativeXTest; i < this.PositiveXTest; i++)
                {

                    //assumes newsource is always at bottom tiles, left or right doesn't matter though

                    for (int j = this.NegativeYTest; j < 1; j++)
                    {
                        bool canPlace = true;

                        subX = Game1.Player.UserInterface.TileSelector.IndexX + i;
                        subY = Game1.Player.UserInterface.TileSelector.IndexY + j;



                        //check if index is out of bounds of current chunk


                        int newGID = this.PlaceID + i + (j * 100);
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
        public void PlayerCollisionInteraction()
        {
            throw new NotImplementedException();
        }
    }



}



