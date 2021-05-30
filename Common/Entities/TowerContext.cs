using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EntitySql.Entity
{
    public partial class TowerContext : DbContext
    {
        public TowerContext()
        {
        }

        public TowerContext(DbContextOptions<TowerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tower> Towers { get; set; }
        public virtual DbSet<TowerValue> TowerValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
              //  optionsBuilder.UseSqlServer("Server=DESKTOP-O51730H\\SQLEXPRESS; user=sa; password=smik; Initial Catalog=PowerTowers");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TowerValue>(entity =>
            {
                entity.HasOne(d => d.Tower)
                    .WithMany(p => p.TowerValues)
                    .HasForeignKey(d => d.TowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Values_Towers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
