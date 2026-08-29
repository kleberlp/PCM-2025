using Microsoft.AspNetCore.Authentication.Cookies;

// PCM em ASP.NET Core. O desenho segue o sistema atual: autenticação por
// cookie (no MVC5 era FormsAuthentication), estado do usuário em Session
// (empresa, unidade, permissões pivotadas no login) e MVC clássico de
// controller + view. A migração das telas é por módulo — ver
// docs/MIGRACAO-PCMCORE.md.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Session guarda o que o login pivota hoje (empresa, unidade, permissões).
// O timeout espelha o comportamento do sistema atual.
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// Cookie de autenticação no lugar do FormsAuthentication: o "name" do
// usuário continua sendo o código (User.Identity.Name), como no MVC5.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.AccessDeniedPath = "/Account/Login";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        o.SlidingExpiration = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapDefaultControllerRoute();

app.Run();
