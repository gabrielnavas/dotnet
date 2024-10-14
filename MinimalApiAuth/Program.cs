using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using MinimalApiAuth;

var builder = WebApplication.CreateBuilder(args);

// obter a chave em array de bytes
var key = Encoding.ASCII.GetBytes(Settings.Secret256Bits);

// adicionar autenticacao
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false;
  options.SaveToken = true;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
  };
});

// add as claims
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
  options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

var app = builder.Build();

// usar autenticacao e autorizacao
app.UseAuthentication();
app.UseAuthorization();

// public
app.MapGet("/anonymous", () => Results.Ok("anonymous"));

// não importa o claim, só precisa estar autenticado
app.MapGet("/authenticated", (ClaimsPrincipal user) => {
  return Results.Ok(new {
    message = $"Authentication as {user.Identity.Name}"
  });
}).RequireAuthorization();

app.MapPost("/login", (User model) =>
{
  var user = UserRepository.Get(model.Username, model.Password);
  if (user == null)
  {
    return Results.NotFound(new
    {
      message = "Invalid username or password"
    });
  }

  var token = TokenService.GenerateToken(model);
  user.Password = "";

  return Results.Ok(new
  {
    user,
    token,
  });
});

app.Run();
