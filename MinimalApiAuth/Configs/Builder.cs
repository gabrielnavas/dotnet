using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MinimalApiAuth
{
  public class Builder
  {

    // configura todo builder com o jwt
    public static WebApplicationBuilder GetBuilder(string[] args)
    {

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

      addClaims(builder);

      return builder;
    }

    private static void addClaims(WebApplicationBuilder builder)
    {
      builder.Services.AddAuthorization(options =>
      {
        options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
        options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
      });
    }
  }
}