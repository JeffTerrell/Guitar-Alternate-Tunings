using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.Models
{

  public class Song
  {

    public Song()
    {
      this.JoinArtist = new HashSet<ArtistSong>();
      this.JoinTuning = new HashSet<SongTuning>();
    }

    [Key]
    public int SongId { get; set; }
    [Required]
    public string Name { get; set; }
    public string Album { get; set; }
    [DisplayName("Guitar Tab")]
    public string Tab { get; set; }
    [DisplayName("Song Video")]
    public string Video { get; set; }
    [DisplayName("Guitar Tutorial")]
    public string Tutorial { get; set; }

    public virtual ICollection<ArtistSong> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinTuning { get; set; }
  }
}
