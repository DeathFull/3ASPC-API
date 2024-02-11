using _3ASPC_API.database;
using _3ASPC_API.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseSqlServer("Data Source=localhost;Initial Catalog=ibay;User Id=SA;Password=Admin1234@;TrustServerCertificate=True;Encrypt=False;", optionsBuilder => optionsBuilder.EnableRetryOnFailure()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
