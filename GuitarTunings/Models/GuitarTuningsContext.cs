using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GuitarTunings.Models
{

  public class GuitarTuningsContext : IdentityDbContext<ApplicationUser>
  {

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Tuning> Tunings { get; set; }
    public DbSet<TuningCategory> TuningCategories { get; set; }

    public DbSet<AlbumArtist> AlbumArtists { get; set; }
    public DbSet<AlbumSong> AlbumSongs {get; set; }
    public DbSet<ArtistSong> ArtistSongs { get; set; }
    public DbSet<ArtistTuning> ArtistTunings { get; set; } 

    public GuitarTuningsContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}