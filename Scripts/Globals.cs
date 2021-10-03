using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public static class Globals
    {
        public static bool DigiPetAlive = true;

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
