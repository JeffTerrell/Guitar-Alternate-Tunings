namespace GuitarTunings.Models
{

  public class SongTuning
  {

    public int SongTuningId { get; set;}
    public int SongId { get; set; }
    public int TuningId { get; set; }

    public virtual Song Song { get; set; }
    public virtual Tuning Tuning { get; set; }
  }
}