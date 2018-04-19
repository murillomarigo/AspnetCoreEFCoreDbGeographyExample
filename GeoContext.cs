using Microsoft.EntityFrameworkCore;

namespace geotest
{
    public class GeoContext : DbContext
    {
        public GeoContext(DbContextOptions<GeoContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Local>(b =>
            {
                b.Property<string>("Ponto").HasColumnType("sys.geography");
                //b.Property(x=> x.Ponto).HasColumnType("sys.geography");
            });
        }
        public DbSet<Local> Locais { get; set; }
    }

    public class Local
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        //public string Ponto { get; set; }
    }
}