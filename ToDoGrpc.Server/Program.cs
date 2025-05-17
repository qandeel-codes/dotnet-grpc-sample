using Microsoft.EntityFrameworkCore;
using ToDoGrpc.Server.Data;
using ToDoGrpc.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt=> opt.UseSqlite("Data Source=ToDoDatabase.db"));
builder.Services.AddGrpc().AddJsonTranscoding();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ToDoService>();
app.MapGet("/",
    () =>
        "Todo gRPC service is up and running; Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();