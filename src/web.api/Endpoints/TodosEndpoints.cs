using web.api.Models;
using web.api.Services;
using Web.API.Endpoints;

namespace web.api.Endpoints;

public static class TodosEndpoints
{
    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder routes)
    {
        var group = routes
             .MapGroup(EndpointGroupNames.TodosGroupName)
             .WithTags(EndpointGroupNames.TodosGroupName);

        group.MapPost("/", async (ITodoTableService todoTableService, Todo todo) =>
        {
            todo.RowKey = Guid.NewGuid().ToString();
            todo.CreatedDate = DateTime.Now;
            // todo.CreatedByEmail = HttpContext.User?.Identity?.Name;
            // todo.CreatedByName = User.Claims.FirstOrDefault(i => i.Type == "name")?.Value;
            await todoTableService.Upsert(todo);

            return Results.Created();
        });

        group.MapGet("/", async (ITodoTableService todoTableService) =>
        {
            var dt = await todoTableService.GetAll();
            return Results.Ok(dt);
        });

        group.MapDelete("/{id}", async (ITodoTableService todoTableService, string id) =>
        {
            await todoTableService.DeleteItem(id);

            return Results.Ok();
        });

        group.MapPut("/{id}", async (ITodoTableService todoTableService, string id) =>
        {
            await todoTableService.CompleteItem(id);
            return Results.Ok();
        });

        return group;
    }


}
