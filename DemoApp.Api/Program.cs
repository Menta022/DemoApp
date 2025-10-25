using DemoApp.Api.Implementation;
using DemoApp.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataAccess, FileDataAccess>(); // agrega dependencia que da tiempo de vida a la solicitud http.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/notes", async (IDataAccess dataAccess, string nota) =>
{
    var id = await dataAccess.CreateRecord(nota);
    return Results.Created($"/notes/{id}", id);
}).WithName("AgregarNota")
.WithOpenApi();

//app.MapGet("/notes", async (IDataAccess dataAccess) =>
//{
//    var records = await dataAccess.GetAllRecords();
//    return Results.Ok(records);
//}).WithName("ObtenerNotas")
//.WithOpenApi();

/*tarea.*/
app.MapGet("/notes/{id}", async (IDataAccess dataAccess, Guid id) =>
{
    var record = await dataAccess.GetRecordsById(id);
    return record is not null ? Results.Ok(record) : Results.NotFound();
}).WithName("ObtenerNotasId")
.WithOpenApi();

app.MapGet("/notes", async (IDataAccess dataAccess, [FromQuery] string? value) =>
{
    var records = await dataAccess.GetAllRecords();
    var filteredRecords = records.Where(record => value is null || record.Note.Contains(value)).ToList();
    return Results.Ok(filteredRecords);
}).WithName("ObtenerNotas")
.WithOpenApi();

app.MapPut("/notes/{id}", async (IDataAccess dataAccess, Guid id, string note) =>
{
var record = await dataAccess.ModificarRecord(id, note);
    return record is not null ? Results.Ok(record) : Results.NotFound();
}).WithName("modificarNota")
.WithOpenApi();

app.MapDelete("/notes/{id}", async (IDataAccess dataAccess, Guid id) =>
{
    await dataAccess.DeleteRecord(id);
}).WithName("Borrar")
.WithOpenApi();



/*tarea: crear metodo getrecordbyid en interface,
 crear el metod en clase filedataaccess.
mostrar datos con el guid*/
//app.MapGet("/notes/{id}", async (IDataAccess dataAccess, Guid id) =>
//{
//    var record = await dataAccess.GetRecordById(id);
//    return record is not null ? Results.Ok(record) : Results.NotFound();
//}).WithName("ObtenerNotaPorId")
//.WithOpenApi();

app.Run();


