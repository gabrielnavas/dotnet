using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BlogPostsEntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Registra o BlogService e a interface IBlogService
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IPostService, PostService>();

// Registra o BloggingContext no contêiner de serviços
builder.Services.AddDbContext<BloggingContext>(options =>
    options.UseSqlite("Data Source=blogging.db"));

// Adiciona suporte a controllers
builder.Services.AddControllers();

var app = builder.Build();

// Habilita o uso de controllers
app.MapControllers();

await app.RunAsync();
