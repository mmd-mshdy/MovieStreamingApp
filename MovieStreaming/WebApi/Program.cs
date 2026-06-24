using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MovieStreaming.Application.DTOs.Mapper;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Infrastructure;
using MovieStreaming.Infrastructure.Repository;
using System.Data;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connecctionstring = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(connecctionstring);
}
);
builder.Services.AddAutoMapper(cfg =>
{

},Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MovieStreaming.Application.AssemblyReference).Assembly);
}
);
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWatchHistoryQueries,WatchHistoryQueries>();
builder.Services.AddScoped<IWatchHistoryRepository,WatchHistoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICastMemberRepository, CastMemberRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService,
    CurrentUserService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Streaming API", Version = "v1" });
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
app.MapControllers();

app.Run();
