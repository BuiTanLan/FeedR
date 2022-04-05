using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, lc) => lc.WriteTo.Console());
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("yarp"));
var app = builder.Build();

app.MapGet("/",() => "FeedR Gateway");
app.MapReverseProxy();

app.Run();