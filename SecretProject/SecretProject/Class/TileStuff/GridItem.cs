using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
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

        int SourceRectangleOffSetX { get; set; }
        int SourceRectangleOffSetY { get; set; }

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

        public void LoadNewItem(ITileManager tileManager)
        {
            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(PlaceID))
            {
                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties.ContainsKey("newSource"))
                {
                    this.RectangleCoordinates = TileUtility.GetNewTileSourceRectangle(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[PlaceID].Properties["newSource"]);
                    this.NegativeX = RectangleCoordinates[0] / 16;
                    this.NegativeY = RectangleCoordinates[1] / 16;
                    this.PositiveX = RectangleCoordinates[2] / 16;
                    this.PositiveY = RectangleCoordinates[3] / 16;

                    SourceRectangle = new Rectangle(SourceRectangle.X + RectangleCoordinates[0], SourceRectangle.Y + RectangleCoordinates[1],
                                           SourceRectangle.Width + RectangleCoordinates[2], SourceRectangle.Height + RectangleCoordinates[3]);
                    SourceRectangleOffSetX = RectangleCoordinates[0];
                    SourceRectangleOffSetY = RectangleCoordinates[1];
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

                    bool canPlaceTotal = true;

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
                                int gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY].GID;

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

                    if(canPlaceTotal)
                    {
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
                                container.Objects.Add(new Collider(this.GraphicsDevice, Vector2.Zero,
                                    new Rectangle(container.AllTiles[0][Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY].DestinationRectangle.X + RectangleCoordinates[0],
                                    container.AllTiles[0][Game1.Player.UserInterface.TileSelector.IndexX, Game1.Player.UserInterface.TileSelector.IndexY].DestinationRectangle.Y + RectangleCoordinates[1],
                                    RectangleCoordinates[2], RectangleCoordinates[3]), this, ColliderType.inert));
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
                            int gid = tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[z][subX, subY].GID;

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
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.X,
                                tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.Y),
                                newSourceRectangle, Color.White * .5f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[3]);
                        }
                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.X,
                                tileManager.ActiveChunks[activeChunkX, activeChunkY].AllTiles[3][subX, subY].DestinationRectangle.Y),
                                newSourceRectangle, Color.Red * .5f,
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



