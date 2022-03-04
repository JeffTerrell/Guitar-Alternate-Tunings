using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    
    [DisplayName("Image Name For A Chord Diagram")]
    public string ImageNameA { get; set; }
    [DisplayName("Upload A Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileA { get; set; }

    [DisplayName("Image Name For B Chord Diagram")]
    public string ImageNameB { get; set; }
    [DisplayName("Upload B Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileB { get; set; }

    [DisplayName("Image Name For C Chord Diagram")]
    public string ImageNameC { get; set; }
    [DisplayName("Upload C Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileC { get; set; }

    [DisplayName("Image Name For D Chord Diagram")]
    public string ImageNameD { get; set; }
    [DisplayName("Upload D Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileD { get; set; }

    [DisplayName("Image Name For E Chord Diagram")]
    public string ImageNameE { get; set; }
    [DisplayName("Upload E Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileE { get; set; }

    [DisplayName("Image Name For F Chord Diagram")]
    public string ImageNameF { get; set; }
    [DisplayName("Upload F Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileF { get; set; }

    [DisplayName("Image Name For G Chord Diagram")]
    public string ImageNameG { get; set; }
    [DisplayName("Upload G Chord Diagram")]
    [NotMapped]
    public IFormFile ImageFileG { get; set; }    

    public int TuningCategoryId { get; set; }
    public virtual TuningCategory TuningCategory { get; set; }
    public virtual ICollection<ArtistTuning> JoinArtist { get; set; }
    public virtual ICollection<SongTuning> JoinSong { get; set; }
  }
}