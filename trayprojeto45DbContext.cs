using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace trayprojeto45
{
    public partial class trayprojeto45DbContext : DbContext
    {

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public trayprojeto45DbContext()
        {
        }

        public trayprojeto45DbContext(DbContextOptions<trayprojeto45DbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseMySql("server=localhost;userid=root;password=1234;database=lasttray1", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.21-mariadb"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

