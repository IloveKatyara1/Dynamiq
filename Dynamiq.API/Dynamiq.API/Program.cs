using AutoMapper;
using Dynamiq.API.DAL.Context;
using Dynamiq.API.Interfaces;
using Dynamiq.API.Mapping;
using Dynamiq.API.Repositories;
using Dynamiq.API.Repository;
using Dynamiq.API.Stripe.Interfaces;
using Dynamiq.API.Stripe.Repositories;
using Dynamiq.API.Stripe.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new()
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthentication();

var mapperConfig = MapperConfig.RegisterMaps();
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//API
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<IRefreshTokenRepo, RefreshTokenRepo>();
builder.Services.AddTransient<IProductRepo, ProductRepo>();

//API.Stripe
builder.Services.AddTransient<IStripePaymentService, StripePaymentService>();
builder.Services.AddTransient<IStripeProductService, StripeProductService>();
builder.Services.AddTransient<IPaymentHistoryRepo, PaymentHistoryRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
