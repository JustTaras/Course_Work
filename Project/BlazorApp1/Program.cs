using Microsoft.EntityFrameworkCore;
using HomeworkTracker.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();