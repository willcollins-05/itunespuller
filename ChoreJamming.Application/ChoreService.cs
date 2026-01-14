using ChoreJamming.Domain;
using ChoreJamming.Domain.Models;

public class ChoreService
{
    private readonly IMusicProvider _music;
    private readonly IRepository<ChoreHistory> _repo;

    public ChoreService(IMusicProvider music, IRepository<ChoreHistory> repo)
    {
        _music = music;
        _repo = repo;
    }

    public async Task<Song?> ProcessChoreAsync(string choreName, bool remix)
    {
        var song = await _music.GetSongAsync(choreName, remix);
        if (song != null)
        {
            var history = new ChoreHistory 
            { 
                ChoreName = choreName, 
                SongTitle = song.Title,
                Date = DateTime.Now
            };
            await _repo.AddAsync(history);
            await _repo.SaveChangesAsync();
        }
        return song;
    }
    
    public async Task<List<ChoreHistory>> GetHistoryAsync()
    {
        return await _repo.GetAllAsync();
    }
}