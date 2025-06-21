using Core;
using Infrastructure;
using Infrastructure.Initializers;
using WebApi;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5221");


string connection = builder.Configuration.GetConnectionString("ConnectionPostgresRender") ?? throw new InvalidOperationException("Connection string 'ConnectionPostgresRender' not found.");

builder.Services.AddDBContext(connection);

// Add services to the container.

builder.Services.AddControllersWithCustomSchema();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithCustomSchema();

builder.Services.AddInfrastuctureService();

builder.Services.AddRepository();

builder.Services.AddValidator();

builder.Services.AddAutoMapper();

builder.Services.AddCustomService();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthenticationWithOptions(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://192.168.1.50:19006", "http://localhost:5173", "http://localhost:8081")
              .AllowAnyHeader()
              .AllowAnyMethod();
              //.AllowCredentials(); 
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await UsersAndRolesInitializer.SeedUsersAndRoles(app);

app.Run();
