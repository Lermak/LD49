using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public static class Globals
    {
        public static bool CreateChalk = false;
        public static bool ButtonNotCool = false;
        public static bool OverheatGameOver = false;
        public static bool PrepareForEndTimes = false;
        public static bool ReadyForEndTimes = false;
        public static bool DigiPetAlive = true;
        public static string[] ClickSounds = new string[] { "Click1", "Click2", "Click3" };
        public static void CreateFile(string name, string contents)
        {
            string fileName = @".\"+name+".txt";

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.Write(contents);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
