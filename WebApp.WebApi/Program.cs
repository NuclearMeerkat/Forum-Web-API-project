using System.Text.Json.Serialization;
using FluentValidation;
using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.BusinessLogic;
using WebApp.BusinessLogic.Services;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Auth;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
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

builder.Services.AddAutoMapper(typeof(AutomapperProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
