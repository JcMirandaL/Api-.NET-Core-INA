//imports 
using InaApp.Api.Extensions;  
using InaApp.Common.Interfaces;
using InaApp.Repository;
using InaApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//defino inyeccion de dependencias
//sin el archivo compartido Extensions se definian aqui las dependencias, pero para mantener el codigo mas limpio
//y organizado se creo ese archivo compartido Extensions y se definieron las dependencias ahi,
//y aqui solo se llama a ese metodo para agregar las dependencias a la coleccion de servicios
builder.Services.AddAplicationServices(builder.Configuration);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
