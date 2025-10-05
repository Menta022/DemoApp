using DemoApp.Api.Implementation;
using DemoApp.Api.Interfaces;

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
app.MapGet("/notes/{id}", async (IDataAccess dataAccess, string id) =>
{
    var record = await dataAccess.GetRecordsById(id);
    return record is not null ? Results.Ok(record) : Results.NotFound();
}).WithName("ObtenerNotasId")
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


