using Fixeon.WebApi.Configuration;
using Fixeon.WebApi.SeedData;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedMasterData.SeedData(scope.ServiceProvider);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.RegisterApp();

app.MapControllers();
app.Run();
