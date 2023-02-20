namespace MaraudersModManager.Steam;

public interface ISteamService
{
    string GetGameRootPath();
    void Initialize();
}