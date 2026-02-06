
var builder = WebApplication.CreateBuilder(args);

//add services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    var cs = builder.Configuration.GetConnectionString("Database");
    Console.WriteLine("DB CONNECTION STRING = " + cs);
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();



var app = builder.Build();

Console.WriteLine($"Registering Carter modules from assembly: {assembly.FullName}");
app.MapCarter();
app.UseExceptionHandler(options =>
{

});
Console.WriteLine("Carter modules registered");


app.Run();