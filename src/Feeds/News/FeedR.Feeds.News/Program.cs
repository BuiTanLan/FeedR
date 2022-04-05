using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, lc) => lc.WriteTo.Console());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();