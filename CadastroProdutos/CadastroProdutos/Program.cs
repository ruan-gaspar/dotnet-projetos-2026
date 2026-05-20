// Program.cs

using CadastroProdutos.Data;

using CadastroProdutos.Models;

using CadastroProdutos.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using System.Text;

using Microsoft.AspNetCore.Authorization;
 
 
var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddSingleton<IAuditoriaService, AuditoriaService>();
 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>

{

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme

    {

        Name = "Authorization",

        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,

        Scheme = "bearer",

        BearerFormat = "JWT",

        In = Microsoft.OpenApi.Models.ParameterLocation.Header,

        Description = "Cole o token JWT (sem o prefixo 'Bearer')."

    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement

    {

        {

            new Microsoft.OpenApi.Models.OpenApiSecurityScheme

            {

                Reference = new Microsoft.OpenApi.Models.OpenApiReference

                {

                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,

                    Id = "Bearer"

                }

            },

            new string[] {}

        }

    });

});
 
builder.Services.AddDbContext<AppDbContext>(opt =>

    opt.UseInMemoryDatabase("CadastroProdutosDb"));
 
//AUTH
 
// Registro do TokenService

builder.Services.AddScoped<ITokenService, TokenService>();
 
// Configuração de autenticação JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(opt =>

    {

        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

        opt.TokenValidationParameters = new TokenValidationParameters

        {

            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(key)

        };

    });
 
builder.Services.AddAuthorization();
 
var app = builder.Build();
 
app.UseAuthentication();

app.UseAuthorization();
 
if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI();

}
 
app.MapControllers();
 
// Seed inicial — popula o banco em memória com 5 produtos exemplo

using (var scope = app.Services.CreateScope())

{

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Produtos.Any())

    {

        db.Produtos.AddRange(

            new Produto { Nome = "Notebook Dell", Categoria = "Informatica", Preco = 4500m, Estoque = 15 },

            new Produto { Nome = "Mouse Logitech", Categoria = "Informatica", Preco = 120m, Estoque = 80 },

            new Produto { Nome = "Cadeira Gamer", Categoria = "Moveis", Preco = 1200m, Estoque = 8 },

            new Produto { Nome = "Monitor LG 27", Categoria = "Informatica", Preco = 1800m, Estoque = 22 },

            new Produto { Nome = "Teclado Mecanico", Categoria = "Informatica", Preco = 350m, Estoque = 40 }

        );

        db.SaveChanges();

    }

}
 
app.Run();
 