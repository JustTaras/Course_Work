using HomeworkTracker.Components;
using Microsoft.EntityFrameworkCore;
using HomeworkTracker.Data;
using HomeworkTracker.Services;
using HomeworkTracker.Repositories;
using HomeworkTracker.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<SubmissionService>();
builder.Services.AddScoped<GroupService>();

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

// Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (!context.Groups.Any())
    {
        var group1 = new Group { Name = "ІПЗ-3/2" };
        var group2 = new Group { Name = "ІПЗ-3/1" };
        var group3 = new Group { Name = "КН-2025" };
        context.Groups.AddRange(group1, group2, group3);

        var teacher1 = new Teacher { Name = "Олександр Петрович", Email = "petrovych@college.ua", PasswordHash = PasswordHelper.Hash("teacher123") };
        var teacher2 = new Teacher { Name = "Марія Іванівна",     Email = "ivanivna@college.ua",  PasswordHash = PasswordHelper.Hash("teacher123") };
        context.Users.AddRange(teacher1, teacher2);

        context.SaveChanges();

        // Студенти групи ІПЗ-3/2
        context.Users.AddRange(
            new Student { Name = "Іван Іваненко",    Email = "ivan@student.ua",   GroupId = group1.Id, PasswordHash = PasswordHelper.Hash("student123") },
            new Student { Name = "Олена Коваленко",  Email = "olena@student.ua",  GroupId = group1.Id, PasswordHash = PasswordHelper.Hash("student123") },
            new Student { Name = "Максим Шевченко",  Email = "maxym@student.ua",  GroupId = group1.Id, PasswordHash = PasswordHelper.Hash("student123") }
        );

        // Студенти групи ІПЗ-3/1
        context.Users.AddRange(
            new Student { Name = "Аня Бондаренко",   Email = "anya@student.ua",   GroupId = group2.Id, PasswordHash = PasswordHelper.Hash("student123") },
            new Student { Name = "Дмитро Лисенко",   Email = "dmytro@student.ua", GroupId = group2.Id, PasswordHash = PasswordHelper.Hash("student123") }
        );

        // Студенти групи КН-2025
        context.Users.AddRange(
            new Student { Name = "Соломія Мельник",  Email = "solo@student.ua",   GroupId = group3.Id, PasswordHash = PasswordHelper.Hash("student123") },
            new Student { Name = "Тарас Гончаренко", Email = "taras@student.ua",  GroupId = group3.Id, PasswordHash = PasswordHelper.Hash("student123") }
        );

        context.SaveChanges();
    }

    // Оновлення паролів для існуючих користувачів без паролю
    var usersWithoutPassword = context.Users.Where(u => u.PasswordHash == "").ToList();
    if (usersWithoutPassword.Any())
    {
        foreach (var user in usersWithoutPassword)
        {
            var defaultPassword = user is Teacher ? "teacher123" : "student123";
            user.PasswordHash = PasswordHelper.Hash(defaultPassword);
        }
        context.SaveChanges();
    }
}

app.Run();