using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MovieStreaming.Application.DTOs.Mapper;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Infrastructure;
using MovieStreaming.Infrastructure.Queries;
using MovieStreaming.Infrastructure.Repositories;
using MovieStreaming.Infrastructure.Repository;
using MovieStreaming.Infrastructure.Services;
using System.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Establish Database Context Connections
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connecctionstring = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(connecctionstring);
});

// 2. Register Global CORS policy for your Vite Frontend UI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUi", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 3. Register JWT Authentication Services
var jwtSecretKey = builder.Configuration["Jwt:Secret"] ?? "YourSuperSecretPremiumStreamingKey123!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// 4. Core Infrastructure Dependencies
builder.Services.AddAutoMapper(cfg =>
{
}, typeof(MovieStreaming.Application.AssemblyReference).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MovieStreaming.Application.AssemblyReference).Assembly);
});
// 3. Application Repositories, Queries & Unit of Work Mappings
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWatchHistoryQueries, WatchHistoryQueries>();
builder.Services.AddScoped<IWatchHistoryRepository, WatchHistoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICastMemberRepository, CastMemberRepository>();
builder.Services.AddScoped<IWatchListRepository, WatchListRepository>();
builder.Services.AddScoped<IWatchListQueries, WatchListQueries>();

// 4. Identity Security, Identity Core Hasher, Context Accessors & Custom App Services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 5. Single, Unified Swagger Generator Customization (.NET 10 & OpenAPI 2.x Safe)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Streaming API", Version = "v1" });

    // 📅 Maps DateOnly properties to behave as proper date strings globally in Swagger
    c.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.String,
        Format = "date"
    });

    // 🔒 Attaches the global 'Bearer JWT' token lock interface wrapper to the top of Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var app = builder.Build();

// 6. Request Pipeline Configurations
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Streaming API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowUi");
app.UseRouting();

// CRITICAL PIPELINE GATE ORDER: Authenticate first, then Authorize!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();