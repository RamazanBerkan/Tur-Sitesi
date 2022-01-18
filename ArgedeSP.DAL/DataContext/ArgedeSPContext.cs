using Microsoft.EntityFrameworkCore;
using ArgedeSP.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ArgedeSP.DAL.DataContext
{
    public class ArgedeSPContext : IdentityDbContext<Kullanici, Rol, string, IdentityUserClaim<string>,
     KullaniciRol, IdentityUserLogin<string>,
     IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        //public string ConnectionString { get; set; }

        public ArgedeSPContext(DbContextOptions<ArgedeSPContext> options) : base(options)
        {
        }

        public DbSet<DbLog> DbLogs { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Referans> Referanslar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<OnHizmet> OnHizmetler { get; set; }
        public DbSet<BlogKategori> BlogKategorileri { get; set; }
        public DbSet<Blog> Bloglar { get; set; }
        public DbSet<Haber> Haberler { get; set; }
        public DbSet<SliderResim> SliderResimleri { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<UrunKategori> UrunKategorileri { get; set; }
        public DbSet<UrunResimGaleri> UrunResimGalerileri { get; set; }
        public DbSet<UrunDokumani> UrunDokumanlari { get; set; }
        public DbSet<AnahtarDeger> AnahtarDegerler { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KullaniciRol>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.KullaniciRolleri)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.KullaniciRolleri)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Referans>(entity => { entity.ToTable(name: "Referanslar"); });
            builder.Entity<Hizmet>(entity => { entity.ToTable(name: "Hizmetler"); });
            builder.Entity<OnHizmet>(entity => { entity.ToTable(name: "OnHizmetler"); });
            builder.Entity<BlogKategori>(entity => { entity.ToTable(name: "BlogKategorileri"); });
            builder.Entity<Blog>(entity => { entity.ToTable(name: "Bloglar"); });
            builder.Entity<Haber>(entity => { entity.ToTable(name: "Haberler"); });
            builder.Entity<SliderResim>(entity => { entity.ToTable(name: "SliderResimleri"); });
            builder.Entity<AnahtarDeger>(entity => { entity.ToTable(name: "AnahtarDeger"); });
            builder.Entity<UrunDokumani>(entity => { entity.ToTable(name: "UrunDokumanlari"); });


            builder.Entity<Rol>(entity => { entity.ToTable(name: "Roller"); });
            builder.Entity<KullaniciRol>(entity => { entity.ToTable("KullaniciRolleri"); });
            builder.Entity<Kullanici>(entity => { entity.ToTable(name: "Kullanicilar"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("KullaniciClaimleri"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("KullaniciGirisleri"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("KullaniciTokenlari"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RolClaimleri"); });
        }
    }
}