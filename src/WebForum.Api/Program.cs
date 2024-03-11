using WebForum.Api.Configuration;
using WebForum.Api.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Host.AddLogging();
builder.Services.AddSecurity();
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
app.UseCors(Constants.Cors.AllowAllPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();