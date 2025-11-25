using Microsoft.EntityFrameworkCore;
using System;

namespace LopCRUDApp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Lop> Lops { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DangKy> DangKys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary key for Lop
            modelBuilder.Entity<Lop>().HasKey(l => l.MaLop);

            // Configure primary key for SinhVien
            modelBuilder.Entity<SinhVien>().HasKey(sv => sv.MaSV);

            // Configure composite primary key for DangKy
            modelBuilder.Entity<DangKy>().HasKey(dk => new { dk.MaSV, dk.MaMon });

            // Configure properties for DangKy
            modelBuilder.Entity<DangKy>(entity =>
            {
                entity.Property(e => e.Diem1).HasColumnType("decimal(4, 2)");
                entity.Property(e => e.Diem2).HasColumnType("decimal(4, 2)");
                entity.Property(e => e.Diem3).HasColumnType("decimal(4, 2)");
            });

            // Configure properties for SinhVien
            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.Property(e => e.HocBong).HasColumnType("decimal(18, 2)");
            });

            // Configure properties for Lop
            modelBuilder.Entity<Lop>(entity =>
            {
                entity.Property(e => e.MaLop).HasMaxLength(10);
                entity.Property(e => e.TenLop).HasMaxLength(50);
                entity.Property(e => e.Khoa).HasMaxLength(10);
                entity.Property(e => e.Site).HasMaxLength(10);
            });

            // Configure properties for SinhVien
            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.Property(e => e.MaSV).HasMaxLength(10);
                entity.Property(e => e.HoTen).HasMaxLength(50);
                entity.Property(e => e.Phai).HasMaxLength(3); // "Nam" or "Nữ"
                entity.Property(e => e.MaLop).HasMaxLength(10);
                entity.Property(e => e.Site).HasMaxLength(10);
            });

            // Configure properties for DangKy
            modelBuilder.Entity<DangKy>(entity =>
            {
                entity.Property(e => e.MaSV).HasMaxLength(10);
                entity.Property(e => e.MaMon).HasMaxLength(10);
                entity.Property(e => e.Site).HasMaxLength(10);
            });
        }
    }
}
