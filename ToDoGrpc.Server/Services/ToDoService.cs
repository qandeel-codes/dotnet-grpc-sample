using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ToDoGrpc.Server.Data;
using ToDoGrpc.Server.Models;

namespace ToDoGrpc.Server.Services;

public class ToDoService(AppDbContext dbContext, ILogger<ToDoService> logger)
    : ToDoIt.ToDoItBase
{
    public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Title and Description are required"));

        var item = new ToDoItem { Title = request.Title, Description = request.Description };

        await dbContext.ToDoItems.AddAsync(item);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Created new item with id {ItemId}.", item.Id);

        return await Task.FromResult(new CreateToDoResponse { Id = item.Id });
    }

    public override async Task<GetToDoItemResponse> GetToDoItem(GetToDoItemRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than zero"));

        var item = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} not found"));

        var response = new GetToDoItemResponse
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsComplete = item.IsComplete
        };

        return await Task.FromResult(response);
    }

    public override async Task<GetToDoListResponse> GetToDoList(GetToDoListRequest request, ServerCallContext context)
    {
        var items = await dbContext.ToDoItems.ToListAsync();

        var resonse = new GetToDoListResponse();
        foreach (var item in items)
        {
            resonse.Items.Add(new GetToDoItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsComplete = item.IsComplete
            });
        }

        return await Task.FromResult(resonse);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0 || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must apply a valid object"));

        var item = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} not found"));

        item.Title = request.Title;
        item.Description = request.Description;
        item.IsComplete = request.IsComplete;

        await dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateToDoResponse { Success = true });
    }

    public override async Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than zero"));

        var item = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Item with id {request.Id} not found"));

        dbContext.ToDoItems.Remove(item);
        await dbContext.SaveChangesAsync();

        return await Task.FromResult(new DeleteToDoResponse { Success = true });
    }
}