using System.IO;
using Microsoft.Extensions.Configuration;

namespace MaraudersModManager.FileSystem;

public interface IFileSystemService
{
    void Write(FileInfo file, string content);
    string GetRootedFilePath(string path);
}

public class FileSystemService : IFileSystemService
{
    private readonly IConfiguration _configuration;
    private FileInfo Root { get; set; } = new FileInfo(Directory.GetCurrentDirectory());
    
    public FileSystemService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Write(FileInfo file, string content) => File.WriteAllText(file.FullName, content);
    public string GetRootedFilePath(string path) => Path.Combine(Root.FullName, path);
}