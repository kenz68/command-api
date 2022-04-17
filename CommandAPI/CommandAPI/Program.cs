using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using AutoMapper;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
// Add services to the container.
var sqlBuilder = new NpgsqlConnectionStringBuilder();
sqlBuilder.ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
sqlBuilder.Username = builder.Configuration["UserID"];
sqlBuilder.Password = builder.Configuration["Password"];
builder
    .Services
    .AddDbContext<CommandContext>(opt => opt.UseNpgsql(sqlBuilder.ConnectionString));
builder.Services.AddControllers();
// add auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddScoped<ICommandApiRepo, MockCommandApiRepo>();
builder.Services.AddScoped<ICommandApiRepo, SqlCommandAPIRepo>();
service.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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