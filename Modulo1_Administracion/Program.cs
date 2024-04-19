using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inyectar la conexión
builder.Services.AddDbContext<DulceSaborContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DulceSaborConnection")
    )
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
