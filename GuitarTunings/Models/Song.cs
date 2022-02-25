using System.Collections.Generic;

namespace GuitarTunings.Models
{

  public class Song
  {

    public Song()
    {
      this.JoinArtist = new HashSet<ArtistSong>();
      this.JoinTuning = new HashSet<SongTuning>();
    }

    public int SongId { get; set; }
    public string Name { get; set; }
    public string Album { get; set; }
    public string Tab { get; set; }
    public string Video { get; set; }
    public string Tutorial { get; set; }

    public virtual ICollection<ArtistSong> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinTuning { get; set; }
  }
}
