using HomeworkTracker.Components;
using Microsoft.EntityFrameworkCore;
using HomeworkTracker.Data;
using HomeworkTracker.Services;
using HomeworkTracker.Repositories;
using HomeworkTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Залишаємо ТІЛЬКИ ОДИН контекст БД (використовуємо tracker.db)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tracker.db"));    

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<SubmissionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 2. ОБ'ЄДНУЄМО Seeding даних в один блок
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    // Створюємо групу та вчителя, якщо їх немає
    if (!context.Groups.Any())
    {
        var group = new Group { Name = "КН-2024" };
        context.Groups.Add(group);
        
        var teacher = new Teacher { Name = "Олександр Петрович", Email = "teacher@test.com" };
        context.Users.Add(teacher);
        
        context.SaveChanges();
    }

    // Додаємо студента (ТЕПЕР ВІН ВСЕРЕДИНІ scope)
    if (!context.Students.Any())
    {
        var group = context.Groups.First();
        var student = new Student 
        { 
            Name = "Іван Іваненко", 
            Email = "student@test.com", 
            GroupId = group.Id 
        };
        context.Students.Add(student);
        context.SaveChanges();
    }
}

app.Run();