namespace GuitarTunings.Models
{

  public class ArtistSong
  {

    public int ArtistTuningId { get; set;}
    public int ArtistId { get; set; }
    public int TuningId { get; set; }

    public virtual Artist Artist { get; set; }
    public virtual Tuning Tuning { get; set; }
  }
}