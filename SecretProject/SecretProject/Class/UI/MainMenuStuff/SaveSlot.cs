﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.UI;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public class SaveSlot
    {
        public GraphicsDevice Graphics { get; set; }
        public int ID { get; set; }
        public bool Occupied { get; set; }
        public string String { get; set; }
        public Button Button { get; set; }

        public string DirectoryPath { get; set; }
        public string SavePath { get; set; }
        public string ChunkPath { get; set; }
        public string UnChunkPath { get; set; }
        public string GameLocationPath { get; set; }

        public SaveSlot(Game1 game1,GraphicsDevice graphics, int id, Button button, bool occupied, string saveName = null)
        {
            this.Graphics = graphics;
            this.ID = id;

            this.Button = button; 
            if(occupied)
            {
                this.Occupied = true;
                this.String = saveName;
            }
            else
            {
                this.Occupied = false;
                this.String = "New Game";
            }



        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update();
            if(this.Button.isClicked)
            {
                Action negativeAction = new Action(Game1.mainMenu.ReturnToDefaultState);
                if (this.Occupied)
                {
                    Game1.mainMenu.IsDrawn = false;
                    Action action = new Action(LoadSave);
                    
                    Game1.mainMenu.AddAlert(AlertType.Confirmation,  Game1.Utility.centerScreen, "Load Game?", action, negativeAction);

                }
                else
                {
                    //  Game1.mainMenu.IsDrawn = false;
                    InitiateNewSave();
                    


                }
            }
        }



        public void InitiateNewSave()
        {
            Game1.mainMenu.ChooseGameMenu.MenuChoice = ChooseGameState.CreateNewCharacter;
            Game1.mainMenu.ChooseGameMenu.CharacterCreationMenu.CurrentSaveSlot = this;

            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, Game1.AllTextures.MenuText, this.String, Button.Position, Button.Color,Utility.StandardButtonDepth, Utility.StandardTextDepth, 2f);

        
            }

        public void LoadSave()
        {
            Game1.mainMenu.IsDrawn = true;
            Game1.SaveLoadManager.Load(this.Graphics,this);
            Game1.mainMenu.StartNewGame();
        }

        public void StartNewSave()
        {
            Game1.SaveLoadManager.CurrentSave = this;
            DirectoryPath = "Content/SaveFiles/GameSaves/Save_" + this.ID.ToString() + "_" + Game1.Player.Name;
            this.ChunkPath = DirectoryPath + "/Chunks";
            this.UnChunkPath = DirectoryPath + "/UnChunks";
            this.GameLocationPath = DirectoryPath + "/GameLocations/";
            System.IO.Directory.CreateDirectory(ChunkPath);
            System.IO.Directory.CreateDirectory(UnChunkPath);
            System.IO.Directory.CreateDirectory(GameLocationPath);

            this.SavePath = DirectoryPath + "/_PrimaryData";
            this.String = Game1.Player.Name + "\n Year " + Game1.GlobalClock.Calendar.CurrentYear + ", " + Game1.GlobalClock.Calendar.CurrentMonth.ToString() + " " + Game1.GlobalClock.Calendar.CurrentDay.ToString();
            AssignPaths();
            Game1.SaveLoadManager.Save(this);

            //  Game1.SaveLoadManager.SaveGameState(SaveType.MenuSave);

          
            
            Game1.mainMenu.StartNewGame();
        }

        public void AssignPaths()
        {
            foreach (Stage stage in Game1.StageManager.AllStages)
            {


                stage.AssignPath(this.DirectoryPath);
            }
        }

        public void SaveString(BinaryWriter writer)
        {
            writer.Write(this.String);
        }

        public void LoadString(BinaryReader reader)
        {
            this.String = reader.ReadString();
        }
    }
}
