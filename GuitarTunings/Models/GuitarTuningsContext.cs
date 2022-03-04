using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Models
{

  public class GuitarTuningsContext : IdentityDbContext<ApplicationUser>
  {

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Tuning> Tunings { get; set; }
    public DbSet<TuningCategory> TuningCategories { get; set; }

    public DbSet<ArtistSong> ArtistSongs { get; set; }
    public DbSet<ArtistTuning> ArtistTunings { get; set; } 
    public DbSet<SongTuning> SongTunings { get; set; }

  }
}