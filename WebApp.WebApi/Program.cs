using FluentValidation;
using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.BusinessLogic;
using WebApp.BusinessLogic.Services;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Auth;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.WebApi.Autorization;
using WebApp.WebApi.Extensions;
using WebApp.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Register DbContext with options
builder.Services.AddDbContext<ForumDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Forum"));
    options.UseSqlServerTriggers();
});

builder.Services.Configure<JwtOptions>(configuration.GetSection("JvtOptions"));

// Register Business Services (BLL)
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITopicService, TopicService>();

builder.Services.AddHttpContextAccessor();

// Register auth interfaces
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Register UnitOfWork and Repositories (DAL)
builder.Services.AddScoped<IUnitOfWork, UnitOFWork>();

builder.Services.AddValidatorsFromAssemblyContaining<MessageCreateModelValidator>();

builder.Services.AddControllers();

builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());
builder.Services.AddScoped<IClaimsTransformation, UserRoleClaimsTransformation>();

builder.Services.AddAuthorization(options =>
{
    // Policy for general users
    options.AddPolicy("UserAccess", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("Role", UserRole.User.ToString()) ||
            context.User.HasClaim("Role", UserRole.Moderator.ToString()) ||
            context.User.HasClaim("Role", UserRole.Admin.ToString())));

    // Policy for moderators, which includes User and Moderator roles
    options.AddPolicy("ModeratorAccess", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("Role", UserRole.Moderator.ToString()) ||
            context.User.HasClaim("Role", UserRole.Admin.ToString())));

    // Policy for admins only
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", UserRole.Admin.ToString()));
});

builder.Services.AddAutoMapper(typeof(AutomapperProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.SeedAdminUser(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
