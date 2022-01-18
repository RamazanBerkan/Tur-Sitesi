using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ArgedeSP.BLL.BusinessServices;
using ArgedeSP.Common;
using ArgedeSP.Contracts.Configurations;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Mail;
using ArgedeSP.Contracts.Interfaces;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.DAL;
using ArgedeSP.DAL.DataContext;
using ArgedeSP.DAL.Repositories;
using ArgedeSP.WebUI.Helpers;
using ArgedeSP.WebUI.Helpers.RouteConstraint;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace ArgedeSP.WebUI
{
    public class Startup
    {
        public static string wwwRootFolder = string.Empty;

        readonly IHostingEnvironment HostingEnvironment;
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Also make top level configuration available (for EF configuration and access to connection string)
            services.AddSingleton(Configuration); //IConfigurationRoot
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSession();
            //Add Support for strongly typed Configuration and map to class
            services.AddOptions();
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            //Set database.
            if (Configuration["AppConfig:UseInMemoryDatabase"] == "false")
            {
                services.AddDbContext<ArgedeSPContext>(opt => opt.UseInMemoryDatabase("DefaultConnection"));
            }
            else
            {
                services.AddDbContext<ArgedeSPContext>(c =>
                    c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            }


            services.AddIdentity<Kullanici, Rol>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<ArgedeSPContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/tr-TR/Hata/YetkisizGiris";
                options.Cookie.Name = "argedesp.webui";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/giris";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            //Cors policy is added to controllers via [EnableCors("CorsPolicy")]
            //or .UseCors("CorsPolicy") globally
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //Instance injection
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IHizmetBS, HizmetBS>();
            services.AddScoped<IHizmetRepository, HizmetRepository>();

            services.AddScoped<IOnHizmetBS, OnHizmetBS>();
            services.AddScoped<IOnHizmetRepository, OnHizmetRepository>();

            services.AddScoped<IBlogKategoriBS, BlogKategoriBS>();
            services.AddScoped<IBlogKategoriRepository, BlogKategoriRepository>();

            services.AddScoped<IBlogBS, BlogBS>();
            services.AddScoped<IBlogRepository, BlogRepository>();

            services.AddScoped<ISliderResimBS, SliderResimBS>();
            services.AddScoped<ISliderResimRepository, SliderResimRepository>();

            services.AddScoped<IAnahtarDegerBS, AnahtarDegerBS>();
            services.AddScoped<IAnahtarDegerRepository, AnahtarDegerRepository>();

            services.AddScoped<IHaberBS, HaberBS>();
            services.AddScoped<IHaberRepository, HaberRepository>();

            services.AddScoped<IKullaniciBS, KullaniciBS>();
            services.AddScoped<IKullaniciRepository, KullaniciRepository>();

            services.AddScoped<IReferansBS, ReferansBS>();
            services.AddScoped<IReferansRepository, ReferansRepository>();

            services.AddScoped<IUrunKategoriBS, UrunKategoriBS>();
            services.AddScoped<IUrunKategoriRepository, UrunKategoriRepository>();

            services.AddScoped<IUrunBS, UrunBS>();
            services.AddScoped<IUrunRepository, UrunRepository>();

            ////Per request injections
            services.AddTransient<ArgedeSPDataInitializer>();
            services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IEmailSender2, EmailSender>(i =>
                   new EmailSender(
                       Configuration["EmailSender:Host"],
                       Configuration.GetValue<int>("EmailSender:Port"),
                       Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                       Configuration["EmailSender:UserName"],
                       Configuration["EmailSender:Password"]
                   )
           );

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            services.AddSingleton<IFileProvider>(physicalProvider);

            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

            services.AddLocalization(options =>
            {
                // Resource (kaynak) dosyalarımızı ana dizin altında "Resources" klasorü içerisinde tutacağımızı belirtiyoruz.
                options.ResourcesPath = "Resources";
            });

            //Add framework services
            services.AddMvc(options =>
            {
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResources));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;
                }
            });

        }



        // TEST İÇİN YAZILMIŞTIR




        //
        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IFileProvider fileProvider,
            ArgedeSPDataInitializer argedeSPDataInitializer)
        {
            // Serilog config
            Serilog.Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFile(pathFormat: "logs\\log-{Date}.log")
                    .CreateLogger();

            if (env.IsDevelopment())
            {
                loggerFactory
                    .AddDebug()
                    .AddConsole()
                    .AddSerilog();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory
                    .AddSerilog();
                app.UseExceptionHandler(errorApp =>

                    //Application level exception handler here - this is just a place holder
                    errorApp.Run(async (context) =>
                    {

                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync("<html><body>\r\n");
                        await context.Response.WriteAsync(
                                "We're sorry, we encountered an un-expected issue with your application.<br>\r\n");

                        //Capture the exception
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //This error would not normally be exposed to the client
                            await context.Response.WriteAsync("<br>Error: " +
                                    HtmlEncoder.Default.Encode(error.Error.Message) + "<br>\r\n");
                        }
                        await context.Response.WriteAsync("<br><a href=\"/\">Home</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
                    }));
            }
            //Console.WriteLine("\r\nPlatform: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            //Console.WriteLine("DB Connection: "  + Configuration.GetConnectionString("StoreDbConnection"));

            app.UseAuthentication();
            app.UseDatabaseErrorPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();


            wwwRootFolder = env.WebRootPath;


            //Apply CORS.
            app.UseCors("CorsPolicy");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("X-Frame-Options");
                context.Response.Headers.Add("X-Frame-Options", "AllowAll");

                await next();

                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/tr-TR/hata";
                    await next();
                }
            });


            var supportedCultures = new List<CultureInfo>
             {
                   new CultureInfo("tr-TR"),
                   new CultureInfo("en-US"),
             };

            // Dil ayarlarını ve varsayılan dil seçimini tanımlıyoruz.
            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture("tr-TR"),
            };
            var requestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(0, requestProvider);

           /* app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    ctx.Request.Path = "/not-found";
                    await next();
                }
            });*/

            // Mevcut MVC routing yapısı yerine özelleştirilmiş yeni routing yapısını tanımlıyoruz.
            app.UseRouter(routes =>
            {

                routes.MapMiddlewareRoute("{culture=tr-TR}/{*mvcRoute}", subApp =>
                {
                    subApp.UseRequestLocalization(localizationOptions);

                    subApp.UseMvc(mvcRoutes =>
                    {
                        //====>>>> Area için routerlar

                        //mvcRoutes.MapRoute(
                        //      name: "ornek-en",
                        //      template: "{culture=en-US}/Admin/AnaSayfa",
                        //      constraints: new { Url = new LanguageConstraint("en-US") },
                        //      defaults: new { controller = "AnaSayfa", action = "AnaSayfa", area = "Admin" });


                        //==>> TEST İÇİNDİR
                       
                      


                        //==>> TEST İÇİNDİR
                        mvcRoutes.MapRoute(
                         name: "giris",
                         template: "giris",
                         defaults: new { controller = "Hesap", action = "Giris", area = "Admin" });

                        //====>>>> Öğrenci portalı için routerlar tr-TR


                        mvcRoutes.MapRoute(
                          name: "areas",
                          template: "{area:exists}/{controller=AnaSayfa}/{action=AnaSayfa}/{id?}");



                        //====>>>> Ön taraf için routerlar tr-TR

                        mvcRoutes.MapRoute(
                             name: "anasayfa-tr",
                             template: "{culture=tr-TR}/anasayfa",
                             constraints: new { Url = new LanguageConstraint("tr-TR") },
                             defaults: new { controller = "AnaSayfa", action = "AnaSayfa" });

                        mvcRoutes.MapRoute(
                             name: "arama-tr",
                             template: "{culture=tr-TR}/arama/{searchTerm}/",
                             constraints: new { Url = new LanguageConstraint("tr-TR") },
                             defaults: new { controller = "Search", action = "Search" });

                        mvcRoutes.MapRoute(
                           name: "hakkimizda-tr",
                           template: "{culture=tr-TR}/hakkimizda",
                           constraints: new { Url = new LanguageConstraint("tr-TR") },
                           defaults: new { controller = "Hakkimizda", action = "Hakkimizda" });

                        mvcRoutes.MapRoute(
                     name: "genelbakis-tr",
                     template: "{culture=tr-TR}/genelbakis",
                     constraints: new { Url = new LanguageConstraint("tr-TR") },
                     defaults: new { controller = "Hakkimizda", action = "genelbakis" });

                        mvcRoutes.MapRoute(
                    name: "degerlerimiz-tr",
                    template: "{culture=tr-TR}/degerlerimiz",
                    constraints: new { Url = new LanguageConstraint("tr-TR") },
                    defaults: new { controller = "Hakkimizda", action = "degerlerimiz" });

                        mvcRoutes.MapRoute(
                   name: "vizyonmisyon-tr",
                   template: "{culture=tr-TR}/vizyonmisyon",
                   constraints: new { Url = new LanguageConstraint("tr-TR") },
                   defaults: new { controller = "Hakkimizda", action = "vizyonmisyon" });

                        mvcRoutes.MapRoute(
                  name: "baskanin-mesaji-tr",
                  template: "{culture=tr-TR}/baskanin-mesaji",
                  constraints: new { Url = new LanguageConstraint("tr-TR") },
                  defaults: new { controller = "Hakkimizda", action = "BaskaninMesaji" });

                        mvcRoutes.MapRoute(
                  name: "yonetim-tr",
                  template: "{culture=tr-TR}/yonetim",
                  constraints: new { Url = new LanguageConstraint("tr-TR") },
                  defaults: new { controller = "Hakkimizda", action = "Yonetim" });

                        mvcRoutes.MapRoute(
                  name: "insan-kaynaklari-tr",
                  template: "{culture=tr-TR}/insan-kaynaklari",
                  constraints: new { Url = new LanguageConstraint("tr-TR") },
                  defaults: new { controller = "Hakkimizda", action = "İnsanKaynaklari" });

                        mvcRoutes.MapRoute(
                              name: "bloglar-tr",
                              template: "{culture=tr-TR}/bloglar/{kategoriSeoUrl?}",
                              constraints: new { Url = new LanguageConstraint("tr-TR") },
                              defaults: new { controller = "Bloglar", action = "Bloglar" });

                        mvcRoutes.MapRoute(
                             name: "blog-detayi-tr",
                             template: "{culture=tr-TR}/bloglar/{seoUrl}/{blogId}",
                             constraints: new { Url = new LanguageConstraint("tr-TR") },
                             defaults: new { controller = "Bloglar", action = "BlogDetayi" });

                        mvcRoutes.MapRoute(
                            name: "resim-galerisi-tr",
                            template: "{culture=tr-TR}/galeri",
                            constraints: new { Url = new LanguageConstraint("tr-TR") },
                            defaults: new { controller = "ResimGaleri", action = "ResimGaleri" });

                        mvcRoutes.MapRoute(
                              name: "haberler-tr",
                              template: "{culture=tr-TR}/haberler",
                              constraints: new { Url = new LanguageConstraint("tr-TR") },
                              defaults: new { controller = "Haberler", action = "HaberDetayi" });

                        mvcRoutes.MapRoute(
                             name: "haber-detayi-tr",
                             template: "{culture=tr-TR}/haberler-ve-duyurular/{seoUrl}/{haberId}",
                             constraints: new { Url = new LanguageConstraint("tr-TR") },
                             defaults: new { controller = "Haberler", action = "HaberDetayi" });

                        //mvcRoutes.MapRoute(
                        //      name: "yorumlar-tr",
                        //      template: "{culture=tr-TR}/yorumlar",
                        //      constraints: new { Url = new LanguageConstraint("tr-TR") },
                        //      defaults: new { controller = "Yorumlar", action = "Yorumlar" });

                        mvcRoutes.MapRoute(
                              name: "iletisim-tr",
                              template: "{culture=tr-TR}/iletisim",
                              constraints: new { Url = new LanguageConstraint("tr-TR") },
                              defaults: new { controller = "Iletisim", action = "Iletisim" });

                        mvcRoutes.MapRoute(
                       name: "talepformu-tr",
                       template: "{culture=tr-TR}/talepformu",
                       constraints: new { Url = new LanguageConstraint("tr-TR") },
                       defaults: new { controller = "Iletisim", action = "TalepFormu" });

                        mvcRoutes.MapRoute(
                    name: "Stoklar-tr",
                    template: "{culture=tr-TR}/Stoklar",
                    constraints: new { Url = new LanguageConstraint("tr-TR") },
                    defaults: new { controller = "Stoklar", action = "Stoklar" });
                        mvcRoutes.MapRoute(
                               name: "urun-kategori-tr",
                               template: "{culture=tr-TR}/urunler/{anaKategoriAdi?}/{altKategoriAdi?}/{filtre?}",
                               defaults: new { controller = "Urunler", action = "Urunler" });


                        mvcRoutes.MapRoute(
                          name: "urun-detayi-tr",
                          template: "{culture=tr-TR}/urunler/{kategoriAdi}/{altKategoriAdi}/{urunSeoUrl}/{urunId}",
                          defaults: new { controller = "Urunler", action = "UrunDetay" });




                        //mvcRoutes.MapRoute(
                        //    name: "referanslar-tr",
                        //    template: "{culture=tr-TR}/referanslar",
                        //    constraints: new { Url = new LanguageConstraint("tr-TR") },
                        //    defaults: new { controller = "Referans", action = "Referanslar" });




                        mvcRoutes.MapRoute(
                         name: "hata-tr",
                         template: "{culture=tr-TR}/hata",
                         constraints: new { Url = new LanguageConstraint("tr-TR") },
                         defaults: new { controller = "Hata", action = "Hata" });

                       mvcRoutes.MapRoute(
                       name: "notfound-tr",
                       template: "{culture=tr-TR}/sayfa-bulunamadi",
                       constraints: new { Url = new LanguageConstraint("tr-TR") },
                       defaults: new { controller = "NotFound", action = "SayfaBulunamadi" });


                        //====>>>> Ön taraf için routerlar en-US 

                        mvcRoutes.MapRoute(
                              name: "anasayfa-en",
                              template: "{culture=en-US}/home",
                              constraints: new { Url = new LanguageConstraint("en-US") },
                              defaults: new { controller = "AnaSayfa", action = "AnaSayfa" });

                        mvcRoutes.MapRoute(
                             name: "search-en",
                             template: "{culture=en-US}/search/{searchTerm}/",
                             constraints: new { Url = new LanguageConstraint("en-US") },
                             defaults: new { controller = "Search", action = "Search" });

                        mvcRoutes.MapRoute(
                             name: "urunler-en",
                             template: "{culture=tr-TR}/products",
                             constraints: new { Url = new LanguageConstraint("en-US") },
                             defaults: new { controller = "Urunler", action = "Urunler" });

                        mvcRoutes.MapRoute(
                             name: "urun-detayi-en",
                             template: "{culture=en-US}/products/{urunSeoUrl}",
                             defaults: new { controller = "Urunler", action = "UrunDetay" });


                        mvcRoutes.MapRoute(
                               name: "hakkimizda-en",
                               template: "{culture=en-US}/about-us",
                               constraints: new { Url = new LanguageConstraint("en-US") },
                               defaults: new { controller = "Hakkimizda", action = "Hakkimizda" });


                        mvcRoutes.MapRoute(
                         name: "bloglar-en",
                         template: "{culture=tr-TR}/blogs/{kategoriId?}",
                         constraints: new { Url = new LanguageConstraint("en-US") },
                         defaults: new { controller = "Bloglar", action = "Bloglar" });

                        mvcRoutes.MapRoute(
                         name: "blog-detayi-en",
                         template: "{culture=en-US}/blogs/{seoUrl}/{blogId}",
                         constraints: new { Url = new LanguageConstraint("en-US") },
                         defaults: new { controller = "Bloglar", action = "BlogDetayi" });

                        mvcRoutes.MapRoute(
                            name: "resim-galerisi-en",
                            template: "{culture=en-US}/gallery",
                            constraints: new { Url = new LanguageConstraint("en-US") },
                            defaults: new { controller = "ResimGaleri", action = "ResimGaleri" });

                        //mvcRoutes.MapRoute(
                        //      name: "haberler-en",
                        //      template: "{culture=en-US}/news-and-announcements",
                        //      constraints: new { Url = new LanguageConstraint("en-US") },
                        //      defaults: new { controller = "Haberler", action = "Haberler" });

                        //mvcRoutes.MapRoute(
                        //     name: "haber-detayi-en",
                        //     template: "{culture=en-US}/news-and-announcements/{seoUrl}/{haberId}",
                        //     constraints: new { Url = new LanguageConstraint("en-US") },
                        //     defaults: new { controller = "Haberler", action = "HaberDetayi" });


                        //mvcRoutes.MapRoute(
                        //    name: "yorumlar-en",
                        //    template: "{culture=en-US}/comments",
                        //    constraints: new { Url = new LanguageConstraint("en-US") },
                        //    defaults: new { controller = "Yorumlar", action = "Yorumlar" });

                        mvcRoutes.MapRoute(
                            name: "iletisim-en",
                            template: "{culture=en-US}/contact",
                            constraints: new { Url = new LanguageConstraint("en-US") },
                            defaults: new { controller = "Iletisim", action = "Iletisim" });

                        mvcRoutes.MapRoute(
                         name: "talepformu-en",
                         template: "{culture=en-US}/talepformu",
                         constraints: new { Url = new LanguageConstraint("en-US") },
                         defaults: new { controller = "Iletisim", action = "talepformu" });


                        mvcRoutes.MapRoute(
                         name: "hata-en",
                         template: "{culture=en-US}/error",
                         constraints: new { Url = new LanguageConstraint("en-US") },
                         defaults: new { controller = "Hata", action = "Hata" });

                        mvcRoutes.MapRoute(
                        name: "notfound-en",
                        template: "{culture=en-US}/not-found",
                        constraints: new { Url = new LanguageConstraint("en-US") },
                        defaults: new { controller = "NotFound", action = "SayfaBulunamadi" });

                        mvcRoutes.MapRoute(
                        name: "default",
                        template: "{culture=tr-TR}/{controller=AnaSayfa}/{action=AnaSayfa}/{id?}");


                    });

                });

            });


            argedeSPDataInitializer.EnsurePopulatedAsync(app.ApplicationServices).Wait();
        }
    }
}
