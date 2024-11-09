using FluentValidation;
using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic;
using WebApp.BusinessLogic.Services;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Auth;
using WebApp.Infrastructure.Interfaces.Auth;
using WebApp.WebApi.Validation;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Register DbContext with options
builder.Services.AddDbContext<ForumDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Forum"));
    options.UseSqlServerTriggers();
});

// Register Business Services (BLL)
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITopicService, TopicService>();

// Register auth interfaces
builder.Services.AddScoped<IJvtProvider, JvtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Register UnitOfWork and Repositories (DAL)
builder.Services.AddScoped<IUnitOfWork, UnitOFWork>();

builder.Services.AddValidatorsFromAssemblyContaining<MessageCreateModelValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

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
app.UseAuthorization();
app.MapControllers();
app.Run();
