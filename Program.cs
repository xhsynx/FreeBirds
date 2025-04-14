var builder = WebApplication.CreateBuilder(args);

// Use Startup for configuration
var startup = new FreeBirds.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();