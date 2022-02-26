namespace GuitarTunings.Models
{

  public class ArtistSong
  {

    public int ArtistSongId { get; set;}
    public int ArtistId { get; set; }
    public int SongId { get; set; }

    public virtual Artist Artist { get; set; }
    public virtual Song Song { get; set; }
  }
}