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
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Description { get; set; }
    // need properties for chord formation diagrams
    public int TuningCategoryId { get; set; }

    public virtual TuningCategory TuningCategory { get; set; }
    public virtual ICollection<ArtistTuning> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinSong { get; set; }
  }
}