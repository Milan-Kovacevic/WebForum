using WebForum.Api.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddRateLimiting();
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Host.AddLogging();
builder.AddModules();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
