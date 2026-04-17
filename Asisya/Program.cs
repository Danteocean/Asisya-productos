using Asisya;
using infrastructure;
using Microservice.core;

var builder = WebApplication.CreateBuilder(args);

// Cargar controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});




// Core
builder.Services.AddCoreLayer();

// Infraestructura: EF + repositorios
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddRepository(builder.Configuration);

// Seguridad JWT 
builder.Services.AddJwtAuthentication(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var app = builder.Build();

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();