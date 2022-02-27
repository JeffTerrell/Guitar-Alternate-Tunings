using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.Models
{

  public class Artist
  {
    
    public Artist() 
    {
      this.JoinSong = new HashSet<ArtistSong>();
      this.JoinTuning = new HashSet<ArtistTuning>();
    }

    [Key]
    public int ArtistId { get; set; }
    [Required]
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    
    // add image property for logo?

    public virtual ICollection<ArtistSong> JoinSong { get; set; }
    public virtual ICollection<ArtistTuning> JoinTuning { get; set; }
  }
}