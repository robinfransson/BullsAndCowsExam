using GameEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace BullsAndCows.IO
{
    public class FileIOWrapper : IFileIOWrapper
    {
        public void AppendToFile(string dataFile, string text)
        {
            File.AppendAllText(dataFile, text);
        }

        public void CreateFile(string dataFile)
        {
            File.Create(dataFile)
                .Close();
        }

        public bool FileExists(string dataFile)
        {
            return File.Exists(dataFile);
        }

        public string[] ReadFile(string dataFile)
        {
            return File.ReadAllLines(dataFile);
        }
    }
}
