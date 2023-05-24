using BL.DALModels;
using BL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    
    options.UseSqlServer("Name = ConnectionStrings:DefaultConnection");
});
builder.Services.AddAutoMapper(
    typeof(MVC.Mapping.AutomapperProfile),
    typeof(BL.Mapping.AutomapperProfile)
    );

 builder.Services.AddScoped<IUserRepository, UserRepository>();
 builder.Services.AddScoped<IGenreRepository, GenreRepository>();
 builder.Services.AddScoped<IVideoRepository, VideoRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Video}/{id?}");

app.Run();
