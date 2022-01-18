using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgedeSP.DAL.Migrations
{
    public partial class initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnahtarDeger",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    Anahtar = table.Column<string>(nullable: true),
                    Deger = table.Column<string>(nullable: true),
                    GorunenAd = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnahtarDeger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    BlogKategoriAdi = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    ArkaPlanResim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    SeoUrl = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false),
                    AnaDilcesi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogKategorileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(nullable: true),
                    MessageTemplate = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Exception = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    LogEvent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Haberler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    Baslik = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Haberler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    HizmetAdi = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    ArkaPlanResim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    Siralama = table.Column<int>(nullable: false),
                    SeoUrl = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ProfilResmi = table.Column<string>(nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    Durum = table.Column<int>(nullable: false),
                    Ad = table.Column<string>(nullable: true),
                    Soyad = table.Column<string>(nullable: true),
                    TCKN = table.Column<string>(nullable: true),
                    EngellenmeTarihi = table.Column<DateTime>(nullable: false),
                    EngellenmeNedeni = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnHizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    OnHizmetAdi = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    ArkaPlanResim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    SeoUrl = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnHizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Referanslar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    ReferansAdi = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    ArkaPlanResmi = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    SeoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referanslar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roller",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SliderResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    ResimUrl = table.Column<string>(nullable: true),
                    ResimSira = table.Column<int>(nullable: false),
                    AltBaslik = table.Column<string>(nullable: true),
                    UstBaslik = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false),
                    SliderYeri = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderResimleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrunKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    Ad = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    NavbarResim = table.Column<string>(nullable: true),
                    ArkaPlanResim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    SeoUrl = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false),
                    AnaDilcesi = table.Column<string>(nullable: true),
                    UstId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunKategorileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunKategorileri_UrunKategorileri_UstId",
                        column: x => x.UstId,
                        principalTable: "UrunKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bloglar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    Baslik = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    ArkaPlanResim = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    Ses = table.Column<string>(nullable: true),
                    BlogEtiket = table.Column<string>(nullable: true),
                    SeoUrl = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false),
                    BlogKategoriId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloglar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bloglar_BlogKategorileri_BlogKategoriId",
                        column: x => x.BlogKategoriId,
                        principalTable: "BlogKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciClaimleri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciClaimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciClaimleri_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciGirisleri",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciGirisleri", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_KullaniciGirisleri_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciTokenlari",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciTokenlari", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_KullaniciTokenlari_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciRolleri",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciRolleri", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_KullaniciRolleri_Roller_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciRolleri_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolClaimleri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolClaimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolClaimleri_Roller_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    UrunAdi = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true),
                    UrunKodu = table.Column<string>(nullable: true),
                    Resim = table.Column<string>(nullable: true),
                    UrunSira = table.Column<int>(nullable: false),
                    DetayAciklama = table.Column<string>(nullable: true),
                    KisaAciklama = table.Column<string>(nullable: true),
                    UzunAciklama = table.Column<string>(nullable: true),
                    TeknikOzellikler = table.Column<string>(nullable: true),
                    NasilCalisir = table.Column<string>(nullable: true),
                    Durum = table.Column<int>(nullable: false),
                    SeoUrl = table.Column<string>(nullable: true),
                    Fiyat = table.Column<string>(nullable: true),
                    IndirimliFiyat = table.Column<string>(nullable: true),
                    Indirim = table.Column<string>(nullable: true),
                    Dil = table.Column<int>(nullable: false),
                    UrunKategoriId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Urunler_UrunKategorileri_UrunKategoriId",
                        column: x => x.UrunKategoriId,
                        principalTable: "UrunKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunDokumanlari",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    DokumanAdi = table.Column<string>(nullable: true),
                    DokumanUrl = table.Column<string>(nullable: true),
                    UrunId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunDokumanlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunDokumanlari_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrunResimGalerileri",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OlusturmaTarihi = table.Column<DateTime>(nullable: false),
                    ResimUrl = table.Column<string>(nullable: true),
                    UrunId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunResimGalerileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunResimGalerileri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bloglar_BlogKategoriId",
                table: "Bloglar",
                column: "BlogKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciClaimleri_UserId",
                table: "KullaniciClaimleri",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciGirisleri_UserId",
                table: "KullaniciGirisleri",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Kullanicilar",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Kullanicilar",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciRolleri_RoleId",
                table: "KullaniciRolleri",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolClaimleri_RoleId",
                table: "RolClaimleri",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roller",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UrunDokumanlari_UrunId",
                table: "UrunDokumanlari",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunKategorileri_UstId",
                table: "UrunKategorileri",
                column: "UstId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_UrunKategoriId",
                table: "Urunler",
                column: "UrunKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunResimGalerileri_UrunId",
                table: "UrunResimGalerileri",
                column: "UrunId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnahtarDeger");

            migrationBuilder.DropTable(
                name: "Bloglar");

            migrationBuilder.DropTable(
                name: "DbLogs");

            migrationBuilder.DropTable(
                name: "Haberler");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropTable(
                name: "KullaniciClaimleri");

            migrationBuilder.DropTable(
                name: "KullaniciGirisleri");

            migrationBuilder.DropTable(
                name: "KullaniciRolleri");

            migrationBuilder.DropTable(
                name: "KullaniciTokenlari");

            migrationBuilder.DropTable(
                name: "OnHizmetler");

            migrationBuilder.DropTable(
                name: "Referanslar");

            migrationBuilder.DropTable(
                name: "RolClaimleri");

            migrationBuilder.DropTable(
                name: "SliderResimleri");

            migrationBuilder.DropTable(
                name: "UrunDokumanlari");

            migrationBuilder.DropTable(
                name: "UrunResimGalerileri");

            migrationBuilder.DropTable(
                name: "BlogKategorileri");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Roller");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropTable(
                name: "UrunKategorileri");
        }
    }
}
