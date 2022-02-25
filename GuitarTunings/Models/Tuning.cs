using System.Collections.Generic;

namespace GuitarTunings.Models
{

  public class Tuning
  {

    public Tuning()
    {
      this.joinArtist = new HashSet<ArtistTuning>();
      this.joinSong = new HashSet<SongTuning>();
    }

    public int TuningId { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Description { get; set; }

  }
}