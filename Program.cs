using ObiletCase.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ObiletClient", client =>
{
    client.BaseAddress = new Uri("https://v2-api.obilet.com/api");
});

builder.Services.AddHttpClient<IObiletApiService, ObiletApiService>();

builder.Services.AddDistributedMemoryCache(); // In-memory cache required for session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // Session remains active for 1 hour
    options.Cookie.HttpOnly = true;              // Blocks access to JavaScript
    options.Cookie.IsEssential = true;           // Required for GDPR compliance
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enables session handling for each request
// The UseSession must be added between UseRouting and UseAuthorization.
app.UseSession(); 

app.UseAuthorization();

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();