namespace GuitarTunings.Models
{

  public class AlbumSong
  {

    public int AlbumSongId { get; set;}
    public int AlbumId { get; set; }
    public int SongId { get; set; }

    public virtual Album Album { get; set; }
    public virtual Song Song { get; set; }
  }
}