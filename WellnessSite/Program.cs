using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WellnessSite.Data;
using Microsoft.AspNetCore.Identity;
using MessagePack.Resolvers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccess", policy =>
    {
        policy.RequireRole("Admin", "OrgAdmin");
    });
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminAccess");
    options.Conventions.AuthorizePage("/auth/Profile");
    options.Conventions.AuthorizePage("/auth/Logout");
});



builder.Services.AddDbContext<WellnessSiteContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("WellnessSiteContext") ?? throw new InvalidOperationException("Connection string 'WellnessSiteContext' not found."));
    options.EnableSensitiveDataLogging();
    });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<WellnessSiteContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/auth/Access-Denied";
    options.LoginPath = "/auth/Login";
    options.LogoutPath = "/auth/Logout";
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<WellnessSiteContext>();
//    context.Database.EnsureCreated();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
