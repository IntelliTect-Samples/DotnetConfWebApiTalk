using MinimalApiNet6;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.EnableSensitiveDataLogging()
        .UseSqlite("DataSource=sugTalk.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Map("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();

app.MapTodos();

using (var scope = app.Services.CreateScope())
{
    var serviceScope = scope.ServiceProvider;

    using var db = serviceScope.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
