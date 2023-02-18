using System.IO;

namespace MaraudersModManager.FileSystem;

public interface IFileInfoEx
{
    void Write();
    void Read();
    void SetContent(string content);
}

public class FileInfoEx : IFileInfoEx
{
    private readonly IFileSystemService _fileSystemService;
    private FileInfo FileInfoObject { get; set; }
    private string FileContent { get; set; }
    
    public FileInfoEx(string fileName, IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService;
        FileInfoObject = new FileInfo(fileName);
        FileContent = File.ReadAllText(FileInfoObject.FullName);
    }
    
    public void Write() => _fileSystemService.Write(FileInfoObject, FileContent);
    public void Read() => FileContent = File.ReadAllText(FileInfoObject.FullName);
    public void SetContent(string content) => FileContent = content;
}