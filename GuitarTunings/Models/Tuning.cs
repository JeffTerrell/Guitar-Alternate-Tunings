using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.Models
{

  public class Tuning
  {

    public Tuning()
    {
      this.JoinArtist = new HashSet<ArtistTuning>();
      this.JoinSong = new HashSet<SongTuning>();
    }

    [Key]
    public int TuningId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Notes { get; set; }
    public string Description { get; set; }
    // need properties for chord formation diagrams
    public int TuningCategoryId { get; set; }

    public virtual TuningCategory TuningCategory { get; set; }
    public virtual ICollection<ArtistTuning> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinSong { get; set; }
  }
}