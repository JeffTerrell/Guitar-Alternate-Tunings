using System.Collections.Generic;

namespace GuitarTunings.Models
{

  public class Tuning
  {

    public Tuning()
    {
      this.JoinArtist = new HashSet<ArtistTuning>();
      this.JoinSong = new HashSet<SongTuning>();
    }

    public int TuningId { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Description { get; set; }

    // need properties for chord formation diagrams

    public virtual ICollection<ArtistTuning> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinSong { get; set; }
  }
}