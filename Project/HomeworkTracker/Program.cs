using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HomeworkTracker;
using HomeworkTracker.Services;
using HomeworkTracker.Data;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSqliteWasmDbContextFactory<AppDbContext>(opts => 
    opts.UseSqlite("Data Source=homework.db"));

builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<SubmissionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<ISqliteWasmDbContextFactory<AppDbContext>>();
    using var dbContext = await factory.CreateDbContextAsync();
    await dbContext.Database.EnsureCreatedAsync();
}

await app.RunAsync();