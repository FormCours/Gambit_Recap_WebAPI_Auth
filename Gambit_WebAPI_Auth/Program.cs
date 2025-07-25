using Gambit_WebAPI_Auth.Handlers;
using Gambit_WebAPI_Auth.Helpers;
using Gambit_WebAPI_Auth.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<MemberHandler>();
builder.Services.AddScoped<TokenHelper>();

builder.Services.AddControllers();
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

app.UseJwks();
app.UseAuthorization();

app.MapControllers();

app.Run();
