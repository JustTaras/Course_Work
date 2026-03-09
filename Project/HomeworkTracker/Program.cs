using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HomeworkTracker;
using HomeworkTracker.Interfaces;
using HomeworkTracker.Repositories;
using HomeworkTracker.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<SubmissionService>();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
