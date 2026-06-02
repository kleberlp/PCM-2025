using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5100")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Configuração de localização
var supportedCultures = new[] { "pt-BR", "es-MX", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("pt-BR"), // ou "es-MX"
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

var filesPath = builder.Configuration.GetSection("StaticFileConfig:FilesPath").Value;

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(filesPath));

builder.Services.AddScoped<ImageService>();

// Configuração do esquema de autenticação padrão (Cookies)
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Caminho para a página de login
        options.AccessDeniedPath = "/Account/AccessDenied"; // Caminho para página de acesso negado
    });

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//// Configuração do Content-Security-Policy (CSP)
app.Use(async (context, next) =>
{

    try
    {
        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
            "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
            "font-src 'self' https://fonts.gstatic.com; " +
            "img-src 'self' data: blob:;" +
            "frame-src 'none'; " +
            "connect-src 'self' https://www.simservices.com.br  ws://www.simservices.com.br https://viacep.com.br;");

        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[CSP Middleware] Erro: {ex.Message}");
        throw; // relança para não engolir o erro original
    }
});

var fileProvider = app.Services.GetRequiredService<IFileProvider>();

//app.UsePathBase("/VC");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = "/PCM/Files"

});


app.UseSession();
app.UseCors("AllowCorsPolicy");

// Configuração das rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


