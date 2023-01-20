namespace MinimalApi;

public static class ValidationHelper
{
    public static async ValueTask<object> ValidateTodo(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var todo = context.GetArgument<Todo>(1);

        if (string.IsNullOrWhiteSpace(todo.Title))
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    {"title", new[] {"Title is required"} }
                });
        }

        return await next(context);
    }

    // Not the right way to do this, this will always be run even if there is no Todo being passed in
    //public static async ValueTask<object> ValidateTodo(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    //{
    //    foreach (var argument in context.Arguments)
    //    {
    //        if (argument != null && (argument is Todo todo))
    //        {
    //            if (string.IsNullOrWhiteSpace(todo.Title))
    //            {
    //                return TypedResults.ValidationProblem(
    //                    new Dictionary<string, string[]>
    //                    {
    //                {"title", new[] {"Title is required"} }
    //                    });
    //            }
    //        }
    //    }

    //    return await next(context);
    //}

    public static EndpointFilterDelegate ValidateTodoFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        var parameters = context.MethodInfo.GetParameters();
        int? todoPosition = null;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(Todo))
            {
                todoPosition = i;
                break;
            }
        }

        if (!todoPosition.HasValue)
        {
            return next;
        }

        return async (invocationContext) =>
        {
            var todo = invocationContext.GetArgument<Todo>(todoPosition.Value);
            if (string.IsNullOrWhiteSpace(todo.Title))
            {
                return TypedResults.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        {
                            "title", new[] {"Title is required."}
                        }
                    });
            }

            return await next(invocationContext);
        };
    }
}
