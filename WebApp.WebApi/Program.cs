using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic;
using WebApp.BusinessLogic.Services;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.DataAccess.Data;

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

// Register UnitOfWork and Repositories (DAL)
builder.Services.AddScoped<IUnitOfWork, UnitOFWork>();

builder.Services.AddControllers();

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
