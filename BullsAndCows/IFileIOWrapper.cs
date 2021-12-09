using System.Collections.Generic;

namespace GameEngine
{
    public interface IFileIOWrapper
    {
        bool Exists(string dataFile);
        void Create(string dataFile);
        string[] ReadAllLines(string dataFile);
        void AppendAllText(string dataFile, string text);
    }
}