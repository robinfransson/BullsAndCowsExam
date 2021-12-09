using GameEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows
{
    public class FileIOWrapper : IFileIOWrapper
    {
        public void AppendAllText(string dataFile, string text)
        {
            File.AppendAllText(dataFile, text);
        }

        public void Create(string dataFile)
        {
            File.Create(dataFile)
                .Close();
        }

        public bool Exists(string dataFile)
        {
            return File.Exists(dataFile);
        }

        public string[] ReadAllLines(string dataFile)
        {
            return File.ReadAllLines(dataFile);
        }
    }
}
