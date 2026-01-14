using ChoreJamming.Domain.Models;

namespace ChoreJamming.Domain;

public interface IMusicProvider
{
    Task<Song?> GetSongAsync(string searchQuery, bool remix);
    
}