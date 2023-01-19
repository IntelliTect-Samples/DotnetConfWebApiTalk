using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiNet6;

public static class TodoApi
{
    public static WebApplication MapTodos(this WebApplication builder)
    {
        builder.MapGet("/api/todo", async (ApplicationDbContext dbContext) =>
        {
            return await dbContext.Todos.AsNoTracking().ToListAsync();
        });

        builder.MapGet("/api/todo/{id}", async (ApplicationDbContext dbContext, int id) =>
        {
            var todo = await dbContext.Todos.Where(t => t.Id == id).SingleOrDefaultAsync();

            return todo == null ? Results.NotFound() : Results.Ok(todo);
        });

        builder.MapPost("/api/todo", async (ApplicationDbContext dbContext, Todo todo) =>
        {
            if (todo == null)
            {
                return Results.BadRequest();
            }

            dbContext.Todos.Add(todo);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/Todo/{todo.Id}", todo);
        });

        return builder;
    }
}