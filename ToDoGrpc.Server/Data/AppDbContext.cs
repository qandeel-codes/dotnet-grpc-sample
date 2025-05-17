using Microsoft.EntityFrameworkCore;
using ToDoGrpc.Server.Models;

namespace ToDoGrpc.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
}