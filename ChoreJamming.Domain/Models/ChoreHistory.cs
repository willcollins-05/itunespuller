namespace ChoreJamming.Domain.Models;

public class ChoreHistory
{
    public int Id { get; set; }
    public string ChoreName { get; set; } = string.Empty;
    public string SongTitle { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    
    public int Rating { get; set; } = 1;
}