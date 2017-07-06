namespace Miae.Logger
{
    public interface ITxtLogger : ILogger
    {
        string DirectoryPath { get; set; }
        string FilePath { get; }
    }
}