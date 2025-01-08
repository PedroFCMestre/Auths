using Auths.Web;
using Auths.Web.Components;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents();
//.AddMicrosoftIdentityConsentHandler();

builder.Services.AddServerSideBlazor()
    .AddInteractiveServerComponents();

// We need this so Microsoft EntraID redirects back to the app after logout
builder.Services.AddRazorPages();

//builder.Services.AddOutputCache();


builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(["User.Read"])
    .AddMicrosoftGraph() //to get the authenticated user profile data        
    .AddInMemoryTokenCaches();
//.AddDistributedTokenCaches();

//builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddMicrosoftIdentityUI();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

//app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
