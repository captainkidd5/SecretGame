﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public enum ClothingLayer
    {
        Hair = 0,
        Shirt = 1,
        Pants = 2,
        Shoes = 3,
        Base = 4
    }
    
    public class Wardrobe : ISaveable
    {
        public int HairIndex { get; set; }
        public int ShirtIndex { get; set; }
        public int PantsIndex { get; set; }
        public int ShoesIndex { get; set; }


        public Sprite[,] BasicMovementAnimations { get; set; }

        public Sprite[,] MiningAnimations { get; set; }
        public Sprite[,] SwipingAnimations { get; set; }
        public Sprite[,] GrabItemAnimations { get; set; }

        public Wardrobe(GraphicsDevice graphics, Vector2 playerPosition)
        {
            this.BasicMovementAnimations = new Sprite[4, 5];
            BasicMovementAnimations[(int)Dir.Down, (int)ClothingLayer.Hair] = new Sprite(graphics, Game1.AllTextures.PlayerHair, 0, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000011f };//, Color = Color.Black };
            BasicMovementAnimations[(int)Dir.Down, (int)ClothingLayer.Shirt] = new Sprite(graphics, Game1.AllTextures.PlayerShirt, 0, 32, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000010f };
            BasicMovementAnimations[(int)Dir.Down, (int)ClothingLayer.Pants] = new Sprite(graphics, Game1.AllTextures.PlayerPants, 0, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000009f };
            BasicMovementAnimations[(int)Dir.Down, (int)ClothingLayer.Shoes] = new Sprite(graphics, Game1.AllTextures.PlayerShoes, 0, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000008f };
            BasicMovementAnimations[(int)Dir.Down, (int)ClothingLayer.Base] = new Sprite(graphics, Game1.AllTextures.PlayerBase, 0, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .000000007f };


            //up
            BasicMovementAnimations[(int)Dir.Up, (int)ClothingLayer.Hair] = new Sprite(graphics, Game1.AllTextures.PlayerHair, 192, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000011f };
            BasicMovementAnimations[(int)Dir.Up, (int)ClothingLayer.Shirt] = new Sprite(graphics, Game1.AllTextures.PlayerShirt, 192, 32, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000010f };
            BasicMovementAnimations[(int)Dir.Up, (int)ClothingLayer.Pants] = new Sprite(graphics, Game1.AllTextures.PlayerPants, 192, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000009f };
            BasicMovementAnimations[(int)Dir.Up, (int)ClothingLayer.Shoes] = new Sprite(graphics, Game1.AllTextures.PlayerShoes, 192, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000008f };
            BasicMovementAnimations[(int)Dir.Up, (int)ClothingLayer.Base] = new Sprite(graphics, Game1.AllTextures.PlayerBase, 192, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .000000007f };

            //Left
            BasicMovementAnimations[(int)Dir.Left, (int)ClothingLayer.Hair] = new Sprite(graphics, Game1.AllTextures.PlayerHair, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000011f, Flip = true };
            BasicMovementAnimations[(int)Dir.Left, (int)ClothingLayer.Shirt] = new Sprite(graphics, Game1.AllTextures.PlayerShirt, 96, 32, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000010f, Flip = true };
            BasicMovementAnimations[(int)Dir.Left, (int)ClothingLayer.Pants] = new Sprite(graphics, Game1.AllTextures.PlayerPants, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000009f, Flip = true };
            BasicMovementAnimations[(int)Dir.Left, (int)ClothingLayer.Shoes] = new Sprite(graphics, Game1.AllTextures.PlayerShoes, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000008f, Flip = true };
            BasicMovementAnimations[(int)Dir.Left, (int)ClothingLayer.Base] = new Sprite(graphics, Game1.AllTextures.PlayerBase, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .000000007f, Flip = true };

            //Right
            BasicMovementAnimations[(int)Dir.Right, (int)ClothingLayer.Hair] = new Sprite(graphics, Game1.AllTextures.PlayerHair, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000011f };
            BasicMovementAnimations[(int)Dir.Right, (int)ClothingLayer.Shirt] = new Sprite(graphics, Game1.AllTextures.PlayerShirt, 96, 32, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000010f };
            BasicMovementAnimations[(int)Dir.Right, (int)ClothingLayer.Pants] = new Sprite(graphics, Game1.AllTextures.PlayerPants, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000009f };
            BasicMovementAnimations[(int)Dir.Right, (int)ClothingLayer.Shoes] = new Sprite(graphics, Game1.AllTextures.PlayerShoes, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .00000008f };
            BasicMovementAnimations[(int)Dir.Right, (int)ClothingLayer.Base] = new Sprite(graphics, Game1.AllTextures.PlayerBase, 96, 0, 16, 32, 6, .1f, playerPosition) { LayerDepth = .000000007f };

        }

        public void UpdateMovementAnimations(Vector2 position, bool setFrameZero = false)
        {
            for (int i = 0; i < BasicMovementAnimations.GetLength(0); i++)
            {
                for (int j = 0; j < BasicMovementAnimations.GetLength(1); j++)
                {
                    if(setFrameZero)
                    {
                        BasicMovementAnimations[i, j].SetFrame(0);
                    }
                   
                    BasicMovementAnimations[i, j].UpdateAnimationPosition(position);
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Load(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
