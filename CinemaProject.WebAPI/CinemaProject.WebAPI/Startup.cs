using CinemaProject.BLL.Helpers;
using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Repositories;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using System;
using System.Text;

namespace CinemaProject.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var optionsBuilder = new DbContextOptionsBuilder<CinemaContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CinemaConnection"));
            using (CinemaContext context = new CinemaContext(optionsBuilder.Options, Configuration))
            {
                context.Database.EnsureCreated();
            }

            services.AddDbContext<CinemaContext>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<UserService>();
            services.AddScoped<TicketService>();
            services.AddScoped<AmenityService>();
            services.AddScoped<ActorService>();
            services.AddScoped<CinemaService>();
            services.AddScoped<CityService>();
            services.AddScoped<FilmService>();
            services.AddScoped<GenreService>();
            services.AddScoped<HallService>();
            services.AddScoped<SeatService>();
            services.AddScoped<SessionService>();
            services.AddScoped<TypeOfSeatService>();
            services.AddMemoryCache();

            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddOData();

            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints
                    .Select()
                    .Filter()
                    .OrderBy()
                    .MaxTop(100)
                    .Count();
                endpoints.EnableDependencyInjection();
                endpoints.MapODataRoute("odata", "odata", GetEdmModel());
            });
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            builder.EntitySet<Ticket>("Tickets");
            builder.EntitySet<Amenity>("Amenities");
            builder.EntitySet<Actor>("Actors");
            builder.EntitySet<Cinema>("Cinemas");
            builder.EntitySet<City>("Cities");
            builder.EntitySet<Film>("Films");
            builder.EntitySet<Genre>("Genres");
            builder.EntitySet<Hall>("Halls");
            builder.EntitySet<Seat>("Seats");
            builder.EntitySet<Session>("Sessions");
            builder.EntitySet<TypeOfSeat>("TypesOfSeat");
            return builder.GetEdmModel();
        }
    }
}
