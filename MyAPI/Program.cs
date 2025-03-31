using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyAPIDb>(opt => opt.UseInMemoryDatabase("MyAPIDbList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MyAPI";
    config.Title = "MyAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "MyAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapPost("/todoitems", async (MyAPI todo, MyAPIDb db) =>
{
    db.MyAPIs.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapGet("/todoitems", async (MyAPIDb db) =>
    await db.MyAPIs.ToListAsync());

app.MapGet("/todoitems/complete", async (MyAPIDb db) =>
    await db.MyAPIs.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, MyAPIDb db) =>
    await db.MyAPIs.FindAsync(id)
        is MyAPI todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (MyAPI todo, MyAPIDb db) =>
{
    db.MyAPIs.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, MyAPI inputTodo, MyAPIDb db) =>
{
    var todo = await db.MyAPIs.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, MyAPIDb db) =>
{
    if (await db.MyAPIs.FindAsync(id) is MyAPI MyAPI)
    {
        db.MyAPIs.Remove(MyAPI);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
/*
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/