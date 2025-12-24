using Microsoft.EntityFrameworkCore;
using Minimal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

app.MapGet("/todos", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapGet("/todos/{id}", async (int id, TodoDb db) =>
{
    return await db.Todos.FindAsync(id);
});

app.MapPost("/todos", async (TodoItem todoItem, TodoDb db) =>
{
     db.Todos.Add(todoItem);
     await db.SaveChangesAsync();
     return Results.Created($"/todos/{todoItem.Id}", todoItem);

});

app.MapPut("/todos/{id}", async (int id, TodoItem todo, TodoDb db) =>
{
    var item = await db.Todos.FindAsync(id);
    if (item == null) return Results.NotFound();
    item.Name = todo.Name;
    item.IsComplete = todo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todos/{id}", async (int id, TodoDb db) =>
{
    if(await db.Todos.FindAsync(id) is TodoItem todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
