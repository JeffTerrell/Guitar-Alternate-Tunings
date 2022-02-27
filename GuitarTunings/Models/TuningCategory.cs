using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuitarTunings.Models
{

  public class TuningCategory
  {

    public TuningCategory()
    {
      this.Tunings = new HashSet<Tuning>();
    }
    [Key]
    public int TuningCategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Tuning> Tunings { get; set; }
  }
}