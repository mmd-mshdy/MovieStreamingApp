using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // Ensure correct namespace for OpenApi Security models
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Application.Models;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Infrastructure;
using MovieStreaming.Infrastructure.Queries;
using MovieStreaming.Infrastructure.Repositories;
using MovieStreaming.Infrastructure.Repository;
using MovieStreaming.Infrastructure.Services;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer =
            builder.Configuration["JwtSettings:Issuer"],

        ValidAudience =
            builder.Configuration["JwtSettings:Audience"],

        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:Secret"]
                    ?? throw new InvalidOperationException(
                        "JWT secret is missing."))),

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(); // Register authorization services

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connecctionstring = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(connecctionstring);
});

builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MovieStreaming.Application.AssemblyReference).Assembly);
});

builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWatchHistoryQueries, WatchHistoryQueries>();
builder.Services.AddScoped<IWatchHistoryRepository, WatchHistoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWatchListRepository, WatchListRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IWatchListQueries, WatchListQueries>();
builder.Services.AddScoped<IRecommendationMovieQueries, RecommendationMovieQueries>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<
    IRecommendationInteractionQueries,
    RecommendationInteractionQueries>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpClient<
    IRecommendationService,
    RecommendationService>((serviceProvider, client) =>
    {
        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        var baseUrl =
            configuration["RecommendationEngine:BaseUrl"]
            ?? throw new InvalidOperationException(
                "RecommendationEngine:BaseUrl is missing.");

        var timeoutSeconds =
            configuration.GetValue<int?>(
                "RecommendationEngine:TimeoutSeconds")
            ?? 10;

        client.BaseAddress = new Uri(baseUrl);
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. CONFIGURE SWAGGER TO ACCEPT TOKENS 🛡️

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Movie Streaming API",
            Version = "v1"
        });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the JWT token only.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

    c.AddSecurityRequirement(doc =>
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference(
                    "Bearer",
                    doc),
                new List<string>()
            }
        });
});

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // Add your exact React port here
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Essential since your application tracks user session states
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Streaming API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// 1. ACTIVATE CORS MIDDLEWARE FIRST (Must sit between UseRouting and UseAuthentication) 🌐
app.UseCors("AllowFrontend");

// 2. RUN SECURITY PIPELINES AFTER CORS ALLOWANCES 🔒
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();