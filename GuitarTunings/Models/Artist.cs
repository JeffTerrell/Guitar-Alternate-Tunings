using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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

    [DisplayName("Upload Artist/Band Image")]
    public string ArtistImageName { get; set; }
    public IFormFile ArtistImageFile { get; set; }

    public virtual ICollection<ArtistSong> JoinSong { get; set; }
    public virtual ICollection<ArtistTuning> JoinTuning { get; set; }
  }
}