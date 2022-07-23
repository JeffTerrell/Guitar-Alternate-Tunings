namespace GuitarTunings.Models
{

  public class AlbumArtist
  {

    public int AlbumArtistId { get; set;}
    public int AlbumId { get; set; }
    public int ArtistId { get; set; }

    public virtual Album Album { get; set; }
    public virtual Artist Artist { get; set; }
  }
}