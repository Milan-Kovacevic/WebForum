using WebForum.Api.Configuration.Extensions;
using WebForum.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddRateLimiting();
builder.Services.AddHttpContextAccessor();
builder.Host.AddLogging();
builder.AddModules();
builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(""));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.MapControllers();
app.Run();
