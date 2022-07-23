using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.Models
{

  public class Song
  {

    public Song()
    {
      this.JoinAlbum = new HashSet<AlbumSong>();
      this.JoinArtist = new HashSet<ArtistSong>();
    }

    [Key]
    public int SongId { get; set; }
    [Required]
    public string Name { get; set; }
    [DisplayName("Guitar Tab")]
    public string Tab { get; set; }
    [DisplayName("Song Video")]
    public string Video { get; set; }
    [DisplayName("Guitar Tutorial")]
    public string Tutorial { get; set; }

    public int TuningId { get; set; }
    public virtual Tuning Tuning { get; set; }
    public virtual ICollection<AlbumSong> JoinAlbum { get; set; }
    public virtual ICollection<ArtistSong> JoinArtist { get; set; }
  }
}
