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

    }
}
