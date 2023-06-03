using BL.DALModels;
using BL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    
    options.UseSqlServer("Name = ConnectionStrings:DefaultConnection");
});
builder.Services.AddAutoMapper(
    typeof(MVC.Mapping.AutomapperProfile),
    typeof(BL.Mapping.AutomapperProfile)
    );
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
 builder.Services.AddScoped<IGenreRepository, GenreRepository>();
 builder.Services.AddScoped<IVideoRepository, VideoRepository>();
 builder.Services.AddScoped<IImageRepository, ImageRepository>();
 builder.Services.AddScoped<ICountryRepository, CountryRepository>();
 builder.Services.AddScoped<ITagRepository, TagRepository>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");


app.Run();
