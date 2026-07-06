using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // Ensure correct namespace for OpenApi Security models
using MovieStreaming.Application.DTOs.Mapper;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Infrastructure;
using MovieStreaming.Infrastructure.Repositories;
using MovieStreaming.Infrastructure.Repository;
using MovieStreaming.Infrastructure.Services;
using System.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. ADD JWT AUTHENTICATION SERVICES 🔑
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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
builder.Services.AddScoped<ICastMemberRepository, CastMemberRepository>();
builder.Services.AddScoped<IWatchListRepository, WatchListRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. CONFIGURE SWAGGER TO ACCEPT TOKENS 🛡️

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Streaming API", Version = "v1" });

    // 1. Define the Bearer Auth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 2. Updated for .NET 10: Use delegate-based function to inject security requirements smoothly
    c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc),
            new List<string>()
        }
    });
});

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

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

// 3. MIDDLEWARE ORDER MATTERS PER ASP.NET CORE RULES 🚦
app.UseAuthentication(); // Must be placed before UseAuthorization!
app.UseAuthorization();

app.MapControllers();

app.Run();