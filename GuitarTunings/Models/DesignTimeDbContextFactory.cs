using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GuitarTunings.Models
{

  public class GuitarTuningsContextFactory : IDesignTimeDbContextFactory<GuitarTuningsContext>
  {

    GuitarTuningsContext IDesignTimeDbContextFactory<GuitarTuningsContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var builder = new DbContextOptionsBuilder<GuitarTuningsContext>();

      builder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

      return new GuitarTuningsContext(builder.Options);  
    }
  }
}