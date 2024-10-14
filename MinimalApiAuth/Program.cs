using System.Security.Claims;
using MinimalApiAuth;

// class builder 
var app = Builder.GetBuilder(args).Build();

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


// precisa estar autenticado e deve ter o claim correto
app.MapGet("/employee", (ClaimsPrincipal user) => {
  return Results.Ok(new {
    message = $"Authentication as {user.Identity.Name}"
  });
}).RequireAuthorization("Employee");


// precisa estar autenticado e deve ter o claim correto
app.MapGet("/admin", (ClaimsPrincipal user) => {
  return Results.Ok(new {
    message = $"Authentication as {user.Identity.Name}"
  });
}).RequireAuthorization("Admin");

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
