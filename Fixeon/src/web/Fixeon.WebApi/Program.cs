using Fixeon.WebApi.Configuration;
using Fixeon.WebApi.SeedData;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy
            .WithOrigins(
                "https://fixeon.netlify.app",
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedMasterData.SeedData(scope.ServiceProvider);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigins");

app.RegisterApp();

app.MapControllers();
app.Run();
