﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.StageFolder.DungeonStuff;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class CommandConsole : IExclusiveInterfaceComponent
    {

        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string EnteredString { get; set; } = string.Empty;
        public string DisplayLog { get; set; } = string.Empty;
        public Vector2 DisplayLogPosition { get; set; } = Game1.Utility.Origin;


        public List<CommandWindowCommand> AllCommands { get; set; }

        private Rectangle backGroundRectangle;
        private Texture2D coloredRectangleTexture;
        private Vector2 backGroundPosition;
        public CommandConsole(GraphicsDevice graphics)
        {

            this.AllCommands = new List<CommandWindowCommand>()
            {
                new CommandWindowCommand("spawn", "spawn (int)[itemID], (int)[count]"),
                new CommandWindowCommand("alert", "alert (int)[size], (string)[text]"),
                new CommandWindowCommand("showitems", "shows all items and their ids"),
                new CommandWindowCommand("warp", "warp (enum)[stagename]"),
                new CommandWindowCommand("teleport", "teleport (int)[X Position], (int)[Y Position]"),
                new CommandWindowCommand("clear", "clears console window"),
                new CommandWindowCommand("settime", "settime (int)[time between 0 and 24]"),
                new CommandWindowCommand("decreaseEnergy", "decreaseEnergy (int)[amount to decrease]"),
                new CommandWindowCommand("add", "add (string)[mobname], (int)[count]"),
                new CommandWindowCommand("swaproom", "swaproom (string)[roomX], (string)[roomY]"),
            };
            this.coloredRectangleTexture = Game1.Utility.GetColoredRectangle(graphics, 600, 400, new Color(211, 211, 211, 2));
            this.backGroundRectangle = Game1.Utility.GetRectangleFromTexture(coloredRectangleTexture);
            this.backGroundPosition = new Vector2(0, Game1.ScreenHeight - backGroundRectangle.Height);
        }
        

        public void Update(GameTime gameTime)
        {
            Game1.freeze = true;
            Keys[] pressedKeys = Game1.KeyboardManager.OldKeyBoardState.GetPressedKeys();
            if(Game1.MouseManager.HasScrollWheelValueIncreased)
            {
                this.DisplayLogPosition = new Vector2(DisplayLogPosition.X, DisplayLogPosition.Y - 20);
            }
            else if (Game1.MouseManager.HasScrollWheelValueDecreased)
            {
                this.DisplayLogPosition = new Vector2(DisplayLogPosition.X, DisplayLogPosition.Y + 20);
            }
            foreach (Keys key in pressedKeys)
            {
                if (Game1.KeyboardManager.WasKeyPressed(key))
                {
                    string keyValue = string.Empty;
                    if (key == Keys.Space)
                    {
                        keyValue = " ";
                        this.EnteredString += keyValue;
                    }
                    else if(key == Keys.Back)
                    {
                        if(this.EnteredString.Length > 0)
                        {
                          this.EnteredString = this.EnteredString.TrimEnd(EnteredString[EnteredString.Length - 1]);
                        }
                    }
                    else if(key == Keys.Enter)
                    {
                        ProcessString();
                        this.EnteredString = String.Empty;
                        return;
                    }
                    else if((int)key > 64 && (int)key <  91)
                    {
                        keyValue = key.ToString();
                        this.EnteredString += keyValue;
                    }
                    else if ((int)key > 47 && (int)key < 58)
                    {
                        keyValue = key.ToString();
                        keyValue = keyValue.TrimStart('D');
                        
                        this.EnteredString += keyValue;
                    }
                    else if (key == Keys.Subtract)
                    {
                        keyValue = "-";
                        this.EnteredString += keyValue;
                    }


                }
            }

        }

        public void ProcessString()
        {
            string[] separatedString = this.EnteredString.Split(' ');

            switch (separatedString[0].ToLower())
            {
                case "spawn":
                    int numberToSpawn = int.Parse(separatedString[2]);
                    int itemID = int.Parse(separatedString[1]);
                    for (int i =0; i < numberToSpawn; i++)
                    {
                        Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(itemID, null));
                    }
                    
                    break;
                case "alert":
                    Game1.Player.UserInterface.AddAlert(AlertType.Normal,  Game1.Utility.centerScreen, separatedString[2]);
                    break;
                case "showitems":
                    for (int i = 0; i < Game1.AllItems.AllItems.Count; i++)
                        this.DisplayLog += Game1.AllItems.AllItems[i].Name + " : " + Game1.AllItems.AllItems[i].ID.ToString() + "\n";
                    break;
                case "help":
                    foreach(CommandWindowCommand command in AllCommands)
                    {
                        this.DisplayLog += command.Name + ": " +command.Description + "\n";
                    }

                    break;
                case "warp":
                    string newString = separatedString[1].ToLower();

                   newString = char.ToUpper(newString[0]) + newString.Substring(1);
                    Stages newStage = (Stages)Enum.Parse(typeof(Stages), newString, true);
                    Game1.SwitchStage(Game1.GetCurrentStageInt(), newStage);
                    break;
                case "clear":
                    this.DisplayLog = string.Empty;
                    break;
                case "teleport":
                    int teleportX = int.Parse(separatedString[1].ToLower());
                    int teleportY = int.Parse(separatedString[2].ToLower());

                    Game1.Player.position = new Vector2(teleportX, teleportY);
                    //if(Game1.GetCurrentStage() == Game1.OverWorld)
                    //{
                    //    Game1.OverWorld.AllTiles.LoadInitialChunks(Game1.Player.position);
                    //}
                    break;

                case "settime":
                    int time = int.Parse(separatedString[1].ToLower());

                    Game1.GlobalClock.TotalHours = time;
                    Game1.GlobalClock.AdjustClockText();
                    break;
                case "decreasee":
                    Game1.Player.UserInterface.StaminaBar.DecreaseStamina(int.Parse(separatedString[1].ToLower()));
                    break;
                case "add":

                    string mobName = separatedString[1].ToLower();
                    int mobCount = int.Parse(separatedString[2].ToLower());

                    //Chunk mouseChunk = Game1.OverWorld.AllTiles.GetChunkFromPosition(Game1.MouseManager.WorldMousePosition);
                    //List<Enemy> enemies = mouseChunk.NPCGenerator.SpawnTargetNPCPack((NPCType)Enum.Parse(typeof(NPCType), mobName, true),
                    //    mouseChunk, mobCount, Game1.MouseManager.WorldMousePosition);
                    //Game1.OverWorld.Enemies.AddRange(enemies);
                    
                    break;
                case "swaproom":
                    int roomX = int.Parse(separatedString[1].ToLower());
                    int roomY = int.Parse(separatedString[2].ToLower());
                    (Game1.ForestDungeon as Dungeon).SwitchRooms(roomX, roomY, Dir.Down);
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.coloredRectangleTexture, this.backGroundPosition, this.backGroundRectangle, Color.White, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.EnteredString, this.backGroundPosition, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.DisplayLog, Game1.Utility.Origin, Color.White, 0f, this.DisplayLogPosition, 2f, SpriteEffects.None, 1f);
        }
    }

    public class CommandWindowCommand
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CommandWindowCommand(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
