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
                //b.Ignore(x=> x.Ponto);
                b.Property<string>("Ponto").HasColumnType("sys.geography");
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