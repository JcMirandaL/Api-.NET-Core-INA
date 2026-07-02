using InaApp.ProyectoInaApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//defino inyeccion de dependencias
//sin el archivo compartido Extensions se definian aqui las dependencias, pero para mantener el codigo mas limpio
//y organizado se creo ese archivo compartido Extensions y se definieron las dependencias ahi,
//y aqui solo se llama a ese metodo para agregar las dependencias a la coleccion de servicios
builder.Services.AddAplicationServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//rutas para las paginas de la aplicacion o vistas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
