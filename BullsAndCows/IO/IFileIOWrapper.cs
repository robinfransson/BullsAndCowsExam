namespace BullsAndCows.IO
{
    public interface IFileIOWrapper
    {
        bool FileExists(string dataFile);
        void CreateFile(string dataFile);
        string[] ReadFile(string dataFile);
        void AppendToFile(string dataFile, string text);
    }
}