﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonRoom
    {
        public bool IsStartingRoom { get; set; }
        public Dungeon Dungeon { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ITileManager TileManager { get; private set; }


        
        public DungeonRoom(Dungeon dungeon, int x, int y, ContentManager content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = new TileManager(Dungeon.TileSet, Dungeon.Map, Dungeon.Graphics, content, (int)Dungeon.TileSetNumber, Dungeon);

        }
        //for starting room
        public DungeonRoom(ITileManager tileManager, Dungeon dungeon, int x, int y, ContentManager content)
        {
            this.Dungeon = dungeon;
            this.X = x;
            this.Y = y;
            this.TileManager = tileManager;

        }

        public void Generate(string path)
        {

            for(int z = 0; z < this.TileManager.AllDepths.Count; z++)
            {
               // for(int i =0; i < )
            }
            Save(path);
        }

        public void Save(string path)
        {
            string newPath = path + this.X + this.Y;
            using (FileStream fileStream = File.OpenWrite(path))
            {


                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    this.TileManager.Save(binaryWriter);
                }
            }
        }

        public void Load(string path)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {


                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    this.TileManager.Load(binaryReader);
                   


                    

                    binaryReader.Close();

                }
            }
        }
    }
}
