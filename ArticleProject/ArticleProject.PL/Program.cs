using ArticleProject.BL.IRepository;
using ArticleProject.BL.Mapper;
using ArticleProject.BL.Repository;
using ArticleProject.DAL.DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add connection string Configuration.
var connection = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connection));

//Add AutoMapper Configurations. 
builder.Services.AddAutoMapper(m => m.AddProfile(new DomainProfile()));

//Add DI Configurations. 
builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));


//Add Configuration For Identity.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    option =>
    {
        option.LoginPath = new PathString("/Account/Login");
        option.AccessDeniedPath = new PathString("/Account/Login");
    }
    );

builder.Services.AddIdentityCore<IdentityUser>(option => option.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);

//Password and userName Configration.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    //Default Password settings.
    option.User.RequireUniqueEmail = true;
    option.Password.RequireDigit = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireUppercase = true;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 0;

}).AddEntityFrameworkStores<ApplicationDbContext>();

//Add Policy.
builder.Services.AddAuthorization(opt => 
{
    opt.AddPolicy("User", p => p.RequireClaim("User","User"));
    opt.AddPolicy("Admin", p => p.RequireClaim("Admin", "Admin"));
});


//????
//builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false);

//
builder.Environment.IsDevelopment();
/**********************************************************************************************************************/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//???
//app.UseMvcWithDefaultRoute();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
