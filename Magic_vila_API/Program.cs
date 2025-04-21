//using Magic_vila_API.Logging;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
// This for serilog log implementation in file. 
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/vilalogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
//only accept Json and XML format from request API
builder.Services.AddControllers(option=>
{
    option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// this is for custom Logging interface implementaion in container
//builder.Services.AddSingleton<ILogging,Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
