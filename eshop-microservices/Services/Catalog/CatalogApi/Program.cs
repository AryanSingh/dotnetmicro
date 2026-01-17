using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

//add services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    var cs = builder.Configuration.GetConnectionString("Database");
    Console.WriteLine("DB CONNECTION STRING = " + cs);
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();



var app = builder.Build();

Console.WriteLine($"Registering Carter modules from assembly: {assembly.FullName}");
app.MapCarter();
Console.WriteLine("Carter modules registered");


app.Run();