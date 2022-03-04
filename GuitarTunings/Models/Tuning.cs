using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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
    
    public string ImageNameA { get; set; }
    [DisplayName("Upload A Chord Diagram")]
    public IFormFile ImageFileA { get; set; }

    public string ImageNameB { get; set; }
    [DisplayName("Upload B Chord Diagram")]
    public IFormFile ImageFileB { get; set; }

    public string ImageNameC { get; set; }
    [DisplayName("Upload C Chord Diagram")]
    public IFormFile ImageFileC { get; set; }

    public string ImageNameD { get; set; }
    [DisplayName("Upload D Chord Diagram")]
    public IFormFile ImageFileD { get; set; }

    public string ImageNameE { get; set; }
    [DisplayName("Upload E Chord Diagram")]
    public IFormFile ImageFileE { get; set; }

    public string ImageNameF { get; set; }
    [DisplayName("Upload F Chord Diagram")]
    public IFormFile ImageFileF { get; set; }

    public string ImageNameG { get; set; }
    [DisplayName("Upload G Chord Diagram")]
    public IFormFile ImageFileG { get; set; }    

    public int TuningCategoryId { get; set; }
    public virtual TuningCategory TuningCategory { get; set; }
    public virtual ICollection<ArtistTuning> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinSong { get; set; }
  }
}