using Grpc.Core;
using Grpc.Net.Client;
using ToDoGrpc.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:5050");
var client = new ToDoIt.ToDoItClient(channel);

var selectService = true;
while (selectService)
{
    Console.WriteLine("------------------------------------------------------------------------------------");
    Console.WriteLine("Welcome to Todo gRPC client console");
    Console.WriteLine("[1] Create Todo");
    Console.WriteLine("[2] Get Todo Item");
    Console.WriteLine("[3] Get Todo List");
    Console.WriteLine("[4] Update Todo");
    Console.WriteLine("[5] Delete Todo");
    Console.WriteLine("[6] Exit");
    Console.Write("Please enter service number: ");
    
    var selection = String.Empty;
    while (string.IsNullOrEmpty(selection))
    {
        selection = Console.ReadLine();
        switch (selection)
        {
            case "1":
                client.CreateTodoItem();
                break;
            case "2":
                client.GetTodoItem();
                break;
            case "3":
                client.GetTodoList();
                break;
            case "4":
                client.UpdateTodoItem();
                break;
            case "5":
                client.DeleteTodoItem();
                break;
            case "6":
                selectService = false;
                break;
            default:
                Console.WriteLine("Please enter a valid option number:");
                break;
        }
    }
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static class ServiceHandlers
{
    public static void CreateTodoItem(this ToDoIt.ToDoItClient client)
    {
        Console.WriteLine("##### [1] Create ToDo Service #####");
        
        var title = string.Empty;
        Console.Write("Please enter item title: ");
        while (string.IsNullOrEmpty(title))
        {
            title = Console.ReadLine();
            if (string.IsNullOrEmpty(title))
                Console.Write("Please enter a valid item title: ");
        }
        
        var description = string.Empty;
        Console.Write("Please enter item description: ");
        while (string.IsNullOrEmpty(description))
        {
            description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
                Console.Write("Please enter a valid item description: ");
        }
        
        try
        {
            var request = new CreateToDoRequest { Title = title, Description = description };
            var response = client.CreateToDo(request);
            Console.WriteLine(response == null
                ? "\nCould not create Todo item"
                : $"\nTodo item created successfully with {response.Id}.");
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"\nError: StatusCode = {exception.Status.StatusCode}, Detail = {exception.Status.Detail}");
        }
        
        HandleOperationDone();
    }
    
    public static void GetTodoItem(this ToDoIt.ToDoItClient client)
    {
        Console.WriteLine("##### [2] Get ToDo Item Service #####");
        
        var id = string.Empty;
        var idInt = 0;
        Console.Write("Please enter item ID: ");
        while (string.IsNullOrEmpty(id) || !int.TryParse(id, out idInt))
        {
            id = Console.ReadLine();
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out idInt))
                Console.Write("Please enter a valid item id: ");
        }
        
        var request = new GetToDoItemRequest { Id = idInt};
        try
        {
            var response = client.GetToDoItem(request);
            Console.WriteLine(response == null
                ? "\nCould not get Todo item"
                : $"\nResult:\n {response.FormatToDoItemOutput()}");

        }
        catch (RpcException exception)
        {
            Console.WriteLine($"\nError: StatusCode = {exception.Status.StatusCode}, Detail = {exception.Status.Detail}");
        }
        
        HandleOperationDone();
    }

    public static void GetTodoList(this ToDoIt.ToDoItClient client)
    {
        Console.WriteLine("##### [3] Get ToDo Item Service #####");
        Console.WriteLine("\nResults:");
        
        var request = new GetToDoListRequest();
        try
        {
            var response = client.GetToDoList(request);
            if (response == null) Console.WriteLine("\nCould not get Todo item");
            
            foreach (var item in response?.Items ?? [])
            {
                Console.WriteLine(item.FormatToDoItemOutput());
            }
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"\nError: StatusCode = {exception.Status.StatusCode}, Detail = {exception.Status.Detail}");
        }
        
        HandleOperationDone();
    }
    
    public static void UpdateTodoItem(this ToDoIt.ToDoItClient client)
    {
        Console.WriteLine("##### [4] Update ToDo Service #####");
        
        var breakFlag = false;
        
        var id = string.Empty;
        var idInt = 0;
        Console.Write("Please enter item ID: ");
        while (!breakFlag)
        {
            id = Console.ReadLine();
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out idInt))
                Console.Write("Please enter a valid item id: ");
            else
                breakFlag = true;
        }
        
        var title = string.Empty;
        Console.Write("Please enter item title: ");
        breakFlag = false;
        while (!breakFlag)
        {
            title = Console.ReadLine();
            if (string.IsNullOrEmpty(title))
                Console.Write("Please enter a valid item title: ");
            else
                breakFlag = true;
        }
        
        var description = string.Empty;
        Console.Write("Please enter item description: ");
        breakFlag = false;
        while (!breakFlag)
        {
            description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
                Console.Write("Please enter a valid item description: ");
            else
                breakFlag = true;
        }
        
        var isCompleted = string.Empty;
        Console.Write("Is item completed? (Y: yes, N: no): ");
        breakFlag = false;
        while (!breakFlag)
        {
            isCompleted = Console.ReadLine();
            if (!string.IsNullOrEmpty(isCompleted) && 
                (isCompleted.ToLower() == "y"
                 || isCompleted.ToLower() == "yes"
                 || isCompleted.ToLower() == "n"
                 || isCompleted.ToLower() == "no"))
                breakFlag = true;
            else 
                Console.Write("Please enter a valid value: ");
        }

        try
        {
            var request = new UpdateToDoRequest
            {
                Id = idInt,
                Title = title,
                Description = description,
                IsComplete = isCompleted.ToLower() == "y" || isCompleted.ToLower() == "yes"
            };
            var response = client.UpdateToDo(request);
            Console.WriteLine(response == null
                ? "\nCould not create Todo item"
                : $"\nTodo item updated successfully.");
        }
        catch (RpcException exception)
        {
            Console.WriteLine(
                $"\nError: StatusCode = {exception.Status.StatusCode}, Detail = {exception.Status.Detail}");
        }

        HandleOperationDone();
    }
    
    public static void DeleteTodoItem(this ToDoIt.ToDoItClient client)
    {
        Console.WriteLine("##### [5] Delete ToDo Service #####");
        
        var breakFlag = false;
        
        var id = string.Empty;
        var idInt = 0;
        Console.Write("Please enter item ID: ");
        while (!breakFlag)
        {
            id = Console.ReadLine();
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out idInt))
                Console.Write("Please enter a valid item id: ");
            else
                breakFlag = true;
        }
        
        try
        {
            var request = new DeleteToDoRequest { Id = idInt };
            var response = client.DeleteToDo(request);
            Console.WriteLine(response == null
                ? "\nCould not get Todo item"
                : "\nTodo item deleted successfully.");
        }
        catch (RpcException exception)
        {
            Console.WriteLine($"\nError: StatusCode = {exception.Status.StatusCode}, Detail = {exception.Status.Detail}");
        }
        
        HandleOperationDone();
    }

    private static void HandleOperationDone()
    {
        Console.WriteLine("\nPress any key to continue ...");
        Console.ReadKey();
    }

    private static string FormatToDoItemOutput(this GetToDoItemResponse item)
    {
        return $$"""{ Id: {{item.Id}}, Title: "{{item.Title}}", Description: "{{item.Description}}", IsComplete: {{item.IsComplete}} }""";
    }
}