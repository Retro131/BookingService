using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = "oidc";
}).AddCookie(o =>
{
    o.Cookie.HttpOnly = true;
    o.SlidingExpiration = true;
    o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
}).AddOpenIdConnect("oidc", o =>
{
    o.Authority = builder.Configuration["Keycloak:Authority"];
    o.ClientId = builder.Configuration["Keycloak:ClientId"];
    o.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
    o.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"];
    o.ResponseType = "code";
    o.RequireHttpsMetadata = false;
    o.SaveTokens = true;
    o.Scope.Add("openid");
    o.Scope.Add("profile");
    o.Scope.Add("email");
    o.GetClaimsFromUserInfoEndpoint = true;
    o.CallbackPath = builder.Configuration["Keycloak:CallbackPath"];
    o.BackchannelHttpHandler = new HttpClientHandler
    {
        UseProxy = false,
        Proxy = null
    };
    o.MapInboundClaims = false;
});
builder.Services.AddAuthorization();

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    if (path.StartsWithSegments("/auth") ||
        path.StartsWithSegments("/auth-kc") ||
        path.StartsWithSegments("/realms"))
    {
        await next();
        return;
    }

    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        await context.ChallengeAsync("oidc");
        return;
    }

    await next();
});


app.MapGet("/auth/login", ctx =>
    ctx.ChallengeAsync("oidc", new AuthenticationProperties { RedirectUri = app.Configuration["Auth:RedirectUri"] }));

app.MapGet("/auth/logout", async ctx =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await ctx.SignOutAsync("oidc", new AuthenticationProperties { RedirectUri = "/" });
    await ctx.Response.CompleteAsync();
});

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Request.Headers["X-User-Id"] = userId;
            }
        }

        await next();
    });
});

app.MapGet("/", () => { })
    .WithOpenApi();

app.Run();