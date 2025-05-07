using EffortLessHRM.Admin.Components;
using EffortLessHRM.Admin.Configurations;
using EffortLessHRM.Admin.Data;
using EffortLessHRM.Admin.Services;
using EffortLessHRM.Admin.Utility;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDbSettings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Register MongoDbContext and UserSettingsService for dependency injection
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<ChatbotSettingsService>();
builder.Services.AddScoped<OpenAIEmbedding>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// Redirect root "/" to "/login"
app.MapGet("/", context =>
{
    context.Response.Redirect("/login");
    return Task.CompletedTask;
});

app.UseAntiforgery();

app.MapStaticAssets();

app.Run();
