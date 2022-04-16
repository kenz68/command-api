using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var sqlBuilder = new NpgsqlConnectionStringBuilder();
sqlBuilder.ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
sqlBuilder.Username = builder.Configuration["UserID"];
sqlBuilder.Password = builder.Configuration["Password"];
builder
    .Services
    .AddDbContext<CommandContext>(opt => opt.UseNpgsql(sqlBuilder.ConnectionString));
builder.Services.AddControllers();
//builder.Services.AddScoped<ICommandApiRepo, MockCommandApiRepo>();
builder.Services.AddScoped<ICommandApiRepo, SqlCommandAPIRepo>();
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