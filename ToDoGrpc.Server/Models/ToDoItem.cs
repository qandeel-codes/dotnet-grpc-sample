namespace ToDoGrpc.Server.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsComplete { get; set; } = false;
}