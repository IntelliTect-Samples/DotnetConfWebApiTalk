using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MinimalApi;

public static class TodoApi
{
    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder builder)
    {
        var route = builder.MapGroup("/api/todo");//.AddEndpointFilterFactory(ValidationHelper.ValidateTodoFactory); // we could do this to apply the validation to all endpoints, but there is another way

        route.MapGet("/", async (ApplicationDbContext dbContext) =>
        {
            return await dbContext.Todos.AsNoTracking().ToListAsync();
        });

        // Didn't discuss this in the talk, but this is another way to add filters and other "attributes"
        var authenticatedRoute = route.MapGroup("/").RequireAuthorization();

        authenticatedRoute.MapGet("/{id}", async Task<Results<NotFound, Ok<Todo>>> (ApplicationDbContext dbContext, int id) =>
        {
            var todo = await dbContext.Todos.Where(t => t.Id == id).SingleOrDefaultAsync();

            return todo == null ? TypedResults.NotFound() : TypedResults.Ok(todo);
        });

        var authenticatedAndValidatedRoute = authenticatedRoute.MapGroup("/").AddEndpointFilterFactory(ValidationHelper.ValidateTodoFactory);

        authenticatedAndValidatedRoute.MapPost("/", async Task<Results<BadRequest, Created<Todo>>> (ApplicationDbContext dbContext, Todo todo) =>
        {
            if (todo == null)
            {
                return TypedResults.BadRequest();
            }

            dbContext.Todos.Add(todo);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/api/Todo/{todo.Id}", todo);
        });//.AddEndpointFilter(ValidationHelper.ValidateTodo).RequireAuthorization();

        authenticatedAndValidatedRoute.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (ApplicationDbContext dbContext, int id, Todo todo) =>
        {
            if (id != todo.Id)
            {
                return TypedResults.BadRequest();
            }

            var rowsAffected = await dbContext.Todos.Where(t => t.Id == id)
                .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(t => t.IsComplete, todo.IsComplete)
                    .SetProperty(t => t.Title, todo.Title));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });//.AddEndpointFilter(ValidationHelper.ValidateTodo); // This line breaks with the original implementation since the Todo is not the 2nd parameter anymore

        return route;
    }
}
