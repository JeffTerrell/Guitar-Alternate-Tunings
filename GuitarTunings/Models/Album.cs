using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Http;

namespace GuitarTunings.Models
{

  public class Album
  {
    
    public Album() 
    {
      this.JoinArtist = new HashSet<AlbumArtist>();
      this.JoinSong = new HashSet<AlbumSong>();
    }

    [Key]
    public int AlbumId { get; set; }
    [Required]
    public string Name { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
    [DisplayName("Release Date")]
    public DateTime ReleaseDate { get; set; }
    public string Description { get; set; }

    public virtual ICollection<AlbumArtist> JoinArtist { get; set; }
    public virtual ICollection<AlbumSong> JoinSong { get; set; }
  }
}