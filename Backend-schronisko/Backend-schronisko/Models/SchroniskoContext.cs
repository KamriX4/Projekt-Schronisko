using Microsoft.EntityFrameworkCore;

namespace Backend_schronisko.Models
{
    public class SchroniskoContext : DbContext
    {
        public SchroniskoContext(DbContextOptions<SchroniskoContext> options) : base(options)
        {

        }
        // Te właściwości reprezentują tabele w bazie MSSQL
        public DbSet<Zwierze> Zwierzeta { get; set; }
        public DbSet<Gatunek> Gatunki { get; set; }

        // Główne miejsce do wpisywania danych na sztywno
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Zasilamy słownik gatunków
            modelBuilder.Entity<Gatunek>().HasData(
                new Gatunek { Id = 1, Nazwa = "Pies" },
                new Gatunek { Id = 2, Nazwa = "Kot" }
            );
        }
    }
}
